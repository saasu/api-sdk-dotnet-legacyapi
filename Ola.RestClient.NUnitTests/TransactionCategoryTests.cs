namespace Ola.RestClient.NUnitTests
{
	using NUnit.Framework;

	using Ola.RestClient.Dto;
	using Ola.RestClient.Exceptions;
	using Ola.RestClient.Proxies;
	using Ola.RestClient.Tasks;
	
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Xml;


	[TestFixture]
	public class TransactionCategoryTests : BaseTests 
	{
		[Test]
		public void TestInsertAndGet()
		{
			TransactionCategoryDto dto = this.GetNewTransactionCategory(AccountType.Income, "For Testing Insert And Get");

			CrudProxy proxy = new TransactionCategoryProxy();
			proxy.Insert(dto);
			
			Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after insert.");

			TransactionCategoryDto fromOla = (TransactionCategoryDto) proxy.GetByUid(dto.Uid);
			AssertEqual(dto, fromOla);
		}


		[Test]
		public void TestUpdate()
		{
			TransactionCategoryDto dto = this.GetNewTransactionCategory(AccountType.Income, "For Testing Update");
			
			CrudProxy proxy = new TransactionCategoryProxy();
			proxy.Insert(dto);
			
			Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after insert.");

			dto.Name += " - Post Update";
			proxy.Update(dto);
			
			TransactionCategoryDto fromOla = (TransactionCategoryDto) proxy.GetByUid(dto.Uid);
			AssertEqual(dto, fromOla);
		}


		[Test]
		public void TestUpdateResultingInRecordHasChangedException()
		{
			TransactionCategoryDto dto = this.GetNewTransactionCategory(AccountType.Income, "For TestUpdateResultingInRecordHasChangedException");
			
			CrudProxy proxy = new TransactionCategoryProxy();
			proxy.Insert(dto);
			
			int uid = dto.Uid;
			string lastUpdatedUid = dto.LastUpdatedUid;

			dto.Name = this.TestUid + "For TestUpdateSyncCheckError - Updated";
			proxy.Update(dto);

			//
			//	Ensure last updated uid is different
			//
			Assert.IsTrue(lastUpdatedUid != dto.LastUpdatedUid, "The LastUpdatedUid should have been updated (1).");
			
			TransactionCategoryDto dto2 = (TransactionCategoryDto) proxy.GetByUid(dto.Uid);
			Assert.IsTrue(lastUpdatedUid != dto.LastUpdatedUid, "The LastUpdatedUid should have been updated (2).");
			
			dto.LastUpdatedUid = lastUpdatedUid;
			dto.Type = AccountType.OtherIncome;

			try
			{
				proxy.Update(dto);
				throw new Exception("The expected exception is not thrown.");
			}
			catch(RestException ex)
			{
				//	This was expected.
				Assert.AreEqual("RecordHasChangedException", ex.Type);
			}
		}


		[Test]
		public void TestInsertReturnsError()
		{
			CrudProxy proxy = new TransactionCategoryProxy();
			TransactionCategoryDto dto = this.GetNewTransactionCategory(AccountType.Asset, "For TestInsertReturnsError");
			proxy.Insert(dto);
			
			//
			//	Set the uid to 0 - and re-insert
			//
			dto.Uid = 0;
			try
			{
				proxy.Insert(dto);
				throw new Exception("Exception expected.");
			}
			catch(RestException ex)
			{
				Assert.AreEqual("DuplicateNameException", ex.Type, "Incorrect error type.");
			}
		}
		

		[Test]
		public void TestDelete()
		{
			CrudProxy proxy = new TransactionCategoryProxy();
			
			TransactionCategoryDto dto = this.GetNewTransactionCategory(AccountType.Liability, "TestDelete");
			proxy.Insert(dto);
			
			proxy.DeleteByUid(dto.Uid);
			
			try
			{
				proxy.GetByUid(dto.Uid);
				throw new Exception("Exception expected.");
			}
			catch(RestException ex)
			{
				Assert.AreEqual("RecordNotFoundException", ex.Type, "Incorrect error type.");
			}
		}


        [Test]
        public void TestInsertAndGetLiability()
        {
            TransactionCategoryDto dto = this.GetNewTransactionCategory(AccountType.Liability, "For Testing Insert And Get of Liability account");

            CrudProxy proxy = new TransactionCategoryProxy();
            proxy.Insert(dto);

            Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after insert.");

            TransactionCategoryDto fromOla = (TransactionCategoryDto)proxy.GetByUid(dto.Uid);
            AssertEqual(dto, fromOla);
        }


        [Test]
        public void TestInsertAndGetLoanForBackwardCompatibility()
        {
            TransactionCategoryDto dto = this.GetNewTransactionCategory("Loan", "For Testing Insert And Get Loan Account");

            CrudProxy proxy = new TransactionCategoryProxy();
            proxy.Insert(dto);

            dto.Type = AccountType.Liability;

            Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after insert.");

            TransactionCategoryDto fromOla = (TransactionCategoryDto)proxy.GetByUid(dto.Uid);
            AssertEqual(dto, fromOla);
        }

        [Test]
        public void TestInsertAndGetHeaderAccount()
        {
            TransactionCategoryDto dto = this.GetNewTransactionCategory(AccountType.Income, "For Testing Insert And Get Header");
            dto.Level = Constants.AccountLevel.Header;

            CrudProxy proxy = new TransactionCategoryProxy();
            proxy.Insert(dto);

            Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after insert.");

            TransactionCategoryDto fromOla = (TransactionCategoryDto)proxy.GetByUid(dto.Uid);
            AssertEqual(dto, fromOla);
        }

	    [Test]
	    public void TestInsertAndGetDetailAccountWithHeaderAccountAssigned()
	    {
            //set up header account first.
            TransactionCategoryDto dto = this.GetNewTransactionCategory(AccountType.Expense, "Header Account For Detail");
            dto.Level = Constants.AccountLevel.Header;

            CrudProxy proxy = new TransactionCategoryProxy();
            proxy.Insert(dto);
                        
            Assert.IsTrue(dto.Uid > 0, "Uid must be > 0 after insert.");
	        var headerAccountUid = dto.Uid; 

            //set up a detail account with the above header account assigned to it.
	        dto = GetNewTransactionCategory(AccountType.Expense, "Detail with Header");
	        dto.Level = Constants.AccountLevel.Detail;
	        dto.HeaderAccountUid = headerAccountUid; 
            proxy.Insert(dto);
            
            TransactionCategoryDto fromOla = (TransactionCategoryDto)proxy.GetByUid(dto.Uid);
            AssertEqual(dto, fromOla);
        }

        private TransactionCategoryDto GetNewTransactionCategory(string accountType, string accountName)
		{
			TransactionCategoryDto dto = new TransactionCategoryDto();
			dto.Type = accountType;
			dto.Name = this.TestUid + accountName;
			dto.LedgerCode = System.Guid.NewGuid().ToString().Substring(0, 5); 
			dto.DefaultTaxCode = "G1"; 
			return dto;
		}
		

		public static void AssertEqual(TransactionCategoryDto expected, TransactionCategoryDto actual)
		{
			Assert.AreEqual(expected.Uid, actual.Uid, "Different Uid.");
		    Assert.AreEqual(expected.Level.ToLowerInvariant(), actual.Level.ToLowerInvariant(), "Different Account Level.");
			Assert.AreEqual(expected.Type, actual.Type, "Different Type.");
			Assert.AreEqual(expected.Name, actual.Name, "Different Name.");
			Assert.AreEqual(expected.LedgerCode, actual.LedgerCode, "Different LedgerCode."); 
			Assert.AreEqual(expected.DefaultTaxCode, actual.DefaultTaxCode, "Different DefaultTaxCode.");
			Assert.AreEqual(expected.IsActive, actual.IsActive, "Different IsActive.");
            Assert.AreEqual(expected.HeaderAccountUid, actual.HeaderAccountUid, "Different Header Account Uid.");
		}
	}
}
