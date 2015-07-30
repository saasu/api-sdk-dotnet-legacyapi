namespace Ola.RestClient.NUnitTests
{
	using NUnit.Framework;

	using Ola.RestClient;
	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Proxies;
	using Ola.RestClient.Utils;

	using System;
	using System.Net;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Xml;


	[TestFixture]
	public class ContactStatementReportTests : RestClientTests
	{
		[TestFixtureSetUp]
		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			this.SetupTransactions();
		}


		protected void SetupTransactions()
		{
			InvoiceDto dto = this.GetServiceSale();
			InvoiceProxy proxy = new InvoiceProxy();
			proxy.Insert(dto);
			Assert.IsTrue(dto.Uid > 0, "Invalid uid after save.");
		}


		public InvoiceDto GetServiceSale()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Service);

			dto.Date = DateTime.Parse("30-Sep-05");
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Test POST sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("1-Dec-05");
			dto.Status = "I";
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO222";
			dto.IsSent = false;

			ServiceInvoiceItemDto item = new ServiceInvoiceItemDto();
			item.Description = "Design & Development of REST WS";
			item.AccountUid = this.IncomeService.Uid;
			item.TaxCode = TaxCode.SaleInclGst;
			item.TotalAmountInclTax = 2132.51M;
			dto.Items.Add(item);

			item = new ServiceInvoiceItemDto();
			item.Description = "Subscription to XYZ";
			item.AccountUid = this.IncomeSubscription.Uid;
			item.TaxCode = TaxCode.SaleInclGst;
			item.TotalAmountInclTax = 11.22M;
			dto.Items.Add(item);

			QuickPaymentDto payment = new QuickPaymentDto();
			payment.DatePaid = dto.Date;
			payment.BankedToAccountUid = this.Westpac.Uid;
			payment.Reference = "CASH";
			payment.Summary = "Quick payment from NUnitTests.";
			payment.Amount = 100;

			dto.QuickPayment = payment;

			return dto;
		}


		[Test]
		public void GetPdfContactStatement()
		{
			NameValueCollection queries = new NameValueCollection();
			queries.Add("contactuid", this.MrSmith.Uid.ToString());	// Required.
			queries.Add("datefrom", "2000-07-01");					// Required.
			queries.Add("dateto", "2008-06-30");					// Required.
			queries.Add("format", "pdf");							// Required.
			string url = RestProxy.MakeUrl("contactstatementreport");
			url += "&" + Util.ToQueryString(queries);
			new WebClient().DownloadFile(url, "ContactStatement" + DateTime.Today.ToString("yyyyMMdd") + ".pdf");
		}
	}
}
