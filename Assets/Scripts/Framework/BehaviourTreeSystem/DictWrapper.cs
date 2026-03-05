using System.Collections.Generic;
using UnityEngine;

namespace Framework.BehaviourTreeSystem
{
    public sealed class DictWrapper
    {
        private readonly Dictionary<string, object> _data = new ();

        public T Get<T>(string key)
        {
            if (_data.TryGetValue(key, out object value))
                return (T)value;
            
            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Should be in PascalCase</param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public void Set<T>(string key, T value) => _data[key] = value;

        /// <summary>
        /// Log every pair in the _data Dictionary.
        /// </summary>
        public void Log()
        {
            foreach (KeyValuePair<string, object> pair in _data)
            {
                Debug.Log(pair.Key + "\n" + pair.Value);
            }
        }
    }
}