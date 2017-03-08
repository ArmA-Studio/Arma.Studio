using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;

namespace ArmA.Studio.Dialogs
{
    public class FileType
    {
        public static IEnumerable<FileType> CurrentFileTypes = GetFileTypes();
        public static IEnumerable<FileType> GetFileTypes()
        {
            var arr = App.Current.TryFindResource("FileTypes") as Array;
            foreach (var it in arr)
            {
                var ft = (FileType)it;
                if (File.Exists(ft.TemplatePath))
                {
                    using (var reader = new StreamReader(ft.TemplatePath))
                    {
                        ft.Content = reader.ReadToEnd();
                    }
                }
                else
                {
                    using (var writer = new StreamWriter(ft.TemplatePath))
                    {
                        writer.Write(ft.DefaultContent);
                    }
                }
                yield return ft;
            }
        }


        public string TemplatePath { get { return Path.Combine(App.FileTemplatePath, string.Concat(this.FileTypeName, ".template.txt")); } }
        public string Image { get; set; }
        public string FileTypeName { get; set; }
        public string DefaultContent { get; set; }
        public string Extension { get; set; }
        public string StaticFileName { get; set; }
        public string Content { get; set; }

    }
}
