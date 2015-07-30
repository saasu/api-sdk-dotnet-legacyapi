using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Inventory;

namespace Ola.RestClient.Tasks
{
	public class InsertComboItemTask : IInsertTask
	{
		#region IInsertTask Members 
			
		private ComboItemDto _EntityToInsert = new ComboItemDto();
		[XmlElement(ElementName = "comboItem", Type = typeof(ComboItemDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._EntityToInsert;
			}
			set
			{
				this._EntityToInsert = (ComboItemDto)value;
			}
		}

		#endregion
	}
}
