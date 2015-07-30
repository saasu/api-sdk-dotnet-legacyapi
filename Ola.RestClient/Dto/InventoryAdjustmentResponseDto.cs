using System;
using System.Xml;
using System.Xml.Serialization;

namespace Ola.RestClient.Dto
{
    [XmlRoot(ElementName = "inventoryAdjustmentResponse")]
    public class InventoryAdjustmentResponseDto : CrudResponseDto
    {
        [XmlElement(ElementName = "inventoryAdjustment")]
        public InventoryAdjustmentDto InventoryAdjustment;
    }
}
