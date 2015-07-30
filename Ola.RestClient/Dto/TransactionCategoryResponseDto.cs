namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "transactionCategoryResponse")]
	public class TransactionCategoryResponseDto : CrudResponseDto
	{
		[XmlElement(ElementName = "transactionCategory")]
		public TransactionCategoryDto TransactionCategory;
	}
}
