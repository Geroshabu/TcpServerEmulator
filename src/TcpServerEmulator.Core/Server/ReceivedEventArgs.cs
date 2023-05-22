namespace TcpServerEmulator.Core.Server
{
    /// <summary>
    /// データを受信したときに発生するイベントのイベントデータ
    /// </summary>
    public class ReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 受信したデータ
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// <see cref="ReceivedEventArgs"/>インスタンスの生成と初期化
        /// </summary>
        /// <param name="data">受信したデータ</param>
        public ReceivedEventArgs(byte[] data)
        {
            Data = data;
        }
    }
}
