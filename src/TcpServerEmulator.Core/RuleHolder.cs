using TcpServerEmulator.Rules;

namespace TcpServerEmulator.Core
{
    public class RuleHolder
    {
        private readonly List<IRule> rules = new();

        /// <summary>ルールが追加されたときに発生する</summary>
        public event EventHandler<RuleAddedEventArgs>? RuleAdded;

        /// <summary>ルールが削除されたときに発生する</summary>
        public event EventHandler<RuleRemovedEventArgs>? RuleRemoved;

        /// <summary>保持しているルール一覧</summary>
        public IEnumerable<IRule> Rules => rules.AsReadOnly();

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
    }
}
