namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class UpdateBankAccountTask : IUpdateTask
	{
		#region IUpdateTask Members

		private BankAccountDto _EntityToUpdate = new BankAccountDto();
		[XmlElement(ElementName = "bankAccount", Type = typeof(BankAccountDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._EntityToUpdate;
			}
			set
			{
				this._EntityToUpdate = (BankAccountDto) value;
			}
		}

		#endregion
	}
}
