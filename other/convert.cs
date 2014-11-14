using System;

abstract public class Base: onlyin<Base> {
    public string v{get; set;}
}

public class A: Base, onlyin<A> {
    public int a = 10;
}
public class B: Base {}
public class C: Base {}

// 非修改行为,把派生类当作基类 没有问题
public interface onlyout<out T>{}
public interface onlyin<in T>{}

public delegate T1 Funcs<in T, out T1>(T arg1);


public class main{

    public static void test1(Base[] items){
        foreach(var things in items){
            Console.WriteLine(things.v);
        }
    }

    // wrong!!
    public static void unsafetest(Base[] items){
        items[0] = new A{v="a"};
    }

    public static void safe1(onlyout<Base> item){

    }

    public static void safe2(onlyin<A> item){
    }


    public static void Main(){
        Funcs<A> f = (n) => {};
        f(new A());
        //f(new B());
    }
}