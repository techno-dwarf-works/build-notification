using System;
using System.Text;
using System.Threading;
using Better.BuildNotification.Platform.Tooling;
using Better.BuildNotification.Runtime.Authorization;
using Better.BuildNotification.UnityPlatform.Runtime.ClientServer;
using Better.EditorTools.Helpers;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

namespace Better.BuildNotification.UnityPlatform.EditorAddons.Window
{
    public class QRModule
    {
        private FirebaseData _data;
        private byte[] _receivedBytes;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private SocketListener _listener;
        private bool _needStatusUpdate;
        private const string SyncingInfoWithApplication = "Syncing info with application";


        public void Setup(FirebaseData data)
        {
            _data = data;
        }

        public bool Validate()
        {
            return _needStatusUpdate;
        }

        public void ShowQR(ServiceInfoData data)
        {
            var serializeObject = JsonConvert.SerializeObject(data, Formatting.Indented);
            var inArray = Encoding.UTF8.GetBytes(serializeObject);

            _listener?.Stop();

            _listener = new SocketListener();
            _listener.Connected += OnConnected;

            _listener.ScheduleWrite(inArray, BytesSent);
            _listener.ReceivedBytes += OnReceivedBytes;
            var qrTexture = GenerateQR(_listener.Port.ToString());
            var openPosition = GUIUtility.GUIToScreenRect(GUILayoutUtility.GetLastRect());
            openPosition.height = qrTexture.height * 2;
            openPosition.width = qrTexture.width * 2;

            var popup = EditorPopup.InitializeAsWindow(qrTexture, openPosition, false, true);
            _listener.Start();

            EditorApplication.update += Update;
            popup.Closed += Stop;
        }

        private void BytesSent()
        {
            EditorMainThreadDispatched.Enqueue(() =>
                EditorUtility.DisplayProgressBar(SyncingInfoWithApplication, "App read bytes", 0.2f));
        }

        private void OnConnected()
        {
            EditorMainThreadDispatched.Enqueue(() =>
                EditorUtility.DisplayProgressBar(SyncingInfoWithApplication, "Connected to app", 0f));
        }

        private void Stop()
        {
            _listener?.Stop();
            _listener = null;
            EditorApplication.update -= Update;
        }

        private void Update()
        {
            _semaphore.Wait();
            if (_receivedBytes == null)
            {
                _semaphore.Release();
                return;
            }

            try
            {
                var token = Encoding.UTF8.GetString(_receivedBytes);
                _data.MessagingData.AddReceiver(token);
                FirebaseDataLoader.Instance.SaveData(_data);

                EditorMainThreadDispatched.Enqueue(() =>
                    EditorUtility.DisplayProgressBar(SyncingInfoWithApplication, "Token acquired", 0.9f));
                _receivedBytes = null;
                _semaphore.Release();
                _needStatusUpdate = true;
            }
            finally
            {
                Stop();
                EditorMainThreadDispatched.Enqueue(() =>
                    EditorUtility.DisplayProgressBar(SyncingInfoWithApplication, "Initialization done", 1f));
                EditorMainThreadDispatched.Enqueue(EditorUtility.ClearProgressBar);
                EditorPopup.CloseInstance();
            }
        }

        private void OnReceivedBytes(byte[] obj)
        {
            _semaphore.Wait();
            _receivedBytes = obj;
            EditorMainThreadDispatched.Enqueue(() =>
                EditorUtility.DisplayProgressBar(SyncingInfoWithApplication, "Received bytes from app", 0.8f));
            _semaphore.Release();
        }

        private Texture2D GenerateQR(string text)
        {
            var encoded = new Texture2D(256, 256);
            var color32 = Encode(text, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            return encoded;
        }

        private static Color32[] Encode(string textForEncoding, int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(textForEncoding);
        }
    }
}