namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "updateInvoicePayment")]
	public class UpdateInvoicePaymentTask : IUpdateTask
	{
		#region IUpdateTask Members
		
		private InvoicePaymentDto _EntityToUpdate = new InvoicePaymentDto();
		[XmlElement(ElementName = "invoicePayment", Type = typeof(InvoicePaymentDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._EntityToUpdate;
			}
			set
			{
				this._EntityToUpdate = (InvoicePaymentDto) value;
			}
		}

		#endregion
	}
}
