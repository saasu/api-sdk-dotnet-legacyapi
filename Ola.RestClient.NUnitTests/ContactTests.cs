using Ola.RestClient.Dto.Enums;

namespace Ola.RestClient.NUnitTests
{
    using NUnit.Framework;

    using Ola.RestClient;
    using Ola.RestClient.Dto;
    using Ola.RestClient.Exceptions;
    using Ola.RestClient.Proxies;
    using Ola.RestClient.Utils;

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;


    [TestFixture]
    public class ContactTests : BaseTests
    {
        protected ContactCategoryDto ProspectCustomerCategory;
        protected ContactCategoryDto CustomerCategory;
        protected ContactCategoryDto ITCategory;
        protected ContactCategoryDto MiningCategory;

        [Test]
        public void InsertAndGet()
        {
            CrudProxy proxy = new ContactProxy();

            ContactDto dto1 = this.GetContact1();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            ContactDto dto2 = (ContactDto)proxy.GetByUid(dto1.Uid);

            AssertEqual(dto1, dto2);
        }


        [Test]
        public void InsertWithTags()
        {
            CrudProxy proxy = new ContactProxy();

            ContactDto dto1 = this.GetContact4();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            ContactDto dto2 = (ContactDto)proxy.GetByUid(dto1.Uid);
            AssertEqual(dto1, dto2, true);
        }

        [Test]
        public void InsertUsingUnsupportedSalutation()
        {
            CrudProxy proxy = new ContactProxy();

            ContactDto dto1 = this.GetContact1();
            dto1.Salutation = "Sir";

            // Should be ok, but won't be shown on UI.
            proxy.Insert(dto1);
        }

        [Test]
        public void InsertContactWithBankDetails()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact3();
            proxy.Insert(dto1);

            AssertEqual(dto1, (ContactDto)proxy.GetByUid(dto1.Uid));
        }

        [Test]
        public void Update()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            int tempContactUid = dto1.Uid;
            string lastUpdatedUid = dto1.LastUpdatedUid;

            dto1 = this.GetContact2();
            dto1.Uid = tempContactUid;
            dto1.LastUpdatedUid = lastUpdatedUid;
            proxy.Update(dto1);

