using System;
using Better.Attributes.EditorAddons.Helpers;

namespace Better.BuildNotification.UnityPlatform.EditorAddons.Window
{
    public class ExtendedEditorPopup : EditorPopup
    {
        public event Action Destroyed;

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}