using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerController.MovementController.States.Root;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class CrouchHandler
    {
        private CrouchSettings CrouchSettings => _stateContext.PlayerSettings.CrouchSettings;
        private GeneralSettings GeneralSettings => _stateContext.PlayerSettings.GeneralSettings;
        
        private readonly MovementStateContext _stateContext;
        
        public CrouchHandler(MovementStateContext stateContext)
        {
            _stateContext = stateContext;
        }
        
        public bool CanStandUp()
        {
            float clearanceThreshold = 0.05f;
            float requiredClearanceHeight = (CrouchSettings.CrouchingHeight / 2f) +
                GeneralSettings.StandingHeight - CrouchSettings.CrouchingHeight + clearanceThreshold;

            float distance = _stateContext.GetDistanceToRoof();
            if (distance == 0f)
            {
                return true;
            }
            
            return distance > requiredClearanceHeight;
        }
    }
}