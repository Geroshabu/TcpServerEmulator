namespace TcpServerEmulator.Core.Perpetuation
{
    /// <summary>
    /// データを読み込むためのインターフェイス
    /// </summary>
    public interface ILoad
    {
        /// <summary>
        /// 指定されたファイルからプロジェクトを読み込む
        /// </summary>
        /// <param name="sourcePath">読み込むファイルパス</param>
        /// <returns>読み込んだプロジェクト</returns>
        /// <exception cref="ArgumentException">
        ///   <paramref name="sourcePath"/>が空など、パスとして正しくない場合
        /// </exception>
        /// <exception cref="IOException">
        ///   <paramref name="sourcePath"/>に使用不可文字が含まれているなど、パスとして正しくない場合。
        ///   または、<paramref name="sourcePath"/>がネットワークパスの場合に、そのネットワークパスが見つからない場合。
        ///   または、他のプロセスで使用中のため、<paramref name="sourcePath"/>へのアクセスが出来ない場合。
        /// </exception>
        /// <exception cref="FileNotFoundException">
        ///   <paramref name="sourcePath"/>で示されるファイルが見つからない場合
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///   <paramref name="sourcePath"/>で示されるファイルまでのパスの一部が見つからない場合
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///   <paramref name="sourcePath"/>と同名のディレクトリがある場合、
        ///   または<paramref name="sourcePath"/>にアクセス権がない場合
        /// </exception>
        /// <exception cref="System.Runtime.Serialization.InvalidDataContractException">
        ///   setter が無かったり、<see cref="System.Runtime.Serialization.DataContractAttribute"/>
        ///   が付与されていないなど、デシリアライズしたインスタンスを生成できない場合
        /// </exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        ///   フォーマットが異なっているなどでデシリアライズできない場合
        /// </exception>
        Project.Project LoadProject(string sourcePath);
    }
}
