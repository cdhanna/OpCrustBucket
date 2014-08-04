using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;


namespace SmallNet
{
    //TODO
        // add recursive base parameter grabbing
        // efficiency
        // support additional data structures (list, hash, etc)
        // generics
        // checking for null values on serialize and deserialize

    class Serializer
    {
        protected const string HEADBEG = "HB";
        protected const string HEADEND = "HE";
        protected const string MSGEND = "ME";
        protected const string OBJTYPE = "OT: ";
        protected const string LHS = "{<";
        protected const string RHS = ">}";
        protected const string SEP = ":";
        protected const string STRHDR = "SS";
        protected static bool flag = false; // move into writeLine() eventually?
        protected enum DATASTRUCTURES {NONE, ARRAY};

        //default constructor
        public Serializer()
        {
    
        }

        public static String serialize(Object obj)
        {
            String str = writeHeader(obj);
            str += writeLine(obj); // recursively builds entire message
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
        private static string writeLine(object obj)
        {
            String str = ""; // begin with empty string
            Type data = obj.GetType();
            List<FieldInfo> bFields = getBaseFields(obj); // get super fields
            List<FieldInfo> fields = data.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList<FieldInfo>();
            fields.AddRange(bFields); // combine super fields with fields at this level
            foreach (FieldInfo f in fields)
            {
                str += LHS;
                if (f.FieldType.IsPrimitive || f.FieldType.Equals(typeof(String)))
                {
                    // don't write nulls
                    if (f.GetValue(obj) != null)
                    {
                        str += (f.FieldType.ToString() + SEP + f.Name + SEP);

                        // if its a string, need to put a header in front so the deserializer will escape the text
                        if (f.FieldType.Equals(typeof(String)))
                        {
                            str += STRHDR;
                            str += f.GetValue(obj).ToString().Length;
                            str += STRHDR;
                        }
                        
                        str += f.GetValue(obj).ToString();
                        str += RHS;
                    }
                }

                // move this to a seperate function or class eventually for clarity
                else if (f.FieldType.IsArray)
                {
                    Array arr = (Array)f.GetValue(obj);
                    string arrayStr = (f.FieldType.ToString() + SEP + f.Name);
                    int size = arr.Length; // might need to check for size = 0 / uninitialized arrays
                    arrayStr = arrayStr.Insert(arrayStr.IndexOf("[") + 1, size.ToString());
                    str += arrayStr;
                    foreach (object element in arr)
                    {
                        if (element != null && !element.GetType().IsPrimitive && !element.GetType().Equals(typeof(string)))
                        {
                            flag = true;
                            str += (LHS + element.GetType().ToString() );
                        }    
                        str += writeLine(element);
                        str += flag ? RHS : "";
                        flag = false;
                    }
                    str += RHS;
                }

                else // its an object that needs its own call to writeLine()
                {
                    str += (f.FieldType.ToString() + SEP + f.Name);
                    str += writeLine(f.GetValue(obj));
                    str += RHS;
                }
            }

            return str;
            
        }
        public static Object deserialize(String str)
        {
            // Figure out type of object needed and instantiate a blank one.
            Type t = getObjType(str);
            ConstructorInfo c = t.GetConstructor(new Type[] { });
            Object obj = c.Invoke(new object[] { });

            // Populate the newly instantiated blank onject with the data from the message.
            deserialize(str, obj);
            return obj;
        }

        public static void deserialize(String str, Object obj)
        {
            string header = str.Substring(str.IndexOf(HEADBEG), str.IndexOf(HEADEND) + HEADEND.Length);

            // Get the body of the message, and if it isn't empty, write the values into the object shell.
            string body = str.Substring(str.IndexOf(HEADEND) + HEADEND.Length);
            if (body.Length != MSGEND.Length)
            {
                body = body.Substring(0, body.Length - MSGEND.Length);
                writeVal(body, obj); // recursively does all the work.
            }
        }

        private static string writeHeader(object obj)
        {
            // Provides book keeping data for the message.
            string str = "";
            str += HEADBEG
                + OBJTYPE + obj.GetType().ToString()
                + HEADEND;
            return str;
        }

        public static Type getObjType(String str)
        {
            int beg = str.IndexOf(OBJTYPE) + OBJTYPE.Length;
            int len = str.IndexOf(HEADEND) - beg;
            string typeStr = str.Substring(beg, len);
            Type t = Type.GetType(typeStr);
            return t;
        }

        // simple for now. Maybe later it will need additional functionality.
        private static string completeMsg()
        {
            return MSGEND;
        }

        private static void writeVal(string msg, Object obj)
        {
            Type data = obj.GetType();
            List<FieldInfo> fields = data.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList<FieldInfo>();
            List<FieldInfo> bFields = getBaseFields(obj);
            fields.AddRange(bFields);

            // this separate list just for names might be bloated. Can we get this info efficiently from the fields list?
            List<string> fieldList = new List<string>(fields.Count);
            foreach (FieldInfo f in fields)
            {
                fieldList.Add(f.Name);
            }
            string str = getNextObj(msg, LHS, RHS);
            
            while ( str.Length != 0)
            {
                // this is used when a non-primitive object is being deserialized.
                // on a primitive or string, the str.split command does nothing.
                string[] pieces = str.Split((LHS + RHS).ToCharArray());
                List<string> p = pieces.ToList<string>();
                p = splitIntoFields(str,SEP);

                int index = 0;
                string type = p[index];
                DATASTRUCTURES datStructType = isDataStruct(type);
               
                string name = p[index+1];
                switch (datStructType)
                {
                    case  DATASTRUCTURES.ARRAY:
                        {
                            int lbrack = type.IndexOf("[");
                            int rbrack = type.IndexOf("]");
                            string sizeStr = type.Substring(lbrack+1, rbrack - lbrack-1);
                            int size = Convert.ToInt32(sizeStr);
                            str = str.Substring(str.IndexOf(LHS));
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
                                            val = val.Replace(RHS, "");
                                            arr.SetValue(Convert.ChangeType(val, elementType), i);
                                        }
                                        else
                                        { // only writing one element, 
                                            str = str.Substring(1);
                                            str = str.Substring(str.IndexOf(LHS));
                                            str = str.Insert(0, LHS);
                                            str = getNextObj(str, LHS, RHS);
                                            writeVal(str, arr.GetValue(i));
                                            str = msg.Substring(msg.IndexOf(str) + str.Length + RHS.Length);
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
                                    Type t = f.FieldType; 
                                    if (t.IsPrimitive || t.Equals(typeof(string)))
                                    {
                                        string val = p[index+2];
                                        val = val.Replace(RHS, "");
                                        f.SetValue(obj, Convert.ChangeType(val, t));
                                        break;
                                    }
                                    
                                    else
                                    {
                                        str = str.Substring(str.IndexOf(LHS));
                                        writeVal(str, f.GetValue(obj));
                                    }   
                                }
                            }
                            break;
                        }
                }
               
                    msg = msg.Substring(str.Length + LHS.Length + RHS.Length);
                    if (msg.IndexOf(LHS) != -1)
                    {
                        str = getNextObj(msg, LHS, RHS);
                    }
                    else
                    {
                        msg = msg.Insert(0, LHS);
                        str = "";
                    }
            }
        }

