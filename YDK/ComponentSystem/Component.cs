using System;
using System.Collections.Generic;
using System.Text;

namespace YDK.ComponentSystem
{
    [Serializable]
    public abstract class Component
    {
        public abstract void Update(object[] newValues);
        public abstract Dictionary<string, object> GetFields();
    }
}
