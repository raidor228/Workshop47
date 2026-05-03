namespace Workshop47.Scripts.Game.Gameplay.Services.Events
{
    public class InteractionEvent : IInteractionEvent
    {
        public string SenderName { get; }
        public int SenderId { get; }
        
        public InteractionEvent(string senderName, int senderId)
        {
            SenderName = senderName;
            SenderId = senderId;
        }
    }
}