namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class InsertInventoryItemTask : IInsertTask
	{
		#region IInsertTask Members

		private InventoryItemDto _EntityToInsert = new InventoryItemDto();
		[XmlElement(ElementName = "inventoryItem", Type = typeof(InventoryItemDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._EntityToInsert;
			}
			set
			{
				this._EntityToInsert = (InventoryItemDto) value;
			}
		}

		#endregion
	}
}
