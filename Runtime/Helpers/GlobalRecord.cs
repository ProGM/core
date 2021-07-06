using ReactUnity.Dispatchers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactUnity.Helpers
{
    public class GlobalRecord : EventDictionary<object>
    {
        private System.Action removeStringDictionaryListener;
        private IDispatcher dispatcher;

        public GlobalRecord() { }

        public GlobalRecord(IDictionary<string, object> dict) : base(dict)
        {
        }

        public static GlobalRecord BindSerializableDictionary(SerializableDictionary dict, IDispatcher dispatcher, bool isSerializing)
        {
            var res = new GlobalRecord();
            res.dispatcher = dispatcher;
            res.BindSerializableDictionary(dict, isSerializing);
            return res;
        }

        public void BindSerializableDictionary(SerializableDictionary dict, bool isSerializing)
        {
            if (this.removeStringDictionaryListener != null) this.removeStringDictionaryListener();

            UpdateStringObjectDictionary(dict, isSerializing);

            removeStringDictionaryListener = dict.AddListener((key, value, dc) =>
            {
                if (key != null) this[key] = value;
                else UpdateStringObjectDictionary(dc, false);
            });

            dict.AddReserializeListener((dc) =>
            {
                UpdateStringObjectDictionary(dc, true);
            });
        }


        public void UpdateStringObjectDictionary(EventDictionary<Object> dict, bool isSerializing)
        {
            ClearWithoutNotify();
            foreach (var entry in dict)
            {
                SetWithoutNotify(entry.Key, entry.Value);
            }

            if (!isSerializing) Change(null, default);
            else if (dispatcher != null)
                dispatcher.OnceUpdate(() => Change(null, default));
        }
    }

}
