﻿// This code is distributed under MIT license. 
// Copyright (c) 2010-2018 George Mamaladze
// See license.txt or https://mit-license.org/

using System.Windows.Forms;

namespace Metatool.Input.MouseKeyHook.Implementation
{
    internal static class KeysExtensionsInternal
    {
        public static Keys Normalize(this Keys key)
        {
            if ((key & Keys.LMenu) == Keys.LMenu ||
                (key & Keys.RMenu) == Keys.RMenu) return Keys.Alt;
            if ((key & Keys.LControlKey) == Keys.LControlKey ||
                (key & Keys.RControlKey) == Keys.RControlKey) return Keys.Control;
            if ((key & Keys.LShiftKey) == Keys.LShiftKey ||
                (key & Keys.RShiftKey) == Keys.RShiftKey) return Keys.Shift;
            return key;
        }
    }
}