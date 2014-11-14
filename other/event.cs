using System;
using System.Collections;

public class LoggerEventArgs: EventArgs
{
    public string Message{get; private set;}
    public int Priority{get; private set;}

    public LoggerEventArgs(int p, string m){
        Priority = p;
        Message = m;
    }
}

public class Logger{
    static Logger(){
        theOnly = new Logger();
    }

    private Logger(){}
    private static Logger theOnly = null;
    public static Logger Singleton{
        get{return theOnly;}
    }

    // EventHandler (sender, msg)
    public event EventHandler<LoggerEventArgs> Log;

    // use delegate handler
    public delegate void handler(int n);
    public event handler Log2;


    public void AddMsg(int priority, string msg){
        EventHandler<LoggerEventArgs> l = Log;
        if(l != null){
            l(this, new LoggerEventArgs(priority, msg));
        }

        if(Log2 != null){
            Log2(priority);
        }
    }
}

public class ConsoleLogger
{
    public static bool enable {get; set;}

    static EventHandler<LoggerEventArgs> f =  (sender, msg) => {
        if(ConsoleLogger.enable){
            Console.Error.WriteLine(sender + msg.ToString());
        }
        };

    static ConsoleLogger(){
        enable = true;
        Logger.Singleton.Log += f;
        Logger.Singleton.Log2 += (int n) => {
            Console.WriteLine("************** " + n);
        };
    }

    static void clear(){
        Logger.Singleton.Log -= f;
    }
}

public class main{
    public static void Main() {
        ConsoleLogger c = new ConsoleLogger();
        Logger.Singleton.AddMsg(1, "test");
    }
}