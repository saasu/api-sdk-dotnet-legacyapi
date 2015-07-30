namespace Ola.RestClient.Dto
{
	using System;


	// NOTE: Tax codes must exist in your file. 
	// https://secure.saasu.com/a/net/taxcodelist.aspx
	public class TaxCode
	{
		public static readonly string ExpInclGst						= "G11";
		public static readonly string ExpGstFree						= "G11,G14";
		//public static readonly string ExpInclGstForInputTaxedSales = "G11,G13(1)";
		//public static readonly string ExpInclGstPrivateNonDeductable	= "G11,G15(1)";
		//public static readonly string ExpGstFreeForInputTaxedSales = "G11,G13(2)";
		//public static readonly string ExpGstFreePrivateNonDeductable	= "G11,G15(2)";
		public static readonly string CapExInclGst						= "G10";
		public static readonly string CapExGstFree						= "G10,G14";
		//public static readonly string CapExInclGstForInputTaxedSales	= "G10,G13(1)";
		//public static readonly string CapExInclGstPrivateNonDeductable	= "G10,G15(1)";
		//public static readonly string CapExGstFreeForInputTaxedSales	= "G10,G13(2)";
		//public static readonly string CapExGstFreePrivateNonDeductable	= "G10,G15(2)";
		public static readonly string ExpAdjustments					= "G18";
		public static readonly string SaleInclGst						= "G1";
		public static readonly string SaleGstFree						= "G1,G3";
		public static readonly string SaleInputTaxed					= "G1,G4";
		public static readonly string SaleExports						= "G1,G2";
		public static readonly string SaleAdjustments					= "G7";
		public static readonly string SalaryWageOtherPaid				= "W1";
		public static readonly string WithheldTaxOnSalaryWage			= "W1,W2";
		public static readonly string WithheldInvestDistribNoTfn		= "W3";
		public static readonly string WithheldPaymentNoAbn				= "W4";
		public static readonly string WineEqualisationTaxPayable		= "1C";
		public static readonly string WineEqualisationTaxRefundable		= "1D";
		public static readonly string LuxuryCarTaxPayable				= "1E";
		public static readonly string LuxuryCarTaxRefundable			= "1F";
		

		private TaxCode()
		{
		}
	}
}
