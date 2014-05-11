using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Common;

namespace DennyTalk
{    /// <summary>
    /// Идентификация клиета инициировавшего передачу файла.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct AuthFileTransfering
    {
        //[FieldOffset(0)]
        //public int port;

        [FieldOffset(0)]
        public int numberOfFiles;

        [FieldOffset(4)]
        public int requestId;

        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string guid;

    }

    /// <summary>
    /// Файл.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct TransferedFileInfo
    {
        [FieldOffset(0)]
        public long size;
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string filename;
    }

    public class FileLoadStartOrCompleteEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public FileLoadStartOrCompleteEventArgs(string filename)
        {
            FileName = filename;
        }
    }

    public class AuthFileTransferingEventArgs : EventArgs
    {
        public string Guid { get; private set; }
        public int RequestId { get; private set; }
        public AuthFileTransferingEventArgs(string guid, int requestId)
        {
            Guid = guid;
            RequestId = requestId;
        }
    }

    public class FileLoadingEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public float Percent { get; private set; }
        public FileLoadingEventArgs(string filename, float percent)
        {
            FileName = filename;
            Percent = percent;
        }
    }

    public abstract class FileTransferClient
    {
        public string CurrentFileName { get; protected set; }
        public int CurrentFileNumber { get; protected set; }
        public float CurrentFileLoadingPercent { get; private set; }
        public int RequestId { get; set; }
        public bool IsCancel { get; private set; }
        public Message Message { get; set; }

        private List<string> loadedFiles = new List<string>();
        private int oldCurrentPercent = 0;

        public event EventHandler<ExceptionEventArgs> Error;
        public event EventHandler<FileLoadStartOrCompleteEventArgs> LoadComplete;
        public event EventHandler<EventArgs> Canceled;


        public FileTransferClient()
        {
            CurrentFileNumber = -1;
        }

        public void Cancel()
        {
            IsCancel = true;
            if (Canceled != null)
                Canceled(this, new EventArgs());
        }

        public string[] GetLoadedFiles()
        {
            lock (loadedFiles)
            {
                return loadedFiles.ToArray();
            }
        }

        protected virtual void OnLoadComplete(string filename)
        {
            if (LoadComplete != null)
                LoadComplete(this, new FileLoadStartOrCompleteEventArgs(filename));
            lock (loadedFiles)
            {
                loadedFiles.Add(filename);
            }
            if (Message != null)
                Message.NotifyPropertyChanged("Text");
        }


        protected virtual void OnLoading(string filename, float percent)
        {
            CurrentFileLoadingPercent = percent;
            int newPercent = (int)(percent / 10) * 10;
            if (newPercent != oldCurrentPercent)
            {
                oldCurrentPercent = newPercent;
                if (Message != null)
                    Message.NotifyPropertyChanged("Text");
            }
        }

        protected virtual void OnLoadStart(string filename)
        {
            CurrentFileName = filename;
            if (Message != null)
                Message.NotifyPropertyChanged("Text");
        }

        protected virtual void OnError(Exception ex)
        {
            if (Error != null)
                Error(this, new ExceptionEventArgs(ex));
        }

    }
}
