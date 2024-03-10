using System;
using System.Collections.Generic;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Queue command, update them one by one each frame, dispose command when completed
    /// </summary>
    public class CommandQueueInvoker<T> where T : ICompletableCommand
    {
        protected Queue<T> commandQueue;
        protected T currentQueueCommand;
        protected bool _isExecutingCommands;
        
        public event Action StartInvokeCommands;
        public event Action CommandHasChanged;
        public event Action AllCommandsHasBeenCompleted;
        
        
        public T GetCurrent => currentQueueCommand;
        public bool IsExecutingCommands
        {
            get => _isExecutingCommands;
            protected set
            {
                if (value == _isExecutingCommands) return;

                _isExecutingCommands = value;

                if (value) StartInvokeCommands?.Invoke();
                else AllCommandsHasBeenCompleted?.Invoke();
            }
        }

        
        public void EnqueueCommand(T queueCommand, bool clearQueue = false)
        {
            if (clearQueue) commandQueue.Clear();

            commandQueue.Enqueue(queueCommand);

            if (IsExecutingCommands) return;
            if (!TryStartNewCommand()) return;

            IsExecutingCommands = true;
        }
        public void Update()
        {
            if (IsExecutingCommands)
            {
                if (!IsCommandCompleted())
                {
                    currentQueueCommand.Execute();
                }
            }
        }
        public void Clear(bool completeCurrent = true)
        {
            if (completeCurrent && IsExecutingCommands)
            {
                currentQueueCommand.Interrupt();
            }

            currentQueueCommand = default;
            IsExecutingCommands = false;
            commandQueue.Clear();
        }
        
        
        protected bool TryStartNewCommand()
        {
            if (commandQueue == null || commandQueue.Count < 1) return false;

            currentQueueCommand = commandQueue.Dequeue();
            return true;
        }
        protected bool IsCommandCompleted()
        {
            if (!currentQueueCommand.IsCompleted) return false;

            OnCommandHasBeenCompleted(currentQueueCommand);

            return true;
        }
        protected virtual void OnCommandHasBeenCompleted(T queueCommand)
        {
            queueCommand.Dispose();

            if (TryStartNewCommand())
            {
                CommandHasChanged?.Invoke();
                return;
            }
            
            IsExecutingCommands = false;
        }
    }
}

