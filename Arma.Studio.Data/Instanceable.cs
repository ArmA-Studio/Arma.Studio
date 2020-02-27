using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    public abstract class Instanceable<T> where T : class, new()
    {
        public static T Instance
        {
            get
            {
                if (_Instance is null)
                {
                    _Instance = new T();
                }
                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }
        private static T _Instance;
    }
}
