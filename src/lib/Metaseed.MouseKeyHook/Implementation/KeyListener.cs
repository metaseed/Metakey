// This code is distributed under MIT license. 
// Copyright (c) 2015 George Mamaladze
// See license.txt or https://mit-license.org/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Metaseed.Input.MouseKeyHook.WinApi;

namespace Metaseed.Input.MouseKeyHook.Implementation
{
    internal abstract class KeyListener : BaseListener, IKeyboardEvents
    {
        protected KeyListener(Subscribe subscribe)
            : base(subscribe)
        {
        }

        public event KeyEventHandler KeyDown;
        public event KeyPressEventHandler KeyPress;
        public event KeyEventHandler KeyUp;

        public void InvokeKeyDown(KeyEventArgsExt e)
        {
            var handler = KeyDown;
            if (handler == null || e.Handled || !e.IsKeyDown)
                return;
            handler(this, e);
        }

        public void InvokeKeyPress(KeyPressEventArgsExt e)
        {
            var handler = KeyPress;
            if (handler == null || e.Handled || e.IsNonChar)
                return;
            handler(this, e);
            Console.WriteLine(new String('\t', _indientCounter)+e.ToString());

        }

        public void InvokeKeyUp(KeyEventArgsExt e)
        {
            var handler = KeyUp;
            if (handler == null || e.Handled || !e.IsKeyUp)
                return;
            handler(this, e);
        }

        private int _indientCounter = 0;

        protected override bool Callback(CallbackData data)
        {
            var eDownUp = GetDownUpEventArgs(data);

            Console.WriteLine(new String('\t',_indientCounter++) + "��" + eDownUp.ToString());

            InvokeKeyDown(eDownUp);

            if (KeyPress != null)
            {
                var pressEventArgs = GetPressEventArgs(data);
                foreach (var pressEventArg in pressEventArgs)
                    InvokeKeyPress(pressEventArg);
            }

            InvokeKeyUp(eDownUp);
            Console.WriteLine(new String('\t', --_indientCounter)+ "��" + eDownUp.ToString());
            return !eDownUp.Handled;
        }

        protected abstract IEnumerable<KeyPressEventArgsExt> GetPressEventArgs(CallbackData data);
        protected abstract KeyEventArgsExt GetDownUpEventArgs(CallbackData data);
    }
}