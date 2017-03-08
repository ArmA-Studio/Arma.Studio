using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using IniParser;
using IniParser.Model;
using IniParser.Parser;
using Utility;

namespace ArmA.Studio
{
    public sealed class ConfigHost
    {
        public enum EIniSelector
        {
            App,
            Coloring,
            Layout
        }
        public static class App
        {
            public static WindowState WindowCurrentState
            {
                get { WindowState state; if (Enum.TryParse(Instance.AppIni.GetValueOrNull(nameof(MainWindow), nameof(MainWindow.WindowState)), out state)) return state; return WindowState.Normal; }
                set { Instance.AppIni.SetValue(nameof(MainWindow), nameof(MainWindow.WindowState), value.ToString()); Instance.Save(EIniSelector.App); }
            }
            public static double WindowHeight
            {
                get { double d; if (double.TryParse(Instance.AppIni.GetValueOrNull(nameof(MainWindow), nameof(MainWindow.Height)), out d)) return d; return -1; }
                set { Instance.AppIni.SetValue(nameof(MainWindow), nameof(MainWindow.Height), value.ToString(CultureInfo.InvariantCulture)); Instance.Save(EIniSelector.App); }
            }
            public static double WindowWidth
            {
                get { double d; if (double.TryParse(Instance.AppIni.GetValueOrNull(nameof(MainWindow), nameof(MainWindow.Width)), out d)) return d; return -1;  }
                set { Instance.AppIni.SetValue(nameof(MainWindow), nameof(MainWindow.Width), value.ToString(CultureInfo.InvariantCulture)); Instance.Save(EIniSelector.App); }
            }
            public static double WindowTop
            {
                get { double d; if (double.TryParse(Instance.AppIni.GetValueOrNull(nameof(MainWindow), nameof(MainWindow.Top)), out d)) return d; return -1; }
                set { Instance.AppIni.SetValue(nameof(MainWindow), nameof(MainWindow.Top), value.ToString(CultureInfo.InvariantCulture)); Instance.Save(EIniSelector.App); }
            }
            public static double WindowLeft
            {
                get { double d; if (double.TryParse(Instance.AppIni.GetValueOrNull(nameof(MainWindow), nameof(MainWindow.Left)), out d)) return d; return -1;  }
                set { Instance.AppIni.SetValue(nameof(MainWindow), nameof(MainWindow.Left), value.ToString(CultureInfo.InvariantCulture)); Instance.Save(EIniSelector.App); }
            }

            public static string WorkspacePath
            {
                get { return Instance.AppIni.GetValueOrNull(nameof(App), nameof(Workspace)); }
                set { Instance.AppIni.SetValue(nameof(App), nameof(Workspace), value); Instance.Save(EIniSelector.App); }
            }

