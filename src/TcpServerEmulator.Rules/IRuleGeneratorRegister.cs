namespace TcpServerEmulator.Rules
{
    /// <summary>
    /// ルールジェネレータを登録する機能を表す
    /// </summary>
    public interface IRuleGeneratorRegister
    {
        /// <summary>
        /// ルールジェネレータを登録する
        /// </summary>
        /// <param name="ruleGenerator">登録するルールジェネレータ</param>
        void Register(IRuleGenerator ruleGenerator);
    }
}
