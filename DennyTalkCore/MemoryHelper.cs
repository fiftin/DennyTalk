using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace DennyTalk
{
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
    }
}
