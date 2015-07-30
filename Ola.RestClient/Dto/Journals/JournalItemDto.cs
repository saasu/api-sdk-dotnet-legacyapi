using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace Ola.RestClient.Dto.Journals
{
	public class JournalItemDto
	{
		[XmlElement(ElementName = "accountUid")]
		public int AccountUid;

		[XmlElement(ElementName = "taxCode")]
		public string TaxCode;

        /// <summary>
        /// Total amount for this JournalItem
        /// </summary>
		[XmlElement(ElementName = "amount")]
		public decimal Amount;

        [XmlElement(ElementName = "type")]
        public string Type;
	}
}
