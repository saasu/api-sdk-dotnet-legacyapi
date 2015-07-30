namespace Ola.RestClient.Proxies
{
	using Ola.RestClient.Dto;
	using Ola.RestClient.Tasks;
	using Ola.RestClient.Utils;

	using System;
	

	public class InventoryItemProxy : CrudProxy
	{
		public const string ResponseXPath = "/inventoryItemListResponse/inventoryItemList/inventoryItem";

		public override string ResourceName
		{
			get
			{
				return Ola.RestClient.ResourceName.InventoryItem;
			}
		}


		public override string ResourceListName
		{
			get { return Ola.RestClient.ResourceName.FullInventoryItemList; }
		}


		protected override Type ResponseDtoType
		{
			get
			{
				return typeof(InventoryItemResponseDto);
			}
		}

		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((InventoryItemResponseDto) responseDto).InventoryItem;
		}

		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertInventoryItemTask();
		}

		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateInventoryItemTask();
		}
	}
}
