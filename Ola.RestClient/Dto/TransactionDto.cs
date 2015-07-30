using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Ola.RestClient.Dto;

namespace Ola.RestClient.Dto
{
	public abstract class TransactionDto : EntityDto
	{
		protected TransactionDto()
		{
		}

		[XmlElement(ElementName = "date", DataType = "date")]
		public DateTime Date;

		[XmlElement(ElementName = "tags")]
		public string Tags;

		[XmlElement(ElementName = "summary")]
		public string Summary;

		[XmlElement(ElementName = "notes")]
		public string Notes;

		[XmlElement(ElementName = "requiresFollowUp")]
		public bool RequiresFollowUp = false;

		[XmlElement(ElementName = "ccy")]
		public string Ccy;

		[XmlElement(ElementName = "autoPopulateFxRate")]
		public bool AutoPopulateFXRate = true;

		[XmlElement(ElementName = "fcToBcFxRate")]
		public decimal FCToBCFXRate;
	}
}
