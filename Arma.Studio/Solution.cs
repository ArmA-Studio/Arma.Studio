using Arma.Studio.Data.IO;
using Arma.Studio.Data.UI;
using Arma.Studio.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Arma.Studio
{
    public class Solution
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public BreakpointManager BreakpointManager { get; }
        public FileManager FileManager { get; }



        public Solution()
        {
            this.FileManager = new FileManager();
            this.BreakpointManager = new BreakpointManager();
        }
        
        private const string CONST_FILE = "File";
        private const string CONST_PBO = "PBO";
        private const string CONST_ATT_NAME = "name";
        private const string CONST_ERROR = "ERROR";

        public static Solution Deserialize(string path)
        {
            Solution solution = null;
            PBO pbo = null;
            var insidefile = false;
            var breakpoints = new List<Breakpoint>();
            using (var reader = new StreamReader(path))
            using (var xmlreader = XmlReader.Create(reader))
            {
                xmlreader.ReadStartElement(nameof(Solution));
                
                while(xmlreader.Read())
                {
                    switch (xmlreader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xmlreader.Name)
                            {
                                case nameof(Solution):
                                    if (solution == null)
                                    {
                                        Logger.Info($"Solution start while in Solution.");
                                    }
                                    else
                                    {
                                        Logger.Info($"Found Solution start.");
                                        solution = new Solution();
                                    }
                                    break;
                                case CONST_FILE:
                                    if (pbo == null)
                                    {
                                        Logger.Error($"File start without PBO.");
                                    }
                                    else if (insidefile)
                                    {
                                        Logger.Error($"File start while already inside of a File.");
                                    }
                                    else
                                    {
                                        Logger.Info($"Found File '{xmlreader.Name}' of PBO '{pbo.Name}' start.");
                                        insidefile = true;
                                    }
                                    break;
                                case CONST_PBO:
                                    if (pbo != null)
                                    {
                                        Logger.Error($"PBO start while within a PBO.");
                                    }
                                    else
                                    {
                                        Logger.Info($"Found PBO start.");
                                        var name = xmlreader.GetAttribute(CONST_ATT_NAME);
                                        if(String.IsNullOrWhiteSpace(name))
                                        {
                                            Logger.Error($"PBO is missing {CONST_ATT_NAME}attribute");
                                            name = CONST_ERROR;
                                        }
                                        pbo = new PBO
                                        {
                                            Name = name
                                        };
                                    }
                                    break;
                                default:
                                    Logger.Error($"Unknown XmlElement '{xmlreader.Name}'.");
                                    break;
                            }
                            break;
                        case XmlNodeType.Text:
                            if (insidefile)
                            {
                                var text = xmlreader.Value.Trim();
                                pbo.Add(text);
                            }
                            else
                            {
                                Logger.Error($"Unexpected XmlText '{xmlreader.Value}'.");
                            }
                            break;
                        case XmlNodeType.EndElement:
                            switch (xmlreader.Name)
                            {
                                case CONST_FILE:
                                    if (insidefile)
                                    {
                                        Logger.Error($"File end without file start.");
                                    }
                                    else
                                    {
                                        Logger.Info($"Found File end.");
                                        insidefile = false;
                                    }
                                    break;
                                case CONST_PBO:
                                    if (insidefile)
                                    {
                                        Logger.Error($"PBO end while file still is open.");
                                    }
                                    else if(pbo == null)
                                    {
                                        Logger.Error($"PBO end without file start.");
                                    }
                                    else
                                    {
                                        solution.FileManager.Add(pbo);
                                        pbo = null;
                                    }
                                    break;
                                default:
                                    Logger.Error($"Unknown XmlElement '{xmlreader.Name}'.");
                                    break;
                            }
                            break;
                    }
                }

                xmlreader.ReadEndElement();
            }
            return solution;
        }
        public Solution Serialize(string path)
        {
            throw new NotImplementedException();
        }
    }
}
