using Arma.Studio.Data.Linting;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio
{
    public static class Extensions
    {
        public static void AddRange<T>(this ICollection<T> col, IEnumerable<T> ts)
        {
            foreach (var it in ts)
            {
                col.Add(it);
            }
        }

        private class Segment : ISegment
        {
            public int Offset { get; set; }
            public int Length { get; set; }
            public int EndOffset { get; set; }
        }
        public static ISegment GetSegment(this LintInfo lintInfo) => new Segment
        {
            Offset = lintInfo.Offset,
            Length = lintInfo.Length,
            EndOffset = lintInfo.Offset + lintInfo.Length
        };
    }
}
