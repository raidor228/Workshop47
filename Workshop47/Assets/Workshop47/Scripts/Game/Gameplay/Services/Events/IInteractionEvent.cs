namespace Workshop47.Scripts.Game.Gameplay.Services.Events
{
    public interface IInteractionEvent
    {
        public string SenderName { get; }
        public int SenderId { get; }
    }
}