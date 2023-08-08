namespace TcpServerEmulator.Core.Perpetuation
{
    /// <summary>
    /// データを保存するためのインターフェイス
    /// </summary>
    public interface ISave
    {
        /// <summary>
        /// 現在のルールをファイルに保存する。
        /// </summary>
        /// <param name="destinationPath">保存先のファイルパス</param>
        /// <param name="ruleHolder">ルール保持者</param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="destinationPath"/>が空文字、または空白文字のみ
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///   実行ユーザーが、<paramref name="destinationPath"/>への書き込み権限を持っていない場合。
        ///   または、<paramref name="destinationPath"/>と同名のフォルダが存在する場合。
        /// </exception>
        /// <exception cref="IOException">
        ///   他のプロセスで使用中のため、<paramref name="destinationPath"/>への書き込みが出来ない場合。
        ///   または、<paramref name="destinationPath"/>がネットワークパスの場合に、そのネットワークパスが見つからない場合。
        ///   または、ファイル名、ディレクトリ名、またはボリューム ラベルの構文が間違っています。
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///   <paramref name="destinationPath"/>に至る途中のパスが存在しない場合
        /// </exception>
        /// <exception cref="System.Runtime.Serialization.InvalidDataContractException">
        ///   <paramref name="ruleHolder"/>内に、シリアライズ方法が不明な型が含まれていた場合
        /// </exception>
        void SaveRules(string destinationPath, RuleHolder ruleHolder);
    }
}
