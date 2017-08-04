using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RealVirtuality.Lang.Preprocessing
{
    public class OnIncludeLookupEventArgs : EventArgs
    {
        public string IncludePath { get; internal set; }
        public string FileSystemPath { get; internal set; }

        /// <summary>
        /// Sets the filepath for the file requested in <see cref="IncludePath"/>.
        /// </summary>
        /// <param name="fileuri">Full path to the file.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="fileuri"/> is not absolute.</exception>
        public void SetFile(Uri fileuri)
        {
            if(!fileuri.IsAbsoluteUri)
            {
                throw new ArgumentException("Provided Uri is not absolute", nameof(fileuri));
            }
            this.FileSystemPath = fileuri.LocalPath.StartsWith("file://") ? fileuri.LocalPath.Substring("file://".Length) : fileuri.LocalPath;
        }
    }
}
