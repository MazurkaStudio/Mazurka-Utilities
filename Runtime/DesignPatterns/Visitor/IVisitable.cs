namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Visitable implement Accept(IVisitor) function.
    /// Visitable allow access to a visitor in the Accept function.
    /// You should call Accept(*your visitor*) with your own logic (trigger, code, ...)
    /// You can create an IVisitable on the collider object, and iterate trough all IVisitable components you store on the actor.
    /// </summary>
    public interface IVisitable
    {
        /// <summary>
        /// Provide access to a visitor.
        /// You can switch the type to filter access.
        /// </summary>
        /// <param name="visitor"></param>
        public void Accept(IVisitor visitor);
    }
}
