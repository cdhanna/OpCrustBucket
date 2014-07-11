using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
// Add references to Soap and Binary formatters. 
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using log4net;

namespace SmallNet
{
    class Serializer
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected const string HEADBEG = "HDRBEG";
        protected const string HEADEND = "HDREND";
        protected const string MSGEND = "MSGEND";
        protected const string OBJTYPE = "OT: ";
        protected const string LHSIDSTR = "LHS = ";
        protected const string RHSIDSTR = "RHS = ";
        protected const string SEPIDSTR = "SEP = ";
        protected const string LHS = "{<<";
        protected const string RHS = ">>}";
        protected const string SEP = "::";
        protected static bool flag = false;
        protected enum DATASTRUCTURES {NONE, ARRAY};
        //default constructor
        public Serializer()
        {
    
    
        }
        public static String serialize(Object obj)
        {
            String str = writeHeader(obj);

            str += writeLine(obj, 0);
            str += completeMsg();
            return str;
        }

        private static List<FieldInfo> getBaseFields(object obj)
        {
            // note that this only goes one level deep (or up I suppose) for now.
            Type data = obj.GetType();
            Type supData = data.BaseType;
            List<FieldInfo> supFields = new List<FieldInfo>();
            if (supData != null)
            {
                supFields = supData.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList<FieldInfo>();
            }

            return supFields;
        }
        private static string writeLine(object obj, int layer)
        {
            Type data = obj.GetType();
            String tbs = "";
            String str = "";
            List<FieldInfo> bFields = getBaseFields(obj);
            List<FieldInfo> fields = data.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList<FieldInfo>();
            fields.AddRange(bFields);
            foreach (FieldInfo f in fields)
            {
                str += tbs + LHS;
                if (f.FieldType.IsPrimitive || f.FieldType.Equals(typeof(String)))
                {
                    str += (f.FieldType.ToString() + SEP + f.Name + SEP + f.GetValue(obj).ToString() + RHS);
                }

                else if (f.FieldType.IsArray)
                {
                    
                    Array arr = (Array)f.GetValue(obj);
                    string arrayStr = (f.FieldType.ToString() + SEP + f.Name);
                    int size = arr.Length;
                    arrayStr = arrayStr.Insert(arrayStr.IndexOf("[") + 1, size.ToString());
                    str += arrayStr;
                    foreach (object element in arr)
                    {
                        if (element != null && !element.GetType().IsPrimitive && !element.GetType().Equals(typeof(string)))
                        {
                            flag = true;
                            str += (LHS + element.GetType().ToString() );
                        }    
                        str += writeLine(element,layer);
                        str += flag ? (RHS ) : "";
                        flag = false;
                    }
                    str += (tbs + RHS);
                }
                else if (f.FieldType.IsSerializable)
                {
                    str += ("We don't know what to do with this, but its serializable!" + SEP);
                    // MSDN example code. Not sure this really serialized the data (how could it??)
                    //  PLACEHOLDER CODE
                    //IFormatter form = new BinaryFormatter();
                    //string streamdata = "datastuff.data";
                    //FileStream fs = new FileStream(streamdata, FileMode.Create);
                    //form.Serialize(fs, f.GetValue(obj));
                    //fs.Close();
                }
                else
                {
                    str += (f.FieldType.ToString() + SEP + f.Name);
                    str += writeLine(f.GetValue(obj),layer + 1);
                    str += (tbs + RHS);
                }
            }

            return str;
            
        }
        public static Object deserialize(String str)
        {
            Type t = getObjType(str);
            
            ConstructorInfo c = t.GetConstructor(new Type[] { });
            Object obj = c.Invoke(new object[] { });
            deserialize(str, obj);
            return obj;
        }

        public static void deserialize(String str, Object obj)
        {
            String left, right, delim;
            string header = str.Substring(str.IndexOf(HEADBEG), str.IndexOf(HEADEND) + HEADEND.Length);
            int l = header.IndexOf(LHSIDSTR) + LHSIDSTR.Length;
            int r = header.IndexOf(RHSIDSTR) + RHSIDSTR.Length;
            int s = header.IndexOf(SEPIDSTR) + SEPIDSTR.Length;
            int e = header.IndexOf(HEADEND);
            left = header.Substring(l, r-l - RHSIDSTR.Length);
            right = header.Substring(r, s-r - SEPIDSTR.Length);
            delim = header.Substring(s, e-s);

            string body = str.Substring(str.IndexOf(HEADEND) + HEADEND.Length);
            if (body.Length != MSGEND.Length)
            {
                body = body.Substring(0, body.Length - MSGEND.Length);
                writeVal(body, obj, left, right, delim);
            }
        }



        private static string writeHeader(object obj)
        {
            string str = "";
            str += HEADBEG
                + OBJTYPE + obj.GetType().ToString()
                + LHSIDSTR + LHS
                + RHSIDSTR + RHS
                + SEPIDSTR + SEP 
                + HEADEND;
            return str;
        }

