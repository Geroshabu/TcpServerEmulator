using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using TcpServerEmulator.Rules;

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

        /// <inheritdoc cref="ISave.SaveRules(string, RuleHolder)"/>
        public void SaveRules(string destinationPath, RuleHolder ruleHolder)
        {
            var serializer = new DataContractSerializer(
                typeof(IRule[]),
                pluginHolder.Plugins.Select(plugin => plugin.RuleType).Concat(new[] { typeof(IRule[]) }).ToArray());

            var setting = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(true),
                Indent = true
            };
            using var writer = XmlWriter.Create(destinationPath, setting);
            serializer.WriteObject(writer, ruleHolder.Rules.ToArray());
            writer.Close();
        }
    }
}
