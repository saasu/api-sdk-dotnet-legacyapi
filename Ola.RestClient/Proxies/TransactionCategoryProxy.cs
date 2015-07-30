namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;

	using System;
	using System.Collections;
	

	public class TransactionCategoryProxy : CrudProxy
	{
		public override string ResourceName
		{
			get
			{
				return "TransactionCategory";
			}
		}


		public override string ResourceListName
		{
			get { return RestClient.ResourceName.TransactionCategoryList; }
		}


		protected override Type ResponseDtoType
		{
			get
			{
				return typeof(TransactionCategoryResponseDto);
			}
		}
		

		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((TransactionCategoryResponseDto) responseDto).TransactionCategory;
		}


		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertTransactionCategoryTask();
		}


		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateTransactionCategoryTask();
		}
	}
}
