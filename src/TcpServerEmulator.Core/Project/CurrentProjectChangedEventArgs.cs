namespace TcpServerEmulator.Core.Project
{
    /// <summary>
    /// 現在アプリに読み込まれているプロジェクトが変更された
    /// ときに発生するイベントのイベントデータ
    /// </summary>
    public class CurrentProjectChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 変更される前にアプリに読み込まれていたプロジェクト。
        /// 変更される前にアプリにプロジェクトが
        /// 読み込まれていなかった場合は<c>null</c>。
        /// </summary>
        public Project? OldProject { get; }

        /// <summary>
        /// 新しく読み込まれたプロジェクト。
        /// </summary>
        public Project NewProject { get; }

        /// <summary>
        /// <see cref="CurrentProjectChangedEventArgs"/>インスタンスの生成と初期化。
        /// </summary>
        /// <param name="oldProject"><see cref="OldProject"/>の初期値</param>
        /// <param name="newProject"><see cref="NewProject"/>の初期値</param>
        public CurrentProjectChangedEventArgs(Project? oldProject, Project newProject)
        {
            OldProject = oldProject;
            NewProject = newProject;
        }
    }
}
