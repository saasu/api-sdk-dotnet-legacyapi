using System;
using System.Xml.Serialization;

namespace Ola.RestClient.Dto
{
    public class InventoryTransferItemDto
    {
        [XmlElement(ElementName = "quantity")]
        public decimal Quantity;

        [XmlElement(ElementName = "inventoryItemUid")]
        public int InventoryItemUid;

        [XmlElement(ElementName = "unitPriceExclTax")]
        public decimal UnitPriceExclTax;

        [XmlElement(ElementName = "totalPriceExclTax")]
        public decimal TotalPriceExclTax;
    }
}
