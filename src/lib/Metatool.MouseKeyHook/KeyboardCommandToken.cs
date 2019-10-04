﻿using System.Collections.Generic;
using System.Linq;
using Metatool.Command;

namespace Metatool.Input
{
    public class KeyboardCommandToken : IKeyboardCommandToken
    {
        private readonly ICommandToken<IKeyEventArgs> _internalCommandToken;
        private readonly IKeyboardCommandTrigger _trigger;

        public KeyboardCommandToken(ICommandToken<IKeyEventArgs> internalCommandToken, IKeyboardCommandTrigger trigger)
        {
            _internalCommandToken = internalCommandToken;
            _trigger = trigger;
        }

        public bool Change(IHotkey key)
        {
            return _trigger.MetaKey.ChangeHotkey(key)!=null;
        }

        public void Remove()
        {
            _internalCommandToken.Remove();
        }

        public bool Change(ICommandTrigger<IKeyEventArgs> key)
        {
            return _internalCommandToken.Change(key);
        }

        internal IMetaKey metaKey => _trigger.MetaKey;
    }

    public class KeyboardCommandTokens: List<IKeyboardCommandToken>, IKeyboardCommandToken
    {
       
        public void Remove()
        {
            this.ForEach(t => t.Remove());
            this.Clear();
        }

        public bool Change(ICommandTrigger<IKeyEventArgs> trigger)
        {
            this.ForEach(t => t.Change(trigger));
            return true;
        }

        public bool Change(IHotkey key)
        {
            this.ForEach(t => t.Change(key));
            return true;
        }
        internal IMetaKey metaKey => new MetaKeys(this.Cast<KeyboardCommandToken>().Select(t=>t.metaKey).ToList());
    }
}