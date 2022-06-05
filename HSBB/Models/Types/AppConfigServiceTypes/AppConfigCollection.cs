using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class AppConfigSet : ICollection<AppConfigType>
    {
        ICollection<AppConfigType> AppConfigTypes = new List<AppConfigType>();

        public bool IsReadOnly => AppConfigTypes.IsReadOnly;

        public int Count => AppConfigTypes.Count;

        public string this[string index]
        {
            get { return AppConfigTypes.FirstOrDefault(x => x.AppConfigCode == index).AppConfigValue; }
        }

        public void Add(AppConfigType item)
        {
            AppConfigTypes.Add(item);
        }

        public void Clear()
        {
            AppConfigTypes.Clear();
        }

        public bool Contains(AppConfigType item)
        {
            return AppConfigTypes.Contains(item);
        }

        public void CopyTo(AppConfigType[] array, int arrayIndex)
        {
            AppConfigTypes.CopyTo(array, arrayIndex);
        }

        public IEnumerator<AppConfigType> GetEnumerator()
        {
            return AppConfigTypes.GetEnumerator();
        }

        public bool Remove(AppConfigType item)
        {
            return AppConfigTypes.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)AppConfigTypes).GetEnumerator();
        }
    }
}