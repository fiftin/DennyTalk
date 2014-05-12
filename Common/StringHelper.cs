using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Common
{
    public static class StringHelper
    {
        public static string Join(string separator, System.Collections.IEnumerable arr)
        {
            string ret = "";
            foreach (object y in arr)
            {
                if (ret != "")
                    ret += separator;
                if (y == null)
                    ret += "<NULL>";
                else
                    ret += y.ToString();
            }
            return ret;
        }

        public static Array Split<T>(string str)
        {
            return SplitAndConvert<T>(str, '|', ',', ';');
        }

        public static int ParseInt(string s)
        {
            if (s.StartsWith("0x"))
                return int.Parse(s.Substring(2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return int.Parse(s);
        }

        public static Array SplitAndConvert<T>(string str, params char[] separators)
        {
            string[] strs = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            Type type = typeof(T);
            if (type.Equals(typeof(int)))
            {
                return Array.ConvertAll(strs, delegate(string s)
                {
                    return ParseInt(s);
                });
            }
            else if (type.Equals(typeof(int)))
            {
            }

            return null;
        }

        public static string Reverse(string str)
        {
            StringBuilder builder = new StringBuilder(str.Length);
            for (int i = str.Length - 1; i >= 0; i--)
            {
                builder.Append(str[i]);
            }
            return builder.ToString();
        }

        public static string Substr(string str, int start, int len)
        {
            if (start < 0)
                start = str.Length + start;
            if (len < 0)
                len = str.Length + len;
            return str.Substring(start, len);
        }
    }
}
