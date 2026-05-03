using R3;
using UnityEngine;

namespace Workshop47.Scripts.Game.Gameplay.Services.Events
{
    public static class EventsHandler
    {
        public static Observable<IInteractionEvent> Events => _events;
        
        private static readonly Subject<IInteractionEvent> _events = new();

        static EventsHandler()
        {
            Events.Subscribe(e =>
            {
                Debug.Log($"EventsHandler.Events: Handled interaction: {e.SenderName} ({e.SenderId})");
            });
        }
        
        public static Observable<IInteractionEvent> Observe()
        {
            return Events;
        }
        
        public static Observable<IInteractionEvent> Observe<T>() where T : IInteractionEvent
        {
            return Events.Where(e => e is T);
        }
        
        public static Observable<IInteractionEvent> Observe(string senderName)
        {
            return Events.Where(e => e.SenderName == senderName);
        }
        
        public static Observable<IInteractionEvent> Observe(int senderId)
        {
            return Events.Where(e => e.SenderId == senderId);
        }
        
        public static void Send(IInteractionEvent interactionEvent)
        {
            _events.OnNext(interactionEvent);
        }
    }
}