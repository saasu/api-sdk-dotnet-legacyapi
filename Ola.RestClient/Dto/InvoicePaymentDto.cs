namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;


	[XmlRoot(ElementName = "invoicePayment")]
	public class InvoicePaymentDto : EntityDto
	{
		public InvoicePaymentDto()
		{
		}


		public InvoicePaymentDto(string transactionType)
		{
			this.TransactionType = transactionType;
		}

		[XmlElement(ElementName = "transactionType")]
		public string		TransactionType;

		[XmlElement(ElementName = "date", DataType="date")]
		public DateTime		Date;

		[XmlElement(ElementName = "ccy")]
		public string Ccy;

		[XmlElement(ElementName = "autoPopulateFxRate")]
		public bool AutoPopulateFXRate;

		[XmlElement(ElementName = "fcToBcFxRate")]
		public decimal FCToBCFXRate;

        [XmlElement(ElementName = "fee")]
        public decimal Fee;
		
		[XmlElement(ElementName = "reference")]
		public string		Reference;
		
		[XmlElement(ElementName = "summary")]
		public string		Summary;

		[XmlElement(ElementName = "notes")]
		public string		Notes;

		[XmlElement(ElementName = "requiresFollowUp")]
		public bool			RequiresFollowUp = false;

		[XmlElement(ElementName = "paymentAccountUid")]
		public int PaymentAccountUid;
	
		[XmlElement(ElementName = "dateCleared", DataType="date")]
		public DateTime DateCleared;

		[XmlArray(ElementName = "invoicePaymentItems")]
		[XmlArrayItem(ElementName = "invoicePaymentItem", Type = typeof(InvoicePaymentItemDto))]
		public ArrayList Items = new ArrayList();
	}
}
