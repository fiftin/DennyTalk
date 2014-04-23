using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public delegate bool ConsoleWaitCallback(string s);
    public static class ConsoleHelper
    {
        public static void Wait()
        {
            Wait(delegate(string s) { return true; });
        }
        public static void Wait(ConsoleWaitCallback callback)
        {
            string l = Console.ReadLine().ToLower();
            while (l != "q" && l != "exit")
            {
                if (!callback(l))
                {
                    break;
                }
                l = Console.ReadLine().ToLower();
            }
        }
    }
}
