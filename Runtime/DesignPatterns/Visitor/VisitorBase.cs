using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public abstract class VisitorBase : MonoBehaviour, IVisitor
    {
        /// <summary>
        /// You can switch the type to do stuff
        /// Should not call this function directly, but provide the visitor as argument to Accept function of a IVisitable object
        /// </summary>
        /// <param name="visitable"></param>
        /// <typeparam name="T"></typeparam>
        public abstract void Visit<T>(T visitable) where T : Component, IVisitable;
    }
}