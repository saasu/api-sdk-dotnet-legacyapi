using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Journals;
using Ola.RestClient.Proxies;

namespace Ola.RestClient.NUnitTests
{
	public class TestOutputSanitising : RestClientTests
	{
		[Test]
		public void TestRemovalOfAsciiControlCharacters()
		{
			var proxy = new JournalProxy();
			var dto = CreateJournalDto();
			proxy.Insert(dto);

			Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after save.");

			// Ascii control characters in the summary string below are not visible in visual studio. you need to copy this string to notepad++ to view the non printable characters
			const string summaryTextWithControlCharacters = "Control characters start between these hashes ##  ## control characters end";
			const string summaryTextWithoutControlCharacters = "Control characters start between these hashes ##  ## control characters end";
			dto.Summary = summaryTextWithControlCharacters;

			proxy.Update(dto);

			var read = (JournalDto)proxy.GetByUid(dto.Uid);

			Assert.IsTrue(read.Summary.Equals(summaryTextWithoutControlCharacters, StringComparison.OrdinalIgnoreCase));
		}

		private JournalDto CreateJournalDto()
		{
			JournalDto dto = new JournalDto();
			dto.Date = DateTime.UtcNow.Date;
			dto.Notes = "Notes";
			dto.RequiresFollowUp = false;
			dto.Reference = "#12345";
			dto.Summary = "Summary";
			dto.Tags = "ABC, DEF";
			dto.FCToBCFXRate = 1M;

			JournalItemDto row1 = new JournalItemDto();
			row1.AccountUid = this.ExpenseOffice.Uid;
			row1.TaxCode = TaxCode.ExpInclGst;
			row1.Type = DebitCreditType.Credit;
			row1.Amount = 123.45M;

			JournalItemDto row2 = new JournalItemDto();
			row2.AccountUid = this.ExpenseTelco.Uid;
			row2.TaxCode = TaxCode.ExpInclGst;
			row2.Type = DebitCreditType.Debit;
			row2.Amount = 123.45M;

			dto.Items = new JournalItemDto[]
			            	{
								row1, row2
			            	};

			return dto;
		}

	}
}
