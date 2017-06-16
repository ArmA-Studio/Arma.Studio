using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ArmA.Studio.Data.UI.Commands;
using ICSharpCode.AvalonEdit.Document;
using Utility;

namespace ArmA.Studio.DataContext
{
    public class SearchDataContext : INotifyPropertyChanged
    {
        public enum ESearchMode
        {
            CurrentDocument,
            OpenDocuments,
            CurrentProject,
            EntireSolution
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callerName = "") { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName)); }

        public string FindText { get { return this._FindText; } set { this._FindText = value; this.ResetSearchVariables(); this.RaisePropertyChanged(); } }
        private string _FindText;

        public string ReplaceText { get { return this._ReplaceText; } set { this.ResetSearchVariables(); this._ReplaceText = value; this.RaisePropertyChanged(); } }
        private string _ReplaceText;

        public bool IsExpanded { get { return this._IsExpanded; } set { this._IsExpanded = value; this.RaisePropertyChanged(); } }
        private bool _IsExpanded;

        public bool IsCaseSensitive { get { return this._IsCaseSensitive; } set { this.ResetSearchVariables(); ; this._IsCaseSensitive = value; this.RaisePropertyChanged(); } }
        private bool _IsCaseSensitive;

        public bool MatchWholeWord { get { return this._MatchWholeWord; } set { this.ResetSearchVariables(); this._MatchWholeWord = value; this.RaisePropertyChanged(); } }
        private bool _MatchWholeWord;

        public bool UseRegex { get { return this._UseRegex; } set { this.ResetSearchVariables(); this._UseRegex = value; this.RaisePropertyChanged(); } }
        private bool _UseRegex;

        public IEnumerable<KeyValuePair<string, ESearchMode>> SearchModes => this._SearchModes;
        private readonly IEnumerable<KeyValuePair<string, ESearchMode>> _SearchModes;

        public object SelectedSearchModeBinding { get { return this._SelectedSearchModeBinding; } set { this._SelectedSearchModeBinding = value; this.ResetSearchVariables(); this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(this.SelectedSearchMode)); } }
        public ESearchMode SelectedSearchMode { get { return this.SelectedSearchModeBinding is ESearchMode ? (ESearchMode)this.SelectedSearchModeBinding : ESearchMode.CurrentDocument; } set { this.SelectedSearchModeBinding = value; } }
        private object _SelectedSearchModeBinding;


        private void ResetSearchVariables()
        {
            this.CurrentCollectionOffset = -1;
            this.CurrentCollectionIndex = 0;
            this.LastSearchEnumerator = null;
        }
        private IEnumerator<Tuple<Data.ProjectFile, MatchCollection>> LastSearchEnumerator;
        private int CurrentCollectionIndex;
        private int CurrentCollectionOffset;

        public ICommand CmdSearchNext => new RelayCommand((param) =>
        {
            switch (this.SelectedSearchMode)
            {
                case ESearchMode.CurrentDocument:
                    {
                        var docBase = Workspace.Instance.GetCurrentDocument();
                        if (!(docBase is Data.UI.TextEditorBaseDataContext))
                        {
                            return;
                        }
                        var doc = docBase as Data.UI.TextEditorBaseDataContext;

                        if (this.LastSearchEnumerator == null)
                        {
                            var result = this.GetMatches(doc, doc.FileReference);
                            this.LastSearchEnumerator = result.GetEnumerator();
                            this.CurrentCollectionIndex = 0;
                            this.CurrentCollectionOffset = -1;
                            if (!this.LastSearchEnumerator.MoveNext())
                            {
                                this.ResetSearchVariables();
                                System.Media.SystemSounds.Asterisk.Play();
                                break;
                            }
                        }
                    }
                    break;
                case ESearchMode.OpenDocuments:
                    {
                        var docs = Workspace.Instance.AvalonDockDocuments.Where((d) => d is Data.UI.TextEditorBaseDataContext && !d.IsTemporary).Cast<Data.UI.TextEditorBaseDataContext>();
                        var docBase = Workspace.Instance.GetCurrentDocument();
                        if (!(docBase is Data.UI.TextEditorBaseDataContext))
                        {
                            docs = docs.MoveStart(docBase as Data.UI.TextEditorBaseDataContext);
                        }

                        if (this.LastSearchEnumerator == null)
                        {
                            var result = this.GetMatches(docBase as Data.UI.TextEditorBaseDataContext, docs.Select((d) => d.FileReference));
                            this.LastSearchEnumerator = result.GetEnumerator();
                            this.CurrentCollectionIndex = 0;
                            this.CurrentCollectionOffset = -1;
                            if (!this.LastSearchEnumerator.MoveNext())
                            {
                                this.ResetSearchVariables();
                                System.Media.SystemSounds.Asterisk.Play();
                                break;
                            }
                        }
                    }
                    break;
                case ESearchMode.CurrentProject:
                    {
                        var docBase = Workspace.Instance.GetCurrentDocument();

                        if (this.LastSearchEnumerator == null)
                        {
                            var result = this.GetMatches(docBase as Data.UI.TextEditorBaseDataContext, docBase.FileReference.OwningProject);
                            this.LastSearchEnumerator = result.GetEnumerator();
                            this.CurrentCollectionIndex = 0;
                            this.CurrentCollectionOffset = -1;
                            if (!this.LastSearchEnumerator.MoveNext())
                            {
                                this.ResetSearchVariables();
                                System.Media.SystemSounds.Asterisk.Play();
                                break;
                            }
                        }
                    }
                    break;
                case ESearchMode.EntireSolution:
                    {
                        var docBase = Workspace.Instance.GetCurrentDocument();

                        if (this.LastSearchEnumerator == null)
                        {
                            var result = this.GetMatches(docBase as Data.UI.TextEditorBaseDataContext, Workspace.Instance.Solution.Projects.SelectMany((p) => p));
                            this.LastSearchEnumerator = result.GetEnumerator();
                            this.CurrentCollectionIndex = 0;
                            this.CurrentCollectionOffset = -1;
                            if (!this.LastSearchEnumerator.MoveNext())
                            {
                                this.ResetSearchVariables();
                                System.Media.SystemSounds.Asterisk.Play();
                                break;
                            }
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            var current = this.LastSearchEnumerator.Current;
            if (current.Item2.Count > this.CurrentCollectionIndex)
            {
                if (this.CurrentCollectionOffset == -1)
                {
                    var tmp = Workspace.Instance.AvalonDockDocuments.FirstOrDefault((d) => d is Data.UI.TextEditorBaseDataContext && (d as Data.UI.TextEditorBaseDataContext).FileReference == current.Item1) as Data.UI.TextEditorBaseDataContext;
                    if (tmp == null || tmp.EditorInstance == null)
                    {
                        this.CurrentCollectionOffset = 0;
                    }
                    else
                    {
                        this.CurrentCollectionOffset = current.Item2.IndexOf((it) => (it as Match).Index > tmp.EditorInstance.CaretOffset);
                        if (this.CurrentCollectionOffset == -1)
                        {
                            this.CurrentCollectionOffset = 0;
                        }
                    }
                }
                var index = this.CurrentCollectionIndex + this.CurrentCollectionOffset;
                if (index >= current.Item2.Count)
                {
                    index -= current.Item2.Count;
                }
                var match = current.Item2[index];
                this.CurrentCollectionIndex++;
                var doc = Workspace.Instance.CreateOrFocusDocument(current.Item1);
                if (doc is Data.UI.TextEditorBaseDataContext)
                {
                    (doc as Data.UI.TextEditorBaseDataContext).GetEditorInstanceAsync().ContinueWith((t) => Application.Current.Dispatcher.Invoke(() =>
                    {
                        t.Result.Select(match.Index, match.Length);
                        t.Result.ScrollToLine(t.Result.TextArea.Caret.Line);
                    }));
                }
            }
            else if (this.LastSearchEnumerator.MoveNext())
            {
                this.CurrentCollectionIndex = 0;
                this.CurrentCollectionOffset = -1;
                this.CmdSearchNext.Execute(param);
            }
            else
            {
                this.ResetSearchVariables();
                System.Media.SystemSounds.Hand.Play();
            }
        });
        public ICommand CmdClose => new RelayCommand((p) => { (p as Popup).IsOpen = false; });
        public ICommand CmdReplaceNext => new RelayCommand((param) =>
        {
            var regex = this.GetRegex(true);
            if (regex == null)
                return;
            switch (this.SelectedSearchMode)
            {
                case ESearchMode.CurrentDocument:
                    {
                        var docBase = Workspace.Instance.GetCurrentDocument();
                        if (!(docBase is Data.UI.TextEditorBaseDataContext))
                        {
                            return;
                        }
                        var doc = docBase as Data.UI.TextEditorBaseDataContext;

                        if (this.LastSearchEnumerator == null)
                        {
                            var result = this.GetMatches(doc, doc.FileReference);
                            this.LastSearchEnumerator = result.GetEnumerator();
                            this.CurrentCollectionIndex = 0;
                            this.CurrentCollectionOffset = -1;
                            if (!this.LastSearchEnumerator.MoveNext())
                            {
                                this.ResetSearchVariables();
                                System.Media.SystemSounds.Asterisk.Play();
                                break;
                            }
                        }
                    }
                    break;
                case ESearchMode.OpenDocuments:
                    {
                        var docs = Workspace.Instance.AvalonDockDocuments.Where((d) => d is Data.UI.TextEditorBaseDataContext && !d.IsTemporary).Cast<Data.UI.TextEditorBaseDataContext>();
                        var docBase = Workspace.Instance.GetCurrentDocument();
                        if (!(docBase is Data.UI.TextEditorBaseDataContext))
                        {
                            docs = docs.MoveStart(docBase as Data.UI.TextEditorBaseDataContext);
                        }

                        if (this.LastSearchEnumerator == null)
                        {
                            var result = this.GetMatches(docBase as Data.UI.TextEditorBaseDataContext, docs.Select((d) => d.FileReference));
                            this.LastSearchEnumerator = result.GetEnumerator();
                            this.CurrentCollectionIndex = 0;
                            this.CurrentCollectionOffset = -1;
                            if (!this.LastSearchEnumerator.MoveNext())
                            {
                                this.ResetSearchVariables();
                                System.Media.SystemSounds.Asterisk.Play();
                                break;
                            }
                        }
                    }
                    break;
                case ESearchMode.CurrentProject:
                    {
                        var docBase = Workspace.Instance.GetCurrentDocument();

                        if (this.LastSearchEnumerator == null)
                        {
                            var result = this.GetMatches(docBase as Data.UI.TextEditorBaseDataContext, docBase.FileReference.OwningProject);
                            this.LastSearchEnumerator = result.GetEnumerator();
                            this.CurrentCollectionIndex = 0;
                            this.CurrentCollectionOffset = -1;
                            if (!this.LastSearchEnumerator.MoveNext())
                            {
                                this.ResetSearchVariables();
                                System.Media.SystemSounds.Asterisk.Play();
                                break;
                            }
                        }
                    }
                    break;
                case ESearchMode.EntireSolution:
                    {
                        var docBase = Workspace.Instance.GetCurrentDocument();

                        if (this.LastSearchEnumerator == null)
                        {
                            var result = this.GetMatches(docBase as Data.UI.TextEditorBaseDataContext, Workspace.Instance.Solution.Projects.SelectMany((p) => p));
                            this.LastSearchEnumerator = result.GetEnumerator();
                            this.CurrentCollectionIndex = 0;
                            this.CurrentCollectionOffset = -1;
                            if (!this.LastSearchEnumerator.MoveNext())
                            {
                                this.ResetSearchVariables();
                                System.Media.SystemSounds.Asterisk.Play();
                                break;
                            }
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            var current = this.LastSearchEnumerator.Current;
            if (current.Item2.Count > this.CurrentCollectionIndex)
            {
                if (this.CurrentCollectionOffset == -1)
                {
                    var tmp = Workspace.Instance.AvalonDockDocuments.FirstOrDefault((d) => d is Data.UI.TextEditorBaseDataContext && (d as Data.UI.TextEditorBaseDataContext).FileReference == current.Item1) as Data.UI.TextEditorBaseDataContext;
                    if (tmp == null || tmp.EditorInstance == null)
                    {
                        this.CurrentCollectionOffset = 0;
                    }
                    else
                    {
                        this.CurrentCollectionOffset = current.Item2.IndexOf((it) => (it as Match).Index > tmp.EditorInstance.CaretOffset);
                        if (this.CurrentCollectionOffset == -1)
                        {
                            this.CurrentCollectionOffset = 0;
                        }
                    }
                }
                var index = this.CurrentCollectionIndex + this.CurrentCollectionOffset;
                if (index >= current.Item2.Count)
                {
                    index = this.CurrentCollectionIndex - this.CurrentCollectionOffset;
                }
                var match = current.Item2[index];
                this.CurrentCollectionIndex++;
                var doc = Workspace.Instance.CreateOrFocusDocument(current.Item1);
                if (doc is Data.UI.TextEditorBaseDataContext)
                {
                    var tebdc = doc as Data.UI.TextEditorBaseDataContext;
                    (doc as Data.UI.TextEditorBaseDataContext).GetEditorInstanceAsync().ContinueWith((t) => Application.Current.Dispatcher.Invoke(() =>
                    {
                        var str = regex.Replace(tebdc.Document.GetText(match.Index, match.Length), this.ReplaceText);
                        tebdc.Document.Replace(match.Index, match.Length, str);
                        t.Result.Select(match.Index, str.Length);
                        t.Result.ScrollToLine(t.Result.TextArea.Caret.Line);
                    }));
                }
            }
            else if (this.LastSearchEnumerator.MoveNext())
            {
                this.CurrentCollectionIndex = 0;
                this.CurrentCollectionOffset = -1;
                this.CmdReplaceNext.Execute(param);
            }
            else
            {
                System.Media.SystemSounds.Hand.Play();
            }
            this.ResetSearchVariables();
        });
        public ICommand CmdReplaceAll => new RelayCommand((param) =>
        {
            var regex = this.GetRegex(true);
            if (regex == null)
                return;
            IEnumerable<Tuple<Data.ProjectFile, MatchCollection>> stuff;
            switch (this.SelectedSearchMode)
            {
                case ESearchMode.CurrentDocument:
                    {
                        var docBase = Workspace.Instance.GetCurrentDocument();
                        if (!(docBase is Data.UI.TextEditorBaseDataContext))
                        {
                            return;
                        }
                        var doc = docBase as Data.UI.TextEditorBaseDataContext;
                        stuff = this.GetMatches(doc, doc.FileReference);
                    }
                    break;
                case ESearchMode.OpenDocuments:
                    {
                        var docs = Workspace.Instance.AvalonDockDocuments.Where((d) => d is Data.UI.TextEditorBaseDataContext && !d.IsTemporary).Cast<Data.UI.TextEditorBaseDataContext>();
                        var docBase = Workspace.Instance.GetCurrentDocument();
                        if (!(docBase is Data.UI.TextEditorBaseDataContext))
                        {
                            docs = docs.MoveStart(docBase as Data.UI.TextEditorBaseDataContext);
                        }
                        stuff = this.GetMatches(docBase as Data.UI.TextEditorBaseDataContext, docs.Select((d) => d.FileReference));
                    }
                    break;
                case ESearchMode.CurrentProject:
                    {
                        var docBase = Workspace.Instance.GetCurrentDocument();
                        stuff = this.GetMatches(docBase as Data.UI.TextEditorBaseDataContext, docBase.FileReference.OwningProject);
                    }
                    break;
                case ESearchMode.EntireSolution:
                    {
                        var docBase = Workspace.Instance.GetCurrentDocument();
                        stuff = this.GetMatches(docBase as Data.UI.TextEditorBaseDataContext, Workspace.Instance.Solution.Projects.SelectMany((p) => p));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            foreach (var it in stuff)
            {
                var str = it.Item1.GetContentAsString();
                var repl = regex.Replace(str, this.ReplaceText);
                it.Item1.SetContentAsString(repl);
            }
        });

        public ICommand CmdKeyDownFindText => new RelayCommand((p) =>
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                this.KeyDownFindTextHandled = true;
                this.CmdSearchNext.Execute(p);
            }
        });
        public ICommand CmdKeyDownReplaceText => new RelayCommand((p) =>
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                this.KeyDownReplaceTextHandled = true;
                this.CmdReplaceNext.Execute(p);
            }
        });

        public bool KeyDownFindTextHandled { get { return this._KeyDownFindTextHandled; } set { this._KeyDownFindTextHandled = value; this.RaisePropertyChanged(); } }
        private bool _KeyDownFindTextHandled;
        public bool KeyDownReplaceTextHandled { get { return this._KeyDownReplaceTextHandled; } set { this._KeyDownReplaceTextHandled = value; this.RaisePropertyChanged(); } }
        private bool _KeyDownReplaceTextHandled;

        public SearchDataContext()
        {
            this._SearchModes = new KeyValuePair<string, ESearchMode>[]
            {
                new KeyValuePair<string, ESearchMode>(Properties.Localization.SearchModeCurrentDocument, ESearchMode.CurrentDocument),
                new KeyValuePair<string, ESearchMode>(Properties.Localization.SearchModeOpenDocuments, ESearchMode.OpenDocuments),
                new KeyValuePair<string, ESearchMode>(Properties.Localization.SearchModeCurrentProject, ESearchMode.CurrentProject),
                new KeyValuePair<string, ESearchMode>(Properties.Localization.SearchModeEntireSolution, ESearchMode.EntireSolution)
            };
            this.SelectedSearchMode = ESearchMode.CurrentDocument;
        }

        private Regex GetRegex(bool testReplace = false)
        {
            var txt = this.FindText;
            if (!this.UseRegex)
            {
                txt = Regex.Escape(txt);
            }
            RegexOptions options = RegexOptions.None;
            if (!this.IsCaseSensitive)
            {
                options |= RegexOptions.IgnoreCase;
            }
            if (this.MatchWholeWord)
            {
                txt = $@"\b({txt})";
            }

            try
            {
                var regex = new Regex(txt, options);
                if (testReplace)
                {
                    regex.Replace(string.Empty, this.ReplaceText);
                }
                return regex;
            }
            catch (Exception ex)
            {
                App.ShowOperationFailedMessageBox(ex);
                return null;
            }
        }

        private IEnumerable<Tuple<Data.ProjectFile, MatchCollection>> GetMatches(Data.UI.TextEditorBaseDataContext curEditor, params Data.ProjectFile[] files) => this.GetMatches(curEditor, (IEnumerable<Data.ProjectFile>)files);
        private IEnumerable<Tuple<Data.ProjectFile, MatchCollection>> GetMatches(Data.UI.TextEditorBaseDataContext curEditor, IEnumerable<Data.ProjectFile> files)
        {
            var regex = this.GetRegex();
            if (regex == null)
                yield break;
            if (curEditor != null)
            {
                yield return new Tuple<Data.ProjectFile, MatchCollection>(curEditor.FileReference, regex.Matches(curEditor.FileReference.GetContentAsString()));
            }
            foreach (var it in files)
            {
                if (curEditor != null && curEditor.FileReference == it)
                {
                    continue;
                }
                var str = it.GetContentAsString();
                if (str.Contains('\0'))
                    continue;
                yield return new Tuple<Data.ProjectFile, MatchCollection>(it, regex.Matches(str));
            }
        }
    }
}