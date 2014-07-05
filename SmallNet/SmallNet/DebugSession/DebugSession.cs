using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
using log4net;
using log4net.Appender;
using log4net.Core;
namespace SmallNet.DebugSession
{
        
    public class DebugSession <T> where T:ClientModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        private DebugForm<T> df;
        private Thread formThread;
        private LogWatcher logWatcher;

        private BaseHost<T> host;
        public BaseHost<T> Host { get { return this.host; } set { this.host = value; } }

        private BaseClient<T> client;
        public BaseClient<T> Client { get { return this.client; } set { this.client = value; } }


        private Thread commandThread;
        private Queue<CommandParameter> commandQueue;

        public DebugSession()
        {
            logWatcher = new LogWatcher();
            logWatcher.Updated += logWatcher_Updated;

            this.df = new DebugForm<T>(this);
            this.df.addCommandOption(new Command_GetAllIP<T>());
            this.df.addCommandOption(new Command_StartHost<T>());
            this.df.addCommandOption(new Command_StopHost<T>());
            this.df.addCommandOption(new Command_ConnectTo<T>());
            this.df.addCommandOption(new Command_Disconnect<T>());
            this.df.addCommandOption(new Command_SendMsg<T>());


            this.formThread = new Thread(() =>
            {
                
                //System.Windows.Forms.Application.set
                try
                {
                    System.Windows.Forms.Application.Run(df);
                }
                catch (Exception e)
                {
                    Console.WriteLine("DEBUG SESSION EXCEPTION AHOY!");
                }

                Console.WriteLine("RUN OVER");
            });
            this.formThread.Name = "DEBUGFORM";
            
        }


        private void logWatcher_Updated(object sender, EventArgs e)
        {
            this.df.setLog(this.logWatcher.LogContent);
            
        }

        public void start()
        {
            this.formThread.Start();

            this.commandQueue = new Queue<CommandParameter>();
            this.commandThread = new Thread(() =>
            {

                while (true)
                {
                    if (commandQueue.Count > 0)
                    {
                        CommandParameter cp = this.commandQueue.Dequeue();
                        cp.command.runCommand(this, cp.paramString);
                    }
                    Thread.Sleep(10);
                }


            });
            this.commandThread.Name = "DEBUG:COMMAND";
            this.commandThread.Start();


        }

        public void runCommand(CommandOption<T> command, String[] parameters)
        {
            this.commandQueue.Enqueue(new CommandParameter(command, parameters));
        }

        public void stop()
        {
          //  System.Windows.Forms.Application.Exit();
            //this.formThread.Abort();
          
        }

        class CommandParameter
        {
            public CommandOption<T> command;
            public String[] paramString;
            public CommandParameter(CommandOption<T> command, String[] paramString)
            {
                this.command = command;
                this.paramString = paramString;
            }
        }

    }
}
