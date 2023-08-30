using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServerEmulator.Core
{
    public class DataHandler
    {
        private readonly RuleCollection ruleCollection;

        public DataHandler(
            RuleCollection ruleCollection)
        {
            this.ruleCollection = ruleCollection;
        }

        public byte[] HandleData(byte[] data)
        {
            var rule = ruleCollection.FirstOrDefault(rule => rule.CanResponse(data));
            if (rule == null)
            {
                return new byte[] { 0x0d, 0x0a };
            }

            return rule.GetResponse(data);
        }
    }
}
