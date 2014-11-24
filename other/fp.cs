using System;
using System.Collections.Generic;
using System.Linq;

public static class FP{
    public static void Use<T>(this T obj, Action<T> action) where T : IDisposable {
       using (obj) {
            action(obj);
       }
    }  
    static Func<X, Z> Compose<X, Y, Z>(Func<X, Y> f, Func<Y, Z> g)
    {
        return (x) => g(f(x));
    }
    public static Func<T> Cache<T>(this Func<T> func, int cacheInterval)
{
    var cachedValue = func();
    var timeCached = DateTime.Now;

    Func<T> cachedFunc = () => {
                                    if ((DateTime.Now - timeCached).Seconds >= cacheInterval)
                                    {
                                        timeCached = DateTime.Now;
                                        cachedValue = func();
                                    }
                                    return cachedValue;
                                };

    return cachedFunc;
}
}


public class Test: IDisposable {
    public string name = "fp";

    public static void test(Test t){
        Console.WriteLine(t.name);
    }

    public void Dispose(){

    }

    public static void linqt(){
        string[] words = new string[] { "C#", ".NET", "ASP.NET", "MVC", "", "Visual Studio" };
        Func<string, char> firstLetter = delegate(string s) { return s[0]; };
        var sorted = words.OrderBy(word => word.Length).ThenBy(firstLetter);
        var sorted2 = from w in words
                      orderby w.Length
                      //thenby firstLetter(w.Length)
                      select w; 
    }


}

class Program{
    static public void Main(){
        Test t = new Test();
        //t.Use( Test.test );
        Test.linqt();
        Console.WriteLine("ok");
    }
}