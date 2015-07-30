namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;

	using System;
	

	public class ContactCategoryProxy : CrudProxy
	{
		public override string ResourceName
		{
			get
			{
				return Ola.RestClient.ResourceName.ContactCategory;
			}
		}


		public override string ResourceListName
		{
			get { return RestClient.ResourceName.ContactCategoryList; }
		}


		protected override Type ResponseDtoType
		{
			get
			{
				return typeof(ContactCategoryResponseDto);
			}
		}


		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((ContactCategoryResponseDto) responseDto).ContactCategory;
		}


		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertContactCategoryTask();
		}


		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateContactCategoryTask();
		}
	}
}
