namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class UpdateTransactionCategoryTask : IUpdateTask
	{
		#region IUpdateTask Members
		private TransactionCategoryDto _EntityToUpdate = new TransactionCategoryDto();
		[XmlElement(ElementName = "transactionCategory", Type = typeof(TransactionCategoryDto))]
		public EntityDto EntityToUpdate
		{
			get
			{
				return this._EntityToUpdate;
			}
			set
			{
				this._EntityToUpdate = (TransactionCategoryDto) value;
			}
		}

		#endregion
	}
}
