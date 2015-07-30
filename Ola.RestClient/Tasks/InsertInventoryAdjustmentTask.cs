using Ola.RestClient.Dto;
using System;
using System.Xml.Serialization;

namespace Ola.RestClient.Tasks
{
    public class InsertInventoryAdjustmentTask : IInsertTask
    {
        #region IInsertTask Members

        private InventoryAdjustmentDto _EntityToInsert = new InventoryAdjustmentDto();
        [XmlElement(ElementName = "inventoryAdjustment", Type = typeof(InventoryAdjustmentDto))]
        public EntityDto EntityToInsert
        {
            get
            {
                return this._EntityToInsert;
            }
            set
            {
                this._EntityToInsert = (InventoryAdjustmentDto)value;
            }
        }

        #endregion
    }
}
