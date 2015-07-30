namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class UpdateInvoiceTask : IUpdateTask
	{
		[XmlAttribute("emailToContact")]
		public bool EmailToContact = false;


		#region IUpdateTask Members
		
		private InvoiceDto _EntityToUpdate = new InvoiceDto();
		[XmlElement(ElementName = "invoice", Type = typeof(InvoiceDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._EntityToUpdate;
			}
			set
			{
				this._EntityToUpdate = (InvoiceDto) value;
			}
		}

		#endregion
		


		private int _TemplateUid;
		[XmlElement(ElementName = "templateUid")]
		public int TemplateUid
		{
			get
			{
				return this._TemplateUid;
			}
			set
			{
				this._TemplateUid = value;
			}
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
