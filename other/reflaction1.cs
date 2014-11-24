using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Base{
    public string tag = "base";
}

class Generate {
    static Dictionary<string, ConstructorInfo> conts;

    // static Generate(){
    //     var types = Assembly.GetCallingAssembly().GetTypes();
    //     foreach (var type in types) 
    //     {
    //             var ctor = type.GetConstructor();
    //             var element = ctor.Invoke(null) as Base;
    //             if(element != null){
    //                 conts.Add(element.tag, ctor);
    //             }
    //     }
    // }

    public static void test(string tag){

    }

    public static void printinfo(System.Object o){
        //Get the type information
        Type type = o.GetType();
        //Get an array with method information
        MethodInfo[] methods = type.GetMethods();

        //Iterate over all methods
        foreach(var method in methods)
        {
            //Get the name of the method
            string methodName = method.Name;
            //Get the name of the return type of the method
            string methodReturnType = method.ReturnType.Name;
            Console.WriteLine("{0}\t{1}", methodName, methodReturnType);
        }
    }
}

class Program {
    public static void Main(){
        Generate.test("base");
        Generate.printinfo(new Generate());
        Console.WriteLine("ok");
    }
}