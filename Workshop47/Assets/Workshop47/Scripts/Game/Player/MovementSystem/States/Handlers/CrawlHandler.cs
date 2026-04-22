using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class CrawlHandler
    {
        private CrawlSettings CrawlSettings => _stateContext.PlayerSettings.CrawlSettings;
        private CrouchSettings CrouchSettings => _stateContext.PlayerSettings.CrouchSettings;
        private GeneralSettings GeneralSettings => _stateContext.PlayerSettings.GeneralSettings;
        
        private readonly MovementStateContext _stateContext;
        
        public CrawlHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }
        
        public bool CanStandUp()
        {
            float clearanceThreshold = 0.05f;
            float requiredClearanceHeight = (CrawlSettings.CrawlingHeight / 2f) +
                GeneralSettings.StandingHeight - CrawlSettings.CrawlingHeight + clearanceThreshold;

            float distance = _stateContext.GetDistanceToRoof();
            if (distance == 0f)
            {
                return true;
            }
            
            return distance > requiredClearanceHeight;
        }

        public bool CanCrouchFromCrawl()
        {
            float distance = _stateContext.GetDistanceToRoof();
            if (distance == 0f)
            {
                return true;
            }
            
            return distance > CrouchSettings.CrouchingHeight - _stateContext.CharacterController.center.y;
        }
    }
}