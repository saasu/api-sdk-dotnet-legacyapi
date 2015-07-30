using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace Ola.RestClient.Dto.Journals
{
	[XmlRoot(ElementName = "journalResponse")]
	public class JournalResponseDto : CrudResponseDto
	{
		[XmlElement(ElementName = "journal")]
		public JournalDto Journal;
	}
}
