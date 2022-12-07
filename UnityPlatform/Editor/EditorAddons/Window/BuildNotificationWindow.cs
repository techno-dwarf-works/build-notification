using System;
using System.Text;
using Better.BuildNotification.Platform.Services;
using Better.BuildNotification.Platform.Tooling;
using Better.BuildNotification.Runtime.Authorization;
using Better.BuildNotification.Runtime.Services;
using Better.BuildNotification.UnityPlatform.Runtime.Services;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.Editor.EditorAddons.Window
{
    public class BuildNotificationWindow : EditorWindow
    {
        private const string ConsoleFirebase = "https://console.firebase.google.com/u/0/";
        private static bool _isDisabled;
        private FirebaseData _fcmData;
        private Vector2 _scrollPos;
        private bool _isReloading;
        private string _key;

        private FirebaseDataDrawer _drawer;
        private bool _sensitive;
        private int _index = 0;

        private string[] _sections = new string[]
        {
            $"{nameof(FirebaseData).PrettyCamelCase()}", "Test section buttons", "Sensitive section",
        };

        [MenuItem("Better/Build Notification/Settings")]
        public static void Init()
        {
            _isDisabled = false;
            // Get existing open window or if none, make a new one:
            var window = GetWindow<BuildNotificationWindow>(false, nameof(BuildNotificationWindow).PrettyCamelCase());
            window.Show();
            window._drawer = new FirebaseDataDrawer();
            window.saveChangesMessage = $"Some changes in {nameof(FirebaseData)} not saved";
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
            Close();
        }

        private void TryLoadScriptable()
        {
            _fcmData = FirebaseDataLoader.Instance.GetData();
            if (_drawer == null)
                _drawer = new FirebaseDataDrawer();
            _drawer.Setup(_fcmData);
        }

        public override void SaveChanges()
        {
            if (hasUnsavedChanges)
            {
                FirebaseDataLoader.Instance.SaveData(_fcmData);
                _drawer?.MarkSaved();
            }

            base.SaveChanges();
        }

        private void OnLostFocus()
        {
            SaveChanges();
        }

        private void OnDestroy()
        {
            if (hasUnsavedChanges)
            {
                FirebaseDataLoader.Instance.SaveData(_fcmData);
                _drawer?.MarkSaved();
            }
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
            DrawSelection();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save"))
            {
                SaveChanges();
            }

            EditorGUILayout.Space();
        }

        private void DrawButtons()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Open Firebase Console"))
                {
                    Application.OpenURL(ConsoleFirebase);
                }

                if (GUILayout.Button($"{LocalizationService.Import} {LocalizationService.FirebaseAccountService}"))
                {
                    var path = EditorUtility.OpenFilePanel(LocalizationService.FirebaseAccountService, "",
                        PathService.JsonExtension);

                    if (string.IsNullOrEmpty(path)) return;
                    var data = ReadPathAndInitializeData<FirebaseAdminSDKData>(path);

                    _fcmData.SetFirebaseAdminSDk(data);
                    FirebaseDataLoader.Instance.SaveData(_fcmData);
                    _drawer.Setup(_fcmData);

                    Debug.Log($"{nameof(FirebaseAdminSDKData)} initialized");
                }
                
                if (GUILayout.Button($"{LocalizationService.Prepare} {LocalizationService.GoogleService}"))
                {
                    var path = EditorUtility.OpenFilePanel(LocalizationService.GoogleService, "",
                        PathService.JsonExtension);

                    if (string.IsNullOrEmpty(path)) return;
                    var data = ReadPathAndInitializeData<ServiceInfoData>(path);

                    var savePath = EditorUtility.SaveFilePanel(LocalizationService.GoogleService, "",
                        $"{nameof(ServiceInfoData)}", PathService.JsonExtension);

                    WriteServiceAccountData(savePath, data);
                    Debug.Log($"{nameof(ServiceInfoData)} initialized");
                }
            }
        }

        private void DrawSelection()
        {
            if (_fcmData != null && _fcmData.IsValid)
            {
                EditorGUILayout.Space();
                _index = DrawToolbar(_index, _sections);
                switch (_index)
                {
                    case 0:
                        hasUnsavedChanges = _drawer.OnGUI();
                        break;
                    case 1:
                        TestSectionButtons();
                        break;
                    case 2:
                        DrawKeySection();
                        break;
                    default:
                        break;
                }
            }
        }

        private void DrawKeySection()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    _key = EditorGUILayout.TextField(new GUIContent("Encryption Key"), _key);
                    var width = GUI.skin.button;
                    var buttonStyle = new GUIContent("Save Project key");
                    var size = width.CalcSize(buttonStyle);
                    if (GUILayout.Button(buttonStyle, GUILayout.Width(size.x)))
                    {
                        FirebaseDataLoader.Instance.SetKey(_key);
                        GUI.FocusControl(null);
                        _key = string.Empty;
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Copy Project key"))
                    {
                        var key = FirebaseDataLoader.Instance.GetCurrentKey();
                        if (!string.IsNullOrEmpty(key))
                        {
                            FirebaseDataLoader.Instance.GetCurrentKey().CopyToClipboard();
                            EditorUtility.DisplayDialog("Key copied", "Your project key copied to clipboard",
                                LocalizationService.Ok);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Key Empty", "Current key is empty",
                                LocalizationService.Ok);
                        }
                    }

                    if (GUILayout.Button("Generate new Project key"))
                    {
                        if (!EditorUtility.DisplayDialog($"Clean data",
                                "This will generate new key and delete current data",
                                LocalizationService.Ok, "Cansel")) return;
                        FirebaseDataLoader.Instance.DeleteData();
                        FirebaseDataLoader.Instance.ClearCurrentKey();
                        FirebaseDataLoader.Instance.GenerateNewKey();
                        TryLoadScriptable();
                    }
                }
            }
        }
        
        private void TestSectionButtons()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button($"Send {LocalizationService.Success} test notification"))
                {
                    _isDisabled = true;
                    TestNotification.TestSucceed(_fcmData, () => _isDisabled = false);
                }

                if (GUILayout.Button($"Send {LocalizationService.Fail} test notification"))
                {
                    _isDisabled = true;
                    TestNotification.TestFailed(_fcmData, () => _isDisabled = false);
                }

                if (GUILayout.Button($"Send Realtime data"))
                {
                    _isDisabled = true;
                    TestDatabase.SendData(_fcmData, () => _isDisabled = false);
                }
            }

            if (GUILayout.Button($"Force Update Token"))
            {
                _isDisabled = true;
                RefreshToken(_fcmData, () => _isDisabled = false);
            }
        }

        private async void RefreshToken(FirebaseData scriptable, Action onComplete)
        {
            await FirebaseUpdater.RefreshToken(scriptable, DateTimeOffset.Now);
            onComplete?.Invoke();
        }

        private bool ValidateScriptable()
        {
            if (_fcmData != null && _drawer != null) return true;
            TryLoadScriptable();
            if (_fcmData != null && _drawer != null) return true;
            var str = new StringBuilder();
            str.AppendLine($"{nameof(FirebaseData)} missing!");
            str.AppendLine("Reimport plugin because it's seems to be corrupted");

            EditorUtility.DisplayDialog($"{nameof(FirebaseData)}{FirebaseUnityLoader.AssetExtensionWithDot} missing",
                str.ToString(), LocalizationService.Ok);
            Close();
            return false;
        }

        private T ReadPathAndInitializeData<T>(string path) where T : class
        {
            if (string.IsNullOrEmpty(path)) return null;
            _isDisabled = true;
            try
            {
                var returnData = FileLoadService.LoadJsonFile<T>(path);

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
                await FileLoadService.SaveFileAsync(path, data);
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

        private int DrawToolbar(int index, string[] label, bool allowDisable = false)
        {
            EditorGUI.BeginChangeCheck();
            var prevIndex = index;
            index = GUILayout.Toolbar(index, label);
            if (EditorGUI.EndChangeCheck())
            {
                if (allowDisable)
                {
                    if (index == prevIndex)
                        index = -1;
                }
            }

            return index;
        }
    }
}