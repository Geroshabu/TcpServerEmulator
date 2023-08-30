namespace TcpServerEmulator.Core.Project
{
    /// <summary>
    /// 現在、アプリに読み込まれているプロジェクトを保持する
    /// </summary>
    public class ProjectHolder
    {
        /// <summary>
        /// 現在アプリに読み込まれているプロジェクトが変更された場合に発生する。
        /// </summary>
        public EventHandler<CurrentProjectChangedEventArgs>? CurrentProjectChanged;

        private Project project;

        /// <summary>
        /// 現在アプリに読み込まれているプロジェクト。
        /// </summary>
        public Project Current
        {
            get => project;
            set
            {
                if (project == value)
                {
                    return;
                }
                var oldProject = project;
                project = value;
                CurrentProjectChanged?.Invoke(
                    this, new CurrentProjectChangedEventArgs(oldProject, value));
            }
        }

        public ProjectHolder()
        {
            project = new Project();
        }
    }
}
