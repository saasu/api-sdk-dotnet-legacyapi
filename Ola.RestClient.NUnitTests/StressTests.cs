namespace Ola.RestClient.NUnitTests
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Proxies;
	using Ola.RestClient.Utils;
	using Ola.RestClient.Tasks;

	using NUnit.Framework;

	using System;
	using System.Configuration;
	using System.Net;
	using System.IO;
	using System.Security.Cryptography.X509Certificates;


	public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
	{
		public TrustAllCertificatePolicy()
		{ }

		public bool CheckValidationResult(ServicePoint sp,
		 X509Certificate cert, WebRequest req, int problem)
		{
			return true;
		}
	}



	[TestFixture]
	public class StressTests : InvoiceTests
	{
		[TestFixtureSetUp]
		public override void TestFixtureSetUp()
		{

			System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

			base.TestFixtureSetUp();
			this.CreatePurchaseForInventoryItemsUsed();
		}


		[Test]
		public void Run()
		{
			CrudProxy proxy = new InvoiceProxy();
			for (int i = 1; i <= 50; i++)
			{
				InvoiceDto serviceDto = this.GetServiceSale();
				proxy.Insert(serviceDto);
				Console.WriteLine("Service invoice #{0}: {1}", i, serviceDto.Uid);
			}

			proxy = new InvoiceProxy();
			for (int i = 1; i <= 50; i++)
			{
				InvoiceDto itemDto = this.GetItemSale();
				proxy.Insert(itemDto);
				Console.WriteLine("Item invoice #{0}: {1}", i, itemDto.Uid);
			}
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


		public InvoiceDto GetUnpaidServiceSale()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Service);

			dto.Date = DateTime.Parse("30-Sep-05");
			dto.ContactUid = this.MrsSmith.Uid;
			dto.Summary = "Test POST sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("1-Dec-05");
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO222";

			ServiceInvoiceItemDto item = new ServiceInvoiceItemDto();
			item.Description = "Design & Development of REST WS";
			item.AccountUid = this.IncomeHardwareSales.Uid;
			item.TaxCode = "G1";
			item.TotalAmountInclTax = 2132.51M;
			dto.Items.Add(item);

			item = new ServiceInvoiceItemDto();
			item.Description = "Testing";
			item.AccountUid = this.IncomeMisc.Uid;
			item.TaxCode = "G1,G3";
			item.TotalAmountInclTax = 11.22M;
			dto.Items.Add(item);

			return dto;
		}


		public InvoiceDto GetServiceSale2()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Service);

			dto.Date = DateTime.Parse("15-Sep-05");
			dto.ContactUid = this.MrsSmith.Uid;
			dto.Summary = "Service Sale 2";
			dto.DueOrExpiryDate = DateTime.Parse("15-Dec-05");
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO123456789";

			ServiceInvoiceItemDto item = new ServiceInvoiceItemDto();
			item.Description = "LINE 1 LINE 1 LINE 1";
			item.AccountUid = this.IncomeService.Uid;
			item.TaxCode = TaxCode.SaleExports;
			item.TotalAmountInclTax = 12345.12M;
			dto.Items.Add(item);

			item = new ServiceInvoiceItemDto();
			item.Description = "Testing";
			item.AccountUid = this.IncomeHardwareSales.Uid;
			item.TaxCode = TaxCode.SaleAdjustments;
			item.TotalAmountInclTax = -123.9M;
			dto.Items.Add(item);

			item = new ServiceInvoiceItemDto();
			item.Description = "Testing";
			item.AccountUid = this.IncomeShipping.Uid;
			item.TaxCode = TaxCode.SaleInclGst;
			item.TotalAmountInclTax = 569.66M;
			dto.Items.Add(item);

			return dto;
		}



		public InvoiceDto GetItemSale()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Item);

			dto.Date = DateTime.Parse("6-Oct-05");
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Test Insert Item Sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("6-Nov-05");
			dto.Layout = InvoiceLayout.Item;
			dto.Status = InvoiceStatus.Invoice;
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO333";

			ItemInvoiceItemDto item = null;

			item = new ItemInvoiceItemDto();
			item.Quantity = 2;
			item.InventoryItemUid = this.AsusLaptop.Uid;
			item.Description = "Asus Laptop";
			item.TaxCode = TaxCode.SaleInclGst;
			item.UnitPriceInclTax = 1200.75M;
			item.PercentageDiscount = 12.50M;

			dto.Items.Add(item);

			item = new ItemInvoiceItemDto();
			item.Quantity = 5.125M;
			item.InventoryItemUid = this.Cat5Cable.Uid;
			item.Description = "Cat 5 Cable (in meter)";
			item.TaxCode = TaxCode.SaleGstFree;
			item.UnitPriceInclTax = 2.1234M;
			dto.Items.Add(item);

			item = new ItemInvoiceItemDto();
			item.Quantity = 3;
			item.InventoryItemUid = this.Cat5Cable.Uid;
			item.Description = "Cat 5 Cable (in meter)";
			item.TaxCode = TaxCode.SaleExports;
			item.UnitPriceInclTax = 5.125M;
			dto.Items.Add(item);

			QuickPaymentDto payment = new QuickPaymentDto();
			payment.DatePaid = dto.Date;
			payment.BankedToAccountUid = this.StGeorge.Uid;
			payment.Reference = "C-001-023";
			payment.Amount = 222.22M;

			dto.QuickPayment = payment;

			return dto;
		}


		public InvoiceDto GetUnpaidItemSale()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Item);

			dto.Date = DateTime.Parse("6-Oct-05");
			dto.ContactUid = this.MrsSmith.Uid;
			dto.Summary = "Test Insert Item Sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = DateTime.Parse("6-Nov-05");
			dto.Status = InvoiceStatus.Invoice;
			dto.InvoiceNumber = "<Auto Number>";
			dto.PurchaseOrderNumber = "PO333";

			ItemInvoiceItemDto item = null;

			item = new ItemInvoiceItemDto();
			item.Quantity = 2;
			item.InventoryItemUid = this.HardDisk.Uid;
			item.Description = this.HardDisk.Description;
			item.TaxCode = TaxCode.SaleInclGst;
			item.UnitPriceInclTax = 120.75M;
			dto.Items.Add(item);

			item = new ItemInvoiceItemDto();
			item.Quantity = 5.125M;
			item.InventoryItemUid = this.Cat5Cable.Uid;
			item.Description = this.Cat5Cable.Description;
			item.TaxCode = TaxCode.SaleInclGst;
			item.UnitPriceInclTax = 2.555M;
			dto.Items.Add(item);

			return dto;
		}
	}
}
