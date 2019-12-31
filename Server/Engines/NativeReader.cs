﻿using System;
using System.Runtime.InteropServices;

namespace Server
{
    public static class NativeReader
    {

        private static readonly INativeReader m_NativeReader;

        static NativeReader()
        {
            if (Core.Unix)
                m_NativeReader = new NativeReaderUnix();
            else
                m_NativeReader = new NativeReaderWin32();
        }

        public static unsafe void Read(IntPtr ptr, void* buffer, int length)
        {
            m_NativeReader.Read(ptr, buffer, length);
        }
    }

    public interface INativeReader
    {
        unsafe void Read(IntPtr ptr, void* buffer, int length);
    }

    public sealed class NativeReaderWin32 : INativeReader
    {
        [DllImport("kernel32")]
        private unsafe static extern int _lread(IntPtr hFile, void* lpBuffer, int wBytes);

        public NativeReaderWin32()
        {
        }

        public unsafe void Read(IntPtr ptr, void* buffer, int length)
        {
            _lread(ptr, buffer, length);
        }
    }

    public sealed class NativeReaderUnix : INativeReader
    {
        [DllImport("libc")]
        private unsafe static extern int read(IntPtr ptr, void* buffer, int length);

        public NativeReaderUnix()
        {
        }

        public unsafe void Read(IntPtr ptr, void* buffer, int length)
        {
            read(ptr, buffer, length);
        }
    }
}