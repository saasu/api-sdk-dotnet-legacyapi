using System;
using System.Collections;
using System.Xml.Serialization;

namespace Ola.RestClient.Dto
{
    [XmlRoot(ElementName = "inventoryTransfer")]
    public class InventoryTransferDto : EntityDto
    {
        [XmlElement(ElementName = "date", DataType = "date")]
        public DateTime Date;

        [XmlElement(ElementName = "tags")]
        public string Tags;

        [XmlElement(ElementName = "summary")]
        public string Summary;

        [XmlElement(ElementName = "notes")]
        public string Notes;

        [XmlElement(ElementName = "requiresFollowUp")]
        public bool RequiresFollowUp = false;

        [XmlArray(ElementName = "items")]
        [XmlArrayItem(ElementName = "item", Type = typeof(InventoryTransferItemDto))]
        public ArrayList Items = new ArrayList();
    }
}
