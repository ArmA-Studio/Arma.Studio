using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.TextEditor
{
    public class LintInfo
    {
        public ESeverity Severity { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public int Length { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
    }
}
