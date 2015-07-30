using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ola.RestClient.Dto.Inventory
{
    [XmlRoot(ElementName = "comboItem")]
    public class ComboItemDto : InventoryItemDto
    {
        [XmlArray(ElementName = "items")]
        [XmlArrayItem(ElementName = "item")]
        public List<ComboItemLineItemDto> Items;
    }

    public class ComboItemLineItemDto
    {
        [XmlElement(ElementName = "uid")]
        public int Uid;

		[XmlElement(ElementName = "code")]
		public string Code;

        [XmlElement(ElementName = "quantity")]
        public decimal Quantity;
    }
}
