using Microsoft.Win32;

namespace Yujt.Common.Helper
{
    public class RegistryHelper
    {
        private RegistryKey mRegistryKey;

        public RegistryHelper(RegistrySet set, string path, bool writable)
        {
            InitRegistry(set, path, writable);
        }

        public RegistryHelper(RegistrySet set, string path)
        {
            InitRegistry(set, path, false);
        }

        public RegistryHelper(string path, bool writable)
        {
            InitRegistry(RegistrySet.CurrentUser, path, writable);
        }

        public RegistryHelper(string path)
        {
            InitRegistry(RegistrySet.CurrentUser, path, false);
        }

        public void SetStringValue(string key, string value)
        {
            mRegistryKey.SetValue(key, value);
        }

        public void SetIntValue(string key, int value)
        {
            mRegistryKey.SetValue(key, value);
        }

        public void SetMultiStringValue(string key, string[] values)
        {
            mRegistryKey.SetValue(key, values, RegistryValueKind.MultiString);
        }

        public void SetDWordValue(string key, object value)
        {
            mRegistryKey.SetValue(key, value, RegistryValueKind.DWord);
        }

        public void SetBinaryValue(string key, object value)
        {
            mRegistryKey.SetValue(key, value, RegistryValueKind.Binary);
        }

        public string GetValue(string key)
        {
            return mRegistryKey.GetValue(key, null).ToString();
        }

        public void Close()
        {
            mRegistryKey.Close();
        }


        #region Private Methods
        private void InitRegistry(RegistrySet set, string path, bool writable)
        {
            var registryKey = set.Equals(RegistrySet.LocalMachine) ? Registry.LocalMachine : Registry.CurrentUser;
            mRegistryKey = registryKey.OpenSubKey(path, writable);
        }

        #endregion

    }

    public enum RegistrySet
    {
        LocalMachine,
        CurrentUser
    }
}
