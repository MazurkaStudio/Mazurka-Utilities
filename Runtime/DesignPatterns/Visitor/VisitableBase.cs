using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public abstract class VisitableBase : MonoBehaviour, IVisitable
    {
        public virtual void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}