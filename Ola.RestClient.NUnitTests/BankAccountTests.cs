namespace Ola.RestClient.NUnitTests
{
	using NUnit.Framework;

	using Ola.RestClient;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Dto;
	using Ola.RestClient.Proxies;
	
	using System;
	using System.Net;
	using System.Collections;
	
	
	[TestFixture]
	public class BankAccountTests : BaseTests
	{
		private TransactionCategoryDto _merchantFeeDto;

		[TestFixtureSetUp]
		public void Setup()
		{
			_merchantFeeDto = new TransactionCategoryDto {Type = "Expense", Name = "MF " + DateTime.UtcNow.Ticks};
			new TransactionCategoryProxy().Insert(_merchantFeeDto);
		}

		[Test]
		public void TestInsertAndGet()
		{
			CrudProxy proxy = new BankAccountProxy();
			BankAccountDto dto1 = this.GetBankAccount();
			proxy.Insert(dto1);
			
			Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");
			
			BankAccountDto dto2 = (BankAccountDto) proxy.GetByUid(dto1.Uid);
			
			AssertEqual(dto1, dto2);
		}

        [Test]
        public void TestInsertAndNullMechantFeeId()
        {
            CrudProxy proxy = new BankAccountProxy();
            BankAccountDto dto1 = this.GetBankAccount();
            dto1.MerchantFeeAccountUid = null;

            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            BankAccountDto dto2 = (BankAccountDto)proxy.GetByUid(dto1.Uid);

            Assert.AreEqual(dto1.MerchantFeeAccountUid, dto2.MerchantFeeAccountUid);
        }

		[Test]
		public void TestInsertInvalidMerchantFee()
		{
			CrudProxy proxy = new BankAccountProxy();
			BankAccountDto dto1 = this.GetBankAccount();
			dto1.MerchantFeeAccountUid = 99999;

			try
			{
				proxy.Insert(dto1);	
				Assert.Fail("No exception thrown.");
			}
			catch(RestException rx)
			{
				Assert.AreEqual("The specified merchant fee account does not exist.", rx.Message, "Incorrect message.");
			}
		}

		public void TestUpdate()
		{
			CrudProxy proxy = new BankAccountProxy();
			BankAccountDto dto1 = this.GetBankAccount();
			proxy.Insert(dto1);
			
			string lastUpdatedUid = dto1.LastUpdatedUid;

			dto1.Type = AccountType.Liability;
		
			proxy.Update(dto1);
			
			Assert.IsTrue(dto1.LastUpdatedUid != lastUpdatedUid);
			
			BankAccountDto dto2 = (BankAccountDto) proxy.GetByUid(dto1.Uid);
			
			AssertEqual(dto1, dto2);
		}


		[Test]
		public void TestDelete()
		{
			CrudProxy proxy = new BankAccountProxy();
			
			BankAccountDto dto1 = this.GetBankAccount();
			proxy.Insert(dto1);
			
			proxy.DeleteByUid(dto1.Uid);
			
			try
			{
				proxy.GetByUid(dto1.Uid);
				throw new Exception("The expected exception was not thrown.");
			}
			catch(RestException ex)
			{
				Assert.AreEqual("RecordNotFoundException", ex.Type, "Incorrect exception type.");
			}
		}


		private static void AssertEqual(BankAccountDto expected, BankAccountDto actual)
		{
			TransactionCategoryTests.AssertEqual(expected, actual);
			Assert.AreEqual(expected.DisplayName, actual.DisplayName, "Different display name.");
			Assert.AreEqual(expected.BSB, actual.BSB, "Different BSB.");
			Assert.AreEqual(expected.AccountNumber, actual.AccountNumber, "Different account number.");
		}

		private BankAccountDto GetBankAccount()
		{
			BankAccountDto dto = new BankAccountDto();
			dto.Type = AccountType.Asset;
			dto.Name = "CBA " + System.Guid.NewGuid().ToString();
			dto.DisplayName = dto.Name + " DisplayName";
			dto.LedgerCode = Guid.NewGuid().ToString().Substring(0, 5); 
			dto.IsActive = true;
			dto.BSB = "111-111";
			dto.AccountNumber = "12345-6789";
			dto.MerchantFeeAccountUid = _merchantFeeDto.Uid;
			return dto;
		}
	}
}
