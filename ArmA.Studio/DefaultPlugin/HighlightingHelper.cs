using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ArmA.Studio.DefaultPlugin
{
    public static class HighlightingHelper
    {
        public static XmlReader GetSqfXml(MemoryStream memstream)
        {
            using (var writer = XmlWriter.Create(memstream))
            {
                writer.WriteStartElement("SyntaxDefinition");
                writer.WriteAttributeString("name", "SQF");
                {
                    writer.WriteStartElement("Digits");
                    writer.WriteAttributeString("name", "Digits");
                    writer.WriteAttributeString("color", "Chocolate");
                    writer.WriteEndElement();

                    writer.WriteStartElement("RuleSets");
                    writer.WriteStartElement("RuleSet");
                    writer.WriteAttributeString("ignorecase", "true");
                    {
                        writer.WriteElementString("Delimiters", @"&><~!@$%^*()-+=|\#/{}[]:;""' , .?");

                        writer.XshdSpan("stringNormal", ConfigHost.Coloring.SyntaxHighlighting.StringNormal.ToString(), "\"", "\"");
                        writer.XshdSpan("stringSingle", ConfigHost.Coloring.SyntaxHighlighting.StringSingle.ToString(), "'", "'");
                        writer.XshdSpan("PreProcessor", ConfigHost.Coloring.SyntaxHighlighting.PreProcessor.ToString(), "#");
                        writer.XshdSpan("LineComment", ConfigHost.Coloring.SyntaxHighlighting.LineComment.ToString(), "//");
                        writer.XshdSpan("MultiLineComment", ConfigHost.Coloring.SyntaxHighlighting.MultiLineComment.ToString(), "/*", "*/");

                        writer.XshdKeywords(from def in ConfigHost.Instance.SqfDefinitions where def.Kind == RealVirtuality.SQF.SqfDefinition.EKind.Binary select def.Name, "Binary", ConfigHost.Coloring.SyntaxHighlighting.BinaryCommands.ToString(), false);
                        writer.XshdKeywords(from def in ConfigHost.Instance.SqfDefinitions where def.Kind == RealVirtuality.SQF.SqfDefinition.EKind.Unary select def.Name, "Unary", ConfigHost.Coloring.SyntaxHighlighting.UnaryCommands.ToString(), false);
                        writer.XshdKeywords(from def in ConfigHost.Instance.SqfDefinitions where def.Kind == RealVirtuality.SQF.SqfDefinition.EKind.Nular select def.Name, "Nular", ConfigHost.Coloring.SyntaxHighlighting.NullarCommands.ToString(), true);
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            memstream.Seek(0, SeekOrigin.Begin);
            return XmlReader.Create(memstream);
        }
        public static XmlReader GetConfigXml(MemoryStream memstream)
        {
            using (var writer = XmlWriter.Create(memstream))
            {
                writer.WriteStartElement("SyntaxDefinition");
                writer.WriteAttributeString("name", "Config");
                {
                    writer.WriteStartElement("Digits");
                    writer.WriteAttributeString("name", "Digits");
                    writer.WriteAttributeString("color", "Chocolate");
                    writer.WriteEndElement();

                    writer.WriteStartElement("RuleSets");
                    writer.WriteStartElement("RuleSet");
                    writer.WriteAttributeString("ignorecase", "true");
                    {
                        writer.WriteElementString("Delimiters", @"&><~!@$%^*()-+=|\#/{}[]:;""' , .?");

                        writer.XshdSpan("stringNormal", "Crimson", "\"", "\"");
                        writer.XshdSpan("stringSingle", "Crimson", "'", "'");
                        writer.XshdSpan("PreProcessor", "Gray", "#");
                        writer.XshdSpan("LineComment", "Darkgreen", "//");
                        writer.XshdSpan("MultiLineComment", "Darkgreen", "/*", "*/");

                        writer.XshdKeywords("ControlStructure", "Blue", true, "class");
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            memstream.Seek(0, SeekOrigin.Begin);
            return XmlReader.Create(memstream);
        }
        private static void XshdSpan(this XmlWriter writer, string name, string color, string begin, string end = "")
        {
            writer.WriteStartElement("Span");
            writer.WriteAttributeString("name", name);
            writer.WriteAttributeString("color", color);
            if (string.IsNullOrWhiteSpace(end))
                writer.WriteAttributeString("stopateol", "true");
            writer.WriteElementString("Begin", begin);
            if (!string.IsNullOrWhiteSpace(end))
                writer.WriteElementString("End", end);
            writer.WriteEndElement();
        }
        private static void XshdKeywords(this XmlWriter writer, string name, string color, bool isBold, params string[] keywords) => XshdKeywords(writer, keywords, name, color, isBold);
        private static void XshdKeywords(this XmlWriter writer, IEnumerable<string> keywords, string name, string color, bool isBold)
        {
            writer.WriteStartElement("KeyWords");
            writer.WriteAttributeString("name", name);
            writer.WriteAttributeString("color", color);
            writer.WriteAttributeString("bold", isBold.ToString().ToLower());
            foreach(var key in keywords)
            {
                writer.WriteStartElement("Key");
                writer.WriteAttributeString("word", key.ToLower());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
