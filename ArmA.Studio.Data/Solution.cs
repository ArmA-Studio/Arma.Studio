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
        public ObservableSortedCollection<Project> Projects { get; private set; }
        public Uri FileUri { get; set; }

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
                proj.ForEachNested((pff) =>
                {
                    pff.OwningSolution = this;
                    pff.OwningProject = proj;
                    foreach (var child in pff)
                    {
                        child.Parent = pff;
                    }
                });
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