        public static Type getObjType(String str)
        {
            int beg = str.IndexOf(OBJTYPE) + OBJTYPE.Length;
            int len = str.IndexOf(LHSIDSTR) - beg;
            string typeStr = str.Substring(beg, len);
            Type t = Type.GetType(typeStr);
            
            return t;
        }

        private static string completeMsg()
        {
            return MSGEND;
        }

        private static void writeVal(string msg, Object obj, string left, string right, string delim)
        {
            Type data = obj.GetType();
            List<FieldInfo> fields = data.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList<FieldInfo>();
            List<FieldInfo> bFields = getBaseFields(obj);
            fields.AddRange(bFields);
            List<string> fieldList = new List<string>(fields.Count);
            foreach (FieldInfo f in fields)
            {
                fieldList.Add(f.Name);
            }
            string str = getNextObj(msg, left, right, delim);
            
            while ( str.Length != 0)
            {
                string[] pieces = str.Split((left + delim).ToCharArray());
                List<string> p = pieces.ToList<string>();

                int idx = 0;
                while (idx < p.Count)
                {
                    if (p[idx].Length == 0)
                    {
                        p.Remove(p[idx]);
                    }
                    else
                    {
                        idx++;
                    }
                }
               // pieces = p.ToArray();
                int index = 0;
                //if (pieces[0] == "")
                //{
                  //  index = 1;
                //}
                string type = p[index];
                type = type.Replace(left,"");
                int datStructType = isDataStruct(type);
                string name = p[index+1];
                switch (datStructType)

                {
                    case  1:
                        {

                            int lbrack = type.IndexOf("[");
                            int rbrack = type.IndexOf("]");
                            string sizeStr = type.Substring(lbrack+1, rbrack - lbrack-1);
                            int size = Convert.ToInt32(sizeStr);
                            str = str.Substring(str.IndexOf(left));
                            int x = fieldList.IndexOf(name);
                            
                            if (x != -1)

                            {
                                FieldInfo f = fields[x];
                                if (f.Name.Equals(name))
                                {
                                    Array arr = (Array)f.GetValue(obj);
                                    Type elementType = arr.GetType().GetElementType();
                                    
                                    for (int i = 0; i < size; i++)
                                    {
                                        if (elementType.IsPrimitive)
                                        {
                                            string val = p[4 + i*3];
                                            val = val.Replace(right, "");
                                            arr.SetValue(Convert.ChangeType(val, elementType), i);
                                        }
                                        else
                                        { // only writing one element, 
                                            str = str.Substring(1);
                                            str = str.Substring(str.IndexOf(left));
                                            str = str.Insert(0, left);
                                            str = getNextObj(str, left, right, delim);
                                            writeVal(str, arr.GetValue(i), left, right, delim);
                                            str = msg.Substring(msg.IndexOf(str) + str.Length + right.Length);
                                        }
                                    }   
                                }
                            }
                            break;
                        }
                    default:
                        {
                            int x = fieldList.IndexOf(name);
                            if (x != -1)
                            {
                                FieldInfo f = fields[x];
                                if (f.Name.Equals(name))
                                {

                                    Type t = f.FieldType;// f.GetValue(obj).GetType(); 
                                    if (t.IsPrimitive || t.Equals(typeof(string)))
                                    {
                                        string val = p[index+2];
                                        val = val.Replace(right, "");
                                        f.SetValue(obj, Convert.ChangeType(val, t));
                                        break;
                                    }
                                    
                                    else
                                    {
                                        str = str.Substring(str.IndexOf(left));
                                        writeVal(str, f.GetValue(obj), left, right, delim);
                                    }
                                    
                                }
                            }
                            break;
                        }
                }
               
                    msg = msg.Substring(1, msg.Length - 1);
                    if (msg.IndexOf(left) != -1)
                    {
                        msg = msg.Substring(msg.IndexOf(left));
                        str = getNextObj(msg, left, right, delim);
                    }
                    else
                    {
                        msg = msg.Insert(0, left);
                        str = "";
                    }
            }
        }

        private static string getNextObj(string msg, string left, string right, string delim)
        {
            
            int begin = msg.IndexOf(left);
            int balance = 1;
            int i = begin;
            while (++i< msg.Length && balance != 0)
            {
                string str = msg.Substring(i, left.Length);
                if (str.CompareTo(left) == 0)
                    balance++;
                else if (str.Equals(right))
                    balance--;
            }
            return msg.Substring(begin+left.Length,i-begin-right.Length-1); // removes LHS and some?? of RHS
        }
        
        private static int isDataStruct(string type)
        {

            // lazy, only does binary check. 
            // LATER should handle any data struct type
            if (type.Length != 0 && type.Substring(type.Length -1,1).CompareTo("]") == 0)
            {
                return 1;
                
            }
            else
            {
                return 0;
            }
        }
    }

 
}
