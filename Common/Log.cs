using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Common
{
    public static class Log
    {
        static Log()
        {
            System.Reflection.Assembly exe = System.Reflection.Assembly.GetEntryAssembly();
            if (exe != null)
            {
                LogFileName = Path.GetDirectoryName(exe.Location) + "\\" +
                    Path.GetFileNameWithoutExtension(exe.Location) + ".log";
                ErrorFileName = Path.GetDirectoryName(exe.Location) + "\\" +
                    Path.GetFileNameWithoutExtension(exe.Location) + ".error";
            }
            else
            {
                LogFileName = "app.log";
                ErrorFileName = "app.error";
            }
        }

        private static object outputLocker = new object();

        public static void WriteError(string format, params object[] args)
        {
            WriteError(string.Format(format, args));
        }

        public static void WriteError(string str)
        {
            lock (outputLocker)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(str);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    File.AppendAllText(ErrorFileName,
                        string.Format("{0}\t{1}\r\n\r\n", DateTime.Now, str));
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }


        /// <summary>
        /// Выводит текст ошибки на экран, а так же делает запись в лог-файле ошибок.
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteError(Exception ex)
        {
            lock (outputLocker)
            {
                try
                {
                    if (ex.InnerException != null)
                    {
                        WriteError("{0}: {1}\r\n{2}\r\n*** INNER ***\r\n{3}: {4}\r\n{5}",
                            ex.GetType().Name, ex.Message, ex.StackTrace,
                            ex.InnerException.GetType().Name, ex.InnerException.Message, ex.InnerException.StackTrace);
                    }
                    else
                        WriteError("{0}: {1}\r\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        public static void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        /// <summary>
        /// Выводит сообщение на экран и добавляет сообщение в лог-файл.
        /// </summary>
        /// <param name="str"></param>
        public static void Write(string str)
        {
            lock (outputLocker)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(str);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    File.AppendAllText(LogFileName,
                        string.Format("{0}\t{1}\r\n", DateTime.Now, str));

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Print(ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        /// <summary>
        /// Выводит только на экран, не пишет в лог-файл.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Print(string format, params object[] args)
        {
            Print(string.Format(format, args));
        }

        /// <summary>
        /// Выводит только на экран, не пишет в лог-файл.
        /// </summary>
        /// <param name="str"></param>
        public static void Print(string str)
        {
            Console.WriteLine(string.Format("{0}\t{1}\r\n", DateTime.Now, str));
        }


        /// <summary>
        /// 
        /// </summary>
        public static string LogFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string ErrorFileName { get; set; }



        public static void StartLoggingUnhandledException()
        {

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                Log.WriteError((Exception)e.ExceptionObject);
            }
            else
            {
                Log.WriteError("Unhandled exception");
            }
            if (e.IsTerminating)
            {
                Log.WriteError("Terminating!");
            }
        }
    }
}
