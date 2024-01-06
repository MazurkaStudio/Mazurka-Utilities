using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// IVisitor can visit any IVisitable object, get the specific type, and do stuff if the visitable accept it.
    /// </summary>
    public interface IVisitor
    {
        /// <summary>
        /// Do stuff on a IVisitable object
        /// </summary>
        /// <param name="visitable"></param>
        /// <typeparam name="T"></typeparam>
        public void Visit<T>(T visitable) where T : Component, IVisitable;
    }
}