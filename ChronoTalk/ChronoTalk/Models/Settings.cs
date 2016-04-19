using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ChronoTalk.Models
{
    public abstract class Settings
    {
        private readonly string identifier;

        protected Settings()
        {
            this.identifier = this.GetType().Name;
        }

        public abstract void Reset();

        protected void Set(object value, [CallerMemberName]string key = null)
        {
            Application.Current.Properties[GetFullKey(key)] = value;
            Application.Current.SavePropertiesAsync().Wait();
        }

        protected object GetOrDefault(object defaultValue, [CallerMemberName]string key = null)
        {
            if (key == null)
                return defaultValue;

            var fullKey = GetFullKey(key);

            if (!Application.Current.Properties.ContainsKey(fullKey))
                return defaultValue;

            return Application.Current.Properties[fullKey];
        }

        protected T GetOrDefault<T>(T defaultValue, [CallerMemberName]string key = null)
        {
            if (key == null)
                return defaultValue;

            var fullKey = GetFullKey(key);

            if (!Application.Current.Properties.ContainsKey(fullKey))
                return defaultValue;

            return (T)Application.Current.Properties[fullKey];
        }

        protected bool TryGet<T>(out T value, [CallerMemberName]string key = null)
        {
            value = default(T);

            if (key == null)
                return false;

            var fullKey = GetFullKey(key);

            if (!Application.Current.Properties.ContainsKey(fullKey))
                return false;

            try
            {
                value = (T)Application.Current.Properties[fullKey];
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GetFullKey(string key)
        {
            return this.identifier + "." + key;
        }
    }
}