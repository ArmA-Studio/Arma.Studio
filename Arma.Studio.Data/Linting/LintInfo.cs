using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Linting
{
    public class LintInfo
    {
        public ESeverity Severity { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
    }
}
