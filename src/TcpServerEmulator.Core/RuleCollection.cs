using System.Collections;
using System.Runtime.Serialization;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator.Core
{
    [DataContract]
    public class RuleCollection : IEnumerable<IRule>
    {
        [DataMember]
        private readonly List<IRule> rules = new();

        /// <summary>ルールが追加されたときに発生する</summary>
        public event EventHandler<RuleAddedEventArgs>? RuleAdded;

        /// <summary>ルールが削除されたときに発生する</summary>
        public event EventHandler<RuleRemovedEventArgs>? RuleRemoved;

        /// <summary>ルールが入れ替わったときに発生する</summary>
        public event EventHandler<RuleReplacedEventArgs>? RuleReplaced;

        public void AddRule(IRule rule)
        {
            rules.Add(rule);
            RuleAdded?.Invoke(this, new RuleAddedEventArgs(rule));
        }

        public void RemoveRule(IRule rule)
        {
            rules.Remove(rule);
            RuleRemoved?.Invoke(this, new RuleRemovedEventArgs(rule));
        }

        /// <summary>
        /// コレクション内のルールインスタンスを入れ替える
        /// </summary>
        /// <param name="oldRule">入れ替える前の古いルール</param>
        /// <param name="newRule">入れ替えたい新しいルール</param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="oldRule"/>がコレクションの中に存在しない
        /// </exception>
        public void ReplaceRule(IRule oldRule, IRule newRule)
        {
            var index = rules.IndexOf(oldRule);
            if (index == -1)
            {
                throw new InvalidOperationException();
            }
            rules[index] = newRule;
            RuleReplaced?.Invoke(this, new RuleReplacedEventArgs(index, oldRule, newRule));
        }

        public IEnumerator<IRule> GetEnumerator() => rules.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => rules.GetEnumerator();
    }
}
