using Ola.RestClient.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Ola.RestClient.Tasks
{
	public class InsertActivityTask : IInsertTask
	{
		#region IInsertTask Members

		private ActivityDto _EntityToInsert = new ActivityDto();
		[XmlElement(ElementName = "activity", Type = typeof(ActivityDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._EntityToInsert;
			}
			set
			{
				this._EntityToInsert = (ActivityDto)value;
			}
		}

		#endregion
	}
}
