using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DennyTalk
{
    public class XmlStorage : IStorage
    {
        private XmlDocument document = new XmlDocument();
        private XmlElement documentElement;
        private XmlStorageNode documentNode;
        private string filename;

        public XmlDocument Document
        {
            get { return document; }
        }

        public XmlStorage(string filename)
        {
            document.Load(filename);
            documentElement = document.DocumentElement;
            documentNode = new XmlStorageNode(documentElement, this);
            this.filename = filename;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IStorageNode this[string key]
        {
            get
            {
                return documentNode[key];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public IStorageNode[] GetNodes(string key)
        {
            return documentNode.GetNodes(key);
        }

        public IEnumerator<KeyValuePair<string,IStorageNode>> GetEnumerator()
        {
            return documentNode.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string key, IStorageNode value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool ContainsKey(string key)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICollection<string> Keys
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(string key)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool TryGetValue(string key, out IStorageNode value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICollection<IStorageNode> Values
        {
            get
            {
                return documentNode.Values;
            }
        }

        public void Add(KeyValuePair<string, IStorageNode> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(KeyValuePair<string, IStorageNode> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(KeyValuePair<string, IStorageNode>[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(IStorageNode item)
        {
            return documentNode.Remove(item);
            //throw new Exception("The method or operation is not implemented.");
        }

        public bool Remove(KeyValuePair<string, IStorageNode> item)
        {
            return documentNode.Remove(item);
            //throw new Exception("The method or operation is not implemented.");
        }

        IEnumerator<KeyValuePair<string, IStorageNode>> IEnumerable<KeyValuePair<string, IStorageNode>>.GetEnumerator()
        {
            return documentNode.GetEnumerator();
        }


        public IStorageNode AddNode(string key)
        {
            return documentNode.AddNode(key);
        }


        public void Load()
        {
            ;
        }

        public void Save()
        {
            //XmlWriter writer = XmlWriter.Create(filename);
            //Document.WriteTo(writer);
            //writer.Close();
            //documentNode.WriteRecursive();

            document.Save(filename);
        }

    }
}