            public static bool ErrorList_IsErrorsDisplayed
            {
                get { bool val; if (bool.TryParse(Instance.AppIni.GetValueOrNull(nameof(DataContext.ErrorListPane), nameof(DataContext.ErrorListPane.IsErrorsDisplayed)), out val)) return val; return true; }
                set { Instance.AppIni.SetValue(nameof(DataContext.ErrorListPane), nameof(DataContext.ErrorListPane.IsErrorsDisplayed), value.ToString(CultureInfo.InvariantCulture)); Instance.Save(EIniSelector.App); }
            }
            public static bool ErrorList_IsWarningsDisplayed
            {
                get { bool val; if (bool.TryParse(Instance.AppIni.GetValueOrNull(nameof(DataContext.ErrorListPane), nameof(DataContext.ErrorListPane.IsWarningsDisplayed)), out val)) return val; return true; }
                set { Instance.AppIni.SetValue(nameof(DataContext.ErrorListPane), nameof(DataContext.ErrorListPane.IsWarningsDisplayed), value.ToString(CultureInfo.InvariantCulture)); Instance.Save(EIniSelector.App); }
            }
            public static bool ErrorList_IsInfosDisplayed
            {
                get { bool val; if (bool.TryParse(Instance.AppIni.GetValueOrNull(nameof(DataContext.ErrorListPane), nameof(DataContext.ErrorListPane.IsInfosDisplayed)), out val)) return val; return true; }
                set { Instance.AppIni.SetValue(nameof(DataContext.ErrorListPane), nameof(DataContext.ErrorListPane.IsInfosDisplayed), value.ToString(CultureInfo.InvariantCulture)); Instance.Save(EIniSelector.App); }
            }
        }
        public static class Coloring
        {
            public static void Reset(bool IsHard)
            {
                var coloringType = typeof(Coloring);
                var list = new List<Type>(coloringType.GetNestedTypes());
                list.Add(coloringType);
                foreach(var it in list)
                {
                    var properties = from prop in it.GetProperties() where prop.PropertyType.IsEquivalentTo(typeof(Color)) select prop;
                    foreach(var prop in properties)
                    {
                        var value = (Color)prop.GetMethod.Invoke(null, null);
                        if(IsHard || value.Equals(Colors.Transparent))
                        {
                            var fields = from field in it.GetFields() where field.FieldType.IsEquivalentTo(typeof(Color)) select field;
                            var f = fields.FirstOrDefault((field) => field.Name.EndsWith("_Default") && field.Name.StartsWith(prop.Name));
                            if (f == null)
                                throw new NotImplementedException();
                            prop.SetMethod.Invoke(null, new object[] { f.GetValue(null) });
                        }
                    }
                }

            }
            public static Color ColorParse(string inputs)
            {
                if (string.IsNullOrWhiteSpace(inputs))
                    return Colors.Transparent;
                var splitInputs = inputs.Split(',');
                if (splitInputs.Length != 4)
                    return Colors.Transparent;
                foreach (var input in splitInputs)
                {
                    if (!input.IsInteger())
                    {
                        return Colors.Transparent;
                    }
                }
                return Color.FromArgb(byte.Parse(splitInputs[0]), byte.Parse(splitInputs[1]), byte.Parse(splitInputs[2]), byte.Parse(splitInputs[3]));
            }
            public static string ColorParse(Color colorInput)
            {
                return string.Join(",", colorInput.A, colorInput.R, colorInput.G, colorInput.B);
            }

            public static class EditorUnderlining
            {
                public static readonly Color ErrorColor_Default = Color.FromArgb(255, 255, 0, 0);
                public static Color ErrorColor
                {
                    get { return ColorParse(Instance.ColoringIni.GetValueOrNull(nameof(EditorUnderlining), nameof(ErrorColor))); }
                    set { Instance.ColoringIni.SetValue(nameof(EditorUnderlining), nameof(ErrorColor), ColorParse(value)); Instance.Save(EIniSelector.Coloring); }
                }

                public static readonly Color WarningColor_Default = Color.FromArgb(255, 255, 153, 0);
                public static Color WarningColor
                {
                    get { return ColorParse(Instance.ColoringIni.GetValueOrNull(nameof(EditorUnderlining), nameof(WarningColor))); }
                    set { Instance.ColoringIni.SetValue(nameof(EditorUnderlining), nameof(WarningColor), ColorParse(value)); Instance.Save(EIniSelector.Coloring); }
                }

                public static readonly Color InfoColor_Default = Color.FromArgb(255, 0, 255, 0);
                public static Color InfoColor
                {
                    get { return ColorParse(Instance.ColoringIni.GetValueOrNull(nameof(EditorUnderlining), nameof(InfoColor))); }
                    set { Instance.ColoringIni.SetValue(nameof(EditorUnderlining), nameof(InfoColor), ColorParse(value)); Instance.Save(EIniSelector.Coloring); }
                }
            }
            
            public static class SelectedLine
            {
                public static readonly Color Background_Default = Color.FromArgb(16, 0, 0, 0);
                public static Color Background
                {
                    get { return ColorParse(Instance.ColoringIni.GetValueOrNull(nameof(SelectedLine), nameof(Background))); }
                    set { Instance.ColoringIni.SetValue(nameof(SelectedLine), nameof(Background), ColorParse(value)); Instance.Save(EIniSelector.Coloring); }
                }

                public static readonly Color Border_Default = Color.FromArgb(32, 0, 0, 0);
                public static Color Border
                {
                    get { return ColorParse(Instance.ColoringIni.GetValueOrNull(nameof(SelectedLine), nameof(Border))); }
                    set { Instance.ColoringIni.SetValue(nameof(SelectedLine), nameof(Border), ColorParse(value)); Instance.Save(EIniSelector.Coloring); }
                }
            }