            AssertEqual(dto1, (ContactDto)proxy.GetByUid(dto1.Uid));
        }


        [Test]
        public void UpdateContactOrganizationDetails()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after save.");

            int tempContactUid = dto1.Uid;
            string lastUpdatedUid = dto1.LastUpdatedUid;

            dto1 = this.GetContact2();
            dto1.OrganisationName = "Updated Organization Name";
            dto1.OrganisationAbn = "U1234567";
            dto1.OrganisationPosition = "Updated Organization Position";
            dto1.OrganisationWebsite = "www.updatedorg.com";

            dto1.Uid = tempContactUid;
            dto1.LastUpdatedUid = lastUpdatedUid;
            proxy.Update(dto1);

            var tmpDto = (ContactDto)proxy.GetByUid(dto1.Uid);
            AssertEqual(dto1, tmpDto);
        }

        [Test]
        public void Delete()
        {
            CrudProxy proxy = new ContactProxy();

            ContactDto dto1 = this.GetContact1();
            proxy.Insert(dto1);

            proxy.DeleteByUid(dto1.Uid);

            try
            {
                proxy.GetByUid(dto1.Uid);
                throw new Exception("The expected exception was not thrown.");
            }
            catch (RestException ex)
            {
                Assert.AreEqual("RecordNotFoundException", ex.Type, "Incorrect exception type.");
            }
        }

        [Test]
        public void InvalidABN()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContactInvalidAbn();
            proxy.Insert(dto1);

            ContactDto fromDB = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual("ABC123", fromDB.OrganisationAbn, "Incorrect org. abn.");
        }


        [Test]
        public void InsertWithCustomField()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.CustomField1 = "This is custom field 1";
            dto1.CustomField2 = "This is custom field 2";
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual("This is custom field 1", dto1.CustomField1, "Incorrect custom field 1");
            Assert.AreEqual("This is custom field 2", dto1.CustomField2, "Incorrect custom field 2");
        }


        [Test]
        public void InsertWithTwitterIdAndSkypeID()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.TwitterId = "Contact1TwitterId";
            dto1.SkypeId = "Contact1SkypeId";

            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual("Contact1TwitterId", dto1.TwitterId, "Incorrect twitter ID");
            Assert.AreEqual("Contact1SkypeId", dto1.SkypeId, "Incorrect skype ID");
        }


        [Test]
        public void InsertWithCustomFieldLongerThan50Chars()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.CustomField1 = "This is custom field 1 This is custom field 1 This is custom field 1 This is custom field 1 This is custom field 1 This is custom field 1 This is custom field 1";
            try
            {
                proxy.Insert(dto1);
                throw new Exception("The expected exception is not thrown.");
            }
            catch (RestException rex)
            {
                Assert.AreEqual("The custom field 1 must not exceed 50 characters.", rex.Message, "Incorrect exception message.");
            }
        }


        [Test]
        public void UpdateWithCustomField()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.CustomField1 = "This is custom field 1";
            dto1.CustomField2 = "This is custom field 2";
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1.CustomField1 = null;
            dto1.CustomField2 = "AAA";
            proxy.Update(dto1);

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual(null, dto1.CustomField1, "Incorrect custom field 1");
            Assert.AreEqual("AAA", dto1.CustomField2, "Incorrect custom field 2");
        }


        [Test]
        public void UpdateWithTwitterAndSkypeID()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.TwitterId = "Contact1TwitterId1";
            dto1.SkypeId = "Contact1SkypeId1";
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1.TwitterId = null;
            dto1.SkypeId = "Contact1SkypeIdNew";
            proxy.Update(dto1);

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual(null, dto1.TwitterId, "Incorrect twitter ID");
            Assert.AreEqual("Contact1SkypeIdNew", dto1.SkypeId, "Incorrect skype ID");
        }


        [Test]
        public void UpdateWithLinkedInPublicProfile()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.LinkedInPublicProfile = "http://www.linkedin.com/pub/kasun-amarasinghe/5/984/b5b";
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1.LinkedInPublicProfile = "http://www.linkedin.com/in/marclehmann";
            proxy.Update(dto1);

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual("http://www.linkedin.com/in/marclehmann", dto1.LinkedInPublicProfile, "Incorrect LinkedIn Public Profile");
        }


        [Test]
        public void SearchByFirstNameLastNameAndOrgName()
        {
            // Look up
            // Is Carl O'Neil from O'Neil Capital already added?
            // If not add it.
            // If yes, ensure details are correct (first name, last name, org name, contact id, cf1 and cf2).
            contactListResponse response = this.SearchCarl();
            if (response.contactList.Count == 0)
            {
                this.AddCarl();
            }

            response = this.SearchCarl();
            Assert.AreEqual(1, response.contactList.Count, "Incorrect number of contacts found.");

            contactListItem cli = response.contactList[0];
            Assert.AreEqual("Carl", cli.givenName, "Incorrect given name");
            Assert.AreEqual("O'Neil", cli.familyName, "Incorrect family name");
            Assert.AreEqual("O'Neil Capital", cli.organisationName, "Incorrect organisation name");
            Assert.AreEqual("O'Neil", cli.customField1, "Incorrect custom field 1");
            Assert.AreEqual("GLD879", cli.contactID, "Incorrect contact id");
        }


        [Test]
        public void SearchByContactID()
        {
            contactListResponse response = this.SearchCarl();
            if (response.contactList.Count == 0)
            {
                this.AddCarl();
            }

            response = this.SearchByContactID("GLD879");	// Note this is the contact id field, not contact uid.
            Assert.AreEqual(1, response.contactList.Count, "Incorrect number of contacts found.");

            contactListItem cli = response.contactList[0];
            Assert.AreEqual("Carl", cli.givenName, "Incorrect given name");
            Assert.AreEqual("O'Neil", cli.familyName, "Incorrect family name");
            Assert.AreEqual("O'Neil Capital", cli.organisationName, "Incorrect organisation name");
            Assert.AreEqual("O'Neil", cli.customField1, "Incorrect custom field 1");
            Assert.AreEqual("GLD879", cli.contactID, "Incorrect contact id");
        }


        [Test]
        public void TradingTermsBackwardsCompatibility()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.SaleTradingTermsPaymentDueInInterval = 14;
            dto1.SaleTradingTermsPaymentDueInIntervalType = 1;
            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual(1, dto1.SaleTradingTerms.Type, "Incorrect Sale Trading Terms Type");
            Assert.AreEqual(14, dto1.SaleTradingTerms.Interval, "Incorrect Sale Trading Terms Interval");
            Assert.AreEqual(1, dto1.SaleTradingTerms.IntervalType, "Incorrect Sale Trading Terms Interval Type");
        }


        [Test]
        public void DueInTradingTermsType()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.SaleTradingTermsPaymentDueInInterval = 0;
            dto1.SaleTradingTermsPaymentDueInIntervalType = 0;
            dto1.PurchaseTradingTermsPaymentDueInInterval = 0;
            dto1.PurchaseTradingTermsPaymentDueInIntervalType = 0;

            dto1.SaleTradingTerms = GetSaleTradingTermsDto1();

            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual(1, dto1.SaleTradingTerms.Type, "Incorrect Sale Trading Terms Type");
            Assert.AreEqual(2, dto1.SaleTradingTerms.Interval, "Incorrect Sale Trading Terms Interval");
            Assert.AreEqual(2, dto1.SaleTradingTerms.IntervalType, "Incorrect Sale Trading Terms Interval Type");
        }


        [Test]
        public void DueInEndOfMonthPlusXDaysTermsType()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.SaleTradingTermsPaymentDueInInterval = 0;
            dto1.SaleTradingTermsPaymentDueInIntervalType = 0;
            dto1.PurchaseTradingTermsPaymentDueInInterval = 0;
            dto1.PurchaseTradingTermsPaymentDueInIntervalType = 0;

            dto1.SaleTradingTerms = GetSaleTradingTermsDto2();

            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual(2, dto1.SaleTradingTerms.Type, "Incorrect Sale Trading Terms Type");
            Assert.AreEqual(14, dto1.SaleTradingTerms.Interval, "Incorrect Sale Trading Terms Interval");
            Assert.AreEqual(1, dto1.SaleTradingTerms.IntervalType, "Incorrect Sale Trading Terms Interval Type");
        }


        [Test]
        public void CashOnDeliveryTermsType()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.SaleTradingTermsPaymentDueInInterval = 0;
            dto1.SaleTradingTermsPaymentDueInIntervalType = 0;
            dto1.PurchaseTradingTermsPaymentDueInInterval = 0;
            dto1.PurchaseTradingTermsPaymentDueInIntervalType = 0;

            dto1.SaleTradingTerms = GetSaleTradingTermsDto3();

            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual(3, dto1.SaleTradingTerms.Type, "Incorrect Sale Trading Terms Type");
            Assert.AreEqual(0, dto1.SaleTradingTerms.Interval, "Incorrect Sale Trading Terms Interval");
            Assert.AreEqual(0, dto1.SaleTradingTerms.IntervalType, "Incorrect Sale Trading Terms Interval Type");
        }


        [Test]
        public void PurchaseTradingTerms()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto1 = this.GetContact1();
            dto1.SaleTradingTermsPaymentDueInInterval = 0;
            dto1.SaleTradingTermsPaymentDueInIntervalType = 0;
            dto1.PurchaseTradingTermsPaymentDueInInterval = 0;
            dto1.PurchaseTradingTermsPaymentDueInIntervalType = 0;

            dto1.SaleTradingTerms = GetPurchaseTradingTermsDto1();

            proxy.Insert(dto1);

            Assert.IsTrue(dto1.Uid > 0, "Uid must be > 0 after insert.");

            dto1 = (ContactDto)proxy.GetByUid(dto1.Uid);
            Assert.AreEqual(2, dto1.SaleTradingTerms.Type, "Incorrect Purchase Trading Terms Type");
            Assert.AreEqual(30, dto1.SaleTradingTerms.Interval, "Incorrect Purchase Trading Terms Interval");
            Assert.AreEqual(1, dto1.SaleTradingTerms.IntervalType, "Incorrect Purchase Trading Terms Interval Type");
        }

        [Test]
        public void InsertWithDefaultSaleDiscount()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto contact = GetContact1();
            contact.DefaultSaleDiscount = 10.50M;
            proxy.Insert(contact);

            ContactDto fromDb = (ContactDto)proxy.GetByUid(contact.Uid);
            AssertEqual(contact, fromDb);
        }

        [Test]
        public void UpdateWithDefaultPurchaseDiscount()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto contact = GetContact1();
            contact.DefaultPurchaseDiscount = 15.75M;
            proxy.Insert(contact);

            contact.DefaultPurchaseDiscount = 14.75M;
            proxy.Update(contact);

            ContactDto fromDb = (ContactDto)proxy.GetByUid(contact.Uid);
            AssertEqual(contact, fromDb);
        }

        [Test]
        public void CanInsertAContractor()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto contact = GetContact1();
            contact.ContactType = (int)ContactType.Contractor;
            proxy.Insert(contact);

            var fromDb = (ContactDto)proxy.GetByUid(contact.Uid);
            AssertEqual(contact, fromDb);
            Assert.IsTrue(((ContactType)fromDb.ContactType).HasFlag(ContactType.Contractor), "Contractor flag should be set.");
        }

        [Test]
        public void CanMarkAContactAsAContractor()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto contact = GetContact1();
            proxy.Insert(contact);

            var fromDb = (ContactDto)proxy.GetByUid(contact.Uid);
            Assert.IsFalse(((ContactType)fromDb.ContactType).HasFlag(ContactType.Contractor), "Shouldn't be flagged a contactor.");

            var contactTypeFlag = ContactType.Contractor | ContactType.Supplier;
            contact.ContactType = (int)contactTypeFlag;
            proxy.Update(contact);

            fromDb = (ContactDto)proxy.GetByUid(contact.Uid);
            Assert.AreEqual((int)contactTypeFlag, fromDb.ContactType, "Incorrect contact type.");

            var contactType = (ContactType)fromDb.ContactType;
            Assert.IsTrue(contactType.HasFlag(ContactType.Contractor), "IsContractor should be true.");
            Assert.IsTrue(contactType.HasFlag(ContactType.Supplier), "IsSupplier should be true.");
            Assert.IsFalse(contactType.HasFlag(ContactType.Customer), "IsCustomer should be false.");
        }

        [Test]
        public void EnsureInvalidControlCharactersAreRemoved()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto validContact = GetContact1();
            ContactDto invalidContact = GetContact1();

            invalidContact.GivenName = invalidContact.GivenName + "\r\n";
            invalidContact.FamilyName = invalidContact.FamilyName + "\r\n";
            invalidContact.OrganisationName = invalidContact.OrganisationName + "&#13;";
            proxy.Insert(invalidContact);

            var fromDb = (ContactDto)proxy.GetByUid(invalidContact.Uid);

            Assert.AreEqual(validContact.GivenName, fromDb.GivenName, "Given name should not contain any invalid characters.");
            Assert.AreEqual(validContact.FamilyName, fromDb.FamilyName, "Family name should not contain any invalid characters");
            Assert.AreEqual(validContact.OrganisationName, fromDb.OrganisationName, "Organisation name should not contain any invalid characters.");
        }

        /// <summary>
        /// Search contact, Carl O'Neil from O'Neil Capital.
        /// </summary>
        /// <returns></returns>
        private contactListResponse SearchCarl()
        {
            NameValueCollection queries = new NameValueCollection();
            queries.Add("givenname", "carl");			// AKA first name.
            queries.Add("familyName", "o'neil");		// AKA last name.
            queries.Add("organisationName", "o'neil capital");	// AKA organisation, company.
            string url = RestProxy.MakeUrl("ContactList");
            url += "&" + Util.ToQueryString(queries);

            string xmlFragment = HttpUtils.Get(url);
            contactListResponse response = (contactListResponse)XmlSerializationUtils.Deserialize(typeof(contactListResponse), xmlFragment);

            return response;
        }


        /// <summary>
        /// Search contact by contact id (not contact uid).
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        private contactListResponse SearchByContactID(string contactID)
        {
            NameValueCollection queries = new NameValueCollection();
            queries.Add("contactid", contactID);
            string url = RestProxy.MakeUrl("ContactList");
            url += "&" + Util.ToQueryString(queries);
            string xmlFragment = HttpUtils.Get(url);
            contactListResponse response = (contactListResponse)XmlSerializationUtils.Deserialize(typeof(contactListResponse), xmlFragment);
            return response;
        }


        private void AddCarl()
        {
            CrudProxy proxy = new ContactProxy();
            ContactDto dto = new ContactDto();
            dto.Salutation = "Mr.";
            dto.GivenName = "Carl";
            dto.FamilyName = "O'Neil";
            dto.OrganisationName = "O'Neil Capital";
            dto.ContactID = "GLD879";
            dto.CustomField1 = "O'Neil";
            proxy.Insert(dto);
            Assert.IsTrue(dto.Uid > 0, "Incorrect uid post save.");
        }


        private static void AssertEqual(ContactDto expected, ContactDto actual)
        {
            AssertEqual(expected, actual, false);
        }


        private static void AssertEqual(ContactDto expected, ContactDto actual, bool checkTags)
        {
            Assert.AreEqual(expected.Uid, actual.Uid, "Different Uid.");
            Assert.AreEqual(expected.Salutation, actual.Salutation, "Different Salutation.");
            Assert.AreEqual(expected.GivenName, actual.GivenName, "Different GivenName.");
            Assert.AreEqual(expected.MiddleInitials, actual.MiddleInitials, "Different MiddleInitials.");
            Assert.AreEqual(expected.FamilyName, actual.FamilyName, "Different FamilyName.");
            Assert.AreEqual(expected.OrganisationName, actual.OrganisationName, "Different OrganisationName.");
            Assert.AreEqual(expected.OrganisationPosition, actual.OrganisationPosition, "Different OrganisationPosition.");
            Assert.AreEqual(expected.WebsiteUrl, actual.WebsiteUrl, "Different WebsiteUrl.");
            Assert.AreEqual(expected.Email, actual.Email, "Different Email.");
            Assert.AreEqual(expected.MainPhone, actual.MainPhone, "Different MainPhone.");
            Assert.AreEqual(expected.OtherPhone, actual.OtherPhone, "Different OtherPhone.");
            // No longer supported.
            //Assert.AreEqual(expected.StatusUid, actual.StatusUid, "Different StatusUid.");
            //Assert.AreEqual(expected.IndustryUid, actual.IndustryUid, "Different IndustryUid.");
            AssertEqual(expected.PostalAddress, actual.PostalAddress);
            Assert.AreEqual(expected.IsActive, actual.IsActive, "Different IsActive.");

            Assert.AreEqual(expected.AcceptDirectDeposit, actual.AcceptDirectDeposit, "Different AcceptDirectDeposit.");
            Assert.AreEqual(expected.DirectDepositBankName, actual.DirectDepositBankName, "Different DirectDepositBankName.");
            Assert.AreEqual(expected.DirectDepositAccountName, actual.DirectDepositAccountName, "Different direct deposit account name.");
            Assert.AreEqual(expected.DirectDepositBsb, actual.DirectDepositBsb, "Different Bsb.");
            Assert.AreEqual(expected.DirectDepositAccountNumber, actual.DirectDepositAccountNumber, "Different direct deposit account number.");
            Assert.AreEqual(expected.AcceptCheque, actual.AcceptCheque, "Different accept cheque.");
            Assert.AreEqual(expected.ChequePayableTo, actual.ChequePayableTo, "Different cheque payable to.");
            Assert.AreEqual(expected.AutoSendStatement, actual.AutoSendStatement, "Different auto send statement.");

            Assert.AreEqual(expected.IsPartner, actual.IsPartner, "Different IsPartner.");
            Assert.AreEqual(expected.IsCustomer, actual.IsCustomer, "Different IsCustomer.");
            Assert.AreEqual(expected.IsSupplier, actual.IsSupplier, "Different IsSupplier.");

            Assert.AreEqual(expected.LinkedInPublicProfile, actual.LinkedInPublicProfile, "Different LinkedInProfile.");
            Assert.AreEqual(expected.SaleTradingTermsPaymentDueInInterval, actual.SaleTradingTermsPaymentDueInInterval, "Different SaleTradingTermsPaymentDueInInterval.");
            Assert.AreEqual(expected.SaleTradingTermsPaymentDueInIntervalType, actual.SaleTradingTermsPaymentDueInIntervalType, "Different SaleTradingTermsPaymentDueInIntervalType.");

            Assert.AreEqual(expected.PurchaseTradingTermsPaymentDueInInterval, actual.PurchaseTradingTermsPaymentDueInInterval, "Different PurchaseTradingTermsPaymentDueInInterval.");
            Assert.AreEqual(expected.PurchaseTradingTermsPaymentDueInIntervalType, actual.PurchaseTradingTermsPaymentDueInIntervalType, "Different PurchaseTradingTermsPaymentDueInIntervalType.");
            Assert.AreEqual(expected.DefaultSaleDiscount, actual.DefaultSaleDiscount, "Different Sale Discount");

            Assert.AreEqual(expected.ContactType, actual.ContactType, "Diff. contact type.");
        }


        private static void AssertEqual(ContactCategoryDto expected, ContactCategoryDto actual)
        {
            Assert.AreEqual(expected.Uid, actual.Uid, "Different uid.");
            Assert.AreEqual(expected.Type, actual.Type, "Different type.");
            Assert.AreEqual(expected.Name, actual.Name, "Different name.");
        }


        public static void AssertEqual(AddressDto expected, AddressDto actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual, "Address is not expected to be null.");
            }

            Assert.AreEqual(expected.Street, !string.IsNullOrEmpty(actual.Street) ? actual.Street.Replace("|", "\r\n") : actual.Street, "Different Street.");
            Assert.AreEqual(expected.City, actual.City, "Different City.");
            Assert.AreEqual(expected.State, actual.State, "Different State.");
            Assert.AreEqual(expected.PostCode, actual.PostCode, "Different PostCode.");
        }


        public ContactDto GetContact1()
        {
            ContactDto dto = new ContactDto();
            dto.Salutation = "Mr.";
            dto.GivenName = "Gawe";
            dto.FamilyName = "Jannie";
            dto.OrganisationName = "ACME Pty Ltd";
            dto.OrganisationPosition = "Director";
            dto.Email = "john.smith@acme.com.au";
            dto.MainPhone = "02 9999 9999";
            dto.MobilePhone = "0444 444 444";

            AddressDto address = new AddressDto();
            address.Street = @"3/33 Victory
Av";
            address.City = "North Sydney";
            address.State = "NSW";
            address.Country = "Sydney";

            AddressDto otherAddres = new AddressDto();
            otherAddres.Street = "111 Elizabeth street";
            otherAddres.City = "Sydney";
            otherAddres.State = "NSW";
            otherAddres.Country = "United States";

            dto.PostalAddress = address;
            dto.OtherAddress = otherAddres;
            dto.AutoSendStatement = true;

            dto.CompanyEmail = "info@acme.com.au";
            dto.IsCustomer = true;

            dto.SaleTradingTermsPaymentDueInInterval = 7;
            dto.SaleTradingTermsPaymentDueInIntervalType = (int)TimeIntervalType.Day;

            dto.PurchaseTradingTermsPaymentDueInInterval = 2;
            dto.PurchaseTradingTermsPaymentDueInIntervalType = (int)TimeIntervalType.Week;

            return dto;
        }


        public ContactDto GetContactInvalidAbn()
        {
            ContactDto dto = new ContactDto();
            dto.Salutation = "Mr.";
            dto.GivenName = "John";
            dto.FamilyName = "Smith";
            dto.OrganisationName = "Invalid ABN Contact Org" + DateTime.UtcNow.ToString();
            dto.OrganisationPosition = "Director";
            dto.OrganisationAbn = "ABC123";
            dto.Email = "john.smith@acme.com.au";
            dto.MainPhone = "02 9999 9999";
            dto.MobilePhone = "0444 444 444";

            AddressDto address = new AddressDto();
            address.Street = "3/33 Victory Av";
            address.City = "North Sydney";
            address.State = "NSW";
            address.Country = "Australia";

            dto.PostalAddress = address;

            return dto;
        }


        public ContactDto GetContact2()
        {
            ContactDto dto = new ContactDto();
            dto.Salutation = Salutation.Mrs;
            dto.GivenName = "Mary";
            dto.FamilyName = "Smith";
            dto.OrganisationName = "Mr. & Mrs. Smith";
            dto.OrganisationPosition = "Director";
            dto.Email = "mary.smith@mrandmrssmith.com.au";
            dto.MainPhone = "02 4444 4444";
            dto.MobilePhone = "0444 444 444";

            return dto;
        }


        public ContactDto GetContact3()
        {
            ContactDto dto = new ContactDto();
            dto.Salutation = Salutation.Mr;
            dto.GivenName = "Lex";
            dto.FamilyName = "Luthor";
            dto.OrganisationName = "Luthor Corp";
            dto.OrganisationPosition = "Director";
            dto.Email = "mary.smith@mrandmrssmith.com.au";
            dto.MainPhone = "02 4444 4444";
            dto.MobilePhone = "0444 444 444";

            dto.IsActive = true;

            dto.AcceptDirectDeposit = true;
            dto.DirectDepositAccountName = "HSBC Australia";
            dto.DirectDepositBsb = "123456";
            dto.DirectDepositAccountNumber = "12345-343";
            dto.AcceptCheque = true;
            dto.ChequePayableTo = "123 Corp";

            return dto;
        }


        public ContactDto GetContact4()
        {
            ContactDto dto = new ContactDto();
            dto.Salutation = "Mr.";
            dto.GivenName = "James";
            dto.FamilyName = "Bond";
            dto.OrganisationName = "007 Enterprise";
            dto.Tags = "Entertainment, Famous Person";
            return dto;
        }


        public ContactCategoryDto GetContactCategory(string type, string name)
        {
            ContactCategoryDto dto = new ContactCategoryDto();
            dto.Type = type;
            dto.Name = this.TestUid + name;
            return dto;
        }


        public TradingTermsDto GetSaleTradingTermsDto1()
        {
            TradingTermsDto dto = new TradingTermsDto();
            dto.Type = 1;
            dto.Interval = 2;
            dto.IntervalType = 2;

            return dto;
        }

        public TradingTermsDto GetSaleTradingTermsDto2()
        {
            TradingTermsDto dto = new TradingTermsDto();
            dto.Type = 2;
            dto.Interval = 14;
            dto.IntervalType = 0;

            return dto;
        }

        public TradingTermsDto GetSaleTradingTermsDto3()
        {
            TradingTermsDto dto = new TradingTermsDto();
            dto.Type = 3;
            dto.Interval = 14;
            dto.IntervalType = 0;

            return dto;
        }

        public TradingTermsDto GetPurchaseTradingTermsDto1()
        {
            TradingTermsDto dto = new TradingTermsDto();
            dto.Type = 2;
            dto.Interval = 30;
            dto.IntervalType = 0;

            return dto;
        }
    }


    public class contactListResponse
    {
        public List<contactListItem> contactList = new List<contactListItem>();
    }


    public class contactListItem
    {
        public int contactUid;
        public string salutation;
        public string givenName;
        public string familyName;
        public string organisationName;
        public string contactID;
        public string customField1;
        public string customField2;
    }
}
