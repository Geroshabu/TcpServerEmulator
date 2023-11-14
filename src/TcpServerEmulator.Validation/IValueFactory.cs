using System.Diagnostics.CodeAnalysis;

namespace TcpServerEmulator.Validation
{
    /// <summary>
    /// アプリで使う様々な型を生成するインターフェイス
    /// </summary>
    /// <typeparam name="T">生成したい型</typeparam>
    public interface IValueFactory<T>
    {
        /// <summary>
        /// 文字列を、<see cref="T"/>インスタンスに変換できるかを試みる
        /// </summary>
        /// <param name="text">変換したい文字列</param>
        /// <param name="result">変換後のインスタンス</param>
        /// <returns>変換が成功した場合は<c>true</c>、それ以外は<c>false</c></returns>
        bool TryParse(string text, [NotNullWhen(true)] out T? result);
    }
}