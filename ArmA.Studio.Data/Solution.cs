using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Utility;
using Utility.Collections;

namespace ArmA.Studio.Data
{
    public class Solution : IXmlSerializable
    {
        public static class XmlHelper
        {
            public static void Serialize_Project(XmlWriter writer, Project p)
            {
                writer.WriteStartElement(nameof(Project));


                writer.WriteAttributeString(nameof(Project.ArmAPath), p.ArmAPath);
                writer.WriteAttributeString(nameof(Project.Name), p.Name);
                writer.WriteAttributeString(nameof(Project.FilePath), p.FilePath);
                writer.WriteAttributeString(nameof(Project.ProjectType), Enum.GetName(typeof(EProjectType), p.ProjectType));

                foreach(var pff in p)
                {
                    Serialize_ProjectFileFolder(writer, pff);
                }


                writer.WriteEndElement();
            }
            public static Project Deserialize_Project(XmlReader reader)
            {
                if (reader.Name != nameof(Project))
                    throw new XmlException($"Invalid {nameof(Project)}");


                var p = new Project();

                p.ArmAPath = reader.GetAttribute(nameof(Project.ArmAPath));
                p.Name = reader.GetAttribute(nameof(Project.Name));
                p.FilePath = reader.GetAttribute(nameof(Project.FilePath));
                EProjectType tmp;
                if (!Enum.TryParse(reader.GetAttribute(nameof(Project.ProjectType)), out tmp))
                    throw new XmlException($"Invalid {nameof(Project.ProjectType)}");
                p.ProjectType = tmp;

                reader.ReadStartElement(nameof(Project));
                while (reader.Name == nameof(ProjectFileFolder))
                {
                    p.Children.Add(Deserialize_ProjectFileFolder(reader));
                }
                reader.ReadEndElement();
                return p;
            }

            public static void Serialize_ProjectFileFolder(XmlWriter writer, ProjectFileFolder pff)
            {
                writer.WriteStartElement(nameof(ProjectFileFolder));
                
                writer.WriteAttributeString(nameof(ProjectFileFolder.ProjectRelativePath), pff.ProjectRelativePath);
                writer.WriteAttributeString(nameof(ProjectFileFolder.IsFolder), pff.IsFolder.ToString());

                writer.WriteEndElement();
            }
            public static ProjectFileFolder Deserialize_ProjectFileFolder(XmlReader reader)
            {
                if (reader.Name != nameof(ProjectFileFolder) || !reader.IsEmptyElement)
                    throw new XmlException($"Invalid {nameof(ProjectFileFolder)}");


                var pff = new ProjectFileFolder();

                pff.ProjectRelativePath = reader.GetAttribute(nameof(ProjectFileFolder.ProjectRelativePath));
                bool flag;
                if (!bool.TryParse(reader.GetAttribute(nameof(ProjectFileFolder.IsFolder)), out flag))
                    throw new XmlException($"Invalid {nameof(Project.ProjectType)}");
                pff.IsFolder = flag;

                reader.ReadStartElement(nameof(ProjectFileFolder));
                return pff;
            }
        }

        public ObservableSortedCollection<Project> Projects { get; private set; }
        public Uri FileUri { get; set; }

        public Solution()
        {
            this.Projects = new ObservableSortedCollection<Project>();
        }

        #region Xml Serialization
        public static void Serialize(Solution solution, System.IO.Stream stream)
        {
            var serializer = new XmlSerializer(typeof(Solution));
            serializer.Serialize(stream, solution);
        }
        public static Solution Deserialize(System.IO.Stream stream, Uri fileUri)
        {
            var serializer = new XmlSerializer(typeof(Solution));
            var sol = serializer.Deserialize(stream) as Solution;
            sol.Deserialize_RepairReferences();
            sol.FileUri = fileUri;
            return sol;
        }

        private void Deserialize_RepairReferences()
        {
            foreach (var proj in this.Projects)
            {
                proj.OwningSolution = this;
                foreach (var pff in proj)
                {
                    pff.OwningSolution = this;
                    pff.OwningProject = proj;
                }
            }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(Solution));

            while (reader.Name == nameof(Project))
            {
                this.Projects.Add(XmlHelper.Deserialize_Project(reader));
            }

            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach(var p in this.Projects)
            {
                XmlHelper.Serialize_Project(writer, p);
            }
        }
        #endregion

        public ProjectFileFolder FindFileFolder(Uri uri)
        {
            foreach (var proj in this.Projects)
            {
                var ff = proj.FindFileFolder(uri);
                if (ff != null)
                    return ff;
            }
            return null;
        }

        /// <summary>
        /// Tries to find a <see cref="ProjectFileFolder"/> for provided ArmA-Path.
        /// will return null object if nothing was found.
        /// </summary>
        /// <param name="armaPath">ArmA Path of the <see cref="ProjectFileFolder"/> to find.</param>
        /// <returns>The correct <see cref="ProjectFileFolder"/> instance or null if no corresponding file was found.</returns>
        public ProjectFileFolder GetProjectFileFolderFromArmAPath(string armaPath)
        {
            foreach (var project in this.Projects)
            {
                if (armaPath.StartsWith(project.ArmAPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (var pff in project)
                    {
                        if (armaPath.Equals(pff.ArmAPath, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return pff;
                        }
                    }
                    break;
                }
            }
            return null;
        }
    }
}