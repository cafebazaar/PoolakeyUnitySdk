
using UnityEngine;

namespace Bazaar
{
    public class Bridge
    {
        protected AndroidJavaObject bridge;
        protected bool isAndroid;
        public Bridge(string className)
        {
            isAndroid = Application.platform == RuntimePlatform.Android;
            if (!isAndroid) return;
            using (var pluginClass = new AndroidJavaClass(className))
            {
                if (pluginClass != null)
                {
                    bridge = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                }
            }
        }
        public string version
        {
            get { return isAndroid ? bridge.Call<string>("getVersion") : ""; }
        }
    }
}