using Ola.RestClient.Dto;
using Ola.RestClient.Tasks;
using Ola.RestClient.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ola.RestClient.Proxies
{
    public class InventoryAdjustmentProxy : CrudProxy
    {
        public override string ResourceName
        {
            get
            {
                return Ola.RestClient.ResourceName.InventoryAdjustment;
            }
        }


    	public override string ResourceListName
    	{
    		get { return RestClient.ResourceName.InventoryAdjustmentList; }
    	}


    	protected override Type ResponseDtoType
        {
            get
            {
                return typeof(InventoryAdjustmentResponseDto);
            }
        }


        protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
        {
            return ((InventoryAdjustmentResponseDto)responseDto).InventoryAdjustment;
        }


        protected override IInsertTask GetNewInsertTaskInstance()
        {
            return new InsertInventoryAdjustmentTask();
        }


        protected override IUpdateTask GetNewUpdateTaskInstance()
        {
            return new UpdateInventoryAdjustmentTask();
        }


        protected virtual ITask CreateInsertTask(InventoryAdjustmentDto inventoryAdjustment)
        {
            InsertInventoryAdjustmentTask task = (InsertInventoryAdjustmentTask)base.CreateInsertTask(inventoryAdjustment);
            return task;
        }


        protected virtual ITask CreateUpdateTask(InventoryAdjustmentDto inventoryAdjustment)
        {
            UpdateInventoryAdjustmentTask task = (UpdateInventoryAdjustmentTask)base.CreateUpdateTask(inventoryAdjustment);
            return task;
        }
    }
}
