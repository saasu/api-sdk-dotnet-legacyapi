using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Ola;
using Ola.RestClient.Dto;
using Ola.RestClient.Proxies;
using NUnit.Framework;


namespace Ola.RestClient.NUnitTests
{
	[TestFixture]
	public class InventoryItemListTests : RestClientTests
	{
		[Test]
		public void GetByCode()
		{
			string code1 =  Guid.NewGuid().ToString().Substring(0, 10);
			string code2 = code1 + "foo";
			InventoryItemProxy proxy = new InventoryItemProxy();

			InventoryItemDto dto = new InventoryItemDto();
			dto.Code = code1;
			dto.Description = "New Inventory Item";			
			proxy.Insert(dto);

			dto = new InventoryItemDto();
			dto.Code = code2;
			dto.Description = "New Inventory Item"; 
			proxy.Insert(dto);

			List<InventoryItemDto> list = proxy.FindList<InventoryItemDto>(InventoryItemProxy.ResponseXPath,
			                                                              "CodeBeginsWith", code1);
			Assert.AreEqual(2, list.Count, string.Format("Incorrect number of items returned for code {0}",code1));
		}


		[Test]
		public void GetByDescription()
		{
			string code1 = Guid.NewGuid().ToString().Substring(0, 10);
			string code2 = Guid.NewGuid().ToString().Substring(0, 10);

			string description1 = Guid.NewGuid().ToString().Substring(0, 20);
			string description2 = description1 + "bar";

			InventoryItemProxy proxy = new InventoryItemProxy();

			InventoryItemDto dto = new InventoryItemDto();
			dto.Code = code1;
			dto.Description = description1;
			proxy.Insert(dto);

			dto = new InventoryItemDto();
			dto.Code = code2;
			dto.Description = description2;
			proxy.Insert(dto);

			List<InventoryItemDto> list = proxy.FindList<InventoryItemDto>(InventoryItemProxy.ResponseXPath,
			                                                              "DescriptionBeginsWith", description1);
			Assert.AreEqual(2, list.Count, string.Format("Incorrect number of items returned for code {0}", description1));
		}


		[Test]
		public void GetByUtcLastModified()
		{
			string code1 = Guid.NewGuid().ToString().Substring(0, 10);
			string code2 = code1 + "foo";
			InventoryItemProxy proxy = new InventoryItemProxy();

			Thread.Sleep(10000); //Wait for ten seconds. Avoid retreiving items from text fixture setup.

			InventoryItemDto dto = new InventoryItemDto();
			dto.Code = code1;
			dto.Description = "New Inventory Item";
			proxy.Insert(dto);

			DateTime lastModifiedFrom = ((InventoryItemDto)proxy.GetByUid(dto.Uid)).UtcLastModified;
			Thread.Sleep(10000); //Wait for ten seconds.

			dto = new InventoryItemDto();
			dto.Code = code2;
			dto.Description = "New Inventory Item";
			proxy.Insert(dto);

			DateTime lastModifiedTo = ((InventoryItemDto)proxy.GetByUid(dto.Uid)).UtcLastModified.AddSeconds(1);

			System.Console.WriteLine("Utc last modified: {0}", lastModifiedFrom.ToString());

			string utcFrom = SqlStrPrep.ToDateTime(lastModifiedFrom).ToString();
			string utcTo   = SqlStrPrep.ToDateTime(lastModifiedTo).ToString();

			utcFrom = utcFrom.Replace('/', '-');
			utcFrom = utcFrom.Replace(' ', 'T');

			utcTo = utcTo.Replace('/', '-');
			utcTo = utcTo.Replace(' ', 'T');

			List<InventoryItemDto> list = proxy.FindList<InventoryItemDto>(InventoryItemProxy.ResponseXPath,
																		   "UtcLastModifiedFrom", utcFrom, "UtcLastModifiedTo", utcTo);
			Assert.AreEqual(2, list.Count, string.Format("Incorrect number of items returned for UtcLastModifiedFrom {0} and UtcLastModifiedTo{1}", utcFrom, utcTo));
		}
	}
}
