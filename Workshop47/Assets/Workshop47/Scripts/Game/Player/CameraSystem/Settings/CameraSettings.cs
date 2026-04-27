using UnityEngine;

namespace Melador.PlayerController.CameraController.Settings
{
    [CreateAssetMenu(fileName = "New CameraSettings", menuName = "Melador/Settings/Camera Settings")]
    public class CameraSettings : ScriptableObject
    {
        [field: SerializeField]
        public GeneralSettings GeneralSettings { get; private set; }
        
        [field: SerializeField]
        public FPSSettings FPSSettings { get; private set; }
        
        [field: SerializeField]
        public TPSSettings TPSSettings { get; private set; }
        
        [field: SerializeField]
        public FreeLookSettings FreeLookSettings { get; private set; }
    }
}