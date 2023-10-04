using System;
using System.Collections.Generic;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class ProductionProcessGroupIntegrationEvent : IntegrationEvent
    {
        public string Operation { get; set; }
        public string ProductionProcessTypeCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OrganizationCode { get; set; }
        public int ObjectStatus { get; set; }
        public List<ProductionProcessIntegrationEvent> ProductionProcesses { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public string ItemCode { get; set; }
        public string Uom { get; set; }
    }

    public class ProductionProcessIntegrationEvent
    {
        public string ProductionProcessGroupCode { get; set; }
        public string ParentProductionProcessCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public List<ProductionProcessPersonnelQualificationIntegrationEvent> ProductionProcessPersonnelQualifications { get; set; }
        public List<ProductionProcessProcedureIntegrationEvent> ProductionProcessProcedures { get; set; }
        public List<ProductionProcessRoomIntegrationEvent> ProductionProcessRooms { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
    }

    public class ProductionProcessPersonnelQualificationIntegrationEvent
    {
        public string ProductionProcessCode { get; set; }
        public string PersonnelQualificationCode { get; set; }
        public string PersonnelQualificationName { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
    }

    public class ProductionProcessProcedureIntegrationEvent
    {
        public string Title { get; set; }
        public string Procedure { get; set; }
        public int Sequence { get; set; }
        public string ProductionProcessCode { get; set; }
        public string Code { get; set; }
        public string AttachmentFile { get; set; }
        public List<ProductionProcessProcedureParameterIntegrationEvent> ProductionProcessProcedureParameters { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
    }

    public class ProductionProcessProcedureParameterIntegrationEvent
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string InputTypeCode { get; set; }
        public Object Properties { get; set; }
        public string ProductionProcessProcedureCode { get; set; }
        public Boolean HasAttachment { get; set; }
        public Boolean IsNullable { get; set; }
        public int Sequence { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
    }

    public class ProductionProcessRoomIntegrationEvent
    {
        public string ProductionProcessCode { get; set; }
        public string RoomCode { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
    }
}