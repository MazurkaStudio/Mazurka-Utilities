using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public interface IState
    {
        public bool CanTransitionToSelf { get; }
        
        public void EnterState();
        public bool TryExistState();
        public void LogicUpdate();
        public void PostUpdate();
        public void PhysicUpdate();
    }
}
