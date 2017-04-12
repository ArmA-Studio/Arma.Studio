using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmA.Studio.Data;

namespace ArmA.Studio
{
    public sealed class KeyManager : IKeyManager
    {
        private List<ASKeyContainer> KeyContainers;

        public KeyManager()
        {
            this.KeyContainers = new List<ASKeyContainer>();
        }

        public void RegisterKey(ASKeyContainer keyContainer)
        {
            this.KeyContainers.Add(keyContainer);
        }
        public bool CheckKeys()
        {
            var flag = false;
            foreach (var cont in this.KeyContainers)
            {
                if (cont.IsPressed())
                {
                    flag = true;
                    cont.Call();
                }
            }
            return flag;
        }
    }
}