using System.IO;
using BuildNotification.Runtime.Tooling.FirebaseImplementation;
using UnityEditor;
using UnityEngine;

namespace BuildNotification.UnityPlatform.EditorAddons
{
    [InitializeOnLoad]
    public class FirebaseScriptableLoader : FirebaseDataLoader
    {
        private static string[] folderPaths = new[] { "BuildNotification", "Editor", "Resources" };
        private static FirebaseScriptable _scriptable;
        private const string AssetExtensionWithDot = ".asset";

        static FirebaseScriptableLoader()
        {
            _instance = new FirebaseScriptableLoader();
        }

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

        public override FirebaseData GetData()
        {
            if (_scriptable != null) return _scriptable.Data;
            _scriptable = Resources.Load<FirebaseScriptable>(nameof(FirebaseScriptable));
            if (_scriptable != null) return _scriptable.Data;
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

            return _scriptable.Data;
        }
    }
}