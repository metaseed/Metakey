﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Metaseed.DataStructures;
using Metaseed.Input.MouseKeyHook.Implementation;
using Metaseed.Input.MouseKeyHook.Implementation.Trie;
using Metaseed.MetaKeyboard;

namespace Metaseed.Input
{
    public enum KeyProcessState
    {
        // well processed
        Done,

        // continue handling
        Continue,

        // can't handle
        Yield
    }

    public class KeyStateMachine
    {
        private readonly Trie<ICombination, KeyEventAction>       _trie = new Trie<ICombination, KeyEventAction>();
        private readonly TrieWalker<ICombination, KeyEventAction> _stateWalker;

        public KeyStateMachine()
        {
            _stateWalker = new TrieWalker<ICombination, KeyEventAction>(_trie);
        }

        public class CombinationRemoveToken : IRemovable
        {
            private readonly ITrie<ICombination, KeyEventAction> _trie;
            private readonly IList<ICombination>                 _combinations;
            private readonly KeyEventAction                      _action;

            public CombinationRemoveToken(ITrie<ICombination, KeyEventAction> trie, IList<ICombination> combinations,
                KeyEventAction action)
            {
                _trie = trie;
                _combinations = combinations;
                _action = action;
            }

            public void Remove()
            {
                var r = _trie.Remove(_combinations, action => action.Equals(_action));
                Console.WriteLine(r);
            }
        }

        public void Reset()
        {
            _stateWalker.GoToRoot();
        }

        public IEnumerable<(string key, IEnumerable<string> descriptions)> Tips
            => _stateWalker.CurrentNode.Tip;

        public IRemovable Add(IList<ICombination> combinations, KeyEventAction action)
        {
            _trie.Add(combinations, action);
            return new CombinationRemoveToken(_trie, combinations, action);
        }

        public IRemovable Add(ICombination combination, KeyEventAction action)
        {
            return Add(new List<ICombination> {combination}, action);
        }

        public KeyProcessState KeyEventProcess(KeyEvent eventType, KeyEventArgsExt args)
        {
            // to handle A+B+C(B is down in Chord)
            var downInChord = false;

            var cadidateChild = _stateWalker.GetChildOrNull((ICombination acc, ICombination combination) =>
            {
                if (eventType == KeyEvent.Down && combination.Chord.Contains(args.KeyCode)) downInChord = true;

                if (args.KeyCode != combination.TriggerKey) return acc;
                var mach = combination.Chord.All(args.KeyboardState.IsDown);
                if (!mach) return acc;
                if (acc == null) return combination;
                return acc.ChordLength >= combination.ChordLength ? acc : combination;
            });

            // no match
            if (cadidateChild == null)
            {
                if (eventType == KeyEvent.Down && downInChord)
                {
                    return KeyProcessState.Continue; // waiting for trigger key
                }

                Reset();
                return KeyProcessState.Yield;
            }

            // matched
            var actionList = cadidateChild.Values() as KeyActionList<KeyEventAction>;
            Debug.Assert(actionList != null, nameof(actionList) + " != null");

            // execute
#if !DEBUG
                try
                {
#endif
            foreach (var keyEventAction in actionList[eventType])
            {
                if (!string.IsNullOrEmpty(keyEventAction.Description))
                    Console.WriteLine(keyEventAction.Description);
                keyEventAction.Action?.Invoke(args);
            }
#if !DEBUG
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
#endif

            if (eventType == KeyEvent.Up)
            {
                // only navigate on up event to handle both the down actions and the up actions
                _stateWalker.GoToChild(cadidateChild);

                if (cadidateChild.ChildrenCount == 0)
                {
                    Reset();
                    return KeyProcessState.Done;
                }

                Notify.ShowKeysTip(cadidateChild.Tip);
                return KeyProcessState.Continue;
            }

            return KeyProcessState.Continue;
        }
    }
}