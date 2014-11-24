using System;


public class Init: IDisposable{
    static int v;
    private int v1;
    static Init(){
        Console.WriteLine("static init");
    }

    public Init():this(1){}
    public Init(int _v){ 
        Console.WriteLine("init1");
        v1 = _v;}

    public int V { get; private set;}

    public void Dispose(){
        Console.WriteLine("dispose 1");
    }

    #region System.IDisposable member
    void System.IDisposable.Dispose(){
        Console.WriteLine("dispose 2");
    }
    #endregion


}

class Program{
    public static void Main(){
        using(Init i = new Init()){
            Console.WriteLine("ok" + i.V);
        }
    }
}