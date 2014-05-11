using System;
using System.Collections.Generic;
using System.Text;
using DennyTalk;

namespace DennyTalkServer
{
    class Program
    {
        private static Messenger messanger;
        private static Server server;

        static void Main(string[] args)
        {
            Common.Log.StartLoggingUnhandledException();

            XmlStorage optionStorage = new XmlStorage("options.xml");
            XmlStorage contactStorage = new XmlStorage("contacts.xml");
            XmlStorage accountStorage = new XmlStorage("account.xml");

            messanger = new Messenger(optionStorage, contactStorage, accountStorage);
            messanger.Initialize();

            RemoteServerListener rsl = new RemoteServerListener(int.Parse((string)optionStorage["TCPPort"].Value));
            server = new Server(messanger, rsl);
            server.Initialize();

            Console.WriteLine("TCP port: {0}", rsl.Port);

            IStorageNode remoteServersNode = optionStorage["RemoteServers"];
            IStorageNode[] remoteServers = remoteServersNode.GetNodes("RemoteServer");
            foreach (IStorageNode remServ in remoteServers)
            {
                IStorageNode addrNode = remServ["Address"];
                Address addr = new Address(
                    (string)addrNode["Host"].Value,
                    int.Parse((string)addrNode["Port"].Value),
                    (string)addrNode["GUID"].Value);
                RemoteServer serv = new RemoteServer(addr);
                serv.Connect();
                server.AddRemoteServer(serv);
                Console.WriteLine("Remote server: {0}:{1} <{2}>", addrNode["Host"].Value, addrNode["Port"].Value, addrNode["GUID"].Value);
            }

            Console.WriteLine("Initialization completed successful");
            Common.ConsoleHelper.Wait();
            Environment.Exit(0);
        }
    }
}
