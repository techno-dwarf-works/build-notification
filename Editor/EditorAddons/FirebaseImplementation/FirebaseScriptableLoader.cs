using System.IO;
using UnityEditor;
using UnityEngine;

namespace BuildNotification.EditorAddons.FirebaseImplementation
{
    public class FirebaseScriptableLoader
    {
        private static string[] folderPaths = new[] { "BuildNotification","Editor","Resources" };
        private static FirebaseScriptable _scriptable;
        public const string AssetExtensionWithDot = ".asset";

        public static FirebaseScriptable GetScriptable()
        {
            if (_scriptable != null) return _scriptable;
            _scriptable = Resources.Load<FirebaseScriptable>(nameof(FirebaseScriptable));
            if (_scriptable != null) return _scriptable;
            var path = "Assets";
            foreach (var folderPath in folderPaths)
            {
                var bufferPath = $"{path}/{folderPath}";
                if (!AssetDatabase.IsValidFolder(bufferPath))
                {
                    AssetDatabase.CreateFolder(path, folderPath);
                }

                path = bufferPath;
            }

            _scriptable = ScriptableObject.CreateInstance<FirebaseScriptable>();
            path = Path.Combine(path, $"{nameof(FirebaseScriptable)}{AssetExtensionWithDot}");
            AssetDatabase.CreateAsset(_scriptable, path);
            AssetDatabase.SaveAssetIfDirty(_scriptable);

            return _scriptable;
        }
    }
}