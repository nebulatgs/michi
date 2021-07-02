using System;

namespace Hexa.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class HelpHideAttribute : Attribute
    {
    }
}