using System.Collections.Specialized;
using System.Configuration;

namespace Ola.RestClient.NUnitTests
{
	using NUnit.Framework;

	using Ola.RestClient.Dto;
	using Ola.RestClient.Dto.Inventory;
	using Ola.RestClient.Proxies;
	
	using System;
	using System.Net;
	using System.IO;
	using System.Text;
	using System.Security.Cryptography.X509Certificates;

	
	public class RestClientTests : BaseTests
	{
		protected ContactDto MrSmith;
		protected ContactDto MrsSmith;
		protected TransactionCategoryDto AssetInventory;
		protected TransactionCategoryDto AssetInventory2;
		protected TransactionCategoryDto CoSHardware;
		protected TransactionCategoryDto ExpenseOffice;
		protected TransactionCategoryDto ExpenseTelco;
		protected TransactionCategoryDto ExpenseMisc;
		protected TransactionCategoryDto IncomeHardwareSales;
		protected TransactionCategoryDto IncomeService;
		protected TransactionCategoryDto IncomeShipping;
		protected TransactionCategoryDto IncomeSubscription;
		protected TransactionCategoryDto IncomeMisc;
		protected BankAccountDto StGeorge;
		protected BankAccountDto Westpac;
		protected InventoryItemDto AsusLaptop;
		protected InventoryItemDto Cat5Cable;
		protected InventoryItemDto HardDisk;
		protected InventoryItemDto Shipping1;
		protected ComboItemDto CPU;

		public int IncTaxTemplateUid;
		public int ShippingTemplateUid;
		public string ApiUserEmailAddress;

		
		
		#region Test fixture setup and teardown.
		[TestFixtureSetUp]
		public virtual void TestFixtureSetUp()
		{
			this.SetupContacts();
			this.SetupTransactionCategories();
			this.SetupBankAccounts();
			this.SetupInventoryItems();
			this.SetupComboItems();

			var proxySettings = (NameValueCollection)ConfigurationManager.GetSection("ola.restclient/proxySettings");
			IncTaxTemplateUid = Int32.Parse(proxySettings["IncTaxTemplateUid"]);
			ShippingTemplateUid = Int32.Parse(proxySettings["ShippingTemplateUid"]);
			ApiUserEmailAddress = proxySettings["ApiUserEmailAddress"];
		}
		
		
		[TestFixtureTearDown]
		public virtual void TestFixtureTearDown()
		{
		}
		#endregion

		protected void SetupContacts()
		{
			CrudProxy contact = new ContactProxy();

			this.MrSmith = new ContactDto();
			this.MrSmith.Salutation = Salutation.Mr;
			this.MrSmith.GivenName = "John";
			this.MrSmith.FamilyName = "Smith";
			this.MrSmith.OrganisationName = "ACME Pty Ltd";
			contact.Insert(this.MrSmith);
			
			this.MrsSmith = new ContactDto();
			this.MrsSmith.Salutation = Salutation.Mrs;
			this.MrsSmith.GivenName = "Mary";
			this.MrsSmith.FamilyName = "Smith";
			this.MrsSmith.OrganisationName = "ACME Pty Ltd";
			contact.Insert(this.MrsSmith);
		}

		
		private void SetupTransactionCategories()
		{
			this.AssetInventory = CreateTransactionCategory(AccountType.Asset, "Inventory");
			this.AssetInventory2 = CreateTransactionCategory(AccountType.Asset, "Inventory 2");
			this.CoSHardware = CreateTransactionCategory(AccountType.CostOfSales, "Hardware");
			this.IncomeHardwareSales = CreateTransactionCategory(AccountType.Income, "Hardware Sales");
			this.IncomeService = CreateTransactionCategory(AccountType.Income, "Service");
			this.IncomeShipping = CreateTransactionCategory(AccountType.Income, "Shipping");
			this.IncomeSubscription = CreateTransactionCategory(AccountType.Income, "Subscription");
			this.IncomeMisc = CreateTransactionCategory(AccountType.Income, "Misc");
			this.ExpenseOffice = CreateTransactionCategory(AccountType.Expense, "Stationary, etc");
			this.ExpenseTelco = CreateTransactionCategory(AccountType.Expense, "Telco");	
			this.ExpenseMisc = CreateTransactionCategory(AccountType.Expense, "Misc");	
		}


		private TransactionCategoryDto CreateTransactionCategory(string accountType, string accountName)
		{
			TransactionCategoryDto dto = new TransactionCategoryDto();
			dto.Type = accountType;
			dto.Name = accountName + " " + System.Guid.NewGuid().ToString();
			new TransactionCategoryProxy().Insert(dto);
			return dto;
		}


		private void SetupBankAccounts()
		{
            var proxySettings = (NameValueCollection)ConfigurationManager.GetSection("ola.restclient/proxySettings");

			CrudProxy bankAccount = new BankAccountProxy();
			
			this.Westpac = new BankAccountDto();
			this.Westpac.Type = AccountType.Asset;
			this.Westpac.Name = "Westpac " + System.Guid.NewGuid().ToString();
			this.Westpac.DisplayName = this.Westpac.Name;
			this.Westpac.BSB = "111-111";
			this.Westpac.AccountNumber = "12345-6789";
            this.Westpac.MerchantFeeAccountUid = int.Parse(proxySettings["MerchantFeeAccountUid"]);
			bankAccount.Insert(this.Westpac);
			
			this.StGeorge = new BankAccountDto();
			this.StGeorge.Type = AccountType.Asset;
			this.StGeorge.Name = "St. George " + System.Guid.NewGuid().ToString();
			this.StGeorge.DisplayName = this.StGeorge.Name;
			this.StGeorge.BSB = "222-222";
			this.StGeorge.AccountNumber = "9897-3698";
			bankAccount.Insert(this.StGeorge);
		}


