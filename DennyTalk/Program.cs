using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace DennyTalk
{
    static class Program
    {
        private static DialogManager dialogManager;
        private static bool exit = false;
        private static Thread updateCheckerThread;
        static void ShowUpdateDialog(Version appVersion, Version newVersion, System.Xml.XmlDocument doc)
        {
            using (UpdateForm f = new UpdateForm())
            {
                string newText = string.Format(f.Text, appVersion, "");
                f.Text = newText;
                f.MoreInfoLink = doc.DocumentElement["info"].InnerText;
                f.Info = string.Format(f.Info, newVersion, DateTime.Parse(doc.DocumentElement["date"].InnerText));

                dialogManager.MainForm.Invoke(new MethodInvoker(delegate()
                {
                    if (f.ShowDialog(dialogManager.MainForm) == DialogResult.OK)
                    {
                        Updater.LaunchUpdater(doc);
                        exit = true;
                    }
                }));
            }
        }

        static XmlStorage optionStorage;

        static void DoCheckUpdate()
        {
            while (true)
            {
                try
                {
                    CheckUpdates();
                    Thread.Sleep(60000);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Common.Log.StartLoggingUnhandledException();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            updateCheckerThread = new Thread(DoCheckUpdate);
            updateCheckerThread.IsBackground = true;
            try
            {
                System.Reflection.Assembly exe = System.Reflection.Assembly.GetEntryAssembly();
                string optionsFileName = System.IO.Path.GetDirectoryName(exe.Location) + "\\" + "options.xml";
                string contactsFileName = System.IO.Path.GetDirectoryName(exe.Location) + "\\" + "contacts.xml";
                string accountFileName = System.IO.Path.GetDirectoryName(exe.Location) + "\\" + "account.xml";
                if (!System.IO.File.Exists(optionsFileName))
                {
                    System.IO.File.WriteAllText(optionsFileName, DennyTalk.Properties.Resources.options);
                }
                optionStorage = new XmlStorage(optionsFileName);
                if (!System.IO.File.Exists(contactsFileName))
                {
                    System.IO.File.WriteAllText(contactsFileName, DennyTalk.Properties.Resources.contacts);
                }
                XmlStorage contactStorage = new XmlStorage(contactsFileName);
                XmlStorage accountStorage;
                if (!System.IO.File.Exists(accountFileName))
                {
                    System.IO.File.WriteAllText(accountFileName, "<?xml version=\"1.0\" encoding=\"utf-8\"?><Account></Account>");
                    accountStorage = new XmlStorage(accountFileName);
                    IStorageNode addressNode = accountStorage.AddNode("Address");
                    string guid = Guid.NewGuid().ToString();
                    addressNode.AddNode("GUID").Value = guid;
                    addressNode.AddNode("Host").Value = "127.0.0.1";
                    addressNode.AddNode("Port").Value = "1000";
                    accountStorage.AddNode("Nick").Value = "User_" + guid.Substring(0, 4);
                    accountStorage.AddNode("AvatarFileName").Value = "";
                    accountStorage.AddNode("Status").Value = "Offline";
                    accountStorage.Save();

                }
                else
                {
                    accountStorage = new XmlStorage(accountFileName);
                }


                Messanger messanger = new Messanger(optionStorage, contactStorage, accountStorage);


                messanger.Initialize();

                ContactStatusesUpdate csu = new ContactStatusesUpdate(messanger.ContactManager, messanger.TelegramListener, messanger.Account);
                dialogManager = new DialogManager(messanger);

                csu.Start();

                dialogManager.MainForm.Shown += new EventHandler(MainForm_Shown);
                dialogManager.CheckUpdates += new EventHandler(dialogManager_CheckUpdates);
                
                updateCheckerThread.Start();

                Application.Run(dialogManager.MainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void CheckUpdatesAndShowUpdateDialog()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate(object x)
            {
                string udateServerHost = (string)optionStorage["UpdateServerHost"].Value;
                if (udateServerHost == null)
                    return;
                Updater.CheckForUpdate(ShowUpdateDialog, udateServerHost+"/DennyTalk.manifest");
                if (exit)
                {
                    Application.Exit();
                }
            });
        }

        static void CheckUpdates()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate(object x)
            {
                string udateServerHost = (string)optionStorage["UpdateServerHost"].Value;
                if (udateServerHost == null)
                    return;
                Updater.CheckForUpdate(new ShowUpdateDialogDelegate(delegate(Version appVersion, Version newVersion, System.Xml.XmlDocument doc)
                {
                    dialogManager.NewVersionAvaliable(newVersion);
                }), udateServerHost + "/DennyTalk.manifest");
            });
        }

        static void dialogManager_CheckUpdates(object sender, EventArgs e)
        {
            CheckUpdatesAndShowUpdateDialog();
        }

        static void MainForm_Shown(object sender, EventArgs e)
        {
            CheckUpdatesAndShowUpdateDialog();
        }

    }
}