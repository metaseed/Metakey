using System;
using System.Collections.Generic;
using System.Linq;
using Metaseed.Input;
using Metaseed.Input.MouseKeyHook.Implementation;

namespace Metaseed.DataStructures
{
    public class TrieNode<TKey, TValue> : TrieNodeBase<TKey, TValue> where TValue: KeyEventAction
    {
        protected readonly Dictionary<TKey, TrieNode<TKey, TValue>> _children;
        private IList<TValue> _values = new KeyActionList<TValue>();

        protected TrieNode()
        {
            _children = new Dictionary<TKey, TrieNode<TKey, TValue>>();
        }
        internal Dictionary<TKey, TrieNode<TKey, TValue>> ChildrenPairs  => _children;


        internal void Clear()
        {
            _children.Clear();
            _values.Clear();
        }

        protected internal override int ChildrenCount => _children.Count;


        internal string Tip=>string.Join(Environment.NewLine,_children.Select(p=>p.Key + " "+ string.Join(";", p.Value._values.Select(a=>(a.KeyEvent==KeyEvent.Up?"��": "��") + a.Action.Description))));

        protected override IEnumerable<TrieNodeBase<TKey, TValue>> Children()
        {
            return _children.Values;
        }

        protected internal override IEnumerable<TValue> Values()
        {
            return _values;
        }

        protected override bool IsRemovable(IList<TKey> query, int position)
        {
            return position < query.Count && _children.Count == 1 && _values.Count == 0 && _children.ContainsKey(query[position]) ||
                   position == query.Count && _values.Count == 0 && _children.Count == 0;
        }

        protected override TrieNodeBase<TKey, TValue> GetOrCreateChild(TKey key)
        {
            if (_children.TryGetValue(key, out var result)) return result;
            result = new TrieNode<TKey, TValue>();
            _children.Add(key, result);
            return result;
        }

        internal TrieNode<TKey, TValue> GetChildOrNull(Func<TKey, TKey, TKey> aggregateFunc, TKey initKey)
        {
            var key = _children.Keys.Aggregate(initKey, aggregateFunc);
            if (EqualityComparer<TKey>.Default.Equals(key, default(TKey))) return null;
            return GetChildOrNull(key);
        }
        internal TrieNode<TKey, TValue> GetChildOrNull(TKey key)
        {

            if (key == null) throw new ArgumentNullException(nameof(key));
            return _children.TryGetValue(key, out var childNode)
                    ? childNode
                    : null;
        }
        protected override TrieNodeBase<TKey, TValue> GetChildOrNull(IList<TKey> query, int position)
        {
            return GetChildOrNull(query[position]);
        }

        protected override void AddValue(TValue value)
        {
            _values.Add(value);
        }

        protected override bool RemoveValue(Predicate<TValue> predicate)
        {
            if (predicate == null)
            {
                _values.Clear();
                return true;
            }

            var i = _values.FirstOrDefault(v => predicate(v));
            if (!object.Equals(i, default(TValue))) return false;
            _values.Remove(i);
            return true;
        }

        protected override void RemoveChild(TKey key)
        {
            _children.Remove(key);
        }
    }
}