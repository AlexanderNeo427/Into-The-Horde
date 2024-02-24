using System;
using System.Collections.Generic;

namespace IntoTheHorde
{
    public static class EventManager 
    {
        static Dictionary<GameEvent, Action<EventArgs>> m_eventMap = null;

        static EventManager() { m_eventMap = new Dictionary<GameEvent, Action<EventArgs>>(); }

        public static void AddListener(GameEvent gameEvent, Action<EventArgs> listener)
        {
            if (m_eventMap.TryGetValue(gameEvent, out Action<EventArgs> evt))
            {
                evt += listener;
                m_eventMap[gameEvent] = evt;
            }
            else
            {
                evt += listener;
                m_eventMap.Add( gameEvent, evt );
            }
        }

        public static void RemoveListener(GameEvent gameEvent, Action<EventArgs> listener)
        {
            if (m_eventMap.TryGetValue(gameEvent, out Action<EventArgs> evt))
            {
                evt -= listener;
            }
        }

        public static void RaiseEvent(GameEvent gameEvent, EventArgs eventArgs)
        {
            if (m_eventMap.TryGetValue(gameEvent, out Action<EventArgs> evt))
            {
                evt?.Invoke( eventArgs );
            }
        }

        public static void ClearAllEvents() => m_eventMap.Clear();
    }
}
