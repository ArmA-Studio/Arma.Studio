using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using ArmA.Studio.Data;
using System.Xml;

namespace ArmA.Studio
{
    public sealed class BreakpointManager : IEnumerable<BreakpointInfo>
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public sealed class BreakPointsChangedEventArgs
        {
            public enum EMode
            {
                Add,
                Remove,
                Update,
                DrasticChange
            }
            public readonly EMode Mode;
            public readonly BreakpointInfo Breakpoint;
            public BreakPointsChangedEventArgs(EMode mode, BreakpointInfo bp)
            {
                this.Mode = mode;
                this.Breakpoint = bp;
            }
        }

        public event EventHandler<BreakPointsChangedEventArgs> OnBreakPointsChanged;

        private Dictionary<ProjectFile, List<BreakpointInfo>> BreakPointDictionary;

        public BreakpointManager()
        {
            this.BreakPointDictionary = new Dictionary<ProjectFile, List<BreakpointInfo>>();
        }

        public IEnumerable<BreakpointInfo> this[ProjectFile pff]
        {
            get { return this.BreakPointDictionary[pff]; }
        }

        public BreakpointInfo SetBreakpoint(ProjectFile pff, int line) => this.SetBreakpoint(pff, new BreakpointInfo() { FileFolder = pff, IsEnabled = true, Line = line, SqfCondition = null});
        public BreakpointInfo SetBreakpoint(ProjectFile pff, BreakpointInfo bpi)
        {
            Logger.Info($"Setting breakpoint {bpi.ToString()} in '{pff.ProjectRelativePath}'.");
            bpi.FileFolder = pff;
            List<BreakpointInfo> bpiList;
            if(!this.BreakPointDictionary.TryGetValue(pff, out bpiList))
            {
                bpiList = new List<BreakpointInfo>();
                this.BreakPointDictionary[pff] = bpiList;
            }
            var index = bpiList.FindIndex((item) => item.Line == bpi.Line);
            var isUpdate = false;
            if (index == -1)
            {
                bpiList.Add(bpi);
            }
            else
            {
                bpiList[index] = bpi;
                isUpdate = true;
            }
            this.OnBreakPointsChanged?.Invoke(this, new BreakPointsChangedEventArgs(isUpdate ? BreakPointsChangedEventArgs.EMode.Update : BreakPointsChangedEventArgs.EMode.Add, bpi));
            return bpi;
        }
        public BreakpointInfo GetBreakpoint(ProjectFile pff, int line)
        {
            List<BreakpointInfo> bpiList;
            if (!this.BreakPointDictionary.TryGetValue(pff, out bpiList))
            {
                bpiList = new List<BreakpointInfo>();
                this.BreakPointDictionary[pff] = bpiList;
            }
            var index = bpiList.FindIndex((item) => item.Line == line);
            if (index == -1)
            {
                return default(BreakpointInfo);
            }
            else
            {
                return bpiList[index];
            }
        }
        public void RemoveBreakpoint(ProjectFile pff, BreakpointInfo bpi) => this.RemoveBreakpoint(pff, bpi.Line);
        public void RemoveBreakpoint(ProjectFile pff, int line)
        {
            var bpiList = this.BreakPointDictionary[pff];
            var index = bpiList.FindIndex((item) => item.Line == line);
            if (index != -1)
            {
                var bpi = bpiList[index];
                Logger.Info($"Removing breakpoint {bpi.ToString()} in '{pff.ProjectRelativePath}'.");
                bpiList.RemoveAt(index);
                this.OnBreakPointsChanged?.Invoke(this, new BreakPointsChangedEventArgs(BreakPointsChangedEventArgs.EMode.Remove, bpi));
            }
        }

        public IEnumerator<BreakpointInfo> GetEnumerator()
        {
            return this.BreakPointDictionary.SelectMany((kvp) => kvp.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator().Cast();
        }

        public void LoadBreakpoints(System.IO.Stream stream)
        {
            var reader = XmlReader.Create(stream);
            ProjectFile currentFile = null;
            while (reader.Read())
            {
                try
                {
                    if (reader.Name.Equals("breakpointinfo") && currentFile != null)
                    {
                        var bpi = new BreakpointInfo();
                        bpi.FileFolder = currentFile;
                        if (reader.MoveToFirstAttribute())
                        {
                            do
                            {
                                switch (reader.Name)
                                {
                                    case nameof(BreakpointInfo.IsEnabled):
                                        bpi.IsEnabled = reader.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
                                        break;
                                    case nameof(BreakpointInfo.Line):
                                        bpi.Line = int.Parse(reader.Value);
                                        break;
                                    case nameof(BreakpointInfo.SqfCondition):
                                        bpi.SqfCondition = reader.Value;
                                        break;
                                }
                            } while (reader.MoveToNextAttribute());
                        }
                        
                    }
                    else if(reader.Name.Equals("file"))
                    {
                        currentFile = Workspace.Instance.GetProjectFileFolderReference(new Uri(reader.Value, UriKind.RelativeOrAbsolute));
                    }
                }
                catch { }
            }

            this.OnBreakPointsChanged?.Invoke(this, new BreakPointsChangedEventArgs(BreakPointsChangedEventArgs.EMode.DrasticChange, default(BreakpointInfo)));
        }
        public void SaveBreakpoints(System.IO.Stream stream)
        {
            var writer = XmlWriter.Create(stream);

            foreach (var kvp in this.BreakPointDictionary)
            {
                if (!kvp.Value.Any())
                    continue;
                writer.WriteStartElement("file");
                writer.WriteAttributeString("path", kvp.Key.FilePath);

                foreach(var bpi in kvp.Value)
                {
                    writer.WriteStartElement("breakpointinfo");
                    writer.WriteAttributeString(nameof(bpi.IsEnabled), bpi.IsEnabled.ToString());
                    writer.WriteAttributeString(nameof(bpi.Line), bpi.Line.ToString());
                    if (bpi.SqfCondition != null)
                    {
                        writer.WriteAttributeString(nameof(bpi.SqfCondition), bpi.SqfCondition);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            stream.Flush();
        }
    }
}
