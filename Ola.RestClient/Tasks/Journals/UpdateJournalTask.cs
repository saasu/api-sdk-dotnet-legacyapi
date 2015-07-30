using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Journals;


namespace Ola.RestClient.Tasks.Journals
{
	public class UpdateJournalTask : IUpdateTask
	{
		private JournalDto _entityToUpdate = new JournalDto();
		[XmlElement(ElementName = "journal", Type = typeof(JournalDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._entityToUpdate;
			}
			set
			{
				this._entityToUpdate = (JournalDto) value;
			}
		}
	}
}
