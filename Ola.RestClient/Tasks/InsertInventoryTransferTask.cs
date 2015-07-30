using Ola.RestClient.Dto;
using System;
using System.Xml.Serialization;

namespace Ola.RestClient.Tasks
{
    public class InsertInventoryTransferTask : IInsertTask
    {
        #region IInsertTask Members

        private InventoryTransferDto _EntityToInsert = new InventoryTransferDto();
        [XmlElement(ElementName = "inventoryTransfer", Type = typeof(InventoryTransferDto))]
        public EntityDto EntityToInsert
        {
            get
            {
                return this._EntityToInsert;
            }
            set
            {
                this._EntityToInsert = (InventoryTransferDto)value;
            }
        }

        #endregion
    }
}
