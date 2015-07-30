namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "quickPayment")]
	public class QuickPaymentDto
	{
		[XmlElement(ElementName = "datePaid", DataType = "date")]
		public DateTime DatePaid;

		[XmlElement(ElementName = "dateCleared", DataType = "date")]
		public DateTime DateCleared;

		[XmlElement(ElementName = "bankedToAccountUid")]
		public int BankedToAccountUid;

		[XmlElement(ElementName = "amount")]
		public decimal Amount;

		[XmlElement(ElementName = "reference")]
		public string Reference;

		[XmlElement(ElementName = "summary")]
		public string Summary;
	}
}
