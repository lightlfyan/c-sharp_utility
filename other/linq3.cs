using System;
using System.Collections.Generic;
using System.Linq;

class Program {

    public static void testselect(){
        int[] numbers = {1,2, 3, 4, 2, 4, 3};
        var query = numbers.Select(n =>  {return n + 1;});
        var query1 = numbers.GroupBy(n => n);

        foreach (var v in query1) 
        {

            Console.WriteLine(v.Key);
        }
    }

    public static void Main(){

        testselect();
        Console.WriteLine("ok");
    }
}