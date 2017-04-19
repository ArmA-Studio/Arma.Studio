using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArmA.Studio.Data;
using ArmA.Studio.Data.UI;

namespace ArmA.Studio.DefaultPlugin
{
    public class ImageViewerDocument : DocumentBase
    {
        public static readonly DataTemplate ImageViewerDocumentDataTemplate = LoadFromEmbeddedResource<DataTemplate>(typeof(ImageViewerDocument).Assembly, @"ArmA.Studio.DefaultPlugin.ImageViewerDocumentDataTemplate.xaml");
        /*
         * ".png", ".paa", "jpeg", "jpe", "jpg", "tga", ""
                 *  <DataTrigger Binding="{Binding Ref.FileExtension}" Value=".png">
                 *      <Setter Property="Source" Value="/ArmA.Studio;component/Resources/IconFile/IconFile.ico"/>
                 *  </DataTrigger>
                 *  <DataTrigger Binding="{Binding Ref.FileExtension}" Value=".paa">
                 *      <Setter Property="Source" Value="/ArmA.Studio;component/Resources/IconFile/IconFile.ico"/>
                 *  </DataTrigger>
                 *  <DataTrigger Binding="{Binding Ref.FileExtension}" Value=".jpeg">
                 *      <Setter Property="Source" Value="/ArmA.Studio;component/Resources/IconFile/IconFile.ico"/>
                 *  </DataTrigger>
                 *  <DataTrigger Binding="{Binding Ref.FileExtension}" Value=".jpe">
                 *      <Setter Property="Source" Value="/ArmA.Studio;component/Resources/IconFile/IconFile.ico"/>
                 *  </DataTrigger>
                 *  <DataTrigger Binding="{Binding Ref.FileExtension}" Value=".jpg">
                 *      <Setter Property="Source" Value="/ArmA.Studio;component/Resources/IconFile/IconFile.ico"/>
                 *  </DataTrigger>
                 *  <DataTrigger Binding="{Binding Ref.FileExtension}" Value=".tga">
                 *      <Setter Property="Source" Value="/ArmA.Studio;component/Resources/IconFile/IconFile.ico"/>
                 *  </DataTrigger>
                 */
        public ImageViewerDocument() : this(null) { }
        public ImageViewerDocument(ProjectFile fileRef) : base(fileRef) { }
        public override bool HasChanges => false;
        public override string Title => this.FileReference.FileName;

        public string Source => this.FileReference.FilePath;

        public override void LoadDocument()
        {
            
        }

        public override void RefreshVisuals() { }

        public override void SaveDocument()
        {
            throw new NotImplementedException();
        }

        public override void SaveDocument(string path)
        {
            throw new NotImplementedException();
        }
    }
}
