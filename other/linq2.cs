using System;
using System.Linq;
using System.Collections.Generic;

public class Program{

    static void test(){
        var l = Enumerable.Range(1, 10).ToList();
        var l1 = new List<List<int>>{Enumerable.Range(1, 10).ToList(), Enumerable.Range(1, 10).ToList()};
        var l2 = l1.SelectMany((n) => n);
        l2.ToList().ForEach((n) => Console.WriteLine(n));

    }

    static void test1(){
        var l = Enumerable.Range(1, 3).ToList();
        var l2 = Enumerable.Range(1, 10).ToList();
        var l3 = l2.Except(l);
        //l3.ToList().ForEach((n) => Console.WriteLine(n));
        var sum = l2.Aggregate((n, acc) => acc + n);
        Console.WriteLine(sum);
    }

    public static void Main(){
        //test();
        test1();
        Console.WriteLine("ok");
    }
}