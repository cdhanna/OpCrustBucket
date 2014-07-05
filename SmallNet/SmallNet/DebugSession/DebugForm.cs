using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using log4net;
using log4net.Appender;
using log4net.Core;

namespace SmallNet.DebugSession
{
    public partial class DebugForm<T>: Form where T:ClientModel
    {

        private List<CommandOption<T>> commandOptions;
        private DebugSession<T> debug;
        private int lineCount;
        private Dictionary<CommandOption<T>, String> parameterTable;

        

        public DebugForm(DebugSession<T> debug)
        {
            this.commandOptions = new List<CommandOption<T>>();
            this.debug = debug;
            this.lineCount = 0;
            this.parameterTable = new Dictionary<CommandOption<T>, String>();

            
            InitializeComponent();
        }

        private void DebugForm_Load(object sender, EventArgs e)
        {
            this.ipText.Text = SNetUtil.getLocalIp();
        }

        public void setLog(String log)
        {
            string newText =log; // running on worker thread
            this.Invoke((MethodInvoker)delegate
            {
                logBox.Text = ""; // runs on UI thread
                logBox.AppendText(log); // use append to auto-scroll text box
            });
            
        }

        public void appendOutput(string output)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.consoleBox.AppendText((lineCount++) + ": " + output + Environment.NewLine);
            });
        }

        public void setClientOnBox(bool client)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.clientOn.Checked = client;
            });
        }

        public void setHostOnBox(bool host)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.serverOn.Checked = host;
            });
            
        }

        public void addCommandOption(CommandOption<T> command)
        {
            this.commandOptions.Add(command);
            this.commandOptionsBox.Items.Add(command);

        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            
            this.Invoke((MethodInvoker)delegate
            {
                
                CommandOption<T> c = (CommandOption<T>)this.commandOptionsBox.SelectedItem;
                debug.runCommand(c, this.paramBox.Text.Split(' '));
                
                //String output = c.runCommand(this.debug, this.paramBox.Text.Split(' '));
                //this.consoleBox.AppendText((lineCount++) + ": [" + c.getName() + "] => " + output + Environment.NewLine);

                //this.parameterTable[c] = this.paramBox.Text;


            });
        }

        private void commandOptionsBox_SelectedValueChanged(object sender, EventArgs e)
        {
            this.paramBox.Text = "";



            CommandOption<T> c = (CommandOption<T>)this.commandOptionsBox.SelectedItem;
            if (this.parameterTable.ContainsKey(c))
            {
                string p = this.parameterTable[c];
                if (p != null)
                {
                    this.paramBox.Text = p;
                }
            }

            string tt = c.getParamDescription();
            if (tt == null)
            {
                tt = "No parameter needed";
                this.paramBox.Enabled = false;
            }
            else
            {
                this.paramBox.Enabled = true;
            }
            
            this.toolTip.SetToolTip(this.paramBox, tt);
            this.toolTip.ShowAlways = true;
        }

        private void logTab_Click(object sender, EventArgs e)
        {

        }

    }
}
