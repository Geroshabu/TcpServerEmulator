namespace TcpServerEmulator.Rules
{
    public interface IRule
    {
        /// <summary>ルールの名前</summary>
        string Name { get; }

        /// <summary>このルールを説明する文</summary>
        string Description { get; }

        /// <summary>受け取ったデータに対して応答できるか</summary>
        /// <param name="receivedData">クライアントから受け取ったデータ</param>
        /// <returns>応答できる場合はtrue、そうでない場合はfalse</returns>
        bool CanResponse(byte[] receivedData);

        /// <summary>受け取ったデータに対して応答データを作成する</summary>
        /// <param name="receivedData">受け取ったデータ</param>
        /// <returns>受け取ったデータに対する応答データ</returns>
        /// <remarks><see cref="CanResponse(byte[])"/>が<c>true</c>の場合のみ呼ばれる</remarks>
        byte[] GetResponse(byte[] receivedData);
    }
}