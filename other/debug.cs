using System;
using System.Runtime.CompilerServices;

class Program {
    static void Main() {
        Foo(); 
    }

    unsafe void BlueFilter(int[,], bitmap){
        int length = bitmap.Length;
        fixed(int* b = bitmap){
            int *p = b;
            for(int i =0; i<Length; i++){
                *p++ &= 0xff;
            }
        }
    }

    static void Foo (
    [CallerMemberName] string memberName = null, 
    [CallerFilePath] string filePath = null, 
    [CallerLineNumber] int lineNumber = 0)
    {
        Console.WriteLine (memberName); Console.WriteLine (filePath); Console.WriteLine (lineNumber);
    } 
}
