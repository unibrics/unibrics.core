namespace Unibrics.Core
{
    using System.Collections.Generic;

    public class ModuleDescriptor
    {
        public string Id { get; }
        
        public List<string> Tags { get; }

        public ModuleDescriptor(string id, List<string> tags)
        {
            Id = id;
            Tags = tags;
        }
    }
}