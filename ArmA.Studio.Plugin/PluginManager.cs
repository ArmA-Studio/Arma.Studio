using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArmA.Studio.Plugin
{
    public class PluginManager<T> where T : class
    {
        /// <summary>
        /// Required for separated assembly load.
        /// Will prevent interruption in current assembly domain.
        /// </summary>
        private class AssemblyLoadMarshal : MarshalByRefObject
        {
            public int Find(string path)
            {
                var assembly = Assembly.LoadFrom(path);
                var TList = new List<T>();
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(T).IsAssignableFrom(type))
                        {
                            TList.Add((T)Activator.CreateInstance(type));
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    //ToDo: Resolve into better exceptions eg. OutdatedTException, MissingAssemblyException, ...
                    throw ex;
                }
                return TList.Count;
            }
        }

        private List<T> _Ts;
        public IEnumerable<T> Plugins => this._Ts;

        public PluginManager()
        {
            this._Ts = new List<T>();
        }
        /// <summary>
        /// Checks provided Assembly DLL pathes for the type <typeparamref name="T"/> and adds those to the Plugins property.
        /// </summary>
        /// <param name="pathes"><see cref="IEnumerable{S}"/> where <typeparamref name="S"/> is of type <see cref="string"/> containing valid pathes to .net DLLs.</param>
        /// <param name="progress">Progress reporter or null.</param>
        /// <param name="exHandler">Exception handler function or null.</param>
        public void LoadPlugins(IEnumerable<string> pathes, IProgress<double> progress = null, Func<Exception, bool> exHandler = null)
        {
            progress?.Report(0);
            var domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory });
            var index = 0;
            this._Ts = new List<T>();
            foreach (var assemblyPath in pathes)
            {
                try
                {
                    progress?.Report(index / pathes.Count());
                    var marshal = (AssemblyLoadMarshal)domain.CreateInstanceAndUnwrap(typeof(AssemblyLoadMarshal).Assembly.FullName, typeof(AssemblyLoadMarshal).FullName);

                    if (marshal.Find(assemblyPath) > 0)
                    {
                        var assembly = Assembly.LoadFrom(assemblyPath);
                        foreach (var type in assembly.GetTypes())
                        {
                            if (typeof(T).IsAssignableFrom(type))
                            {
                                var mod = (T)Activator.CreateInstance(type);
                                this._Ts.Add(mod);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var flag = exHandler?.Invoke(ex);
                    if (!flag.HasValue || !flag.Value)
                    {
                        throw ex;
                    }
                }
                index++;
            }
            progress?.Report(1);
            AppDomain.Unload(domain);
        }
    }
}
