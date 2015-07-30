namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "bankAccountResponse")]
	public class BankAccountResponseDto : CrudResponseDto
	{
		[XmlElement(ElementName = "bankAccount")]
		public BankAccountDto BankAccount;
	}
}
