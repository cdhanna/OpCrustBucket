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
        
    public class DebugSession <T, H> where T:ClientModel where H:HostModel<T>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        private DebugForm<T, H> df;
        private Thread formThread;
        private LogWatcher logWatcher;

        private BaseHost<T, H> host;
        public BaseHost<T, H> Host { get { return this.host; } set { this.host = value; } }
        private bool hostListener;

        private BaseClient<T> client;
        public BaseClient<T> Client { get { return this.client; } set { this.client = value; } }
        private bool clientListener;

        private Thread commandThread;
        private Queue<CommandParameter> commandQueue;

        public DebugSession()
        {
            logWatcher = new LogWatcher();
            logWatcher.Updated += logWatcher_Updated;

            this.df = new DebugForm<T, H>(this);
            this.df.addCommandOption(new Command_HostAndClient<T, H>());
            this.df.addCommandOption(new Command_GetAllIP<T, H>());
            this.df.addCommandOption(new Command_StartHost<T, H>());
            this.df.addCommandOption(new Command_StopHost<T, H>());
            this.df.addCommandOption(new Command_ConnectTo<T, H>());
            this.df.addCommandOption(new Command_Disconnect<T, H>());
            this.df.addCommandOption(new Command_SendMsg<T, H>());
            this.df.addCommandOption(new Command_SendMoveMsg<T, H>());


            this.formThread = new Thread(() =>
            {
                
                //System.Windows.Forms.Application.set
                try
                {
                    System.Windows.Forms.Application.Run(df);
                }
                catch (ThreadAbortException e)
                {
                    Console.WriteLine("Debug has been aborted");
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
                try{
                    while (true)
                    {
                        if (commandQueue.Count > 0)
                        {
                            CommandParameter cp = this.commandQueue.Dequeue();
                            df.appendOutput(cp.command.runCommand(this, cp.paramString));


                            if (Host != null && !this.hostListener)
                            {
                                this.hostListener = true;
                                Host.Connected += (sender2, args) =>
                                {
                                    df.setHostOnBox(Host.IsRunning);
                                };
                                df.setHostOnBox(Host.IsRunning);
                            }
                            if (Client != null && !this.clientListener)
                            {
                                this.clientListener = true;
                                Client.Connected += (sender2, args) =>
                                    {
                                        df.setClientOnBox(Client.IsRunning);

                                    };
                                Client.NewModel += (sender2, args) =>
                                    {
                                        Client.ClientModel.MessageRecieved += (sender3, mArgs) =>
                                            {
                                                df.appendOutput("MSG FROM " + mArgs.getMessage().SenderId + " : " + (SNetUtil.getCurrentTime() - mArgs.getMessage().TimeSent) + " - " + mArgs.getMessage().ToString() + Environment.NewLine);
                                            };
                                    };



                                df.setClientOnBox(Client.IsRunning);
                            }

                        }
                        Thread.Sleep(10);
                    }
                }
                catch (ThreadAbortException e)
                {
                        Console.WriteLine("Debug Command thread aborted");
                }
            

            });
            this.commandThread.Name = "DEBUG:COMMAND";
            this.commandThread.Start();


        }

        public void runCommand(CommandOption<T, H> command, String[] parameters)
        {
            this.commandQueue.Enqueue(new CommandParameter(command, parameters));
        }

        public void stop()
        {
            //if (this.client != null)
            //    this.client.disconnect();

            if (this.host != null)
                this.host.shutdown();

           
            this.formThread.Abort();
            this.commandThread.Abort();

           // System.Windows.Forms.Application.Exit();
          
        }

        class CommandParameter
        {
            public CommandOption<T, H> command;
            public String[] paramString;
            public CommandParameter(CommandOption<T, H> command, String[] paramString)
            {
                this.command = command;
                this.paramString = paramString;
            }
        }

    }
}
