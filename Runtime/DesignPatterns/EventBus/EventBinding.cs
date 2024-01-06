using System;

namespace TheMazurkaStudio.Utilities
{
    internal interface IEventBinding<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }
    

    
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        protected Action<T> onEvent = _ => { };
        protected Action onEventNoArgs = delegate { };
        
        Action<T> IEventBinding<T>.OnEvent
        {
            get => onEvent;
            set => onEvent = value;
        }
        Action IEventBinding<T>.OnEventNoArgs
        {
            get => onEventNoArgs;
            set => onEventNoArgs = value;
        }

        public EventBinding(Action<T> onEvent) =>  this.onEvent = onEvent;
        public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

        public void Add(Action<T> evt) => onEvent += evt;
        public void Remove(Action<T> evt) => onEvent -= evt;
        
        public void Add(Action evtNoArgs) => onEventNoArgs += evtNoArgs;
        public void Remove(Action evtNoArgs) => onEventNoArgs -= evtNoArgs;
    }
}