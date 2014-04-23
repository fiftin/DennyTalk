using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace Common
{
    public static class XmlHelper
    {
        public static bool HasChild(XmlElement elem, string tagName)
        {
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.Name == tagName)
                {
                    return true;
                }
            }
            return false;
        }

        private static readonly Regex TIMESPAN_RX = new Regex(@"(?<hours>\d+h)?\s*(?<minutes>\d+m)?\s*(?<seconds>\d+s)?");

        public static TimeSpan ParseTimeSpan(string str)
        {
            int ms;
            if (int.TryParse(str, out ms))
                return TimeSpan.FromMilliseconds(ms);
            var match = TIMESPAN_RX.Match(str);
            if (!match.Success)
                throw new ArgumentException("Invalid timespan format", "str");
            int h = match.Groups["hours"].Success ? int.Parse(StringHelper.Substr(match.Groups["hours"].Value, 0, -1)) : 0;
            int m = match.Groups["minutes"].Success ? int.Parse(StringHelper.Substr(match.Groups["minutes"].Value, 0, -1)) : 0;
            int s = match.Groups["seconds"].Success ? int.Parse(StringHelper.Substr(match.Groups["seconds"].Value, 0, -1)) : 0;
            return new TimeSpan(h, m, s);
        }
    }
}
