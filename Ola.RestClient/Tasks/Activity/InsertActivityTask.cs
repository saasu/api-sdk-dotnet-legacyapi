using Ola.RestClient.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ola.RestClient.Tasks
{
	[XmlRoot(ElementName = "insertActivityResult")]
	public class InsertActivityResult : InsertResult
	{
	}

	public class InsertActivityTask : IInsertTask
	{
		#region IInsertTask Members

		private ActivityDto _entityToInsert = new ActivityDto();
		[XmlElement(ElementName = "activity", Type = typeof(ActivityDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._entityToInsert;
			}
			set
			{
				this._entityToInsert = (ActivityDto)value;
			}
		}

		#endregion
	}
}
