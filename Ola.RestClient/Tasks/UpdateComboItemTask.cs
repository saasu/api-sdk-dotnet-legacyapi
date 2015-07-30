using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Inventory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ola.RestClient.Tasks
{
	[XmlRoot(ElementName = "updateComboItem")]
	public class UpdateComboItemTask : IUpdateTask
	{
		private ComboItemDto _EntityToUpdate = new ComboItemDto();
		[XmlElement(ElementName = "comboItem", Type = typeof(ComboItemDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._EntityToUpdate;
			}
			set
			{
				this._EntityToUpdate = (ComboItemDto)value;
			}
		}
	}
}
