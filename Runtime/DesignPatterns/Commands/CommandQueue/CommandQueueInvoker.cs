using System;
using System.Collections.Generic;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Queue command, update them one by one each frame, dispose command when completed
    /// </summary>
    public class CommandQueueInvoker<T> where T : class, IThickCommand
    {
        protected Queue<T> commandQueue;
        protected T currentQueueCommand;
        protected bool _isExecutingCommands;
        
        public event Action CommandHasChanged;
        public event Action AllCommandsHasBeenCompleted;
        
        
        public T GetCurrent => currentQueueCommand;

        
        public void EnqueueCommand(T queueCommand, bool clearQueue = false)
        {
            if (clearQueue) Clear();

            commandQueue.Enqueue(queueCommand);
            TryStartNewCommand();
        }
        public void Update(float deltaTime)
        {
            currentQueueCommand?.Execute(deltaTime);
        }
        public void Clear(bool ignoreCallbacks = false)
        {
            if (currentQueueCommand != null)
            {
                currentQueueCommand.OnDispose -= CurrentQueueCommandOnDispose;
                currentQueueCommand.Interrupt(ignoreCallbacks);
            }
            
            currentQueueCommand = null;
            _isExecutingCommands = false;
            commandQueue.Clear();
        }
        
        private bool TryStartNewCommand()
        {
            if (commandQueue == null || _isExecutingCommands || commandQueue.Count < 1) return false;

            currentQueueCommand = commandQueue.Dequeue();
            currentQueueCommand.OnDispose += CurrentQueueCommandOnDispose;
            _isExecutingCommands = true;
            return true;
        }

        private void CurrentQueueCommandOnDispose(CommandStatus status)
        {
            currentQueueCommand.OnDispose -= CurrentQueueCommandOnDispose;
            currentQueueCommand = null;
            _isExecutingCommands = false;
            
            if (TryStartNewCommand())  CommandHasChanged?.Invoke();
            else AllCommandsHasBeenCompleted?.Invoke();
        }
    }
}

