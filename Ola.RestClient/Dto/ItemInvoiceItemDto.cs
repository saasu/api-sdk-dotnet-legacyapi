namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;

	
	public class ItemInvoiceItemDto
	{
		[XmlAttribute(AttributeName = "uid")] 
		public int Uid;

		[XmlElement(ElementName = "quantity")]	
		public decimal Quantity;

		[XmlElement(ElementName = "inventoryItemUid")]	
		public int InventoryItemUid;

		[XmlElement(ElementName = "description")]	
		public string Description;

		[XmlElement(ElementName = "taxCode")]	
		public string TaxCode;

		[XmlElement(ElementName = "unitPriceInclTax")]	
		public decimal UnitPriceInclTax;

        [XmlElement(ElementName = "totalAmountInclTax")]
        public decimal TotalAmountInclTax;

        [XmlElement(ElementName = "totalAmountExclTax")]
        public decimal TotalAmountExclTax;

        [XmlElement(ElementName = "totalTaxAmount")]
        public decimal TotalTaxAmount;

		[XmlElement(ElementName = "percentageDiscount")]	
		public decimal PercentageDiscount;

		[XmlElement(ElementName = "tags")]
		public string Tags;
	}
}
