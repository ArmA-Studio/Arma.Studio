using Arma.Studio.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Arma.Studio
{
    /// <summary>
    /// Singleton class.
    /// Allows loading of plugins.
    /// </summary>
    public class PluginManager
    {
        public class NoPluginPresentException : InvalidOperationException
        {

        }
        [XmlRoot("plugin")]
        public class PluginFile
        {
            [XmlElement("library")]
            public string Library { get; set; }
        }
        public struct PluginContainer
        {
            private readonly IPlugin ActualPlugin;
            public Assembly Assembly { get; }
            public PluginFile InfoFile { get; }
            public string Folder { get; }
            public PluginContainer(Assembly ass, PluginFile infoFile, IPlugin plugin, string folder)
            {
                this.ActualPlugin = plugin;
                this.Assembly = ass;
                this.InfoFile = infoFile;
                this.Folder = folder;
            }

            public IPlugin Plugin() => this.ActualPlugin;
            public T Plugin<T>() where T : class => this.ActualPlugin as T;
        }
        static PluginManager()
        {
            Instance = new PluginManager();
        }

        public static PluginManager Instance { get; }
        private PluginManager()
        {
            this.Plugins = new ObservableCollection<PluginContainer>();
        }

        public ObservableCollection<PluginContainer> Plugins { get; }
        public IEnumerable<T> GetPlugins<T>() where T : class => this.Plugins.Where((it) => it.Plugin() is T).Select((it) => it.Plugin<T>());

        public Assembly LoadAssemblySafe(string path)
        {
            var folder = Path.GetDirectoryName(path);
            var versionInfo = FileVersionInfo.GetVersionInfo(path);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var it in assemblies)
            {
                if (it.IsDynamic)
                {
                    continue;
                }
                try
                {
                    var versioninfoOther = FileVersionInfo.GetVersionInfo(it.Location);
                    if (versioninfoOther.FileVersion == versionInfo.FileVersion &&
                        versioninfoOther.FileDescription == versionInfo.FileDescription &&
                        versioninfoOther.Comments == versionInfo.Comments)
                    {
                        return it;
                    }
                }
                catch
                {
                    continue;
                }
            }
            return Assembly.LoadFrom(path);
        }

        /// <summary>
        /// Loads provided plugin into the plugin list.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException">Thrown when the provided <paramref name="path"/> is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the provided DLL is not containing any implementation of <see cref="IPlugin"/> OR
        /// the corresponding implementation is not providing a valid empty constructor..</exception>
        private IEnumerable<IPlugin> LoadPlugin(PluginFile file, string path)
        {
            if (!System.IO.Path.GetExtension(path).Equals(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException(String.Empty, nameof(path));
            }
            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException(String.Empty, nameof(path));
            }
            var folder = Path.GetDirectoryName(path);
            var files = Directory.GetFiles(folder, "*.dll", SearchOption.TopDirectoryOnly);
            foreach(var f in files)
            {
                if (f == path)
                {
                    continue;
                }
                LoadAssemblySafe(f);
            }
            
            var ass = LoadAssemblySafe(path);
            var plugins = ass.GetTypes().Where((t) => typeof(IPlugin).IsAssignableFrom(t));
            if (plugins.Any())
            {
                foreach (var it in plugins)
                {
                    var constructors = it.GetConstructors().Where((c) => !c.GetParameters().Any() && !c.IsGenericMethod);
                    if (!constructors.Any())
                    {
                        throw new InvalidOperationException();
                    }
                    var instance = constructors.First().Invoke(new object[0]) as IPlugin;
                    this.Plugins.Add(new PluginContainer(ass, file, instance, folder));
                    yield return instance;
                }
            }
            else
            {
                throw new NoPluginPresentException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">The plugin folder</param>
        /// <returns></returns>
        public IEnumerable<IPlugin> LoadPlugin(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }
            var pluginFile = Path.Combine(path, "plugin.xml");
            if (!File.Exists(pluginFile))
            {
                throw new FileNotFoundException(path);
            }
            using (var fstream = File.OpenRead(pluginFile))
            {
                var serializer = new XmlSerializer(typeof(PluginFile));
                var file = serializer.Deserialize(fstream) as PluginFile;
                return LoadPlugin(file, Path.Combine(path, file.Library)).ToArray();
            }
        }
    }
}
