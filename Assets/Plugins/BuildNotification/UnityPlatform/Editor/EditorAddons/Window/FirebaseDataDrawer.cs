using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Better.BuildNotification.Platform.Tooling;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.EditorAddons.Window
{
    public class FirebaseDataDrawer
    {
        private Dictionary<string, Tuple<FieldInfo, object>> _fieldInfos =
            new Dictionary<string, Tuple<FieldInfo, object>>();

        private string _cloudMessagingDataName = "_cloudMessagingData";
        private string _realtimeDatabaseDataName = "_realtimeDatabaseData";
        private string _firebaseAdminSDKDataName = "firebaseAdminSDKData";
        private string _expirationTimeName = "_expirationTime";
        private string _lastRequestSuccessfulName = "_lastRequestSuccessful";
        private string _cloudMessagingDataReceiversName = "_receivers";

        private bool _isReady;

        private bool _lastRequestSuccessful;
        private long _expirationTime;
        private List<string> _receivers;
        private Vector2 _scroll;
        private bool _hasUnsaved;

        private float TextHeight => EditorGUIUtility.singleLineHeight * 2.5f;
        private float MaxScrollHeight => TextHeight * 3f;
        public bool IsReady => _isReady;

        public void Setup(FirebaseData firebaseData)
        {
            if (_isReady) return;
            _fieldInfos.Clear();
            _isReady = true;
            var cloudMessageData = GetFieldValue<CloudMessagingData>(firebaseData, _cloudMessagingDataName);
            var realtimeDatabaseData = GetFieldValue<RealtimeDatabaseData>(firebaseData, _realtimeDatabaseDataName);
            var serviceAccountData = GetFieldValue<FirebaseAdminSDKData>(firebaseData, _firebaseAdminSDKDataName);
            _expirationTime = GetFieldValue<long>(firebaseData, _expirationTimeName);
            _lastRequestSuccessful = GetFieldValue<bool>(firebaseData, _lastRequestSuccessfulName);
            _receivers = GetFieldValue<List<string>>(cloudMessageData, _cloudMessagingDataReceiversName);
        }

        public bool OnGUI()
        {
            if (!_isReady) return false;
            DrawStringList();

            DrawInfos();
            
            return _hasUnsaved;
        }

        private void DrawInfos()
        {
            using (var disable = new EditorGUI.DisabledGroupScope(true))
            {
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    _expirationTime = EditorGUILayout.LongField(
                        new GUIContent(nameof(_expirationTime).PrettyCamelCase().FirstCharToUpper()),
                        _expirationTime);
                    SaveIfHasChanges(check.changed, _expirationTimeName, _expirationTime);
                }

                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    _lastRequestSuccessful = EditorGUILayout.Toggle(
                        new GUIContent(nameof(_lastRequestSuccessful).PrettyCamelCase().FirstCharToUpper()),
                        _lastRequestSuccessful);
                    SaveIfHasChanges(check.changed, _lastRequestSuccessfulName, _lastRequestSuccessful);
                }
            }
        }

        private void SaveIfHasChanges(bool changed, string name, object newValue)
        {
            if (!changed || !_fieldInfos.TryGetValue(name, out var value)) return;
            value.Item1.SetValue(value.Item2, newValue);
            _hasUnsaved = true;
        }

        public void Reset()
        {
            _isReady = false;
        }

        private void DrawStringList()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                using (var verticalScope = new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField(new GUIContent(nameof(_receivers).PrettyCamelCase().FirstCharToUpper()));

                    using (var scroll = new EditorGUILayout.ScrollViewScope(_scroll, GUILayout.MaxHeight(MaxScrollHeight),
                               GUILayout.Height(0)))
                    {
                        using (var vertical = new EditorGUI.IndentLevelScope(1))
                        {
                            for (int i = 0; i < _receivers.Count; i++)
                            {
                                _receivers[i] = EditorGUILayout.TextField(new GUIContent($"Element {i}"), _receivers[i],
                                    GUILayout.Height(TextHeight));
                            }
                        }

                        _scroll = scroll.scrollPosition;
                    }

                    using (var horizontal =
                           new EditorGUILayout.HorizontalScope(GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                    {
                        GUILayout.FlexibleSpace();
                        using (var horizontalButtons = new EditorGUILayout.HorizontalScope(GUILayout.MaxHeight(100)))
                        {
                            using (var disableButton = new EditorGUI.DisabledGroupScope(_receivers.Count <= 0))
                            {
                                if (GUILayout.Button("-"))
                                {
                                    _receivers.RemoveAt(_receivers.Count - 1);
                                }
                            }

                            if (GUILayout.Button("+"))
                            {
                                _receivers.Add(string.Empty);
                            }
                        }
                    }
                }

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                SaveIfHasChanges(check.changed, _cloudMessagingDataReceiversName, _receivers);
            }
        }

        private T GetFieldValue<T>(object root, string name)
        {
            var info = root.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (info == null) return default;
            _fieldInfos.Add(name, new Tuple<FieldInfo, object>(info, root));
            var value = info.GetValue(root);

            if (value.Cast<T>(out var castedValue))
            {
                return castedValue;
            }

            return default;
        }

        public void MarkSaved()
        {
            _hasUnsaved = false;
        }
    }
}