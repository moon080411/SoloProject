using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01.Script.Core.GameEventSystem
{
    public class GameEvent { }

    [CreateAssetMenu(fileName = "GameEventChannelSO", menuName = "SO/GameEventChannelSO")]
    public class GameEventChannelSO : ScriptableObject
    {
        private Dictionary<Type, Action<GameEvent>> _events = new Dictionary<Type, Action<GameEvent>>();

        private Dictionary<Delegate, Action<GameEvent>> _lookUpTable = new Dictionary<Delegate, Action<GameEvent>>();

        public void AddListener<T>(Action<T> handler) where T : GameEvent
        {
            if (_lookUpTable.ContainsKey(handler) == true) return;

            Action<GameEvent> castHandler = evt => handler.Invoke(evt as T);

            Type evtType = typeof(T);

            if (_events.ContainsKey(evtType))
                _events[evtType] += castHandler;
            else
                _events[evtType] = castHandler;
        }

        public void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
            Type evtType = typeof(T);
            if (_lookUpTable.TryGetValue(handler, out Action<GameEvent> castHandler))
            {
                if (_events.TryGetValue(evtType, out Action<GameEvent> internalHandler))
                {
                    internalHandler -= castHandler;
                    if (internalHandler == null)
                        _events.Remove(evtType);
                    else
                        _events[evtType] = internalHandler;
                }
                _lookUpTable.Remove(handler);
            }
        }

        public void RaiseEvent(GameEvent evt)
        {
            if (_events.TryGetValue(evt.GetType(), out Action<GameEvent> castHandler))
                castHandler.Invoke(evt);
        }

        public void Clear()
        {
            _events.Clear();
            _lookUpTable = null;
        }
    }
}