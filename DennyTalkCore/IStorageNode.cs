using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStorageNode : IDictionary<string, IStorageNode>
    {
        /// <summary>
        /// 
        /// </summary>
        object Value
        {
            get;
            set;
        }
        //void Write();
        //void WriteRecursive();
        bool Remove(IStorageNode item);
        IStorageNode AddNode(string key);
        IStorageNode[] GetNodes(string key);
    }
}
