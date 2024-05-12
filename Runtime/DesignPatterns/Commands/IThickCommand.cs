using System;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Executable command handle an command over time, and send callback when dispose (complete or fail)
    /// Thick command should take a least 1 frame to completed to avoid infinite loop when a command fail in loop
    /// </summary>
    public interface IThickCommand
    {
        public event Action<CommandStatus> OnDispose;
        
        public bool IsCompleted { get; }
        public void Execute(float deltaTime);
        public void Interrupt(bool ignoreCallbacks = false);
        public void Dispose(bool ignoreCallbacks = false);
    }

    public abstract class ThickCommand : IThickCommand
    {
        public event Action<CommandStatus> OnDispose;
        public bool IsCompleted { get; private set; }
        private CommandStatus status = CommandStatus.Pending;
        private bool haveStart;
        private bool delayCallback;
        protected float ElapsedTime { get; private set; }
        
        public void Execute(float deltaTime)
        {
            if (!haveStart)
            {
                haveStart = true;
                Start();
            }

            if (delayCallback)
            {
                if (ElapsedTime <= 0f) return;
                
                delayCallback = false;
                OnDispose?.Invoke(status);
                return;
            }
            
            if (IsCompleted) return;
            
            ElapsedTime += deltaTime;
            Update(deltaTime);
        }

        public void Interrupt(bool ignoreCallbacks = false)
        {
            status = CommandStatus.Fail;
            Dispose(ignoreCallbacks);
        }
        
        protected void Complete()
        {
            status = CommandStatus.Success;
            Dispose();
        }
        
        public void Dispose(bool ignoreCallbacks = false)
        {
            if (IsCompleted || delayCallback) return;
            IsCompleted = true;
            
            CleanUp();
            
            if (ignoreCallbacks) return;

            if (ElapsedTime <= 0f)
            {
                delayCallback = true;
                return;
            }
            
            OnDispose?.Invoke(status != CommandStatus.Success ? status = CommandStatus.Fail : CommandStatus.Success);
        }

        protected abstract void Start();
        protected abstract void Update(float deltaTime);
        protected abstract void CleanUp();
    }
}