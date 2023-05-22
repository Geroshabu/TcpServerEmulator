namespace TcpServerEmulator.Logger.OnMemory
{
    /// <summary>
    /// 画面表示のためにメモリ上に文言を保持するロガー
    /// </summary>
    public class Logger : ILogger
    {
        private readonly LinkedList<string> messages = new();

        private readonly object lockObject = new();

        /// <summary>メッセージが追加されたときに発生する</summary>
        public event EventHandler? MessageAdded;

        /// <summary>本ロガーで蓄積している全てのメッセージ</summary>
        public string JoinedMessage
        {
            get
            {
                lock (lockObject)
                {
                    return string.Join(Environment.NewLine, messages);
                }
            }
        }

        /// <inheritdoc cref="ILogger.Log(string)"/>
        public void Log(string message)
        {
            lock (lockObject)
            {
                if (messages.Count >= 1000)
                {
                    messages.RemoveLast();
                }
                messages.AddFirst(message);

                MessageAdded?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}