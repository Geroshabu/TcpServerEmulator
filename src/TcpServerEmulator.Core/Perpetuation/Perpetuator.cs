using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace TcpServerEmulator.Core.Perpetuation
{
    /// <summary>
    /// データの永続化を行うクラス
    /// </summary>
    public class Perpetuator : ISave, ILoad
    {
        private readonly RulePluginHolder pluginHolder;

        public Perpetuator(RulePluginHolder pluginHolder)
        {
            this.pluginHolder = pluginHolder;
        }

        /// <inheritdoc cref="ILoad.LoadProject(string)"/>
        public Project.Project LoadProject(string sourcePath)
        {
            var serializer = createSerializer();

            using var reader = XmlReader.Create(sourcePath);
            object? result = serializer.ReadObject(reader);
            reader.Close();

            if (result is Project.Project resultProject)
            {
                return resultProject;
            }
            else
            {
                throw new InvalidDataException($"{sourcePath} is not Project. (${result?.GetType()})");
            }
        }

        /// <inheritdoc cref="ISave.SaveProject(string, Project.Project)"/>
        public void SaveProject(string destinationPath, Project.Project project)
        {
            var serializer = createSerializer();

            var setting = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(true),
                Indent = true
            };
            using var writer = XmlWriter.Create(destinationPath, setting);
            serializer.WriteObject(writer, project);
            writer.Close();
        }

        private DataContractSerializer createSerializer()
        {
            return new DataContractSerializer(
                typeof(Project.Project),
                pluginHolder.Plugins.Select(plugin => plugin.RuleType).ToArray());
        }
    }
}
