namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "contact")]
	public class ContactDto : EntityDto
	{
		[XmlElement(ElementName = "salutation")]
		public string		Salutation;

		[XmlElement(ElementName = "givenName")]
		public string		GivenName;

		[XmlElement(ElementName = "middleInitials")]
		public string		MiddleInitials;

		[XmlElement(ElementName = "familyName")]
		public string		FamilyName;

		[XmlElement(ElementName = "organisationUid")]
		public string		OrganisationUid;

		[XmlElement(ElementName = "organisationName")]
		public string		OrganisationName;

        [XmlElement(ElementName = "organisationAbn")]
        public string       OrganisationAbn;

		[XmlElement(ElementName = "companyEmail")]
		public string CompanyEmail;

        [XmlElement(ElementName = "organisationWebsite")]
        public string       OrganisationWebsite;

		[XmlElement(ElementName = "organisationPosition")]
		public string		OrganisationPosition;

        [XmlElement(ElementName = "contactID")]
        public string       ContactID;

		[XmlElement(ElementName = "websiteUrl")]
		public string		WebsiteUrl;

		[XmlElement(ElementName = "email")]
		public string		Email;

		[XmlElement(ElementName = "mainPhone")]
		public string		MainPhone;

        [XmlElement(ElementName = "homePhone")]
        public string HomePhone;

		[XmlElement(ElementName = "fax")]
		public string		Fax;

		[XmlElement(ElementName = "mobilePhone")]
		public string		MobilePhone;

		[XmlElement(ElementName = "otherPhone")]
		public string		OtherPhone;
	
		[XmlElement(ElementName = "postalAddress")]
		public AddressDto	PostalAddress = new AddressDto();

        [XmlElement(ElementName = "otherAddress")]
        public AddressDto OtherAddress = new AddressDto();

		[XmlElement(ElementName = "isActive")]
		public bool			IsActive = true;

		[XmlElement(ElementName = "acceptDirectDeposit")]
		public bool			AcceptDirectDeposit;

		[XmlElement(ElementName = "directDepositBankName")]
		public string		DirectDepositBankName;

		[XmlElement(ElementName = "directDepositAccountName")]
		public string		DirectDepositAccountName;

		[XmlElement(ElementName = "directDepositBsb")]
		public string		DirectDepositBsb;

		[XmlElement(ElementName = "directDepositAccountNumber")]
		public string		DirectDepositAccountNumber;

		[XmlElement(ElementName = "acceptCheque")]
		public bool			AcceptCheque;

		[XmlElement(ElementName = "chequePayableTo")]
		public string		ChequePayableTo;

		[XmlElement(ElementName = "tags")]
		public string Tags;

		[XmlElement(ElementName = "customField1")]
		public string CustomField1;

		[XmlElement(ElementName = "customField2")]
		public string CustomField2;

		[XmlElement(ElementName = "twitterID")]
		public string TwitterId;

		[XmlElement(ElementName = "skypeID")]
		public string SkypeId;

		[XmlElement(ElementName = "linkedInPublicProfile")] 
		public string LinkedInPublicProfile;

		[XmlElement(ElementName = "autoSendStatement")]
		public bool AutoSendStatement;

		[XmlElement(ElementName = "isPartner")]
		public bool IsPartner;

		[XmlElement(ElementName = "isCustomer")]
		public bool IsCustomer;

		[XmlElement(ElementName = "isSupplier")]
		public bool IsSupplier;

		[XmlElement(ElementName = "contactManagerUid")]
		public int ContactManagerUid;

		[XmlElement(ElementName = "saleTradingTermsPaymentDueInInterval")]
		public int SaleTradingTermsPaymentDueInInterval;

		[XmlElement(ElementName = "saleTradingTermsPaymentDueInIntervalType")]
		public int SaleTradingTermsPaymentDueInIntervalType;

		[XmlElement(ElementName = "purchaseTradingTermsPaymentDueInInterval")]
		public int PurchaseTradingTermsPaymentDueInInterval;

		[XmlElement(ElementName = "purchaseTradingTermsPaymentDueInIntervalType")]
		public int PurchaseTradingTermsPaymentDueInIntervalType;

		[XmlElement(ElementName = "saleTradingTerms")]
		public TradingTermsDto SaleTradingTerms;

		[XmlElement(ElementName = "purchaseTradingTerms")]
		public TradingTermsDto PurchaseTradingTerms;

		[XmlElement(ElementName = "defaultSaleDiscount")]
		public decimal DefaultSaleDiscount;

		[XmlElement(ElementName = "defaultPurchaseDiscount")]
		public decimal DefaultPurchaseDiscount;

		[XmlElement(ElementName = "contactType")]
		public int ContactType;
	}
}
