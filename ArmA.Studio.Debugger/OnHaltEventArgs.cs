using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ArmA.Studio.Debugger
{
    public class OnHaltEventArgs : EventArgs
    {
        public readonly string DocumentPath;
        public readonly int Line;
        public readonly int Column;
        public OnHaltEventArgs(string documentPath, int line, int column)
        {
            this.DocumentPath = documentPath;
            this.Line = line;
            this.Column = column;
        }
    }
}