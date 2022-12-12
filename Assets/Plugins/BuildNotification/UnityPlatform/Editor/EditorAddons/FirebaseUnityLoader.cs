using System;
using System.IO;
using Better.BuildNotification.Platform.Services;
using Better.BuildNotification.Platform.Tooling;
using UnityEditor;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.EditorAddons
{
    [InitializeOnLoad]
    public class FirebaseUnityLoader : FirebaseDataLoader
    {
        private static readonly string[] _folderPaths;
        private FirebaseData _data;

        static FirebaseUnityLoader()
        {
            _folderPaths = new string[] { Application.dataPath, "BuildNotification", "Editor", "Resources" };
            _instance = new FirebaseUnityLoader();
        }

        private FirebaseUnityLoader()
        {
            GenerateNewKey();
        }

        public override FirebaseData GetData()
        {
            if (_data != null) return _data;

            var path = GeneratePath();
            path = Path.Combine(path, $"{nameof(FirebaseData)}{AssetExtensionWithDot}");

            _data = FileLoadService.LoadEncryptedFile<FirebaseData>(path, GetCurrentKeyBytes());

            if (_data != null)
            {
                return _data;
            }

            _data = new FirebaseData();
            FileLoadService.SaveEncryptedFile(path, _data, GetCurrentKeyBytes());
            
            AssetDatabase.Refresh();
            return _data;
        }

        private static string GeneratePath()
        {
            var bufferPath = Path.Combine(_folderPaths);
            if (!Directory.Exists(bufferPath))
            {
                Directory.CreateDirectory(bufferPath);
            }

            return bufferPath;
        }

        public override void DeleteData()
        {
            var path = GeneratePath();
            path = Path.Combine(path, $"{nameof(FirebaseData)}{AssetExtensionWithDot}");
            if (FileLoadService.FileExists(path))
            {
                FileLoadService.DeleteFile(path);
            }

            _data = null;
            AssetDatabase.Refresh();
        }

        public override void SaveData(FirebaseData data)
        {
            if (data == null) return;
            _data = data;
            var path = GeneratePath();
            path = Path.Combine(path, $"{nameof(FirebaseData)}{AssetExtensionWithDot}");

            FileLoadService.SaveEncryptedFile(path, _data, GetCurrentKeyBytes());
        }

        public override void SetKey(string encryptionKey)
        {
            if (!string.IsNullOrEmpty(encryptionKey))
            {
                EditorPrefs.SetString(Encryption.ProjectKeyName, encryptionKey);
            }
        }

        public override string GetCurrentKey()
        {
            return EditorPrefs.GetString(Encryption.ProjectKeyName, string.Empty);
        }

        public override void ClearCurrentKey()
        {
            EditorPrefs.DeleteKey(Encryption.ProjectKeyName);
        }

        public override void GenerateNewKey()
        {
            byte[] key;
            string stringKey;
            if (EditorPrefs.HasKey(Encryption.ProjectKeyName))
            {
                stringKey = EditorPrefs.GetString(Encryption.ProjectKeyName, string.Empty);

                if (!string.IsNullOrEmpty(stringKey))
                {
                    key = Convert.FromBase64String(stringKey);
                }
                else
                {
                    key = Encryption.NewKey();
                }
            }
            else
            {
                key = Encryption.NewKey();
            }

            stringKey = Convert.ToBase64String(key);
            EditorPrefs.SetString(Encryption.ProjectKeyName, stringKey);
        }

        public override byte[] GetCurrentKeyBytes()
        {
            return Convert.FromBase64String(GetCurrentKey());
        }
    }
}