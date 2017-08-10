using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ArmA.Studio.Data.Lint;
using Utility;
using Utility.Collections;

namespace ArmA.Studio.Data
{
    public sealed class FileType
    {
        public Uri IconSource { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string FileTemplate { get; set; }
        public Func<string, bool> IsFileTypeCondition { get; private set; }
        public string DefaultExtension => _Extensions[0];
        public IEnumerable<string> Extensions => _Extensions;
        private readonly string[] _Extensions;

        public string StaticFileName { get; set; }
        public bool HasStaticFileName => !string.IsNullOrWhiteSpace(this.StaticFileName);
        public bool CanCreate { get; set; }

        public ILinterHost Linter { get; set; }

        /// <summary>
        /// Checks if provided file is valid to be used with this FileType.
        /// </summary>
        /// <param name="fileUri">The <see cref="Uri"/> leading to the file.</param>
        /// <returns>true if file behind <see cref="Uri"/> is matching this FileType. false in any other case including null values.</returns>
        public bool IsFileType(Uri fileUri) => this.IsFileType(fileUri?.LocalPath);
        /// <summary>
        /// Checks if provided file is valid to be used with this <see cref="FileType"/>.
        /// </summary>
        /// <param name="fileUri">The <see cref="string"/> leading to the file.</param>
        /// <returns>true if file behind <see cref="string"/> is matching this <see cref="FileType"/>. false in any other case including null values.</returns>
        public bool IsFileType(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                return false;
            var ext = Path.HasExtension(filepath) ? Path.GetExtension(filepath) : Path.GetFileName(filepath);

            return this._Extensions.Any((it) => ext.Equals(it, StringComparison.InvariantCultureIgnoreCase)) && this.IsFileTypeCondition(ext);
        }

        /// <summary>
        /// Creates a new FileType.
        /// </summary>
        /// <param name="isFileTypeCondition">the condition for this ft to validate a string. string passed will contain the extension only or the filename in case no extension present. The extension will be prefixed with a dot. Example: .txt; .sqf; $PBOPREFIX$; db</param>
        /// <param name="name">Name of the FileType</param>
        /// <param name="extensions">Extensions of the filetype. Needs to be prefixed by a dot. Example: .txt</param>
        public FileType(string name, Func<string, bool> isFileTypeCondition, params string[] extensions) : this(name, null as Uri, string.Empty, isFileTypeCondition, extensions) { }
        /// <summary>
        /// Creates a new FileType.
        /// </summary>
        /// <param name="isFileTypeCondition">the condition for this ft to validate a string. string passed will contain the extension only or the filename in case no extension present. The extension will be prefixed with a dot. Example: .txt; .sqf; $PBOPREFIX$; db</param>
        /// <param name="name">Name of the FileType</param>
        /// <param name="extensions">Extensions of the filetype. Needs to be prefixed by a dot. Example: .txt</param>
        /// <param name="iconSource">Path to some Icon file. Should be .ico with 16, 22, 32, 44, 64 pixels. PNG is not recommended.</param>
        public FileType(string name, Uri iconSource, Func<string, bool> isFileTypeCondition, params string[] extensions) : this(name, iconSource, string.Empty, isFileTypeCondition, extensions) { }
        /// <summary>
        /// Creates a new FileType.
        /// </summary>
        /// <param name="isFileTypeCondition">the condition for this ft to validate a string. string passed will contain the extension only or the filename in case no extension present. The extension will be prefixed with a dot. Example: .txt; .sqf; $PBOPREFIX$; db</param>
        /// <param name="name">Name of the FileType</param>
        /// <param name="extensions">Extensions of the filetype. Needs to be prefixed by a dot. Example: .txt</param>
        /// <param name="iconSource">Path to some Icon file. Should be .ico with 16, 22, 32, 44, 64 pixels. PNG is not recommended.</param>
        public FileType(string name, string iconSource, Func<string, bool> isFileTypeCondition, params string[] extensions) : this(name, new Uri(iconSource, UriKind.RelativeOrAbsolute), string.Empty, isFileTypeCondition, extensions) { }
        /// <summary>
        /// Creates a new FileType.
        /// </summary>
        /// <param name="isFileTypeCondition">the condition for this ft to validate a string. string passed will contain the extension only or the filename in case no extension present. The extension will be prefixed with a dot. Example: .txt; .sqf; $PBOPREFIX$; db</param>
        /// <param name="name">Name of the FileType</param>
        /// <param name="extensions">Extensions of the filetype. Needs to be prefixed by a dot. Example: .txt</param>
        /// <param name="iconSource">Path to some Icon file. Should be .ico with 16, 22, 32, 44, 64 pixels. PNG is not recommended.</param>
        /// <param name="description">Text which would describe this FileType.</param>
        public FileType(string name, string iconSource, string description, Func<string, bool> isFileTypeCondition, params string[] extensions) : this(name, new Uri(iconSource, UriKind.RelativeOrAbsolute), description, isFileTypeCondition, extensions) { }
        /// <summary>
        /// Creates a new FileType.
        /// </summary>
        /// <param name="isFileTypeCondition">the condition for this ft to validate a string. string passed will contain the extension only or the filename in case no extension present. The extension will be prefixed with a dot. Example: .txt; .sqf; $PBOPREFIX$; db</param>
        /// <param name="name">Name of the FileType</param>
        /// <param name="extensions">Extensions of the filetype. Needs to be prefixed by a dot. Example: .txt</param>
        /// <param name="iconSource">Path to some Icon file. Should be .ico with 16, 22, 32, 44, 64 pixels. PNG is not recommended.</param>
        /// <param name="description">Text which would describe this FileType.</param>
        public FileType(string name, Uri iconSource, string description, Func<string, bool> isFileTypeCondition, params string[] extensions)
        {
            Contract.Requires(extensions != null && extensions.Any());
            this._Extensions = extensions.ToArray();
            this.Name = name;
            this.IconSource = iconSource;
            this.Description = description;
            this.IsFileTypeCondition = isFileTypeCondition;
            this.FileTemplate = string.Empty;
            this.StaticFileName = string.Empty;
            this.CanCreate = true;
        }
        //Serialization constructor
        private FileType() { }
    }
}
