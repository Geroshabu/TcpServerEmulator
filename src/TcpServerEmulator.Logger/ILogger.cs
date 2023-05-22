namespace TcpServerEmulator.Logger
{
    /// <summary>
    /// ロガーを表すインターフェイス
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// ログを残す
        /// </summary>
        /// <param name="message">残したい文言</param>
        void Log(string message);
    }
}