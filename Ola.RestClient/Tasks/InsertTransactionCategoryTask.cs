namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class InsertTransactionCategoryTask : IInsertTask
	{
		#region IInsertTask Members

		private TransactionCategoryDto _EntityToInsert = new TransactionCategoryDto();
		[XmlElement(ElementName = "transactionCategory", Type = typeof(TransactionCategoryDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._EntityToInsert;
			}
			set
			{
				this._EntityToInsert = (TransactionCategoryDto) value;
			}
		}

		#endregion
	}
}
