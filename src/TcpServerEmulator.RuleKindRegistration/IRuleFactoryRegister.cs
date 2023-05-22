using TcpServerEmulator.Rules;

namespace TcpServerEmulator.RuleKindRegistration
{
    /// <summary>
    /// ルールファクトリを登録する機能を表す
    /// </summary>
    public interface IRuleFactoryRegister
    {
        /// <summary>
        /// ルールファクトリを登録する
        /// </summary>
        /// <param name="ruleFactory">登録するルールファクトリ</param>
        void Register(IRuleGenerator ruleFactory);
    }
}