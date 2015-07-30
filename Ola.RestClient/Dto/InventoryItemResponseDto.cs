namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "inventoryItemResponse")]
	public class InventoryItemResponseDto : CrudResponseDto
	{
		[XmlElement(ElementName = "inventoryItem")]
		public InventoryItemDto InventoryItem;
	}
}
