using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using TcpServerEmulator.Logger;

namespace TcpServerEmulator.Core.Server
{
    public class TcpServer
    {
        private TcpListener? listener;

        private Task? listenTask;

        private CancellationTokenSource? cancelTokenSource;

        private readonly object lockObject = new();

        private readonly ILogger dataLogger;
        private readonly DataHandler dataHandler;

        /// <summary>
        /// 接続を待ち受けるポート番号
        /// </summary>
        public PortNumber Port { get; set; }

        [MemberNotNullWhen(true, nameof(listener), nameof(listenTask), nameof(cancelTokenSource))]
        public bool IsRunning
        {
            get
            {
                lock (lockObject)
                {
                    return listener != null
                        && listenTask != null
                        && cancelTokenSource != null;
                }
            }
        }

        /// <summary>
        /// <see cref="IsRunning"/>が変わったときに発生する
        /// </summary>
        public event EventHandler? IsRunningChanged;

        public TcpServer(
            ILogger dataLogger,
            DataHandler dataHandler)
        {
            this.dataLogger = dataLogger;
            this.dataHandler = dataHandler;
        }

        public void Run()
        {
            lock (lockObject)
            {
                if (IsRunning)
                {
                    throw new InvalidOperationException();
                }

                listener = new TcpListener(IPAddress.Any, Port.Value);
                cancelTokenSource = new CancellationTokenSource();
                listenTask = listenAsync(cancelTokenSource.Token);

                IsRunningChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            lock (lockObject)
            {
                if (IsRunning)
                {
                    cancelTokenSource.Cancel();

                    listenTask.Wait();

                    cancelTokenSource = null;
                    listenTask = null;
                    listener = null;

                    IsRunningChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private async Task listenAsync(CancellationToken cancelToken)
        {
            Debug.Assert(listener != null);

            try
            {
                listener.Start();

                while (true)
                {
                    using var tcpClient = await listener.AcceptTcpClientAsync(cancelToken).ConfigureAwait(false);

                    if (tcpClient.Client.RemoteEndPoint is IPEndPoint clientEndPoint)
                    {
                        dataLogger.Log($"Connected: {clientEndPoint.Address}:{clientEndPoint.Port}");
                    }

                    tcpClient.NoDelay = true;

                    using var stream = tcpClient.GetStream();

                    while (true)
                    {
                        var buffer = new byte[256];
                        var size = 0;
                        do
                        {
                            size += await stream.ReadAsync(buffer, cancelToken).ConfigureAwait(false);
                        }
                        while (stream.DataAvailable);

                        // 切断
                        if (size == 0)
                        {
                            break;
                        }

                        var data = new Span<byte>(buffer, 0, size).ToArray();

                        dataLogger.Log($"> {convertMessage(data)}");

                        var res = dataHandler.HandleData(data);

                        dataLogger.Log($"< {convertMessage(res)}");

                        await stream.WriteAsync(res, cancelToken).ConfigureAwait(false);
                    }

                    dataLogger.Log("Disconnect");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                listener.Stop();
            }
        }

        private string convertMessage(byte[] data)
        {
            var text = System.Text.Encoding.ASCII.GetString(data);
            var characters = text.Select(c => char.IsControl(c) ? "0x" + Convert.ToByte(c).ToString() : c.ToString());
            return $"{{{string.Join(",", data)}}} 【{string.Join(",", characters)}】";
        }
    }
}
