namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "transactionCategory")]
	public class TransactionCategoryDto : EntityDto
	{
        [XmlElement(ElementName = "level")]
        public string Level;

		[XmlElement(ElementName = "type")]
		public string		Type;

		[XmlElement(ElementName = "name")]
		public string		Name;
		
		[XmlElement(ElementName = "isActive")]
		public bool			IsActive = true;

		[XmlElement(ElementName = "ledgerCode")] public string LedgerCode;

		[XmlElement(ElementName = "defaultTaxCode")] public string DefaultTaxCode;

        [XmlElement(ElementName = "headerAccountUid")]
        public int HeaderAccountUid;
	}
}
