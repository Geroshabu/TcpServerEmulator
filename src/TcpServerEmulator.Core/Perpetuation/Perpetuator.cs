using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace TcpServerEmulator.Core.Perpetuation
{
    /// <summary>
    /// データの永続化を行うクラス
    /// </summary>
    public class Perpetuator : ISave
    {
        private readonly RulePluginHolder pluginHolder;

        public Perpetuator(RulePluginHolder pluginHolder)
        {
            this.pluginHolder = pluginHolder;
        }

        /// <inheritdoc cref="ISave.SaveProject(string, Project.Project)"/>
        public void SaveProject(string destinationPath, Project.Project project)
        {
            var serializer = new DataContractSerializer(
                typeof(Project.Project),
                pluginHolder.Plugins.Select(plugin => plugin.RuleType).ToArray());

            var setting = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(true),
                Indent = true
            };
            using var writer = XmlWriter.Create(destinationPath, setting);
            serializer.WriteObject(writer, project);
            writer.Close();
        }
    }
}
