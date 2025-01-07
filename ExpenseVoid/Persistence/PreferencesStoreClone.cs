using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ExpenseVoid.Persistence
{
    public class PreferencesStoreClone
    {
        public void Set(string key, object value)
        {
            string keyvalue = JsonSerializer.Serialize(value);
            if (keyvalue != null && !string.IsNullOrEmpty(keyvalue))
            {
                Preferences.Set(key, keyvalue);
            }
        }

        public T Get<T>(string key)
        {
            T UnpackedValue = default;
            string keyvalue = Preferences.Get(key, string.Empty);

            if (keyvalue != null && !string.IsNullOrEmpty(keyvalue))
            {
                UnpackedValue = JsonSerializer.Deserialize<T>(keyvalue);
            }
            return UnpackedValue;
        }

        public void Delete(string key)
        {
            Preferences.Remove(key);
        }

        public bool Exists(string key)
        {
            return Preferences.ContainsKey(key);
        }

        public void ClearAll()
        {
            Preferences.Clear();
        }
    }
}
