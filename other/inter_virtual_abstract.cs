using System;

public interface msg{
    void message();
}

public abstract class BaseB{
    public abstract void f1();
}

public abstract class BaseC: BaseB{
    public virtual void virtual_fun(){
        Console.WriteLine("default virtual fun");
    }

    public sealed override void f1(){}

    public abstract void abstract_fun();
}

public class Deri: BaseC {
    public override void abstract_fun(){}
}

public class c1: msg {
    // can use virtual
    public void message() {
        Console.WriteLine("c1");
    }
}

public class c2: c1 {
    // new message
    public new void message() {
        Console.WriteLine("c2");
    }
}

public class main {
    public static void Main(){
        c2 c = new c2();

        c.message();
        msg m = c as msg;
        m.message();
        //(c as msg).message();



        Console.WriteLine("ok");
    }
}