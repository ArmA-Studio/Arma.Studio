using ArmA.Studio.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dedbugger
{
    public static class Extensions
    {
        //https://github.com/dedmen/ArmaDebugEngine/blob/master/BIDebugEngine/BIDebugEngine/breakPoint.cpp#L12 void BreakPoint::Serialize(JsonArchive& ar)
        /*
         *  {
         *      "action": {
         *          "code": "string",
         *          "basePath" : "string",
         *          "type": 0
         *      },
         *      "condition": "string",
         *      "filename": "string",
         *      "line": 0
         *  }
         */

        public static JObject AsJToken(this BreakpointInfo b) => new JObject
        {
            { "filename", b.FileRef.ArmAPath.Replace('/', '\\').Replace(@"\\", @"\") },
            { "line", b.Line },
            {
                "action", new JObject
                {
                    { "type", 2 }
                }
            },
            { "label", "" }
        };
    }
}