        private static string getNextObj(string msg, string left, string right)
        {
            // probably crimminally inefficient. Try pulling all LHS and RHS indeces and then
            // applying some logic instead of scanning char by char.

            // msg always contains its leading LHS control. so known left origin
            
            int begin = msg.IndexOf(left);
            int balance = 1;
            int i = begin;
            while (++i < msg.Length && balance != 0)
            {
                string str = msg.Substring(i, left.Length);

                if (str.Equals(STRHDR))
                {
                    String str2 = msg.Substring(i + STRHDR.Length);
                    int chop = str2.IndexOf(STRHDR);
                    int skip = Convert.ToInt32(str2.Substring(0,chop));
                    i += (2 * STRHDR.Length + skip);
                }
                if (str.CompareTo(left) == 0)
                    balance++;
                else if (str.Equals(right))
                    balance--;
            }
            return msg.Substring(begin+left.Length,i-begin-right.Length-1); // removes LHS and some?? of RHS
        }

        private static String stripStringHeader(String msg)
        {
            msg = msg.Substring(msg.IndexOf(STRHDR) + STRHDR.Length);
            return msg.Substring(msg.IndexOf(STRHDR)+STRHDR.Length);
            
        }

        private static int getStringLiteralLength(String msg)
        {
            String str = msg.Substring(STRHDR.Length);
            int chop = str.IndexOf(STRHDR);
            return Convert.ToInt32(str.Substring(0, chop));
           
        }

        private static List<String> splitIntoFields(String msg,String delim)
        {
            List<String> fields = new List<String>();
            String f;
            while (msg.Length != 0)
            {
                // if its the value for a string (2 part check may be over kill?)
                if ( msg.IndexOf(STRHDR) >= 0 &&  msg.Substring(0, STRHDR.Length).Equals(STRHDR))
                {
                    // pull out the literal data of the string
                    int len = getStringLiteralLength(msg);
                    msg = msg.Substring(STRHDR.Length);
                    msg = msg.Substring(msg.IndexOf(STRHDR) + STRHDR.Length);
                    f = msg.Substring(0, len);
                }
                else
                {
                    if (msg.IndexOf(delim) > 0)
                        f = msg.Substring(0, msg.IndexOf(delim));
                    else
                        f = msg;
                }
                fields.Add(f);
                msg = msg.Substring(f.Length);
                if (msg.Length != 0)
                {
                    msg = msg.Substring(delim.Length);
                }
            }
            return fields;

        }
        
        private static DATASTRUCTURES isDataStruct(string type)
        {
            // lazy, only does binary check. 
            // LATER should handle any data struct type
             
            if (type.Length != 0 && type.Substring(type.Length -1,1).CompareTo("]") == 0)
            {
                return DATASTRUCTURES.ARRAY;
            }
            else
            {
                return DATASTRUCTURES.NONE;
            }
        }
    }

}
