using System;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Command that can start, update and complete
    /// </summary>
    public abstract class CompletableCommand : ICompletableCommand
    {
        public event Action<CommandStatus> CommandHasBeenCompleted; 

        public bool IsRunning { get; private set; }


        public bool IsCompleted { get; private set; }
        
        public float StartTime { get; private set; }
        public float ElapsedTime => Time.time - StartTime;

        protected CommandStatus status;
        
        public void Execute()
        {
            if (IsCompleted) return;
            
            if (!IsRunning)
            {
                status = CommandStatus.Pending;
                StartTime = Time.time;
                OnStart();
                IsRunning = true;
            }
            
            OnUpdate();
        }

        public void Interrupt()
        {
            if (IsCompleted) return;
            
            IsRunning = false;
            Complete(CommandStatus.Fail);
        }

        public virtual void Dispose()
        {
            if (IsCompleted) return;
            
            IsRunning = false;
            IsCompleted = false;
        }

        public void Complete(CommandStatus completeStatus)
        {
            if (IsCompleted) return;
            
            status = completeStatus;
            IsCompleted = true;
            IsRunning = false;
            OnComplete();
            CommandHasBeenCompleted?.Invoke(status);
        }
        
        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnComplete();
    }
}
