using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class Program{

    public TResult CallInterface<T, TResult>(Expression<Func<T, TResult>> op){
        var exp = op.Body as MethodCallExpression;

        return defualt(TResult);
    }

    public static void  Main(){

    }
}