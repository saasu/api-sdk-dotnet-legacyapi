namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class InsertBankAccountTask : IInsertTask
	{
		#region IInsertTask Members

		private BankAccountDto _EntityToInsert = new BankAccountDto();
		[XmlElement(ElementName = "bankAccount", Type = typeof(BankAccountDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._EntityToInsert;
			}
			set
			{
				this._EntityToInsert = (BankAccountDto) value;
			}
		}

		#endregion
	}
}
