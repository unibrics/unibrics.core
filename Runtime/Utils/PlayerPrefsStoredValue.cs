namespace Unibrics.Utils
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Helper class for wrapping work with PlayerPrefs.Read/Write for some field
    /// </summary>
    public class PlayerPrefsStoredValue<T>
    {
        private readonly string key;

        private string FullKey => $"local:{key}";
        
        private T value;

        private readonly Action<T> onValueChanged;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                Save(value);
                onValueChanged(value);
            }
        }
        
        public PlayerPrefsStoredValue(string key,Action<T> onValueChanged, T defaultValue)
        {
            this.key = key;
            this.onValueChanged = onValueChanged;
            value = (T) Restore(defaultValue);
            
            onValueChanged(value);
        }

        public PlayerPrefsStoredValue(string key, T defaultValue) : this(key, _ => {}, defaultValue)
        {
        }

        private void Save(T value)
        {
            switch (value)
            {
                case bool boolValue:
                    PlayerPrefs.SetString(FullKey, boolValue.ToString());
                    break;
                case Enum e:
                    PlayerPrefs.SetString(FullKey, e.ToString());
                    break;
                case float f:
                    PlayerPrefs.SetFloat(FullKey, f);
                    break;
                case string s:
                    PlayerPrefs.SetString(FullKey, s);
                    break;
                default:
                    throw new Exception($"type {value.GetType()} is not supported yet in PlayerPrefsStoredValue");
            }
        }
        
        private object Restore(T defaultValue)
        {
            if (!PlayerPrefs.HasKey(FullKey))
            {
                Save(defaultValue);
                return defaultValue;
            }

            object result;
            value = defaultValue;
            switch (value)
            {
                case bool _:
                    var val = PlayerPrefs.GetString(FullKey);
                    result = val.Equals("True");
                    break;
                case Enum _:
                    result = (T) Enum.Parse(typeof(T), PlayerPrefs.GetString(FullKey));
                    break;
                case float _:
                    result = PlayerPrefs.GetFloat(FullKey);
                    break;
                case string _:
                    result = PlayerPrefs.GetString(FullKey);
                    break;
                default:
                    throw new Exception($"type {value.GetType()} is not supported yet in PlayerPrefsStoredValue");
            }
            
            return result;

        }


    }
}