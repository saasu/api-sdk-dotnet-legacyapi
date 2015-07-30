namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;

	using System;
	using System.IO;
	using System.Net;


	public class InvoiceProxy : CrudProxy
	{
		public override string ResourceName
		{
			get
			{
				return Ola.RestClient.ResourceName.Invoice;
			}
		}


		public override string ResourceListName
		{
			get { return RestClient.ResourceName.InvoiceList; }
		}


		protected override Type ResponseDtoType
		{
			get
			{
				return typeof(InvoiceResponseDto);
			}
		}


		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((InvoiceResponseDto) responseDto).Invoice;
		}


		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertInvoiceTask();
		}


		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateInvoiceTask();
		}

		
		public virtual void GetPdfInvoiceByUid(int uid, string filename)
		{
			new WebClient().DownloadFile(this.GetPdfInvoiceByUidUri(uid), filename);
		}

		public virtual void GetPdfInvoiceByUid(int uid, int templateUid, string filename)
		{
			new WebClient().DownloadFile(this.GetPdfInvoiceByUidUri(uid, templateUid), filename);
		}


		public virtual string GetPdfInvoiceByUidUri(int uid)
		{
			string url = this.MakeUrl() +  "&uid=" + uid + "&format=pdf";
			Console.WriteLine("Url is: " + url);
			return url;
		}


		public virtual string GetPdfInvoiceByUidUri(int uid, int templateUid)
		{
			return this.MakeUrl() + "&uid=" + uid + "&templateuid=" + templateUid + "&format=pdf";
		}


		public virtual void Email(int invoiceUid, EmailMessageDto emailMessage)
		{
			Email(invoiceUid, 0 /* Use default template */, emailMessage);
		}


		public virtual void Email(int invoiceUid, int templateUid, EmailMessageDto emailMessage)
		{
			TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);
			tasksRunner.Tasks.Add(this.CreateEmailTask(invoiceUid, templateUid, emailMessage));
			this.HandleEmailResult(tasksRunner.Execute());
		}


		protected virtual ITask CreateEmailTask(int invoiceUid, int templateUid, EmailMessageDto emailMessage)
		{
			EmailPdfInvoiceTask task = new EmailPdfInvoiceTask();
			task.InvoiceUid = invoiceUid;
			task.TemplateUid = templateUid;
			task.EmailMessage = emailMessage;
			return task;
		}


		protected virtual void HandleEmailResult(TasksResponse tasksResponse)
		{
			this.CheckErrors(tasksResponse.Errors);
			EmailPdfInvoiceResult result = (EmailPdfInvoiceResult) tasksResponse.Results[0];
			this.CheckErrors(result.Errors);
		}


		public void InsertAndEmail(InvoiceDto invoice, EmailMessageDto emailMessage)
		{
			this.InsertAndEmail(invoice, 0, emailMessage);
		}


		public virtual void InsertAndEmail(InvoiceDto invoice, int templateUid, EmailMessageDto emailMessage)
		{
			TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);
			tasksRunner.Tasks.Add(this.CreateInsertTask(invoice, templateUid, emailMessage));
			this.HandleInsertResult(tasksRunner.Execute(), invoice);
			invoice.IsSent = true;	//	Reaches here - no error. Assume invoice has been sent successfully.
		}


		public virtual void UpdateAndEmail(InvoiceDto invoice, int templateUid, EmailMessageDto emailMessage)
		{
			TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);
			tasksRunner.Tasks.Add(this.CreateUpdateTask(invoice, templateUid, emailMessage));
			this.HandleUpdateResult(tasksRunner.Execute(), invoice);
			invoice.IsSent = true;	//	Reaches here - no error. Assume invoice has been sent successfully.
		}


		protected virtual ITask CreateInsertTask(InvoiceDto invoice, int templateUid, EmailMessageDto emailMessage)
		{
			InsertInvoiceTask task = (InsertInvoiceTask) base.CreateInsertTask(invoice);
			task.EmailToContact = true;
			task.TemplateUid = templateUid;
			task.EmailMessage = emailMessage;
			return task;
		}


		protected virtual ITask CreateUpdateTask(InvoiceDto invoice, int templateUid, EmailMessageDto emailMessage)
		{
			UpdateInvoiceTask task = (UpdateInvoiceTask) base.CreateUpdateTask(invoice);
			task.EmailToContact = true;
			task.TemplateUid = templateUid;
			task.EmailMessage = emailMessage;
			return task;
		}


		protected override void HandleInsertResult(TasksResponse tasksResponse, EntityDto entity)
		{
			base.HandleInsertResult(tasksResponse, entity);
			
			InvoiceDto dto = (InvoiceDto) entity;
			InsertInvoiceResult result = (InsertInvoiceResult) tasksResponse.Results[0];
			string s = Util.DefaultValueToEmpty(result.GeneratedInvoiceNumber);
			if (s.Length > 0)
			{
				dto.InvoiceNumber = s;
			}
			
			s = Util.DefaultValueToEmpty(result.GeneratedPurchaseOrderNumber);
			if (s.Length > 0)
			{
				dto.PurchaseOrderNumber = s;
			}
		}
	}
}
