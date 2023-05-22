namespace TcpServerEmulator.Rules.ReturnConstant
{
    /// <summary>
    /// 受け取った値に対して、固定値を応答するルール
    /// </summary>
    internal class Rule : IRule
    {
        /// <inheritdoc cref="IRule.Name"/>
        public string Name { get; }

        /// <summary>受け取ったデータ</summary>
        public byte[] ReceiveData { get; }

        /// <summary>返却するデータ</summary>
        public byte[] ResponseData { get; }

        /// <inheritdoc cref="IRule.Description"/>
        public string Description
        {
            get
            {
                return "{" + string.Join(',', ReceiveData.Take(10))
                    + (ReceiveData.Length > 10 ? "..." : string.Empty) + "}に対し{"
                    + string.Join(',', ResponseData.Take(10))
                    + (ResponseData.Length > 10 ? "..." : string.Empty) + "}を返します。";
            }
        }

        public Rule(
            string name,
            string receiveData,
            string responseData)
        {
            Name = name;

            var receiveDataTexts = receiveData.Split(',');
            if (!receiveDataTexts.All(num => byte.TryParse(num, out _)))
            {
                throw new ArgumentException($"parameter {receiveData} can not parse into byte array", nameof(receiveData));
            }
            ReceiveData = receiveDataTexts.Select(byte.Parse).ToArray();

            var responseDataTexts = responseData.Split(',');
            if (!responseDataTexts.All(num => byte.TryParse(num, out _)))
            {
                throw new ArgumentException($"parameter {responseData} can not parse into byte array", nameof(responseData));
            }
            ResponseData = responseDataTexts.Select(byte.Parse).ToArray();
        }

        /// <inheritdoc cref="IRule.CanResponse(byte[])"/>
        public bool CanResponse(byte[] receivedData)
        {
            return ReceiveData.SequenceEqual(receivedData);
        }

        /// <inheritdoc cref="IRule.GetResponse(byte[])"/>
        public byte[] GetResponse(byte[] receivedData)
        {
            return ResponseData;
        }
    }
}
