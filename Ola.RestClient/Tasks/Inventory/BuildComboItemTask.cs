using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ola.RestClient.Tasks.Inventory
{
    [XmlRoot(ElementName = "buildComboItem")]
    public class BuildComboItemTask : ITask
    {
        public BuildComboItemTask()
        {
        }

        public BuildComboItemTask(int uid, decimal quantity)
        {
            Uid = uid;
            Quantity = quantity;
        }

        [XmlElement(ElementName = "uid")]
        public int Uid;

        [XmlElement(ElementName = "quantity")]
        public decimal Quantity;
    }
}
