using Arma.Studio.Data.TextEditor;
using ICSharpCode.AvalonEdit;
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
        public static ISegment GetSegment(this LintInfo lintInfo, TextDocument document)
        {
            var offset = document.GetOffset(lintInfo.Line, lintInfo.Column);
            return new Segment
            {
                Offset = offset,
                Length = lintInfo.Length,
                EndOffset = offset + lintInfo.Length
            };
        }
        public static int GetStartOffset(this TextEditor editor)
        {
            int off = editor.CaretOffset;
            if (off <= 0 || off > editor.Document.TextLength)
            {
                return off;
            }


            int start;

            //find start
            for (start = off - 1; start >= 0; start--)
            {
                char c = editor.Document.GetCharAt(start);
                if (Char.IsWhiteSpace(c))
                {
                    start++;
                    return start;
                }
            }
            return 0;
        }
        /// <summary>
        /// Tries to find the start of a word.
        /// </summary>
        /// <param name="editor">A valid <see cref="TextEditor"/> instance.</param>
        /// <param name="isSeparatorCharacter">The method to be used to test the characters. Should return True unless the char is not valid.</param>
        /// <returns></returns>
        public static int GetStartOffsetByCaret(this TextEditor editor, Func<char, bool> isSeparatorCharacter = null)
        {
            if (isSeparatorCharacter == null)
            {
                isSeparatorCharacter = (c) => !Char.IsLetter(c);
            }
            int off = editor.CaretOffset;
            if (off <= 0 || off > editor.Document.TextLength)
            {
                return off;
            }


            int start;

            // find start
            for (start = off - 1; start >= 0; start--)
            {
                char c = editor.Document.GetCharAt(start);
                if (isSeparatorCharacter(c))
                {
                    start++;
                    return start;
                }
            }
            return 0;
        }
        /// <summary>
        /// Tries to find the start of a word.
        /// </summary>
        /// <param name="document">A valid <see cref="TextDocument"/> instance.</param>
        /// <param name="isSeparatorCharacter">The method to be used to test the characters. Should return True unless the char is not valid.</param>
        /// <returns></returns>
        public static int GetStartOffset(this TextDocument document, int baseOffset, Func<char, bool> isSeparatorCharacter = null)
        {
            if (isSeparatorCharacter == null)
            {
                isSeparatorCharacter = (c) => !Char.IsLetter(c);
            }
            int off = baseOffset;
            if (off <= 0 || off > document.TextLength)
            {
                return off;
            }


            int start;

            // find start
            for (start = off - 1; start >= 0; start--)
            {
                char c = document.GetCharAt(start);
                if (isSeparatorCharacter(c))
                {
                    start++;
                    return start;
                }
            }
            return -1;
        }
        /// <summary>
        /// Tries to find the start of a word.
        /// </summary>
        /// <param name="document">A valid <see cref="TextDocument"/> instance.</param>
        /// <param name="isSeparatorCharacter">The method to be used to test the characters. Should return True unless the char is not valid.</param>
        /// <returns></returns>
        public static int GetEndOffset(this TextDocument document, int baseOffset, Func<char, bool> isSeparatorCharacter = null)
        {
            if (isSeparatorCharacter == null)
            {
                isSeparatorCharacter = (c) => !Char.IsLetter(c);
            }
            int off = baseOffset;
            if (off <= 0 || off > document.TextLength)
            {
                return off;
            }


            int start;

            // find start
            for (start = off; start < document.TextLength; start++)
            {
                char c = document.GetCharAt(start);
                if (isSeparatorCharacter(c))
                {
                    return start;
                }
            }
            return -1;
        }

    }
}
