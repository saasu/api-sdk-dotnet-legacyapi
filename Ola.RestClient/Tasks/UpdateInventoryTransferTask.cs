using Ola.RestClient.Dto;
using System;
using System.Xml.Serialization;

namespace Ola.RestClient.Tasks
{
    public class UpdateInventoryTransferTask : IUpdateTask
    {
        #region IUpdateTask Members

        private InventoryTransferDto _EntityToUpdate = new InventoryTransferDto();
        [XmlElement(ElementName = "inventoryTransfer", Type = typeof(InventoryTransferDto))]
        public EntityDto EntityToUpdate
        {
            get
            {
                return this._EntityToUpdate;
            }
            set
            {
                this._EntityToUpdate = (InventoryTransferDto)value;
            }
        }

        #endregion
    }
}
