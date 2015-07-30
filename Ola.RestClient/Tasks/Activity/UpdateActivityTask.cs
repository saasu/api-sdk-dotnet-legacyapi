using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Ola.RestClient.Dto;


namespace Ola.RestClient.Tasks
{
	[XmlRoot(ElementName = "updateActivityResult")]
	public class UpdateActivityResult : UpdateResult
	{
	}
	public class UpdateActivityTask: IUpdateTask
	{
		private ActivityDto _entityToUpdate = new ActivityDto();
		[XmlElement(ElementName = "activity", Type = typeof(ActivityDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._entityToUpdate;
			}
			set
			{
				this._entityToUpdate = (ActivityDto) value;
			}
		}
	}
}
