namespace Ola.RestClient.NUnitTests
{
	using NUnit.Framework;

	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Proxies;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;

	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Xml;


	[TestFixture]
	public class ManualUpdate
	{
		[Test]
		public void GetInventoryItemList()
		{
			string url = "http://localhost/restapi/inventoryitemlist?wsaccesskey=4962-A8E9-D414-4DCA-80F2-4A77-CEE6-7DBD&fileuid=1697";
			// string url = "https://secure.saasu.com/alpha/webservices/rest/r1/inventoryitemlist?wsaccesskey=4962-A8E9-D414-4DCA-80F2-4A77-CEE6-7DBD&fileuid=1697";
			string xmlFragment = HttpUtils.Get(url);

			inventoryItemListResponse response = (inventoryItemListResponse)XmlSerializationUtils.Deserialize(typeof(inventoryItemListResponse), xmlFragment);
			inventoryItemListResponseInventoryItemList list = response.Items[0];
			Console.WriteLine("Number of items: {0}", list.inventoryItemListItem.Length);

			foreach (inventoryItemListResponseInventoryItemListInventoryItemListItem item in list.inventoryItemListItem)
			{
				// Console.WriteLine("Inventory item uid: {0}", item.inventoryItemUid);
				this.Count(item.code);
			}
		}


		private void Count(string code)
		{
			string url = "http://localhost/restapi/inventoryitemlist?wsaccesskey=4962-A8E9-D414-4DCA-80F2-4A77-CEE6-7DBD&fileuid=1697&codebeginswith=" + code;
			// string url = "https://secure.saasu.com/alpha/webservices/rest/r1/inventoryitemlist?wsaccesskey=4962-A8E9-D414-4DCA-80F2-4A77-CEE6-7DBD&fileuid=1697&codebeginswith=" + code;
			string xmlFragment = HttpUtils.Get(url);

			// inventoryItemListResponse response = (inventoryItemListResponse)XmlSerializationUtils.Deserialize(typeof(inventoryItemListResponse), xmlFragment);
			// inventoryItemListResponseInventoryItemList list = response.Items[0];
			// Console.WriteLine("Find {0}. # of items = {1}", code, list.inventoryItemListItem.Length);
		}


		[Test]
		public void Post()
		{

			// string url = "https://secure.saasu.com/webservices/rest/r1/tasks?wsaccesskey=4962-A8E9-D414-4DCA-80F2-4A77-CEE6-7DBD&fileuid=1697";
			string url = "http://localhost/restapi/tasks?wsaccesskey=4962-A8E9-D414-4DCA-80F2-4A77-CEE6-7DBD&fileuid=1697";
			string xmlFragment = @"<?xml version=""1.0"" encoding=""utf-16""?>  
 <tasks>
  <insertInvoice>
    <invoice uid=""0"">
      <transactionType>S</transactionType>
      <date>2009-10-28</date>
      <contactUid>0</contactUid>
      <shipToContactUid>0</shipToContactUid>
      <summary>invoice for GOW order O200910788</summary>
      <requiresFollowUp>false</requiresFollowUp>
      <layout>I</layout>
      <status>I</status>
      <invoiceNumber>&amp;lt;Auto Number&amp;gt;</invoiceNumber>
      <invoiceItems>
        <itemInvoiceItem>
          <quantity>1</quantity>
          <inventoryItemUid>2995</inventoryItemUid>
          <description>gow_test_code_1</description>
          <taxCode>G1</taxCode>
          <unitPriceInclTax>30.0</unitPriceInclTax>
          <percentageDiscount>0</percentageDiscount>
        </itemInvoiceItem>
        <itemInvoiceItem>
          <quantity>1</quantity>
          <inventoryItemUid>2995</inventoryItemUid>
          <description>Delivery Charge</description>
          <taxCode>G1</taxCode>
          <unitPriceInclTax>30.0</unitPriceInclTax>
          <percentageDiscount>0</percentageDiscount>
        </itemInvoiceItem>
      </invoiceItems>
      <quickPayment>
        <datePaid>2009-10-28</datePaid>
        <bankedToAccountUid>76535</bankedToAccountUid>
        <amount>60.0</amount>
        <reference>Payment for GOW order number O200910788</reference>
      </quickPayment>
    </invoice>
  </insertInvoice>
</tasks>
";
			string result = HttpUtils.Post(url, xmlFragment);

		}

	}
}
