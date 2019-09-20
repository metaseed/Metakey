﻿using System.Runtime.InteropServices;
using System.Text;
using Clipboard.Core.Desktop.ComponentModel;
using Clipboard.Core.Desktop.Enums;

namespace Clipboard.Core.Desktop.Interop.Interfaces
{
    [ComImport, Guid(Consts.PersistFile), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPersistFile
    {
        uint GetCurFile([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile);

        uint IsDirty();

        uint Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.U4)] Stgm dwMode);

        uint Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, bool fRemember);

        uint SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
    }
}
