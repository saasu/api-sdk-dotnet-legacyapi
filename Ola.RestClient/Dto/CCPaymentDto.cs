using System;
using System.Xml.Serialization;


namespace Ola.RestClient.Dto
{
	[XmlRoot(ElementName = "ccPayment")]
	public class CCPaymentDto
	{
		[XmlElement(ElementName = "cardholderName")]
		public string CardholderName = "";

		[XmlElement(ElementName = "ccNumber")]
		public string CCNumber = "";

		/// <summary>
		/// CC expiry date in MMYY format, e.g. 0610 for cc expiring on Jun 2010.
		/// </summary>
		[XmlElement(ElementName = "ccExpiryDate")]
		public string CCExpiryDate = "";

		/// <summary>
		/// CVV = Card verfication value code. Visa: CVV2, Mastercard: CVC2, Amex: CID.
		/// </summary>
		[XmlElement(ElementName = "cvvCode")]
		public string CvvCode = "";

		[XmlElement(ElementName = "amount")]
		public decimal Amount;

		[XmlElement(ElementName = "requestID")]
		public string RequestID = "";

		[XmlElement(ElementName = "referenceNumber")]
		public string ReferenceNumber = "";	//	For result.

		[XmlElement(ElementName = "processingTimestamp")]
		public DateTime ProcessingTimestamp;
	}
}
