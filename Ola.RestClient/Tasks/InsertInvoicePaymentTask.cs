namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class InsertInvoicePaymentTask : IInsertTask
	{
		#region IInsertTask Members
		
		private InvoicePaymentDto _EntityToInsert = new InvoicePaymentDto();
		[XmlElement(ElementName = "invoicePayment", Type = typeof(InvoicePaymentDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._EntityToInsert;
			}
			set
			{
				this._EntityToInsert = (InvoicePaymentDto) value;
			}
		}

		#endregion
	}
}
