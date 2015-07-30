using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ola.RestClient.Dto.Inventory
{
    [XmlRoot(ElementName = "comboItemResponse")]
    public class ComboItemResponseDto : CrudResponseDto
    {
        [XmlElement(ElementName = "comboItem")]
        public ComboItemDto ComboItem;
    }
}
