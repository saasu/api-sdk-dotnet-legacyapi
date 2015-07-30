using Ola.RestClient.Dto;
using System;
using System.Xml.Serialization;

namespace Ola.RestClient.Tasks
{
    public class UpdateInventoryAdjustmentTask : IUpdateTask
    {
        #region IUpdateTask Members

        private  InventoryAdjustmentDto _EntityToUpdate = new InventoryAdjustmentDto();
        [XmlElement(ElementName = "inventoryAdjustment", Type = typeof(InventoryAdjustmentDto))]
        public EntityDto EntityToUpdate
        {
            get
            {
                return this._EntityToUpdate;
            }
            set
            {
                this._EntityToUpdate = (InventoryAdjustmentDto)value;
            }
        }

        #endregion
    }
}
