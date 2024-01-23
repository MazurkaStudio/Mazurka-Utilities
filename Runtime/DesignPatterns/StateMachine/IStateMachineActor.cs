using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public interface IStateMachineActor
    {
        public StateMachine GetStateMachine { get; }
    }
}
