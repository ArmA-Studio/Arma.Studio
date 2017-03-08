
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using IniParser.Parser;
using System.IO;
using RealVirtuality.SQF;

namespace ArmA.Studio
{
    public static class IniHelper
    {
        public static string GetValueOrNull(this IniData data, string key1, string key2)
        {
            if (data[key1] == null)
                return null;
            else
                return data[key1][key2];
        }
        public static void SetValue(this IniData data, string key1, string key2, string value)
        {
            if (data[key1] == null)
                data.Sections.Add(new SectionData(key1));
            data[key1][key2] = value;
        }
    }
}