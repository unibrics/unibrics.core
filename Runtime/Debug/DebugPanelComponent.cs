namespace Unibrics.Debug
{
    using UnityEngine;

    /// <summary>
    /// This script should be placed on prefabs under Resources folder.
    /// DebugPanel module (or its alternative) if installed must process it
    /// and make it visible to user in some way
    /// </summary>
    public class DebugPanelComponent : MonoBehaviour
    {
        [SerializeField]
        private string componentName;

        public string ComponentName => componentName;
    }
}