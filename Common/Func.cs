using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public delegate TResult Func<out TResult>();

    public delegate TResult Func<in T,out TResult>(T arg);

    public delegate TResult Func<in T1,in T2,in T3,out TResult>(T1 arg1, T2 arg2, T3 arg3);

}
