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
                    writer.WriteAttributeString("color", ConfigHost.Coloring.SyntaxHighlightingSqf.Digits.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("RuleSets");
                    writer.WriteStartElement("RuleSet");
                    writer.WriteAttributeString("ignorecase", "true");
                    {
                        writer.WriteElementString("Delimiters", @"&><~!@$%^*()-+=|\#/{}[]:;""' , .?");

                        writer.XshdSpan("stringNormal", ConfigHost.Coloring.SyntaxHighlightingSqf.StringNormal.ToString(), "\"", "\"");
                        writer.XshdSpan("stringSingle", ConfigHost.Coloring.SyntaxHighlightingSqf.StringSingle.ToString(), "'", "'");
                        writer.XshdSpan("PreProcessor", ConfigHost.Coloring.SyntaxHighlightingSqf.PreProcessor.ToString(), "#");
                        writer.XshdSpan("LineComment", ConfigHost.Coloring.SyntaxHighlightingSqf.LineComment.ToString(), "//");
                        writer.XshdSpan("MultiLineComment", ConfigHost.Coloring.SyntaxHighlightingSqf.MultiLineComment.ToString(), "/*", "*/");

                        writer.XshdKeywords(from def in ConfigHost.Instance.SqfDefinitions where def.Kind == RealVirtuality.SQF.SqfDefinition.EKind.Binary select def.Name, "Binary", ConfigHost.Coloring.SyntaxHighlightingSqf.BinaryCommands.ToString(), false);
                        writer.XshdKeywords(from def in ConfigHost.Instance.SqfDefinitions where def.Kind == RealVirtuality.SQF.SqfDefinition.EKind.Unary select def.Name, "Unary", ConfigHost.Coloring.SyntaxHighlightingSqf.UnaryCommands.ToString(), false);
                        writer.XshdKeywords(from def in ConfigHost.Instance.SqfDefinitions where def.Kind == RealVirtuality.SQF.SqfDefinition.EKind.Nular select def.Name, "Nular", ConfigHost.Coloring.SyntaxHighlightingSqf.NullarCommands.ToString(), true);
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
                    writer.WriteAttributeString("color", ConfigHost.Coloring.SyntaxHighlightingConfig.Digits.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("RuleSets");
                    writer.WriteStartElement("RuleSet");
                    writer.WriteAttributeString("ignorecase", "true");
                    {
                        writer.WriteElementString("Delimiters", @"&><~!@$%^*()-+=|\#/{}[]:;""' , .?");

                        writer.XshdSpan("stringNormal", ConfigHost.Coloring.SyntaxHighlightingConfig.StringNormal.ToString(), "\"", "\"");
                        writer.XshdSpan("stringSingle", ConfigHost.Coloring.SyntaxHighlightingConfig.StringSingle.ToString(), "'", "'");
                        writer.XshdSpan("PreProcessor", ConfigHost.Coloring.SyntaxHighlightingConfig.PreProcessor.ToString(), "#");
                        writer.XshdSpan("LineComment", ConfigHost.Coloring.SyntaxHighlightingConfig.LineComment.ToString(), "//");
                        writer.XshdSpan("MultiLineComment", ConfigHost.Coloring.SyntaxHighlightingConfig.MultiLineComment.ToString(), "/*", "*/");


                        writer.XshdKeywords("ControlStructure", ConfigHost.Coloring.SyntaxHighlightingConfig.Keywords.ToString(), true, "class");
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
