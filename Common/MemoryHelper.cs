using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace Common
{
    public class StreamReadingException : Exception { }

    public class RemoteClientReturnsZiroException : Exception { }

    public static class MemoryHelper
    {

        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = new T();
            object obj = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            stuff = (T)obj;
            handle.Free();
            return stuff;
        }

        public static T ByteArrayToStructure<T>(byte[] bytes, int offset) where T : struct
        {
            byte[] tmp = new byte[bytes.Length - offset];
            Buffer.BlockCopy(bytes, offset, tmp, 0, bytes.Length - offset);
            return ByteArrayToStructure<T>(tmp);
        }

        public static T ByteArrayToStructure<T>(byte[] bytes, int offset, int count) where T : struct
        {
            byte[] tmp = new byte[count];
            Buffer.BlockCopy(bytes, offset, tmp, 0, count);
            return ByteArrayToStructure<T>(tmp);
        }

        public static byte[] StructureToByteArray<T>(T structure) where T : struct
        {
            int headerSize = Marshal.SizeOf(typeof(T));
            IntPtr p = Marshal.AllocHGlobal(headerSize);
            Marshal.StructureToPtr(structure, p, false);
            byte[] bytes = new byte[headerSize];
            Marshal.Copy(p, bytes, 0, headerSize);
            Marshal.FreeHGlobal(p);
            return bytes;
        }

        public static byte[] JoinBuffers(byte[] buf1, byte[] buf2)
        {
            byte[] bytes = new byte[buf1.Length + buf2.Length];
            Buffer.BlockCopy(buf1, 0, bytes, 0, buf1.Length);
            Buffer.BlockCopy(buf2, 0, bytes, buf1.Length, buf2.Length);
            return bytes;
        }
        public static T ReadStructureFromStream<T>(System.IO.Stream stream) where T : struct
        {
            return ReadStructureFromStream<T>(stream, 60000);
        }
        public static T ReadStructureFromStream<T>(System.IO.Stream stream, int milliseconds) where T: struct
        {
            int len = SizeOfStructure<T>();
            byte[] buffer = new byte[len];
            FillBufferFromStream(buffer, stream, milliseconds);
            return ByteArrayToStructure<T>(buffer);
        }

        public static void FillBufferFromStream(byte[] buffer, System.IO.Stream stream)
        {
            FillBufferFromStream(buffer, stream, -1);
        }

        public static void FillBufferFromStream(byte[] buffer, System.IO.Stream stream, int milliseconds)
        {
            int totalReceived = 0;
            while (totalReceived < buffer.Length)
            {
                ManualResetEvent readCompleteEv = new ManualResetEvent(false);
                IAsyncResult result = stream.BeginRead(buffer, totalReceived, buffer.Length - totalReceived, null, null);

                if (!result.AsyncWaitHandle.WaitOne(milliseconds, false))
                    throw new StreamReadingException();

                if (!stream.CanRead)
                    throw new StreamReadingException();

                int bytesRead = stream.EndRead(result);
                if (bytesRead > 0)
                    totalReceived += bytesRead;
                else
                    throw new RemoteClientReturnsZiroException();
            }
        }

        public static int SizeOfStructure<T>() where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            return size;
        }

    }
}
