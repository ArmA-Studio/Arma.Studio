using ArmA.Studio.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio
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
        public struct PluginContainer
        {
            public IPlugin Plugin { get; }
            public Assembly Assembly { get; }
            public PluginContainer(Assembly ass, IPlugin plugin)
            {
                this.Plugin = plugin;
                this.Assembly = ass;
            }
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

        /// <summary>
        /// Loads provided plugin into the plugin list.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException">Thrown when the provided <paramref name="path"/> is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the provided DLL is not containing any implementation of <see cref="IPlugin"/> OR
        /// the corresponding implementation is not providing a valid empty constructor..</exception>
        public void LoadPlugin(string path)
        {
            if (!System.IO.File.Exists(path) || !System.IO.Path.GetExtension(path).Equals(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException(String.Empty, nameof(path));
            }
            var ass = Assembly.LoadFrom(path);
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
                    this.Plugins.Add(new PluginContainer(ass, instance));
                }
            }
            else
            {
                throw new NoPluginPresentException();
            }
        }
    }
}
