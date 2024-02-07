namespace Unibrics.Core
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class UnibricsModuleTagAttribute : Attribute
    {
        public string Id { get; }

        public UnibricsModuleTagAttribute(string id)
        {
            Id = id;
        }
    }
}