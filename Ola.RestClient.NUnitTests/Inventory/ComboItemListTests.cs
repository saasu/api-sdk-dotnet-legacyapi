using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Netaccounts.Core.Data.Utils;
using NUnit.Framework;
using Ola.RestClient.Dto.Inventory;
using Ola.RestClient.Proxies;

namespace Ola.RestClient.NUnitTests.Inventory
{
	public class ComboItemListTests : ComboItemTests
	{
		[Test]
		public void GetByCode()
		{
			ComboItemProxy proxy = new ComboItemProxy();
			ComboItemDto dto = GetComboItem01();
			proxy.Insert(dto);

			List<ComboItemDto> list = proxy.FindList<ComboItemDto>(ComboItemProxy.ResponseXPath,
																		  "CodeBeginsWith", dto.Code);
			Assert.AreEqual(1, list.Count, string.Format("Incorrect number of combo items returned for code {0}", dto.Code));
		}


		[Test]
		public void GetByDescription()
		{
			string description01 = Guid.NewGuid().ToString().Substring(0, 15);
			string description02 = description01 + "foo";
			ComboItemProxy proxy = new ComboItemProxy();
			ComboItemDto dto = GetComboItem01();

			dto.Description = description01;
			proxy.Insert(dto);

			dto = GetComboItem01();
			dto.Description = description02;
			proxy.Insert(dto);

			List<ComboItemDto> list = proxy.FindList<ComboItemDto>(ComboItemProxy.ResponseXPath,
			                                                       "DescriptionBeginsWith", description01);

			Assert.AreEqual(2, list.Count, "Incorrect number of combo items returned for description " );
		}


		[Test]
		public void GetByUtcLastModified()
		{
			ComboItemProxy proxy = new ComboItemProxy();

			Thread.Sleep(10000); //Wait for ten seconds. Avoid retreiving items from text fixture setup.

			ComboItemDto dto = GetComboItem01();
			proxy.Insert(dto);

			DateTime lastModifiedFrom = ((ComboItemDto)proxy.GetByUid(dto.Uid)).LastModified;
			Thread.Sleep(10000); //Wait for ten seconds.

			dto = GetComboItem01();
			proxy.Insert(dto);

			DateTime lastModifiedTo = ((ComboItemDto)proxy.GetByUid(dto.Uid)).LastModified.AddSeconds(1);

			string utcFrom = SqlStrPrep.ToDateTime(lastModifiedFrom).ToString();
			string utcTo = SqlStrPrep.ToDateTime(lastModifiedTo).ToString();

			utcFrom = utcFrom.Replace('/', '-');
			utcFrom = utcFrom.Replace(' ', 'T');

			utcTo = utcTo.Replace('/', '-');
			utcTo = utcTo.Replace(' ', 'T');

			List<ComboItemDto> list = proxy.FindList<ComboItemDto>(ComboItemProxy.ResponseXPath,
																		  "UtcLastModifiedFrom", utcFrom, "UtcLastModifiedTo", utcTo);
			Assert.AreEqual(2, list.Count, string.Format("Incorrect number of combo items returned for UtcLastModifiedFrom {0} and UtcLastModifiedTo{1}", utcFrom, utcTo));
		}
	}
}
