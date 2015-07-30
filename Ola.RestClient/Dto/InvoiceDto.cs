namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;
	
	[XmlRoot(ElementName = "invoice")]
	public class InvoiceDto : TransactionDto
	{
		public InvoiceDto()
		{
            TradingTerms = new TradingTermsDto();
            QuickPayment = new QuickPaymentDto();
		}
		
		public InvoiceDto(string transactionType, string layout)
		{
			Layout = layout;
            TransactionType = transactionType;
            TradingTerms = new TradingTermsDto();
            QuickPayment = new QuickPaymentDto();
		}

        [XmlElement(ElementName = "transactionType")]
        public string TransactionType;

		[XmlElement(ElementName = "invoiceType")]
		public string InvoiceType;
		
		[XmlElement(ElementName = "contactUid")]
		public int			ContactUid;

		[XmlElement(ElementName = "shipToContactUid")]
		public int ShipToContactUid;

		[XmlElement(ElementName = "externalNotes")]
		public string ExternalNotes;

		[XmlElement(ElementName = "dueOrExpiryDate", DataType="date")]
		public				DateTime DueOrExpiryDate;

		[XmlElement(ElementName = "layout")]
		public string		Layout;

		[XmlElement(ElementName = "status")]
		public string		Status;

		[XmlElement(ElementName = "invoiceNumber")]
		public string		InvoiceNumber;

		[XmlElement(ElementName = "purchaseOrderNumber")]
		public string		PurchaseOrderNumber;
		
		[XmlArray(ElementName = "invoiceItems")]
		[XmlArrayItem(ElementName = "serviceInvoiceItem", Type = typeof(ServiceInvoiceItemDto))]
		[XmlArrayItem(ElementName = "itemInvoiceItem", Type = typeof(ItemInvoiceItemDto))]
		public ArrayList Items = new ArrayList();

		[XmlElement(ElementName = "quickPayment")]
        public QuickPaymentDto QuickPayment { get; set; }	

        [XmlElement(ElementName = "tradingTerms")]
        public TradingTermsDto TradingTerms { get; set; }	

		[XmlElement(ElementName = "isSent")]
		public bool			IsSent;

        [XmlElement(ElementName = "totalAmountInclTax")]
        public decimal TotalAmountInclTax;

        [XmlElement(ElementName = "totalAmountExclTax")]
        public decimal TotalAmountExclTax;

        [XmlElement(ElementName = "totalTaxAmount")]
        public decimal TotalTaxAmount;
	}
}
