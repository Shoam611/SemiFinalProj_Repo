using System.Collections.Generic;
using System.Linq;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class StoreService
    {
        public Dictionary<string, dynamic> StoreDictionary { get; set; }

        public StoreService()
        {
            StoreDictionary = new Dictionary<string, dynamic>();
        }
        public void Add(string key, dynamic obj)
        {
            if (obj == null) return;
            if (!StoreDictionary.TryGetValue(key, out var val))
                StoreDictionary.Add(key, obj);
            else StoreDictionary[key] = obj;
        }

        public dynamic Get(string key) => StoreDictionary[key] ?? null;

        public List<string> GetAllKeys() => PrivateGetAllKeys().ToList() ?? new List<string>();

        public void Remove(string key) => StoreDictionary.Remove(key);

        IEnumerable<string> PrivateGetAllKeys()
        {
            foreach (var item in StoreDictionary) yield return item.Key;
        }
    }
}

//public dynamic this[string key]
//{
//    get { return storeDictionary[key]; }
//    set { storeDictionary[key] = value; }
//}
