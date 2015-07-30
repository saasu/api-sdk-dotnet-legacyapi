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


	[TestFixture]
	public class CheckoutTests : InvoiceTests
	{
		protected InventoryItemDto Item1 = null;


		[TestFixtureSetUp]
		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			this.SetupInventoryItems();
		}


		private void SetupInventoryItems()
		{
			this.Item1 = this.GetItem1();
			CrudProxy proxy = new InventoryItemProxy();
			proxy.Insert(this.Item1);
			Assert.IsTrue(this.Item1.Uid > 0, "Uid must be > 0 after save.");
		}


		[Test]
		public void Insert1()
		{
			InvoiceProxy proxy = new InvoiceProxy();
			InvoiceDto dto1 = this.GetSale1();

			EmailMessageDto emailMessage = new EmailMessageDto();
			emailMessage.From = ConfigurationSettings.AppSettings["NUnitTests.Email.From"];
			emailMessage.To = ConfigurationSettings.AppSettings["NUnitTests.Email.To"];
			emailMessage.Subject = "Invoice - Sent using NetAccounts OLA REST API (TestInsertAndEmail).";
			emailMessage.Body = "Insert Invoice then email.";

			proxy.InsertAndEmail(dto1, emailMessage);

			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

			//
			//	Ensure IsSent is updated.
			//
			dto1 = (InvoiceDto)proxy.GetByUid(dto1.Uid);
			Assert.IsTrue(dto1.IsSent, "Invoice should have been sent.");
		}


		[Test]
		public void Checkout()
		{
			TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);

			CheckoutTask task = new CheckoutTask();
			task.PgcUid = 1;
			task.BillingContact = this.GetBillingContact();
			task.Sale = this.GetSale1();
			task.CCPayment.Amount = 1500;
			task.CCPayment.CardholderName = "SAASU TESTER";
			task.CCPayment.CCNumber = "4111111111111111";
			task.CCPayment.CCExpiryDate = "1010";
			//task.EmailReceipt = true;
			//task.EmailReceiptUsingTemplateUid = 79;
			tasksRunner.Tasks.Add(task);
			TasksResponse response = tasksRunner.Execute();
		}


		private ContactDto GetBillingContact()
		{
			ContactDto contact = new ContactDto();
			contact.Salutation = "Mr.";
			contact.GivenName = "Jusa";
			contact.FamilyName = "Chin";
			contact.OrganisationName = "Quick Pic Pty Ltd";
			contact.PostalAddress.Street = "1/111 Elizabeth St";
			contact.PostalAddress.City = "Sydney";
			contact.PostalAddress.State = "NSW";
			contact.PostalAddress.PostCode = "2220";
			contact.PostalAddress.Country = "Australia";
			contact.MainPhone = "1300 360 733";
			contact.Email = "jusa.chin@saasu.com";
			return contact;
		}


		private InventoryItemDto GetItem1() 
		{
			string code = "CODE_" + System.Guid.NewGuid().ToString().Remove(0, 10);

			InventoryItemDto info = new InventoryItemDto();
			info.Code = code;
			info.Description = "BOOKING FOR STUFF";
			info.IsActive = true;

			info.IsSold = true;
			info.SaleIncomeAccountUid = this.IncomeSubscription.Uid;
			info.SaleTaxCode = TaxCode.SaleInclGst;
			info.RrpInclTax = 1500M;

			return info;
		}


		public InvoiceDto GetSale1()
		{
			InvoiceDto dto = new InvoiceDto(TransactionType.Sale, InvoiceLayout.Item);
			
			dto.Date = DateTime.Today;
			dto.ContactUid = this.MrSmith.Uid;
			dto.Summary = "Test Insert Item Sale";
			dto.Notes = "From REST";
			dto.DueOrExpiryDate = dto.Date.AddMonths(1);
			dto.Layout = InvoiceLayout.Item;
			dto.Status = InvoiceStatus.Invoice;
			dto.InvoiceNumber = "<Auto Number>";
			
			ItemInvoiceItemDto item = null;

			item = new ItemInvoiceItemDto();
			item.Quantity = 1;
			item.InventoryItemUid = this.Item1.Uid;
			item.Description = this.Item1.Description;
			item.TaxCode = TaxCode.SaleInclGst;
			item.UnitPriceInclTax = this.Item1.RrpInclTax;
			dto.Items.Add(item);

			dto.Tags = "CC";
			
			return dto;
		}
	}
}
