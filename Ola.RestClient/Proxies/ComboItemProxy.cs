using System;
using System.Collections.Generic;
using System.Text;
using Ola.RestClient.Dto;
using Ola.RestClient.Dto.Inventory;
using Ola.RestClient.Tasks;
using Ola.RestClient.Tasks.Inventory;

namespace Ola.RestClient.Proxies
{
    public class ComboItemProxy : CrudProxy
	{
		public const string ResponseXPath = "/comboItemListResponse/comboItemList/comboItem";


		public override string ResourceName
		{
			get
			{
                return Ola.RestClient.ResourceName.ComboItem;
			}
		}


    	public override string ResourceListName
    	{
			get { return RestClient.ResourceName.FullComboItemList; }
    	}


    	protected override Type ResponseDtoType
		{
			get
			{
				return typeof(ComboItemResponseDto);
			}
		}


		protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
		{
			return ((ComboItemResponseDto) responseDto).ComboItem;
		}


		protected override IInsertTask GetNewInsertTaskInstance()
		{
			return new InsertComboItemTask();
		}


		protected override IUpdateTask GetNewUpdateTaskInstance()
		{
			return new UpdateComboItemTask();
		}


        public BuildComboItemResult Build(int comboItemUid, decimal quantity)
        {
            TasksRunner tasksRunner = new TasksRunner(this.WSAccessKey, this.FileUid);
            tasksRunner.Tasks.Add(new BuildComboItemTask(comboItemUid, quantity));
            TasksResponse response = tasksRunner.Execute();
            
            this.CheckErrors(response.Errors);

            BuildComboItemResult buildResponse = (BuildComboItemResult) response.Results[0];
            return buildResponse;
        }
    }
}
