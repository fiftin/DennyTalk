using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Common
{
    public static class SimplestConfig
    {
        static SimplestConfig()
        {
            System.Reflection.Assembly exe = System.Reflection.Assembly.GetEntryAssembly();

            ConfigFileName = Path.GetDirectoryName(exe.Location) + "\\" +
                Path.GetFileNameWithoutExtension(exe.Location) + ".config";
        }
        public static IDictionary<string, string> Read(string filename)
        {
            return Read(new StreamReader(filename));
        }
        public static IDictionary<string, string> Read()
        {
            if (ConfigFileName != null && File.Exists(ConfigFileName))
                return Read(new StreamReader(ConfigFileName));
            else
                return new Dictionary<string, string>();
        }

        public static IDictionary<string, string> Read(TextReader reader)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string line = reader.ReadLine();
            while (line != null)
            {
                string key = null;
                string val = null;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '=')
                    {
                        key = line.Substring(0, i);
                        val = line.Substring(i + 1);
                        break;
                    }
                }
                if (key != null)
                {
                    if (ret.ContainsKey(key))
                    {
                        ret[key] = val;
                    }
                    else
                    {
                        ret.Add(key, val);
                    }
                }
                line = reader.ReadLine();
            }

            foreach (var opt in ret)
            {
                string str = string.Format("{0} = {1}", opt.Key, opt.Value);
                Console.WriteLine(str.Length > 76 ? str.Substring(0, 76) + "..." : str);
            }

            return ret;
        }

        public static string ConfigFileName { get; set; }
    }
}
