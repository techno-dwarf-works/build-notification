using System;
using System.Text;
using System.Threading.Tasks;
using BetterAttributes.Runtime;
using BetterExtensions.Runtime.Extension;
using BuildNotification.EditorAddons.Authorization;
using BuildNotification.EditorAddons.FirebaseImplementation;
using BuildNotification.Runtime;
using BuildNotification.Runtime.Authorization;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BuildNotification.EditorAddons.Window
{
    public class BuildNotificationWindow : EditorWindow
    {
        private const string ConsoleFirebase = "https://console.firebase.google.com/u/0/";
        private static bool _isDisabled;
        private Editor _embeddedInspector;
        private FirebaseScriptable _fcmScriptable;
        private Vector2 _scrollPos;
        private bool _isReloading;
        private bool _testSection;

        [MenuItem("Window/Build Notification")]
        public static void Init()
        {
            _isDisabled = false;
            // Get existing open window or if none, make a new one:
            var window = GetWindow<BuildNotificationWindow>(false, nameof(BuildNotificationWindow).PrettyCamelCase());
            window.Show();
        }

        private void OnEnable()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        private void OnDisable()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        }

        private void OnAfterAssemblyReload()
        {
            _isReloading = false;
        }

        private void OnBeforeAssemblyReload()
        {
            _isReloading = true;
        }

        private void TryLoadScriptable()
        {
            _fcmScriptable = FirebaseScriptableLoader.GetScriptable();
            RecycleInspector(_fcmScriptable);
        }

        private void RecycleInspector(Object target)
        {
            _embeddedInspector ??= Editor.CreateEditor(target);
        }

        private void ClearEmbeddedInspector()
        {
            if (_embeddedInspector == null) return;
            DestroyImmediate(_embeddedInspector);
            _embeddedInspector = null;
        }

        private void OnGUI()
        {
            if (_isReloading)
            {
                EditorGUILayout.LabelField("Reloading assembly...");
                return;
            }

            if (!ValidateScriptable()) return;

            GUI.enabled = !_isDisabled;

            DrawButtons();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            if (_embeddedInspector != null)
            {
                _embeddedInspector.OnInspectorGUI();

                if (_embeddedInspector.serializedObject.hasModifiedProperties)
                {
                    _embeddedInspector.serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open Firebase Console"))
            {
                Application.OpenURL(ConsoleFirebase);
            }

            if (GUILayout.Button($"{LocalizationService.Import} {LocalizationService.FirebaseAccountService}"))
            {
                var path = EditorUtility.OpenFilePanel(LocalizationService.FirebaseAccountService, "", PathService.JsonExtension);

                if (string.IsNullOrEmpty(path)) return;
                var data = ReadPathAndInitializeData<FirebaseAdminSDKData>(path);

                _fcmScriptable.SetFirebaseAdminSDk(data);

                EditorUtility.SetDirty(_fcmScriptable);
                AssetDatabase.SaveAssetIfDirty(_fcmScriptable);

                Debug.Log($"{nameof(FirebaseAdminSDKData)} initialized", _fcmScriptable);
            }

            EditorGUILayout.EndHorizontal();

            if (_fcmScriptable != null && _fcmScriptable.IsValid)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button($"{LocalizationService.Prepare} {LocalizationService.GoogleService}"))
                {
                    var path = EditorUtility.OpenFilePanel(LocalizationService.GoogleService, "", PathService.JsonExtension);

                    if (string.IsNullOrEmpty(path)) return;
                    var data = ReadPathAndInitializeData<Runtime.Authorization.ServiceInfoData>(path);

                    var savePath = EditorUtility.SaveFilePanel(LocalizationService.GoogleService, "",
                        $"{nameof(Runtime.Authorization.ServiceInfoData)}", PathService.JsonExtension);
                    WriteServiceAccountData(savePath, data);

                    Debug.Log($"{nameof(Runtime.Authorization.ServiceInfoData)} initialized", _fcmScriptable);
                }

                EditorGUILayout.EndHorizontal();

                TestSectionButtons();
            }
        }

        private void TestSectionButtons()
        {
            _testSection = EditorGUILayout.BeginFoldoutHeaderGroup(_testSection, "Test section buttons");
            if (_testSection)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button($"Send {LocalizationService.Success} test notification"))
                {
                    _isDisabled = true;
                    TestNotification.TestSucceed(_fcmScriptable,() => _isDisabled = false);
                }

                if (GUILayout.Button($"Send {LocalizationService.Fail} test notification"))
                {
                    _isDisabled = true;
                    TestNotification.TestFailed(_fcmScriptable,() => _isDisabled = false);
                }
                
                if (GUILayout.Button($"Send Realtime data"))
                {
                    _isDisabled = true;
                    TestDatabase.SendData(_fcmScriptable,() => _isDisabled = false);
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button($"Force Update Token"))
                {
                    _isDisabled = true;
                    RefreshToken(_fcmScriptable,() => _isDisabled = false);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private async void RefreshToken(FirebaseScriptable scriptable, Action onComplete)
        {
            await FirebaseScriptableUpdater.RefreshToken(scriptable, DateTimeOffset.Now);
            onComplete?.Invoke();
        }

        private bool ValidateScriptable()
        {
            if (_fcmScriptable != null) return true;
            TryLoadScriptable();
            if (_fcmScriptable != null) return true;
            var str = new StringBuilder();
            str.AppendLine("FCMScriptable missing!");
            str.AppendLine("Reimport plugin because it's seems to be corrupted");

            EditorUtility.DisplayDialog("FCMScriptable missing", str.ToString(), LocalizationService.Ok);
            Close();
            return false;
        }

        private T ReadPathAndInitializeData<T>(string path) where T : class
        {
            if (string.IsNullOrEmpty(path)) return null;
            _isDisabled = true;
            try
            {
                var data = Task.Run(()=> FileLoader.LoadJsonFile<T>(path));

                while (!data.IsCompleted)
                {
                    
                }

                var returnData = data.Result;
                
                if (returnData == null)
                {
                    EditorUtility.DisplayDialog(LocalizationService.Fail,
                        "Fail parsing provided json file. Try another one.", LocalizationService.Ok);
                    _isDisabled = false;
                    return null;
                }

                return returnData;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
            finally
            {
                _isDisabled = false;
            }
        }


        private async void WriteServiceAccountData<T>(string path, T data) where T : class
        {
            if (string.IsNullOrEmpty(path)) return;
            _isDisabled = true;

            try
            {
                await FileLoader.SaveFile(path, data);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
            finally
            {
                _isDisabled = false;
            }
        }
    }
}