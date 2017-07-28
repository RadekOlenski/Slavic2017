using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        private Queue m_eventQueue = new Queue();
        [SerializeField] private bool limitQueueProcessing = false;

        public bool LimitQueueProcessing
        {
            get
            {
                return limitQueueProcessing;
                
            }
        }

        [SerializeField] private float queueProcessTime = 0.0f;

        public float QueueProcessTime
        {
            get { return queueProcessTime; }
        }

        #region Singletton Pattern

        //We implement singletton pattern to access the same instance of EventManager from anywhere in code
        private void Start()
        {
            if (instance == null)

                instance = this;

            else if (instance != this)

                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private static EventManager instance;

        public static EventManager Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = GameObject.FindObjectOfType(typeof(EventManager)) as EventManager;
                return instance;
            }
        }

        #endregion

        #region Delegates

        //Declaration of Events Delegates
        public delegate void EventDelegate<in T>(T e) where T : GameEvent;

        private delegate void EventDelegate(GameEvent e);

        //Method for adding delegates
        private EventDelegate AddDelegate<T>(EventDelegate<T> eventDelegate) where T : GameEvent
        {
            //If we have already registered that delegate return null
            if (lookupDelegates.ContainsKey(eventDelegate))
                return null;

            // Create a new non-generic delegate which calls our generic one.
            // This is the delegate we actually invoke.
            EventDelegate internalDelegate = e => eventDelegate((T) e);
            lookupDelegates[eventDelegate] = internalDelegate;

            EventDelegate tempDelegate;
            if (delegates.TryGetValue(typeof(T), out tempDelegate))
                delegates[typeof(T)] = tempDelegate += internalDelegate;
            else
                delegates[typeof(T)] = internalDelegate;

            return internalDelegate;
        }

        #endregion

        #region Dictionaries

        //Dictionaries of delegates
        private readonly Dictionary<Type, EventDelegate> delegates = new Dictionary<Type, EventDelegate>();

        private readonly Dictionary<Delegate, EventDelegate> lookupDelegates = new Dictionary<Delegate, EventDelegate>();

        //Dictionary of delegates that we want to listen to only once
        private readonly Dictionary<Delegate, Delegate> onceLookupDelegates = new Dictionary<Delegate, Delegate>();

        #endregion

        #region Listeners - Add

        //Methods for adding event listeners
        public void AddListener<T>(EventDelegate<T> eventDelegate) where T : GameEvent
        {
            //We add delegate to event we'll be listening to
            AddDelegate(eventDelegate);
        }

        //That method allows to add listener that will wait for event only once
        public void AddListenerOnce<T>(EventDelegate<T> eventDelegate) where T : GameEvent
        {
            var result = AddDelegate(eventDelegate);

            if (result != null)
                onceLookupDelegates[result] = eventDelegate;
        }

        #endregion

        #region Listeners - Remove

        //Method for removing single listener
        public void RemoveListener<T>(EventDelegate<T> eventDelegate) where T : GameEvent
        {
            //We search for listener delegate type we want to remove
            EventDelegate internalDelegate;
            if (!lookupDelegates.TryGetValue(eventDelegate, out internalDelegate)) return;
            EventDelegate tempDelegate;
            if (delegates.TryGetValue(typeof(T), out tempDelegate))
            {
                tempDelegate -= internalDelegate;
                if (tempDelegate == null)
                    delegates.Remove(typeof(T));
                else
                    delegates[typeof(T)] = tempDelegate;
            }

            lookupDelegates.Remove(eventDelegate);
        }

        //Method for removing all of listeners
        public void RemoveAllListeners()
        {
            delegates.Clear();
            lookupDelegates.Clear();
            onceLookupDelegates.Clear();
        }

        #endregion

        #region Listeners - Other

        public bool HasListener<T>(EventDelegate<T> eventDelegate) where T : GameEvent
        {
            return lookupDelegates.ContainsKey(eventDelegate);
        }

        #endregion

        #region Queue events

        public bool QueueEvent(GameEvent gameEvent)
        {
            if (!delegates.ContainsKey(gameEvent.GetType()))
            {
                Debug.LogWarning("EventManager: QueueEvent failed due to no listeners fir event: " + gameEvent.GetType());
                return false;
            }

            m_eventQueue.Enqueue(gameEvent);
            return true;
        }

        #endregion

        #region Event Invoke

        public void InvokeEvent(GameEvent gameEvent)
        {
            //Here we invoke desired event from our dictionary
            EventDelegate eventDelegate;
            if (delegates.TryGetValue(gameEvent.GetType(), out eventDelegate))
            {
                eventDelegate.Invoke(gameEvent);
                gameEvent.DebugEvent();

                //Remove listeners that should be called once
                foreach (var delegate1 in delegates[gameEvent.GetType()].GetInvocationList())
                {
                    var key = (EventDelegate) delegate1;
                    if (onceLookupDelegates.ContainsKey(key))
                    {
                        delegates[gameEvent.GetType()] -= key;


                        if (delegates[gameEvent.GetType()] == null)
                            delegates.Remove(gameEvent.GetType());

                        lookupDelegates.Remove(onceLookupDelegates[key]);
                        onceLookupDelegates.Remove(key);
                    }
                }
            }
            else
            {
                //Debug.WriteLine("Event: " + gameEvent.GetType() + " has no listeners");

                //Debug.LogWarning("Event: " + gameEvent.GetType() + " has no listeners");
            }
        }

        #endregion

        private void Update()
        {
            float timer = 0.0f;
            while (m_eventQueue.Count > 0)
            {
                if (LimitQueueProcessing)
                {
                    if (timer > QueueProcessTime)
                        return;
                }

                GameEvent gameEvent = m_eventQueue.Dequeue() as GameEvent;
                InvokeEvent(gameEvent);

                if (LimitQueueProcessing)
                    timer += Time.deltaTime;
            }
        }

        private void OnApplicationQuit()
        {
            RemoveAllListeners();
            m_eventQueue.Clear();
            instance = null;
        }
    }
}