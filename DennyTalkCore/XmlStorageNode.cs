using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DennyTalk
{

    /// <summary>
    /// 
    /// </summary>
    public class XmlStorageNode : IStorageNode
    {

        private string value;

        private List<KeyValuePair<string, IStorageNode>> nodes = new List<KeyValuePair<string,IStorageNode>>();

        private XmlElement xmlElement;

        public XmlElement XmlElement
        {
            get { return xmlElement; }
        }

        public XmlStorageNode(XmlElement xmlElement, XmlStorage storage)
        {
            this.xmlElement = xmlElement;
            this.storage = storage;

            ReadRecursive();
        }

        private void ReadRecursive()
        {
            List<KeyValuePair<string, IStorageNode>> tempNodes = new List<KeyValuePair<string, IStorageNode>>();
            value = xmlElement.InnerText;
            foreach (XmlNode xmlNode in xmlElement)
            {
                
                if (xmlNode.NodeType == XmlNodeType.Element)
                {
                    value = null;

                    IStorageNode[] nodeArray = GetNodes(xmlNode.Name);

                    if (nodeArray.Length == 0)
                    {
                        XmlStorageNode node = new XmlStorageNode((XmlElement)xmlNode, Storage);
                        node.ReadRecursive();
                        tempNodes.Add(new KeyValuePair<string, IStorageNode>(xmlNode.Name, node));
                    }
                    else
                    {
                        bool found = false;
                        XmlStorageNode nodeFound = null;
                        foreach (XmlStorageNode node in nodeArray)
                        {
                            if (node.XmlElement == xmlNode)
                            {
                                found = true;
                                nodeFound = node;
                                break;
                            }
                        }

                        if (found)
                        {
                            nodeFound.ReadRecursive();
                            tempNodes.Add(new KeyValuePair<string, IStorageNode>(xmlNode.Name, nodeFound));
                        }
                        else
                        {
                            XmlStorageNode node = new XmlStorageNode((XmlElement)xmlNode, Storage);
                            nodeFound.ReadRecursive();
                            tempNodes.Add(new KeyValuePair<string, IStorageNode>(xmlNode.Name, node));
                        }
                    }
                }
            }
            nodes = tempNodes;
        }

        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value == null)
                    value = "";
                this.value = value.ToString();
                xmlElement.InnerText = value.ToString();
            }
        }


        private int Find(string key)
        {
            return nodes.FindIndex(delegate(KeyValuePair<string, IStorageNode> x)
            {
                return x.Key == key;
            });
        }


        public IStorageNode[] GetNodes(string key)
        {
            int index = Find(key);
            if (index < 0)
            {
                return new XmlStorageNode[0];
            }
            else
            {
                List<IStorageNode> ret = new List<IStorageNode>();
                for (int i = index; i < nodes.Count; i++)
                {
                    if (nodes[i].Key == key)
                    {
                        ret.Add(nodes[i].Value);
                    }
                    else
                    {
                        break;
                    }
                }
                return ret.ToArray();
            }
        }


        class NodeKeyValuePairComparer : IComparer<KeyValuePair<string, IStorageNode>>
        {
            public int Compare(KeyValuePair<string, IStorageNode> x, KeyValuePair<string, IStorageNode> y)
            {
                return x.Key.CompareTo(y.Key);
            }
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
                int index = Find(key);
                if (index < 0)
                {
                    return AddNode(key);
                    //return new XmlStorageNode(null, Storage);
                    //throw new ArgumentException("key not exists", "key");
                }
                else
                {
                    if (index + 1 < nodes.Count && nodes[index + 1].Key == key)
                    {
                        throw new ArgumentException("more then one key exists", "key");
                    }
                }
                return nodes[index].Value;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        private XmlStorage storage;

        internal XmlStorage Storage
        {
            get { return storage; }
            set { storage = value; }
        }

        public IStorageNode AddNode(string key)
        {
            value = null;
            XmlElement elem = storage.Document.CreateElement(key);
            xmlElement.AppendChild(elem);
            XmlStorageNode node = new XmlStorageNode(elem, Storage);
            int index = Find(key);
            nodes.Add(new KeyValuePair<string, IStorageNode>(key, node));
            return node;
        }

        public bool ContainsKey(string key)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICollection<string> Keys
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(IStorageNode item)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Value == item)
                {
                    XmlStorageNode xmlStorageNode = (XmlStorageNode)nodes[i].Value;
                    XmlNode tmp = xmlElement.RemoveChild(xmlStorageNode.xmlElement);
                    nodes.RemoveAt(i);
                    return true;
                }
            }
            return false;
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
            get { throw new Exception("The method or operation is not implemented."); }
        }


        void IDictionary<string, IStorageNode>.Add(string key, IStorageNode value)
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<string, IStorageNode>>.Add(KeyValuePair<string, IStorageNode> item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            nodes.Clear();
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
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, IStorageNode> item)
        {
            return false;
            /*
            nodes.RemoveAll(delegate(KeyValuePair<string, IStorageNode> x)
            {
            });
             */
            
        }


        public IEnumerator<KeyValuePair<string, IStorageNode>> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
