using TcpServerEmulator.Rules;

namespace TcpServerEmulator.Core
{
    /// <summary>
    /// ルールジェネレータを保持しておくクラス
    /// </summary>
    public class RuleGeneratorHolder : IRuleGeneratorRegister
    {
        private readonly List<IRuleGenerator> generators = new();

        /// <summary>
        /// 保持しているルールジェネレータ
        /// </summary>
        public IEnumerable<IRuleGenerator> Generators => generators.AsReadOnly();

        /// <summary>
        /// 新しくルールジェネレータが登録されたときに発生する
        /// </summary>
        public event EventHandler<RuleGeneratorRegisteredEventArgs>? GeneratorRegistered;

        /// <inheritdoc cref="IRuleGeneratorRegister.Register(IRuleGenerator)"/>
        public void Register(IRuleGenerator ruleGenerator)
        {
            generators.Add(ruleGenerator);
            GeneratorRegistered?.Invoke(this, new RuleGeneratorRegisteredEventArgs(ruleGenerator));
        }
    }
}
