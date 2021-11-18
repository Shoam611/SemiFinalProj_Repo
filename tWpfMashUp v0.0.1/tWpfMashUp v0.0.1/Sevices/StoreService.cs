using System;
using System.Collections.Generic;
using System.Linq;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class StoreService
    {
        private readonly Dictionary<string, dynamic> storeDictionary;

        public event EventHandler CurrentContactChanged;

        public StoreService()
        {
            storeDictionary = new Dictionary<string, dynamic>();
        }
        public void Add(string key, dynamic obj)
        {
            if (obj == null) return;
            if (!storeDictionary.TryGetValue(key, out _))
            {
                storeDictionary.Add(key, obj);
            }
            else { storeDictionary[key] = obj; }
        }

        public dynamic Get(string key) => storeDictionary.TryGetValue(key, out var val) ? val : null;

        public void InformContactChanged(object source, EventArgs selectionChangedEventArgs) => CurrentContactChanged?.Invoke(source, selectionChangedEventArgs);

        public bool HasKey(string key) => storeDictionary.ContainsKey(key);

        public List<string> GetAllKeys() => PrivateGetAllKeys().ToList() ?? new List<string>();

        public void Remove(string key) => storeDictionary.Remove(key);

        private IEnumerable<string> PrivateGetAllKeys()
        {
            foreach (var item in storeDictionary) yield return item.Key;
        }
    }
}

