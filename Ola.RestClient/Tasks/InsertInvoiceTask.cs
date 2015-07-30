namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class InsertInvoiceTask : IInsertTask
	{
		[XmlAttribute("emailToContact")]
		public bool EmailToContact = false;
		

		#region IInsertTask Members

		private InvoiceDto _EntityToInsert = new InvoiceDto();
		[XmlElement(ElementName = "invoice", Type = typeof(InvoiceDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._EntityToInsert;
			}
			set
			{
				this._EntityToInsert = (InvoiceDto) value;
			}
		}

		#endregion


		private int _TemplateUid;
		[XmlElement(ElementName = "templateUid")]
		public int TemplateUid
		{
			get { return _TemplateUid; }
			set { _TemplateUid = value; }
		}


		private EmailMessageDto _EmailMessage;
		[XmlElement(ElementName = "emailMessage")]
		public EmailMessageDto EmailMessage
		{
			get
			{
				return this._EmailMessage;
			}
			set
			{
				this._EmailMessage = value;
			}
		}
	}
}
