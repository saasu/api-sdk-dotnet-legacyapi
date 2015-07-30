using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ola.RestClient.Tasks.Inventory
{
    public class BuildComboItemResult : TaskResult
    {
        [XmlAttribute("uid")]
        public int Uid;

        [XmlAttribute("lastUpdatedUid")]
        public string LastUpdatedUid;
    }
}
