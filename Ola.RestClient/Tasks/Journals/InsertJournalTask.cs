using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Journals;


namespace Ola.RestClient.Tasks.Journals
{
	public class InsertJournalTask : IInsertTask
	{
		private JournalDto _entityToInsert = new JournalDto();
		[XmlElement(ElementName = "journal", Type = typeof(JournalDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._entityToInsert;
			}
			set
			{
				this._entityToInsert = (JournalDto)value;
			}
		}
	}
}
