using System;

namespace TheMazurkaStudio.Utilities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute { }
}