using UnityEngine;

namespace Melador.PlayerController.MovementController.Settings
{
    [CreateAssetMenu(fileName = "New PlayerSettings", menuName = "Melador/Settings/Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [field: SerializeField] 
        public GeneralSettings GeneralSettings { get; private set; }

        [field: SerializeField] 
        public WalkSettings WalkSettings { get; private set; }
        
        [field: SerializeField] 
        public RunSettings RunSettings { get; private set; }
        
        [field: SerializeField] 
        public SprintSettings SprintSettings { get; private set; }
        
        [field: SerializeField] 
        public SlideSettings SlideSettings { get; private set; }
        
        [field: SerializeField] 
        public JumpSettings JumpSettings { get; private set; }
        
        [field: SerializeField] 
        public LongJumpSettings LongJumpSettings { get; private set; }
        
        [field: SerializeField] 
        public CrouchSettings CrouchSettings { get; private set; }
        
        [field: SerializeField] 
        public CrawlSettings CrawlSettings { get; private set; }

        [field: SerializeField]
        public ClimbSettings ClimbSettings { get; private set; }

        [field: SerializeField]
        public LedgeSettings LedgeSettings { get; private set; }

        [field: SerializeField]
        public SwimSettings SwimSettings { get; private set; }

        [field: SerializeField]
        public VaultSettings VaultSettings { get; private set; }

        [field: SerializeField]
        public WallRunSettings WallRunSettings { get; private set; }
        
        [field: SerializeField]
        public RollSettings RollSettings { get; private set; }
    }
}