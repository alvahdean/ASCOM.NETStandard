using System;

namespace ASCOM
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ServedClassNameAttribute : Attribute
    {
        public ServedClassNameAttribute(string servedClassName)
        {
            DisplayName = servedClassName;
        }
        public string DisplayName { get; }
    }
}
