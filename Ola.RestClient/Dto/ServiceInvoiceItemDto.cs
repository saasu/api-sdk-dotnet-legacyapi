namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;
	
	public class ServiceInvoiceItemDto
	{
		[XmlElement(ElementName = "description")]	
		public string Description;

		[XmlElement(ElementName = "accountUid")]	
		public int AccountUid;

		[XmlElement(ElementName = "taxCode")]	
		public string TaxCode;

        [XmlElement(ElementName = "totalAmountInclTax")]	
		public decimal	TotalAmountInclTax;

        [XmlElement(ElementName = "totalAmountExclTax")]
        public decimal TotalAmountExclTax;

        [XmlElement(ElementName = "totalTaxAmount")]
        public decimal TotalTaxAmount;

		[XmlElement(ElementName = "tags")]
		public string Tags;
	}
}
