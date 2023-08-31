using TcpServerEmulator.Core.Project;

namespace TcpServerEmulator.Core
{
    public class DataHandler
    {
        private readonly ProjectHolder projectHolder;

        public DataHandler(
            ProjectHolder projectHolder)
        {
            this.projectHolder = projectHolder;
        }

        public byte[] HandleData(byte[] data)
        {
            var rule = projectHolder.Current.RuleCollection.FirstOrDefault(rule => rule.CanResponse(data));
            if (rule == null)
            {
                return new byte[] { 0x0d, 0x0a };
            }

            return rule.GetResponse(data);
        }
    }
}
