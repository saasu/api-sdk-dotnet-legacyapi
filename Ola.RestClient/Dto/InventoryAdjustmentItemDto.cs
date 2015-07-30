using System;
using System.Xml.Serialization;

namespace Ola.RestClient.Dto
{
    public class InventoryAdjustmentItemDto
    {
        [XmlElement(ElementName = "quantity")]
        public decimal Quantity;

        [XmlElement(ElementName = "inventoryItemUid")]
        public int InventoryItemUid;

        [XmlElement(ElementName = "accountUid")]
        public int AccountUid;

        [XmlElement(ElementName = "unitPriceExclTax")]
        public decimal UnitPriceExclTax;

        [XmlElement(ElementName = "totalPriceExclTax")]
        public decimal TotalPriceExclTax;
    }
}
