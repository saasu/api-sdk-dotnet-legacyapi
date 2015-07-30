using System;
using System.Xml;
using System.Xml.Serialization;

namespace Ola.RestClient.Dto
{
    [XmlRoot(ElementName = "inventoryTransferResponse")]
    public class InventoryTransferResponseDto : CrudResponseDto
    {
        [XmlElement(ElementName = "inventoryTransfer")]
        public InventoryTransferDto InventoryTransfer;
    }
}
