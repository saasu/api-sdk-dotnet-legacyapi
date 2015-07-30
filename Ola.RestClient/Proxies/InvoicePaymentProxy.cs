namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;

	using System;
	using System.IO;
	using System.Net;


	public class InvoicePaymentProxy : CrudProxy
	{
		public override string ResourceName
		{
			get
			{
				return Ola.RestClient.ResourceName.InvoicePayment;
			}
		}


		public override string ResourceListName
		{
			get { return RestClient.ResourceName.InvoicePaymentList; }
		}


		protected override Type ResponseDtoType
		{
			get
			{
				return typeof(InvoicePaymentResponseDto);
			}
		}


		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((InvoicePaymentResponseDto) responseDto).InvoicePayment;
		}


		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertInvoicePaymentTask();
		}


		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateInvoicePaymentTask();
		}
	}
}