            public static class BreakPoint
            {
                public static readonly Color MainColor_Default = Color.FromArgb(255, 255, 0, 0);
                public static Color MainColor
                {
                    get { return ColorParse(Instance.ColoringIni.GetValueOrNull(nameof(BreakPoint), nameof(MainColor))); }
                    set { Instance.ColoringIni.SetValue(nameof(BreakPoint), nameof(MainColor), ColorParse(value)); Instance.Save(EIniSelector.Coloring); }
                }

                public static readonly Color BorderColor_Default = Color.FromArgb(255, 255, 255, 255);
                public static Color BorderColor
                {
                    get { return ColorParse(Instance.ColoringIni.GetValueOrNull(nameof(BreakPoint), nameof(BorderColor))); }
                    set { Instance.ColoringIni.SetValue(nameof(BreakPoint), nameof(BorderColor), ColorParse(value)); Instance.Save(EIniSelector.Coloring); }
                }

                public static readonly Color TextHighlightColor_Default = Color.FromArgb(32, 200, 0, 0);
                public static Color TextHighlightColor
                {
                    get { return ColorParse(Instance.ColoringIni.GetValueOrNull(nameof(BreakPoint), nameof(TextHighlightColor))); }
                    set { Instance.ColoringIni.SetValue(nameof(BreakPoint), nameof(TextHighlightColor), ColorParse(value)); Instance.Save(EIniSelector.Coloring); }
                }
            }


        }
        public static ConfigHost Instance { get; private set; }
        static ConfigHost()
        {
            Instance = new ConfigHost();
            Coloring.Reset(false);
        }

        public IniData LayoutIni { get; private set; }
        public IniData AppIni { get; private set; }
        public IniData ColoringIni { get; private set; }
        public IEnumerable<RealVirtuality.SQF.SqfDefinition> SqfDefinitions { get; private set; }

        private Dictionary<EIniSelector, bool> SaveTriggers;


        public ConfigHost()
        {
            this.SaveTriggers = new Dictionary<EIniSelector, bool>();
            string fPath;
            fPath = Path.Combine(Studio.App.ConfigPath, "Layout.ini");
            if (File.Exists(fPath))
            {
                var parser = new FileIniDataParser();
                this.LayoutIni = parser.ReadFile(fPath);
            }
            else
            {
                this.LayoutIni = new IniData();
            }
            fPath = Path.Combine(Studio.App.ConfigPath, "App.ini");
            if (File.Exists(fPath))
            {
                var parser = new FileIniDataParser();
                this.AppIni = parser.ReadFile(fPath);
            }
            else
            {
                this.AppIni = new IniData();
            }
            fPath = Path.Combine(Studio.App.ConfigPath, "Coloring.ini");
            if (File.Exists(fPath))
            {
                var parser = new FileIniDataParser();
                this.ColoringIni = parser.ReadFile(fPath);
            }
            else
            {
                this.ColoringIni = new IniData();
            }
            fPath = Path.Combine(Studio.App.ExecutablePath, "SqfDefinition.xml");
            if (File.Exists(fPath))
            {
                this.SqfDefinitions = fPath.XmlDeserialize<List<RealVirtuality.SQF.SqfDefinition>>();
            }
            else
            {
                this.SqfDefinitions = new List<RealVirtuality.SQF.SqfDefinition>();
            }
        }
        public void SaveAll()
        {
            foreach(EIniSelector sel in Enum.GetValues(typeof(EIniSelector)))
            {
                this.Save(sel);
            }
            this.ExecSave();
        }

        public void Save(EIniSelector selector)
        {
            if (!Directory.Exists(Studio.App.ConfigPath))
            {
                Directory.CreateDirectory(Studio.App.ConfigPath);
            }
            this.SaveTriggers[selector] = true;
        }
        public void ExecSave()
        {
            var parser = new FileIniDataParser();
            foreach (var pair in this.SaveTriggers)
            {
                if(pair.Value)
                {
                    switch (pair.Key)
                    {
                        case EIniSelector.App:
                            parser.WriteFile(Path.Combine(Studio.App.ConfigPath, "App.ini"), this.AppIni);
                            break;
                        case EIniSelector.Coloring:
                            parser.WriteFile(Path.Combine(Studio.App.ConfigPath, "Coloring.ini"), this.ColoringIni);
                            break;
                        case EIniSelector.Layout:
                            parser.WriteFile(Path.Combine(Studio.App.ConfigPath, "Layout.ini"), this.LayoutIni);
                            break;
                    }
                }
            }
        }
    }
}
