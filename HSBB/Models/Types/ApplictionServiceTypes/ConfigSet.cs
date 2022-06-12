using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class ConfigSet : ICollection<ConfigType>
    {
        ICollection<ConfigType> configTypes = new List<ConfigType>();

        public bool IsReadOnly => configTypes.IsReadOnly;

        public int Count => configTypes.Count;

        public string this[string index]
        {
            set { configTypes.FirstOrDefault(x => x.ConfigCode == index).ConfigValue = value; }
            get { return configTypes.FirstOrDefault(x => x.ConfigCode == index).ConfigValue; }
        }

        public void Add(ConfigType item)
        {
            configTypes.Add(item);
        }

        public void Clear()
        {
            configTypes.Clear();
        }

        public bool Contains(ConfigType item)
        {
            return configTypes.Contains(item);
        }

        public void CopyTo(ConfigType[] array, int arrayIndex)
        {
            configTypes.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ConfigType> GetEnumerator()
        {
            return configTypes.GetEnumerator();
        }

        public bool Remove(ConfigType item)
        {
            return configTypes.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)configTypes).GetEnumerator();
        }
    }
}