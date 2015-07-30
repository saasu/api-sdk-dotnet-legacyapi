using NUnit.Framework;
using Ola.RestClient;
using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Journals;
using Ola.RestClient.Exceptions;
using Ola.RestClient.Proxies;
using Ola.RestClient.Tasks;
using Ola.RestClient.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace Ola.RestClient.NUnitTests
{
	
	// [TestFixture]
	/// <summary>
	/// NOTE: Used for debugging. Got hardcoded uids. That's why it's excluded from test.
	/// </summary>
	public class DirectXmlTests : BaseTests
	{
		// [Test]
		public void InsertContact()
		{
			string xml =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<tasks>
  <insertContact>
    <contact uid=""0"">
      <salutation>Mr.</salutation>
      <givenName>John</givenName>
      <familyName>Smith</familyName>
      <organisationName>Saasy.tv</organisationName>
      <organisationAbn>777888999</organisationAbn>      
      <organisationPosition>Director</organisationPosition>
      <email>john.smith@saasy.tv</email>
      <mainPhone>02 9999 9999</mainPhone>
      <mobilePhone>0444 444 444</mobilePhone>
      <contactID>XYZ123</contactID>
      <tags>Gold Prospect, Film</tags>
      <postalAddress>
        <street>3/33 Victory Av</street>
        <city>North Sydney</city>
        <state>NSW</state>
        <country>Australia</country>
      </postalAddress>
      <otherAddress>
        <street>111 Elizabeth street</street>
        <city>Sydney</city>
        <state>NSW</state>
        <country>Australia</country>
      </otherAddress>
      <isActive>true</isActive>
      <acceptDirectDeposit>false</acceptDirectDeposit>
      <acceptCheque>false</acceptCheque>
      <customField1>This is custom field 1</customField1>
      <customField2>This is custom field 2</customField2>
    </contact>
  </insertContact>
</tasks>
";

			string url = RestProxy.MakeUrl("Tasks");
			string result = HttpUtils.Post(url, xml);
		}


		// [Test]
		public void InsertInventoryItem()
		{
			string id = DateTime.Now.Ticks.ToString();
			string xml =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<tasks>
  <insertInventoryItem>
    <!-- NOTE: Replaced all Uids with the ones from your file. -->
    <inventoryItem uid=""0"">
      <code>CODE_" +
				id + @"</code>
      <description>Description for " + id +
				@"</description>
      <isActive>true</isActive>
      <notes>Notes for CODE_0c8-4ab2-9333-0890659b5d51</notes>
      <isInventoried>true</isInventoried>
      <assetAccountUid>87346</assetAccountUid> 
      <stockOnHand>0</stockOnHand>
      <currentValue>0</currentValue>
      <isBought>true</isBought>
      <purchaseExpenseAccountUid>0</purchaseExpenseAccountUid>
      <purchaseTaxCode>G11</purchaseTaxCode>
      <minimumStockLevel>99</minimumStockLevel>
      <primarySupplierContactUid>205235</primarySupplierContactUid>
      <primarySupplierItemCode>S_CODE</primarySupplierItemCode>
      <defaultReOrderQuantity>20</defaultReOrderQuantity>
      <isSold>true</isSold>
      <saleIncomeAccountUid>87349</saleIncomeAccountUid>
      <saleTaxCode>G1</saleTaxCode>
      <saleCoSAccountUid>87348</saleCoSAccountUid>
      <sellingPrice>7.75</sellingPrice>
      <isSellingPriceIncTax>true</isSellingPriceIncTax>
    </inventoryItem>
  </insertInventoryItem>
</tasks>
";

			string url = RestProxy.MakeUrl("Tasks");
			string result = HttpUtils.Post(url, xml);
		}

		// [Test]
		public void InsertContact2()
		{
			string xml =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
		<tasks>
		<insertContact>
		<contact uid=""0"">
			<salutation/>
			<givenName>fred</givenName>
			<familyName>person</familyName>
			<organisationName/>
			<organisationAbn/>
			<organisationWebsite/>
			<organisationPosition/>
			<email>test@person.com</email>
			<mainPhone>123456789</mainPhone>
			<homePhone/>
			<mobilePhone/>
			<tags>retail customer, retail, customer</tags>
			<isActive>true</isActive>
		</contact>
		</insertContact>
		</tasks>
		";
			string url = RestProxy.MakeUrl("Tasks");
			string result = HttpUtils.Post(url, xml);
			Console.WriteLine("Result:");
			Console.WriteLine(result);
		}

		// [Test]
		public void InsertInventoryItem2()
		{
			string xml =
				@"<insertInventoryItem>
<inventoryItem uid=""0""> 
<code>LS3900</code>
<description>13-foot Heavy Duty Light  Stand</description> 
<isActive>true</isActive> 
<notes></notes> 
<isInventoried></isInventoried> 
<assetAccountUid></assetAccountUid> 
<stockOnHand></stockOnHand> 
<currentValue></currentValue> 
<isBought>false</isBought> 
<purchaseExpenseAccountUid></purchaseExpenseAccountUid> 
<purchaseTaxCode></purchaseTaxCode> 
<minimumStockLevel></minimumStockLevel> 
<primarySupplierContactUid></primarySupplierContactUid> 
<primarySupplierItemCode></primarySupplierItemCode> 
<defaultReOrderQuantity></defaultReOrderQuantity> 
<isSold></isSold> 
<saleIncomeAccountUid></saleIncomeAccountUid> 
<saleTaxCode></saleTaxCode> 
<saleCoSAccountUid></saleCoSAccountUid> 
<sellingPrice>105.00000</sellingPrice> 
<isSellingPriceIncTax></isSellingPriceIncTax> 
</inventoryItem> 
</insertInventoryItem>
";
			string url = RestProxy.MakeUrl("Tasks");
			string result = HttpUtils.Post(url, xml);
			Console.WriteLine("Result:");
			Console.WriteLine(result);
		}

		// [Test]
		public void InsertItemSaleQuote()
		{
			string xml =
				@"<insertInvoice> 
<invoice uid=""0""> 
<transactionType>S</transactionType> 
<date>2011-03-24</date> 
<contactUid>2681</contactUid> 
<shipToContactUid></shipToContactUid> 
<folderUid></folderUid>
<tags></tags> 
<reference></reference> 
<summary></summary> 
<notes>test</notes> 
<requiresFollowUp></requiresFollowUp> 
<dueOrExpiryDate></dueOrExpiryDate> 
<layout>I</layout> 
<status>Q</status> 
<invoiceNumber>&lt;Auto Number&gt;</invoiceNumber> 
<purchaseOrderNumber></purchaseOrderNumber> 
<ccy>AUD</ccy> 
<autoPopulateFxRate>true</autoPopulateFxRate> 
<fcToBcFxRate></fcToBcFxRate> 
<invoiceItems> <itemInvoiceItem> 
<quantity>1</quantity> 
<inventoryItemUid>637345</inventoryItemUid> 
<description>13-foot Heavy Duty Light  Stand</description> 
<taxCode>GST</taxCode> 
<unitPriceInclTax>115.50</unitPriceInclTax> 
<percentageDiscount></percentageDiscount> 
</itemInvoiceItem> <itemInvoiceItem> 
<quantity>1</quantity> 
<inventoryItemUid>637346</inventoryItemUid> 
<description>Shipping for Joomla / Virtuemart sales</description> 
<taxCode>GST</taxCode> 
<unitPriceInclTax>38.5</unitPriceInclTax> 
<percentageDiscount></percentageDiscount> 
</itemInvoiceItem> 
</invoiceItems> 
</invoice> 
</insertInvoice>
			";
			string url = RestProxy.MakeUrl("Tasks");
			string result = HttpUtils.Post(url, xml);
			Console.WriteLine("Result:");
			Console.WriteLine(result);
		}

		// [Test]
		public void InsertJournal_MoreThan2Decimals()
		{
			string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<tasks xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""
http://www.w3.org/2001/XMLSchema"">
<insertJournal>
<journal uid=""0"">
<utcLastModified>0001-01-01T00:00:00</utcLastModified>
<date>2012-07-03</date>
<summary>Pay run for pay period ending: 06/07/2012</summary>
<requiresFollowUp>false</requiresFollowUp>
<autoPopulateFxRate>true</autoPopulateFxRate>
<fcToBcFxRate>0</fcToBcFxRate>
<reference>233</reference>
<journalItems>
<journalItem>
<accountUid>299019</accountUid>
<amount>9761.66000</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<taxCode>W1</taxCode>
<amount>13300.98648</amount>
<type>Debit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<taxCode>W1</taxCode>
<amount>0</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<taxCode>W1</taxCode>
<amount>0</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<taxCode>W1</taxCode>
<amount>0</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<taxCode>W1</taxCode>
<amount>0</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<taxCode>W1</taxCode>
<amount>0</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<taxCode>W1</taxCode>
<amount>0</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>2361</accountUid>
<taxCode>W1</taxCode>
<amount>0</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>299028</accountUid>
<taxCode>W1</taxCode>
<amount>301.92000</amount>
<type>Debit</type>
</journalItem>
<journalItem>
<accountUid>299028</accountUid>
<taxCode>W1</taxCode>
<amount>0</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<taxCode>W1</taxCode>
<amount>0</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<taxCode>W1</taxCode>
<amount>969.24648</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>741</accountUid>
<amount>2872.00000</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>745</accountUid>
<amount>1197.08000</amount>
<type>Debit</type>
</journalItem>
<journalItem>
<accountUid>743</accountUid>
<amount>1197.08000</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>744</accountUid>
<amount>969.23000</amount>
<type>Debit</type>
</journalItem>
<journalItem>
<accountUid>2374</accountUid>
<amount>969.23000</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>2371</accountUid>
<taxCode>W1</taxCode>
<amount>2872.00000</amount>
<type>Credit</type>
</journalItem>
<journalItem>
<accountUid>2371</accountUid>
<taxCode>W1,W2</taxCode>
<amount>2872.00000</amount>
<type>Debit</type>
</journalItem>
</journalItems>
</journal>
</insertJournal>
</tasks>
";
			string url = RestProxy.MakeUrl("Tasks");
			string result = HttpUtils.Post(url, xml);

			Console.WriteLine("Result:");
			Console.WriteLine(result);

			var tasksResponse = (TasksResponse)XmlSerializationUtils.Deserialize(typeof(TasksResponse), result);
			Assert.AreEqual(0, tasksResponse.Errors.Count, "Should save successfully.");
		}
	}
}
