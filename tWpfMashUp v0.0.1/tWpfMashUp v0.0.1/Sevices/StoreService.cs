using System.Collections.Generic;
using System.Linq;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class StoreService
    {
        public Dictionary<string, dynamic> storeDictionary { get; set; }

        //public dynamic this[string key]
        //{
        //    get { return storeDictionary[key]; }
        //    set { storeDictionary[key] = value; }
        //}

        public void Add(string key, dynamic obj)
        {
            if(obj!=null)
            storeDictionary.Add(key, obj);
        }

        public dynamic Get(string key) => storeDictionary[key] ?? null;


        public List<string> GetAllKeys()
            => privateGetAllKeys().ToList() ?? new List<string>();

        public void Remove(string key) => storeDictionary.Remove(key);

        IEnumerable<string> privateGetAllKeys()
        {
            foreach (var item in storeDictionary)   yield return item.Key;            
        }
    }
}
