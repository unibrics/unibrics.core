namespace Unibrics.Core
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly)]
    public class UnibricsModuleIdAttribute : Attribute
    {
        public string Id { get; }

        public UnibricsModuleIdAttribute(string id)
        {
            Id = id;
        }
    }
}