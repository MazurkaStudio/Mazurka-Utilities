using System;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Command that can start, update and complete
    /// </summary>
    public abstract class CompletableCommand : ICompletableCommand
    {
        public event Action<CompletableCommand> CommandHasBeenCompleted; 

        public bool IsRunning { get; private set; }
        public bool IsCompleted { get; private set; }
        
        public float StartTime { get; private set; }
        public float ElapsedTime => Time.time - StartTime;
        
        public void Execute()
        {
            if (IsCompleted) return;
            
            if (!IsRunning)
            {
                StartTime = Time.time;
                OnStart();
                IsRunning = true;
            }
            
            OnUpdate();
        }

        public virtual void Dispose()
        {
            IsRunning = false;
            IsCompleted = false;
        }

        public void Complete()
        {
            IsCompleted = true;
            OnComplete();
            CommandHasBeenCompleted?.Invoke(this);
        }
        
        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnComplete();
    }
}
