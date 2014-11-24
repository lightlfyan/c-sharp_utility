using System;
using System.Collections.Generic;
using System.Linq;

public class Base { public string name="Base";}
public class C1: Base { public new string name="C1";}
public class C2: Base { public new string name="C2";}

public interface ForBase<T> {
    void f1(T t);
}

public class Gen<T> {
    public List<T> storage = null;

    public Gen(T t){
        storage = new List<T>();
        storage.Add(t);
    }
}


class Program {
    public delegate void Funs<in T>(T t);
    public delegate T Funs2<T>();

    static void outs<Base>(Gen<Base> l){
        Console.WriteLine("outs");
        l.storage.ToList().ForEach(n => Console.WriteLine(n));
    }

    static void forbase(Base b){
        Console.WriteLine(b.name);
    }

    static C1 returnChild(){
        return new C1();
    }

    static void test(){
        Funs<C1> f = forbase;
        f(new C1());
        Funs2<Base> f1 = returnChild;
        C1 c1 = f1() as C1;
        Console.WriteLine(c1.name);

        //

        Gen<C1> gc1 = new Gen<C1>(new C1());
        outs(gc1);

    }

    static void interfaceTest(ForBase<Base> b){
        
    }

    static public void Main(){
        test();
        Console.WriteLine("ok");
    }
}