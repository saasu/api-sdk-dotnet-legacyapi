using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace Ola.RestClient.Dto.Journals
{
	[XmlRoot(ElementName = "journal")]
	public class JournalDto : TransactionDto
	{
        [XmlElement(ElementName = "reference")]
        public string Reference;
        
		[XmlArray(ElementName = "journalItems")]
		[XmlArrayItem(ElementName = "journalItem", Type = typeof(JournalItemDto))]
		public JournalItemDto[] Items;
	}
}