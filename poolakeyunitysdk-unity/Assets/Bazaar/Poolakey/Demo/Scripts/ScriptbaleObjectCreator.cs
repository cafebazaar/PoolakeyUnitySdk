using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PoolakeyDemo
{
    public static  class ScriptableObjectCreator 
    {
#if UNITY_EDITOR
        [MenuItem("Window/Helpers/Create ScriptableObject")]
        public static void Create()
        {
            if(Selection.activeObject is not MonoScript script)
                return;
            var type = script.GetClass();
            var scriptableObject = UnityEngine. ScriptableObject.CreateInstance(type);
            var path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(script));
            AssetDatabase.CreateAsset(scriptableObject, $"{path}/{Selection.activeObject.name}.asset");
        }
#endif
    }
}