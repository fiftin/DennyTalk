using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStorage : IDictionary<string, IStorageNode>
    {
        IStorageNode AddNode(string key);
        bool Remove(IStorageNode item);
        void Load();
        void Save();
        IStorageNode[] GetNodes(string key);
    }
}
