namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;

	
	public class InsertContactCategoryTask : IInsertTask
	{
		#region IInsertTask Members
		
		private ContactCategoryDto _EntityToInsert = new ContactCategoryDto();
		[XmlElement(ElementName = "contactCategory", Type = typeof(ContactCategoryDto))]
		public EntityDto EntityToInsert
		{
			get
			{
				return this._EntityToInsert;
			}
			set
			{
				this._EntityToInsert = (ContactCategoryDto) value;
			}
		}

		#endregion
	}
}
