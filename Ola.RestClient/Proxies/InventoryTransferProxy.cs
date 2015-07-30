using Ola.RestClient.Dto;
using Ola.RestClient.Tasks;
using Ola.RestClient.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ola.RestClient.Proxies
{
    public class InventoryTransferProxy : CrudProxy
    {
        public override string ResourceName
        {
            get
            {
                return Ola.RestClient.ResourceName.InventoryTransfer;
            }
        }


    	public override string ResourceListName
    	{
    		get { return RestClient.ResourceName.InventoryTransferList; }
    	}


    	protected override Type ResponseDtoType
        {
            get
            {
                return typeof(InventoryTransferResponseDto);
            }
        }


        protected override EntityDto ExtractContentFromGetByUidResponse(ResponseDto responseDto)
        {
            return ((InventoryTransferResponseDto)responseDto).InventoryTransfer;
        }


        protected override IInsertTask GetNewInsertTaskInstance()
        {
            return new InsertInventoryTransferTask();
        }


        protected override IUpdateTask GetNewUpdateTaskInstance()
        {
            return new UpdateInventoryTransferTask();
        }


        protected virtual ITask CreateInsertTask(InventoryTransferDto inventoryTransfer)
        {
            InsertInventoryTransferTask task = (InsertInventoryTransferTask)base.CreateInsertTask(inventoryTransfer);
            return task;
        }


        protected virtual ITask CreateUpdateTask(InventoryTransferDto inventoryTransfer)
        {
            UpdateInventoryTransferTask task = (UpdateInventoryTransferTask)base.CreateUpdateTask(inventoryTransfer);
            return task;
        }

    }
}