		private void SetupInventoryItems()
		{
			CrudProxy inventoryItem = new InventoryItemProxy();

			string guid = Guid.NewGuid().ToString().Remove(0, 20);

			this.AsusLaptop = new InventoryItemDto();
			this.AsusLaptop.Code = "ASUS-" + guid;
			this.AsusLaptop.Description = "Asus Laptop";
			this.AsusLaptop.IsInventoried = true;
			this.AsusLaptop.AssetAccountUid = this.AssetInventory.Uid;
			this.AsusLaptop.IsBought = true;
			this.AsusLaptop.PurchaseTaxCode = TaxCode.ExpInclGst;
			this.AsusLaptop.IsSold = true;
			this.AsusLaptop.SaleIncomeAccountUid = this.IncomeHardwareSales.Uid;
			this.AsusLaptop.SaleTaxCode = TaxCode.SaleInclGst;
			this.AsusLaptop.RrpInclTax = 1111.11M;
			this.AsusLaptop.SaleCoSAccountUid = this.CoSHardware.Uid;
			inventoryItem.Insert(this.AsusLaptop);

			this.Cat5Cable = new InventoryItemDto();
			this.Cat5Cable.Code = "Cat5-" + guid;
			this.Cat5Cable.Description = "Cat 5 Cable (in meter)";
			this.Cat5Cable.IsInventoried = true;
			this.Cat5Cable.AssetAccountUid = this.AssetInventory.Uid;
			this.Cat5Cable.IsBought = true;
			this.Cat5Cable.PurchaseTaxCode = TaxCode.ExpInclGst;
			this.Cat5Cable.IsSold = true;
			this.Cat5Cable.SaleIncomeAccountUid = this.IncomeHardwareSales.Uid;
			this.Cat5Cable.SaleTaxCode = TaxCode.SaleInclGst;
			this.Cat5Cable.RrpInclTax = 2.55M;
			this.Cat5Cable.SaleCoSAccountUid = this.CoSHardware.Uid;
			inventoryItem.Insert(this.Cat5Cable);

			this.HardDisk = new InventoryItemDto();
			this.HardDisk.Code = "Seagate-" + guid;
			this.HardDisk.Description = "Seagate HDD - 300G";
			this.HardDisk.IsInventoried = true;
			this.HardDisk.AssetAccountUid = this.AssetInventory.Uid;
			this.HardDisk.IsBought = true;
			this.HardDisk.PurchaseTaxCode = TaxCode.ExpInclGst;
			this.HardDisk.IsSold = true;
			this.HardDisk.SaleIncomeAccountUid = this.IncomeHardwareSales.Uid;
			this.HardDisk.SaleTaxCode = TaxCode.SaleInclGst;
			this.HardDisk.RrpInclTax = 110.95M;
			this.HardDisk.SaleCoSAccountUid = this.CoSHardware.Uid;
			inventoryItem.Insert(this.HardDisk);
			
			this.Shipping1 = new InventoryItemDto();
			this.Shipping1.Code = "Shipping1-" + guid;
			this.Shipping1.Description = "Shipping - 1";
			this.Shipping1.IsBought = true;
			this.Shipping1.PurchaseExpenseAccountUid = this.ExpenseMisc.Uid;
			this.Shipping1.PurchaseTaxCode = TaxCode.ExpInclGst;
			this.Shipping1.IsSold = true;
			this.Shipping1.SaleIncomeAccountUid = this.IncomeShipping.Uid;
			this.Shipping1.SaleTaxCode = TaxCode.SaleInclGst;
			this.Shipping1.RrpInclTax = 59.95M;
			inventoryItem.Insert(this.Shipping1);
		}


		private void SetupComboItems()
		{
			string guid = Guid.NewGuid().ToString().Remove(0, 20);
			ComboItemProxy comboItem = new ComboItemProxy();
			this.CPU = new ComboItemDto();
			this.CPU.Code = "DESKTOP " + guid;
			this.CPU.Description = "Desktop PC";
			this.CPU.IsInventoried = true;
			this.CPU.AssetAccountUid = this.AssetInventory.Uid;
			this.CPU.IsSold = true;
			this.CPU.SaleCoSAccountUid = this.CoSHardware.Uid;
			this.CPU.SaleIncomeAccountUid = this.IncomeHardwareSales.Uid;
			this.CPU.RrpInclTax = 575.00M;
			this.CPU.Items = new System.Collections.Generic.List<ComboItemLineItemDto>();

			ComboItemLineItemDto lineItem = new ComboItemLineItemDto();
			lineItem.Uid = this.HardDisk.Uid;
			lineItem.Code = this.HardDisk.Code;
			lineItem.Quantity = 1;
			this.CPU.Items.Add(lineItem);

			lineItem = new ComboItemLineItemDto();
			lineItem.Uid = this.Cat5Cable.Uid;
			lineItem.Code = this.Cat5Cable.Code;
			lineItem.Quantity = 1;
			this.CPU.Items.Add(lineItem);

			comboItem.Insert(this.CPU);
		}
	}
}
