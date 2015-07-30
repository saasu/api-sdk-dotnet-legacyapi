namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;
	
	
	[XmlRoot(ElementName = "updateInventoryItem")]
	public class UpdateInventoryItemTask : IUpdateTask
	{
		#region IUpdateTask Members
		
		private InventoryItemDto _EntityToUpdate = new InventoryItemDto();
		[XmlElement(ElementName = "inventoryItem", Type = typeof(InventoryItemDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._EntityToUpdate;
			}
			set
			{
				this._EntityToUpdate = (InventoryItemDto) value;
			}
		}

		#endregion
	}
}
