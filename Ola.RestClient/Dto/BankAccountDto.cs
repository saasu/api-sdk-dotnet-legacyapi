namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "bankAccount")]
	public class BankAccountDto : TransactionCategoryDto
	{
		[XmlElement(ElementName = "displayName")]
		public string DisplayName;

		[XmlElement(ElementName = "bsb")]
		public string BSB;

		[XmlElement(ElementName = "accountNumber")]
		public string AccountNumber;

        [XmlElement(ElementName = "merchantFeeAccountUid", IsNullable = true)]
        public int? MerchantFeeAccountUid;
	}
}
