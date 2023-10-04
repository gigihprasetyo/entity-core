using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.Models;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using qcs_product.API.WorkflowModels.cs;
using static Grpc.Core.Metadata;

namespace qcs_product.API.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public partial class QcsProductContext : DbContext
    {
        public QcsProductContext(DbContextOptions<QcsProductContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public virtual DbSet<NowTimestamp> NowTimestamp { get; set; }
        public virtual DbSet<UserTesting> UserTestings { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemGroups> ItemGroups { get; set; }
        public virtual DbSet<ProductForm> ProductForms { get; set; }
        public virtual DbSet<ProductTestType> ProductTestTypes { get; set; }
        public virtual DbSet<TestType> TestTypes { get; set; }
        public virtual DbSet<TypeForm> TypeForms { get; set; }
        public virtual DbSet<ProductProductionPhase> ProductProductionPhases { get; set; }
        public virtual DbSet<ProductionPhase> ProductionPhases { get; set; }
        public virtual DbSet<RequestQcs> RequestQcs { get; set; }
        public virtual DbSet<TestTypeQcs> ProductTestTypeQcs { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<WorkflowHistory> WorkflowHistories { get; set; }
        public virtual DbSet<EnumConstant> EnumConstant { get; set; }
        public virtual DbSet<FormMaterial> FormMaterial { get; set; }
        public virtual DbSet<FormParameter> FormParameters { get; set; }
        public virtual DbSet<FormProcedure> FormProcedure { get; set; }
        public virtual DbSet<FormTool> FormTool { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<ItemBatchNumber> ItemBatchNumbers { get; set; }
        public virtual DbSet<StorageTemperature> StorageTemperatures { get; set; }
        public virtual DbSet<GradeRoom> GradeRooms { get; set; }
        public virtual DbSet<TestScenario> TestScenarios { get; set; }
        public virtual DbSet<TestParameter> TestParameters { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<SamplingPoint> SamplingPoints { get; set; }
        public virtual DbSet<TestVariable> TestVariables { get; set; }
        public virtual DbSet<EmProductionPhase> EmProductionPhases { get; set; }
        public virtual DbSet<SamplingTestParam> SamplingTestParameters { get; set; }
        public virtual DbSet<TestParamRoom> TestParamRoomModels { get; set; }
        public virtual DbSet<RelGradeRoomScenario> RelGradeRoomScenarios { get; set; }
        public virtual DbSet<RelTestScenarioParam> RelTestScenarioParams { get; set; }
        public virtual DbSet<RelSamplingTestParam> RelSamplingTestParams { get; set; }
        public virtual DbSet<RelItemTestScenario> RelItemTestScenarios { get; set; }
        public virtual DbSet<RequestSampling> RequestSamplings { get; set; }
        public virtual DbSet<QcProcess> QcProcess { get; set; }
        public virtual DbSet<QcSampling> QcSamplings { get; set; }
        public virtual DbSet<WorkflowQcSampling> WorkflowQcSampling { get; set; }
        public virtual DbSet<WorkflowQcTransactionGroup> WorkflowQcTransactionGroup { get; set; }
        public virtual DbSet<QcSamplingTools> QcSamplingTools { get; set; }
        public virtual DbSet<QcSamplingMaterial> QcSamplingMaterials { get; set; }
        public virtual DbSet<QcSample> QcSamples { get; set; }
        public virtual DbSet<Tool> Tools { get; set; }

        public virtual DbSet<ToolActivity> ToolActivities { get; set; }
        public virtual DbSet<ToolPurpose> ToolPurposes { get; set; }
        public virtual DbSet<ToolPurposeToMasterPurpose> ToolPurposeToMasterPurposes { get; set; }
        public virtual DbSet<ToolSamplingPointLayout> ToolSamplingPointLayouts { get; set; }


        public virtual DbSet<Activity> Activities { get; set; }

        public virtual DbSet<ToolGroup> ToolGroups { get; set; }
        public virtual DbSet<RelRoomSampling> RelRoomSamplings { get; set; }
        public virtual DbSet<RelSamplingTool> RelSamplingTools { get; set; }
        public virtual DbSet<VSamplePointTestParam> VSamplePointTestParams { get; set; }
        public virtual DbSet<VToolActivity> VToolActivities { get; set; }
        public virtual DbSet<VRoomSamplePoint> VRoomSamplePoints { get; set; }
        public virtual DbSet<QcPersonel> QcPersonels { get; set; }
        public virtual DbSet<QcRequestType> QcRequestTypes { get; set; }
        public virtual DbSet<QcSamplingType> QcSamplingTypes { get; set; }
        public virtual DbSet<QcSamplingShipment> QcSamplingShipments { get; set; }
        public virtual DbSet<QcSamplingShipmentTracker> QcSamplingShipmentTrackers { get; set; }
        public virtual DbSet<AuthenticatedUserBiohr> AuthenticatedUserBiohrs { get; set; }
        public virtual DbSet<QcTransactionGroup> QcTransactionGroups { get; set; }
        public virtual DbSet<DigitalSignature> digitalSigantures { get; set; }
        public virtual DbSet<QcTransactionGroupSample> QcTransactionSamples { get; set; }
        public virtual DbSet<QcTransactionGroupSampling> QcTransactionSamplings { get; set; }
        public virtual DbSet<QcTransactionGroupSampleValue> QcTransactionSampleValues { get; set; }
        public virtual DbSet<QcTransactionGroupProcess> QcTransactionGroupProcesses { get; set; }
        public virtual DbSet<QcTransactionGroupFormProcedure> QcTransactionGroupFormProcedures { get; set; }
        public virtual DbSet<QcTransactionGroupFormParameter> QcTransactionGroupFormParameters { get; set; }
        public virtual DbSet<QcTransactionGroupFormMaterial> QcTransactionGroupFormMaterials { get; set; }
        public virtual DbSet<QcTransactionGroupFormTool> QcTransactionGroupFormTools { get; set; }
        public virtual DbSet<QcTransactionGroupValue> QcTransactionGroupValues { get; set; }
        public virtual DbSet<Microflora> Microfloras { get; set; }
        public virtual DbSet<InputType> InputTypes { get; set; }
        public virtual DbSet<Uom> Uoms { get; set; }
        public virtual DbSet<QcResult> QcResults { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<Purpose> Purposes { get; set; }
        public virtual DbSet<PurposesPersonel> PurposesPersonel { get; set; }
        public virtual DbSet<RequestPurpose> RequestPurposes { get; set; }
        public virtual DbSet<FormSection> FormSections { get; set; }
        public virtual DbSet<QcTransactionGroupFormSection> QcTransactionGroupFormSections { get; set; }
        public virtual DbSet<RequestRoom> RequestRooms { get; set; }
        public virtual DbSet<RelEmProdPhaseToRoom> RelEmProdPhaseToRooms { get; set; }
        public virtual DbSet<RoomFacility> RoomFacilities { get; set; }
        public virtual DbSet<RequestAhu> RequestAhus { get; set; }
        public virtual DbSet<QcSamplingAttachment> QcSamplingAttachment { get; set; }
        public virtual DbSet<RoomSamplingPointLayout> RoomSamplingPointLayout { get; set; }
        public virtual DbSet<RelSamplingPurposeToolGroup> RelSamplingPurposeToolGroups { get; set; }
        public virtual DbSet<ItemDosageForm> ItemDosageForms { get; set; }
        public virtual DbSet<ItemProductGroup> ItemProductGroups { get; set; }
        public virtual DbSet<RelProductProdPhaseToRoom> RelProductProdPhaseToRooms { get; set; }
        public virtual DbSet<ItemPresentation> ItemPresentations { get; set; }
        public virtual DbSet<RelItemsItemPresentation> RelItemsItemPresentations { get; set; }
        public virtual DbSet<ProductProductionPhasesPersonel> ProductProductionPhasesPersonels { get; set; }
        public virtual DbSet<QcSamplingPersonel> QcSamplingPersonels { get; set; }
        public virtual DbSet<RoomPurpose> RoomPurpose { get; set; }
        public virtual DbSet<RoomPurposeToMasterPurpose> RoomPurposeToMasterPurposes { get; set; }
        public virtual DbSet<RequestGroupRoomPurpose> RequestGroupRoomPurpose { get; set; }
        public virtual DbSet<RequestGroupToolPurpose> RequestGroupToolPurpose { get; set; }
        public virtual DbSet<ToolPurposeToMasterPurpose> ToolPurposeToMasterPurpose { get; set; }
        public virtual DbSet<ToolSamplingPointLayout> ToolSamplingPointLayout { get; set; }
        public virtual DbSet<TransactionActivity> TransactionActivity { get; set; }
        public virtual DbSet<TransactionBatch> TransactionBatches { get; set; }
        public virtual DbSet<TransactionBatchAttachment> TransactionBatchAttachments { get; set; }
        public virtual DbSet<TransactionBatchLine> TransactionBatchLines { get; set; }
        public virtual DbSet<TransactionFacility> TransactionFacility { get; set; }
        public virtual DbSet<TransactionFacilityRoom> TransactionFacilityRoom { get; set; }
        public virtual DbSet<TransactionGradeRoom> TransactionGradeRoom { get; set; }
        public virtual DbSet<TransactionOrganization> TransactionOrganization { get; set; }
        public virtual DbSet<TransactionPurposes> TransactionPurposes { get; set; }
        public virtual DbSet<TransactionRelGradeRoomScenario> TransactionRelGradeRoomScenario { get; set; }
        public virtual DbSet<TransactionRelRoomSamplingPoint> TransactionRelRoomSamplingPoint { get; set; }
        public virtual DbSet<TransactionRelSamplingPurposeToolGroup> TransactionRelSamplingPurposeToolGroup { get; set; }
        public virtual DbSet<TransactionRelSamplingTestParam> TransactionRelSamplingTestParam { get; set; }
        public virtual DbSet<TransactionRelSamplingTool> TransactionRelSamplingTool { get; set; }
        public virtual DbSet<TransactionRelTestScenarioParam> TransactionRelTestScenarioParam { get; set; }
        public virtual DbSet<TransactionRoom> TransactionRoom { get; set; }
        public virtual DbSet<TransactionRoomPurpose> TransactionRoomPurpose { get; set; }
        public virtual DbSet<TransactionRoomPurposeToMasterPurpose> TransactionRoomPurposeToMasterPurpose { get; set; }
        public virtual DbSet<TransactionRoomSamplingPointLayout> TransactionRoomSamplingPointLayout { get; set; }
        public virtual DbSet<TransactionSamplingPoint> TransactionSamplingPoint { get; set; }
        public virtual DbSet<TransactionTestParameter> TransactionTestParameter { get; set; }
        public virtual DbSet<TransactionTestScenario> TransactionTestScenario { get; set; }
        public virtual DbSet<TransactionTestVariable> TransactionTestVariable { get; set; }
        public virtual DbSet<TransactionTool> TransactionTool { get; set; }
        public virtual DbSet<TransactionToolActivity> TransactionToolActivity { get; set; }
        public virtual DbSet<TransactionToolGroup> TransactionToolGroup { get; set; }
        public virtual DbSet<TransactionToolPurpose> TransactionToolPurpose { get; set; }
        public virtual DbSet<TransactionToolPurposeToMasterPurpose> TransactionToolPurposeToMasterPurpose { get; set; }
        public virtual DbSet<TransactionToolSamplingPointLayout> TransactionToolSamplingPointLayout { get; set; }
        public virtual DbSet<TransactionBuilding> TransactionBuilding { get; set; }
        public virtual DbSet<QcSamplingTemplate> QcSamplingTemplate { get; set; }
        public virtual DbSet<TemplateTestingPersonnel> TemplateTestingPersonnel { get; set; }
        public virtual DbSet<TemplateTestingNote> TemplateTestingNote { get; set; }
        public virtual DbSet<TemplateTestingAttachment> TemplateTestingAttachment { get; set; }
        public virtual DbSet<TemplateOperatorTesting> TemplateOperatorTesting { get; set; }
        public virtual DbSet<TransactionTesting> TransactionTesting { get; set; }
        public virtual DbSet<TransactionTestingProcedure> TransactionTestingProcedures { get; set; }
        public virtual DbSet<TransactionTestingProcedureParameter> TransactionTestingProcedureParameters { get; set; }
        public virtual DbSet<QcTransactionTemplateTestingType> QcTransactionTemplateTestingType { get; set; }
        public virtual DbSet<QcTransactionTemplateTestingTypeMethod> QcTransactionTemplateTestingTypeMethod { get; set; }
        public virtual DbSet<QcTransactionTemplateTestingTypeMethodResultParameter> QcTransactionTemplateTestingTypeMethodResultParameter { get; set; }
        public virtual DbSet<QcTransactionTemplateTestingTypeMethodValidationParameter> QcTransactionTemplateTestingTypeMethodValidationParameter { get; set; }
        //public virtual DbSet<QcTransactionTemplateTestingTypeProcessProcedure> QcTransactionTemplateTestingTypeProcessProcedure { get; set; }
        //public virtual DbSet<QcTransactionTemplateTestingTypeProcessProcedureParameter> QcTransactionTemplateTestingTypeProcessProcedureParameter { get; set; }
        public virtual DbSet<TransactionTemplateTestTypeMethod> TransactionTemplateTestTypeMethod { get; set; }
        public virtual DbSet<TransactionTemplateTestTypeProcess> TransactionTemplatetestTypeProcess { get; set; }
        public virtual DbSet<TransactionTemplateTestTypeProcessProcedure> TransactionTemplateTestTypeProcessProcedure { get; set; }
        public virtual DbSet<TransactionTemplateTestTypeProcessProcedureParameter> TransactionTemplateTestTypeProcessProcedureParameter { get; set; }
        public virtual DbSet<TransactionTestingPersonnel> TransactionTestingPersonnel { get; set; }
        public virtual DbSet<TransactionTestingNote> TransactionTestingNote { get; set; }
        public virtual DbSet<TransactionTestingAttachment> TransactionTestingAttachment { get; set; }
        public virtual DbSet<TransactionHtrTestingPersonnel> TransactionHtrTestingPersonnel { get; set; }
        public virtual DbSet<TransactionHtrTestingNote> TransactionHtrTestingNote { get; set; }
        public virtual DbSet<TransactionHtrTestingProcedureParameter> TransactionHtrTestingProcedureParameter { get; set; }
        public virtual DbSet<TransactionHtrTestingAttachment> TransactionHtrTestingAttachment { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<TransactionTestingProcedureParameterAttachment> TransactionTestingProcedureParameterAttachments { get; set; }
        public virtual DbSet<TransactionTestingProcedureParameterNote> TransactionTestingProcedureParameterNotes { get; set; }
        public virtual DbSet<TransactionTestingSampling> TransactionTestingSampling { get; set; }
        public virtual DbSet<TransactionHtrProcessProcedureParameterAttachment> TransactionHtrProductionProcessProcedureParameterAttachments { get; set; }
        public DbSet<TransactionTestTypeMethodValidationParameter> TransactionTestTypeMethodValidationParameters { get; set; }
        public virtual DbSet<TransactionTestingTypeMethodResultparameter> TransactionTestingTypeMethodResultparameter { get; set; }

        /* public virtual DbSet<TransactionTemplateTestTypeProcedure> TransactionTmpltTestingProcedures { get; set; }
         public virtual DbSet<TransactionTmpltTestingProcedureParameter> TransactionTmpltTestingProcedureParameters { get; set; }
 */


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<NowTimestamp>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CurrentTimestamp)
                    .HasColumnName("current_timestamp");

                entity.ToView(nameof(NowTimestamp));
            });

            builder.Entity<TransactionTestingPersonnel>(entity =>
            {
                entity.ToTable("transaction_testing_personnel", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nama)
                .HasColumnName("nama")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.NewNIK)
                .HasColumnName("newnik")
                .HasColumnType("character varying");

                entity.Property(e => e.Nik)
                .HasColumnName("nik")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.TestingId)
                .HasColumnName("testing_id")
                .IsRequired();

                entity.Property(e => e.TestingCode)
                .HasColumnName("testing_code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Posisi)
                .HasColumnName("posisi")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.PosisiCode)
                .HasColumnName("posisi_code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.PosisiId)
                .IsRequired()
                .HasColumnName("posisi_id");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.CheckIn).HasColumnName("check_in");
                entity.Property(e => e.CheckOut).HasColumnName("check_out");
            });
            
            builder.Entity<TransactionHtrTestingPersonnel>(entity =>
            {
                entity.ToTable("transaction_htr_testing_personnel", "transaction_history");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NewNik)
                .HasColumnName("newnik")
                .HasColumnType("character varying");
                
                entity.Property(e => e.Name)
                .HasColumnName("nama")
                .IsRequired()
                .HasColumnType("character varying");


                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.TestingCode)
                .HasColumnName("testing_code")
                .IsRequired()
                .HasColumnType("character varying");
                
                entity.Property(e => e.Nik)
                .HasColumnName("nik")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                
                entity.Property(e => e.TestingId)
                .HasColumnName("testing_id")
                .IsRequired();

                entity.Property(e => e.PositionCode)
                .HasColumnName("posisi_code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.PositionId)
                .IsRequired()
                .HasColumnName("posisi_id");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");
                
                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Position)
                .HasColumnName("posisi")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Action).HasColumnName("action");
                entity.Property(e => e.Note).HasColumnName("note");
            });

            builder.Entity<TransactionTestingNote>(entity =>
            {
                entity.ToTable("transaction_testing_note", "transaction");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.Note)
                .HasColumnName("note")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.TestingId)
                .HasColumnName("testing_id");

                entity.Property(e => e.TestingCode)
                .HasColumnName("testing_code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .HasColumnType("character varying");

                entity.Property(e => e.Position)
                .HasColumnName("position")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");
            });
            
            builder.Entity<TransactionHtrTestingNote>(entity =>
            {
                entity.ToTable("transaction_htr_testing_note", "transaction_history");

                entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasColumnType("character varying");
                
                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");
                
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                
                entity.Property(e => e.TestingId)
                .HasColumnName("testing_id");
                
                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");
                
                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .HasColumnType("character varying");

                entity.Property(e => e.Position)
                .HasColumnName("position")
                .IsRequired()
                .HasColumnType("character varying");
                
                entity.Property(e => e.Id)
                .HasColumnName("id");
                
                entity.Property(e => e.TestingCode)
                .HasColumnName("testing_code")
                .IsRequired()
                .HasColumnType("character varying");
                
                entity.Property(e => e.Note)
                .HasColumnName("note")
                .IsRequired()
                .HasColumnType("character varying");
            });

            builder.Entity<TransactionTestingAttachment>(entity =>
            {
                entity.ToTable("transaction_testing_attachment", "transaction");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.Filename)
                .HasColumnName("filename")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.ExecutorNik)
                .HasColumnName("executor_nik")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.ExecutorName)
                .HasColumnName("executor_name")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.ExecutorPosition)
                .HasColumnName("executor_position")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.MediaLink)
                .HasColumnName("media_link")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.TestingId)
                .HasColumnName("testing_id");

                entity.Property(e => e.TestingCode)
                .HasColumnName("testing_code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .HasColumnType("character varying");

                entity.Property(e => e.Ext)
                .HasColumnName("ext")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");
            });
            
            builder.Entity<TransactionHtrTestingAttachment>(entity =>
            {
                entity.ToTable("transaction_htr_testing_attachment", "transaction_history");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .HasColumnType("character varying");

                entity.Property(e => e.Filename)
                .HasColumnName("filename")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.ExecutorNik)
                .HasColumnName("executor_nik")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.MediaLink)
                .HasColumnName("media_link")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.ExecutorName)
                .HasColumnName("executor_name")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.TestingId)
                .HasColumnName("testing_id");

                entity.Property(e => e.TestingCode)
                .HasColumnName("testing_code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.ExecutorPosition)
                .HasColumnName("executor_position")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Ext)
                .HasColumnName("ext")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.Action).HasColumnName("action");
                entity.Property(e => e.Note).HasColumnName("note");
            });

            builder.Entity<TransactionTemplateTestTypeMethod>(entity =>
            {
                entity.ToTable("transaction_tmplt_test_type_method", "transaction");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Code)
                .HasColumnName("code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.TestTypeId)
                .HasColumnName("test_type_id");

                entity.Property(e => e.TestTypeCode)
                .HasColumnName("test_type_code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.StandardProcedureNumber)
                .HasColumnName("standard_precedure_number")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");
            });

            builder.Entity<TransactionTemplateTestTypeProcess>(entity =>
            {
                entity.ToTable("transaction_tmplt_test_type_process", "transaction");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.MethodCode)
                .HasColumnName("method_code");

                entity.Property(e => e.Methodid)
                .HasColumnName("method_id");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");
            });

            builder.Entity<TransactionTemplateTestTypeProcessProcedure>(entity =>
            {
                entity.ToTable("transaction_tmplt_test_type_process_procedure", "transaction");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.TestTypeProcessId)
                .HasColumnName("test_type_process_id");

                entity.Property(e => e.TransactionTemplateTestingId)
              .HasColumnName("transaction_template_testing_id");

                entity.Property(e => e.TestTypeProcessCode)
                .HasColumnName("test_type_process_code")
                .HasColumnType("character varying");

                entity.Property(e => e.Title)
                .HasColumnName("title")
               
                .HasColumnType("character varying");

                entity.Property(e => e.Instruction)
                .HasColumnName("instruction")
                .HasColumnType("text");
               

                entity.Property(e => e.AttachmentFile)
                .HasColumnName("attachment_file")
                .HasColumnType("character varying");

                entity.Property(e => e.AttachmentStorageName)
                .HasColumnName("attachment_storage_name")
                .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .HasColumnType("character varying");
            });

            builder.Entity<TransactionTemplateTestTypeProcessProcedureParameter>(entity =>
            {
                entity.ToTable("transaction_tmplt_test_type_process_procedure_parameter", "transaction");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasColumnType("character varying");

                entity.Property(e => e.Code)
                .HasColumnName("code")
                .HasColumnType("character varying");

                entity.Property(e => e.DeviationAttachment)
                .HasColumnName("deviation_attachment");

                entity.Property(e => e.DeviationLevel)
                .HasColumnName("deviation_level");

                entity.Property(e => e.DeviationNote)
                .HasColumnName("deviation_note")
                .HasColumnType("text");

                entity.Property(e => e.HasAttachment)
                .HasColumnName("has_attachment");

                entity.Property(e => e.InputTypeId)
                .HasColumnName("input_type_id");

                entity.Property(e => e.IsNullable)
                .HasColumnName("is_nullable");

                entity.Property(e => e.TestTypeProcessPrecedureId)
                .HasColumnName("test_type_process_procedure_id");

                entity.Property(e => e.TestTypeProcessPrecedureCode)
                .HasColumnName("test_type_process_procedure_code")
                .HasColumnType("character varying");

                entity.Property(e => e.Properties)
                .HasColumnName("properties")
                .HasColumnType("jsonb");


                entity.Property(e => e.PropertiesValue)
                .HasColumnName("properties_value")
                .HasColumnType("jsonb");

                entity.Property(e => e.Sequence)
                .HasColumnName("sequence");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.ComponentName).HasColumnName("component_name");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .HasColumnType("character varying");
            });

            builder.Entity<Status>(entity =>
            {
                entity.ToTable("status", "workflow_service");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasColumnType("character varying")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(128);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(128);

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasColumnType("timestamp with time zone");
            });

            builder.Entity<TestParamRoom>(e =>
            {
                e.HasNoKey();
                //e.ToView("test_parameter_by_room_id_view");
                e.ToView("transaction_test_parameter_by_room_id_view_v2", "transaction");
                e.Property(e => e.RoomId).HasColumnName("room_id");
                e.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");
                e.Property(e => e.TestScenarioName).HasColumnName("test_scenario_name");
                e.Property(e => e.TestScenarioLabel).HasColumnName("test_scenario_label");
                e.Property(e => e.SampleCode).HasColumnName("sample_code");
                e.Property(e => e.TestParameterId).HasColumnName("test_parameter_id");
                e.Property(e => e.TestParameterName).HasColumnName("test_parameter_name");
                e.Property(e => e.TestParameterSquence).HasColumnName("test_parameter_sequence");
                e.Property(e => e.TotalTestParameter).HasColumnName("total_test_parameter");

            });

            builder.Entity<VSamplePointTestParam>(e =>
            {
                e.HasNoKey();
                //e.ToView("sample_point_test_parameter_view");
                e.ToView("transaction_sample_point_test_parameter_view_v2", "transaction");
                e.Property(e => e.RoomId).HasColumnName("room_id");
                e.Property(e => e.ToolId).HasColumnName("tool_id");
                e.Property(e => e.ToolName).HasColumnName("tool_name");
                e.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");
                e.Property(e => e.GradeRoomName).HasColumnName("grade_room_name");
                e.Property(e => e.SamplePointId).HasColumnName("sample_point_id");
                e.Property(e => e.SamplePointName).HasColumnName("sample_point_name");
                e.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");
                e.Property(e => e.TestScenarioName).HasColumnName("test_scenario_name");
                e.Property(e => e.TestScenarioLabel).HasColumnName("test_scenario_label");
                e.Property(e => e.TestParameterId).HasColumnName("test_parameter_id");
                e.Property(e => e.TestParameterName).HasColumnName("test_parameter_name");
                e.Property(e => e.TestGroupId).HasColumnName("test_group_id");
                e.Property(e => e.Seq).HasColumnName("seq");
            });

            builder.Entity<VRoomSamplePoint>(e =>
            {
                e.HasNoKey();
                //e.ToView("room_sample_point_view");
                e.ToView("transaction_room_sample_point_view_v2", "transaction");
                e.Property(e => e.RoomId).HasColumnName("room_id");
                e.Property(e => e.ToolId).HasColumnName("tool_id");
                e.Property(e => e.ToolName).HasColumnName("tool_name");
                e.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");
                e.Property(e => e.GradeRoomName).HasColumnName("grade_room_name");
                e.Property(e => e.SamplePointId).HasColumnName("sample_point_id");
                e.Property(e => e.SamplePointName).HasColumnName("sample_point_name");
            });

            builder.Entity<VToolActivity>(e =>
            {
                e.HasNoKey();
                //e.ToView("tool_activity_view");
                e.ToView("transaction_tool_activity_view", "transaction");
                e.Property(e => e.ToolId).HasColumnName("tool_id");
                e.Property(e => e.ToolCode).HasColumnName("tool_code");
                e.Property(e => e.ToolName).HasColumnName("tool_name");
                e.Property(e => e.ToolGroupId).HasColumnName("tool_group_id");
                e.Property(e => e.ToolGroupName).HasColumnName("tool_group_name");
                e.Property(e => e.ToolGroupLabel).HasColumnName("tool_group_label");
                e.Property(e => e.RoomId).HasColumnName("room_id");
                e.Property(e => e.RoomName).HasColumnName("room_name");
                e.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");
                e.Property(e => e.GradeRoomName).HasColumnName("grade_room_name");
                e.Property(e => e.ActivityDateValidation).HasColumnName("activity_date_validation");
                e.Property(e => e.ExpireDateValidation).HasColumnName("expired_date_validation");
                e.Property(e => e.ActivityDateCalibration).HasColumnName("activity_date_calibration");
                e.Property(e => e.ExpireDateCalibration).HasColumnName("expired_date_calibration");
            });

            builder.Entity<UserTesting>(e =>
            {
                e.ToTable("UserTestings");
                e.Property(e => e.Id).HasColumnName("id2");
                e.Property(e => e.Name).HasColumnName("name");
                e.Property(e => e.JenisKelamin).HasColumnName("jenis_kelamin");

            });



            builder.Entity<Item>(e =>
            {
                e.ToTable("items");
                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemCode).HasColumnName("item_code")
                .HasMaxLength(20);

                //e.Property(e => e.Name).HasColumnName("name")
                //.HasMaxLength(200);

                //e.Property(e => e.ProductFormId).HasColumnName("prod_form_id");
                e.Property(e => e.ObjectStatus).HasColumnName("object_status");

                //e.Property(e => e.Temperature).HasColumnName("temperature")
                //.HasMaxLength(10);

                //e.Property(e => e.ItemGroupId).HasColumnName("item_group_id");

                //e.Property(e => e.ItemGroupName).HasColumnName("item_group_name");

                //e.Property(e => e.LabelId).HasColumnName("label_id");

                //e.Property(e => e.OrgId).HasColumnName("org_id");

                //e.Property(e => e.OrgName).HasColumnName("org_name");

                //e.Property(e => e.RowStatus).HasColumnName("row_status")
                //.HasMaxLength(10);


                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.ProductGroupId).HasColumnName("product_group_id");
            });

            builder.Entity<ProductForm>(e =>
            {
                e.ToTable("product_forms");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.FormCode).HasColumnName("form_code")
                .HasMaxLength(20);

                e.Property(e => e.Name).HasColumnName("name")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);


                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ProductTestType>(e =>
            {
                e.ToTable("product_test_types");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemId).HasColumnName("item_id");

                e.Property(e => e.TestTypeId).HasColumnName("test_type_id");

                e.Property(e => e.SampleAmountCount).HasColumnName("sample_amount_count");

                e.Property(e => e.SampleAmountVolume).HasColumnName("sample_amount_volume");

                e.Property(e => e.SampleAmountUnit).HasColumnName("sample_amount_unit")
                .HasMaxLength(10);

                e.Property(e => e.SampleAmountPresentation).HasColumnName("sample_amount_presentation")
                .HasMaxLength(10);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);


                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");


            });

            builder.Entity<TestType>(e =>
            {
                e.ToTable("test_types");

                e.Property(e => e.Id).HasColumnName("id");

                //e.Property(e => e.OrgId).HasColumnName("org_id");

                //e.Property(e => e.OrgName).HasColumnName("org_name");

                //e.Property(e => e.TestTypeCode).HasColumnName("test_type_code")
                //.HasMaxLength(20);

                //e.Property(e => e.Name).HasColumnName("name")
                //.HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<TypeForm>(e =>
            {
                e.ToTable("type_forms");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.TypeFormCode).HasColumnName("type_form_code")
                .HasMaxLength(20);

                e.Property(e => e.Name).HasColumnName("name")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ProductProductionPhase>(e =>
            {
                e.ToTable("product_production_phases");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ProductProdPhaseCode).HasColumnName("product_prod_phase_code")
                .HasMaxLength(20);

                e.Property(e => e.ItemId).HasColumnName("item_id");

                e.Property(e => e.Name).HasColumnName("name")
                .HasMaxLength(200);

                e.Property(e => e.RoomId).HasColumnName("room_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                 .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ProductionPhase>(e =>
            {
                e.ToTable("production_phases");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ProdPhaseCode).HasColumnName("prod_phase_code")
                .HasMaxLength(20);

                e.Property(e => e.Name).HasColumnName("name")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
               .HasMaxLength(10);


                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RequestQcs>(e =>
            {
                e.ToTable("request_qcs");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Date).HasColumnName("date");

                e.Property(e => e.ReceiptDate).HasColumnName("receipt_date");

                e.Property(e => e.ReceiptDateQA).HasColumnName("receipt_date_qa");

                e.Property(e => e.ReceiptDateKabag).HasColumnName("receipt_date_kabag");

                e.Property(e => e.NoRequest).HasColumnName("no_request");

                e.Property(e => e.NoBatch).HasColumnName("no_batch");

                e.Property(e => e.TypeRequestId).HasColumnName("type_request_id");

                e.Property(e => e.TypeRequest).HasColumnName("type_request");

                e.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");

                e.Property(e => e.TestScenarioName).HasColumnName("test_scenario_name");

                e.Property(e => e.TestScenarioLabel).HasColumnName("test_scenario_label");

                e.Property(e => e.TypeFormId).HasColumnName("type_form_id");

                e.Property(e => e.TypeFormName).HasColumnName("type_form_name");

                e.Property(e => e.ItemId).HasColumnName("item_id");

                e.Property(e => e.ItemName).HasColumnName("item_name");

                //e.Property(e => e.RequestQcsId).HasColumnName("request_qcs_id");

                //e.Property(e => e.RequestQcsNo).HasColumnName("request_qcs_no");

                e.Property(e => e.StorageTemperatureId).HasColumnName("storage_temperature_id");

                e.Property(e => e.StorageTemperatureName).HasColumnName("storage_temperature_name");

                e.Property(e => e.PurposeId).HasColumnName("purpose_id");

                e.Property(e => e.PurposeName).HasColumnName("purpose_name");

                e.Property(e => e.EmRoomId).HasColumnName("em_room_id");

                e.Property(e => e.EmRoomName).HasColumnName("em_room_name");

                e.Property(e => e.EmRoomGradeId).HasColumnName("em_room_grade_id");

                e.Property(e => e.EmRoomGradeName).HasColumnName("em_room_grade_name");

                e.Property(e => e.EmPhaseId).HasColumnName("em_phase_id");

                e.Property(e => e.EmPhaseName).HasColumnName("em_phase_name");

                e.Property(e => e.ProductFormId).HasColumnName("product_form_id");

                e.Property(e => e.ProductFormName).HasColumnName("product_form_name");

                e.Property(e => e.ProductGroupId).HasColumnName("product_group_id");

                e.Property(e => e.ProductGroupName).HasColumnName("product_group_name");

                e.Property(e => e.ProductPresentationId).HasColumnName("product_presentation_id");

                e.Property(e => e.ProductPresentationName).HasColumnName("product_presentation_name");

                e.Property(e => e.ProductPhaseId).HasColumnName("product_phase_id");

                e.Property(e => e.ProductPhaseName).HasColumnName("product_phase_name");

                e.Property(e => e.ProductTemperature).HasColumnName("product_temperature");

                e.Property(e => e.FacilityId).HasColumnName("facility_id");

                e.Property(e => e.FacilityCode).HasColumnName("facility_code");

                e.Property(e => e.FacilityName).HasColumnName("facility_name");

                e.Property(e => e.Status).HasColumnName("status");

                e.Property(e => e.OrgId).HasColumnName("org_id");

                e.Property(e => e.OrgName).HasColumnName("org_name");

                e.Property(e => e.CreatedBy).HasColumnName("created_by").HasMaxLength(100);

                e.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy).HasColumnName("updated_by").HasMaxLength(100);

                e.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.WorkflowStatus).HasColumnName("workflow_status");

                e.Property(e => e.NoDeviation).HasColumnName("dev_number");

                e.Property(e => e.Conclusion).HasColumnName("conclusion");

                e.Property(e => e.Location)
                    .HasMaxLength(50)
                    .HasColumnName("location");
                e.Property(e => e.ProcessDate).HasColumnName("process_date");
                e.Property(e => e.ProcessId).HasColumnName("process_id");
                e.Property(e => e.ProcessName).HasColumnName("process_name");
                e.Property(e => e.ItemTemperature)
                    .HasMaxLength(20)
                    .HasColumnName("item_temperature");

                e.Property(e => e.IsNoBatchEditable).HasColumnName("is_no_batch_editable");

                //e.Property(e => e.IsFromBulkRequest)
                //   .HasDefaultValue(false)
                //   .HasColumnName("is_from_bulk_request");
            });

            builder.Entity<TestTypeQcs>(e =>
            {
                e.ToTable("product_test_type_qcs");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.RequestQcsId).HasColumnName("request_qcs_id");

                //e.Property(e => e.PurposeId).HasColumnName("purpose_id");
                e.Property(e => e.TestTypeId).HasColumnName("test_type_id");

                e.Property(e => e.TestTypeName).HasColumnName("test_type_name")
                .HasMaxLength(70);

                //e.Property(e => e.TestTypeMethodId).HasColumnName("test_type_method_id");

                //e.Property(e => e.TestTypeMethodName).HasColumnName("test_type_method_name")
                //.HasMaxLength(70);

                e.Property(e => e.TestParameterId).HasColumnName("test_parameter_id");

                e.Property(e => e.TestParameterName).HasColumnName("test_parameter_name")
                .HasMaxLength(70);

                e.Property(e => e.SampleAmountCount).HasColumnName("sample_amount_count");

                e.Property(e => e.SampleAmountVolume).HasColumnName("sample_amount_volume");

                e.Property(e => e.SampleAmountUnit).HasColumnName("sample_amount_unit")
                .HasMaxLength(10);

                e.Property(e => e.SampleAmountPresentation).HasColumnName("sample_amount_presentation")
                .HasMaxLength(10);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.OrgId).HasColumnName("org_id");

                e.Property(e => e.OrgName).HasColumnName("org_name")
                .HasMaxLength(70);
            });

            builder.Entity<Organization>(e =>
            {
                e.ToTable("organization");

                e.Property(e => e.Id).HasColumnName("id");

                //e.Property(e => e.OrgCode).HasColumnName("org_code")
                //.HasMaxLength(20);

                //e.Property(e => e.Name).HasColumnName("name")
                //.HasMaxLength(200);

                //e.Property(e => e.BIOHROrganizationId).HasColumnName("biohr_organization_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<WorkflowHistory>(e =>
            {
                e.ToTable("workflow_history");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.WorkflowDocumentCode).HasColumnName("workflow_document_code");

                e.Property(e => e.Action).HasColumnName("action")
                    .HasMaxLength(50);

                e.Property(e => e.Note).HasColumnName("note")
                    .HasMaxLength(200);

                e.Property(e => e.WorkflowStatus).HasColumnName("workflow_status");

                e.Property(e => e.PicNik).HasColumnName("pic_nik");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                  .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                     .HasColumnName("created_by")
                     .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<EnumConstant>(e =>
            {
                e.ToTable("enum_constant");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.TypeId).HasColumnName("type_id");

                e.Property(e => e.KeyGroup).HasColumnName("key_group")
                .HasMaxLength(50);

                e.Property(e => e.keyValueLabel).HasColumnName("key_value_label")
                .HasMaxLength(200);

                e.Property(e => e.Name).HasColumnName("name")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<FormMaterial>(entity =>
            {
                entity.ToTable("form_material");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.BatchNumber).HasColumnName("batch_number");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.DefaultPackageQty)
                    .HasColumnName("default_package_qty")
                    .HasColumnType("numeric");

                entity.Property(e => e.DefaultQty)
                    .HasColumnName("default_qty")
                    .HasColumnType("numeric");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasColumnName("group_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.ProcessId).HasColumnName("process_id");

                entity.Property(e => e.SectionId).HasColumnName("section_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.Uom).HasColumnName("uom");

                entity.Property(e => e.UomPackage).HasColumnName("uom_package");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.FormMaterial)
                    .HasForeignKey(d => d.ProcessId)
                    .HasConstraintName("fk_qc_process");
            });

            builder.Entity<FormParameter>(e =>
            {
                e.ToTable("form_parameter");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.Label).HasColumnName("label");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.InputType).HasColumnName("input_type");

                e.Property(e => e.Uom).HasColumnName("uom");

                e.Property(e => e.ThresholdOperator).HasColumnName("threshold_operator");

                e.Property(e => e.ThresholdValue).HasColumnName("threshold_value");

                e.Property(e => e.ThresholdValueFrom).HasColumnName("threshold_value_from");

                e.Property(e => e.ThresholdValueTo).HasColumnName("threshold_value_to");

                e.Property(e => e.NeedAttachment).HasColumnName("need_attachment");

                e.Property(e => e.Note).HasColumnName("note");

                e.Property(e => e.ProcedureId).HasColumnName("procedure_id");

                e.Property(e => e.IsForAllSample).HasColumnName("is_for_all_sample");

                e.Property(e => e.IsResult).HasColumnName("is_result");

                e.Property(e => e.DefaultValue).HasColumnName("default_value");

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                /*entity.HasOne(d => d.Procedure)
                    .WithMany(p => p.FormParameter)
                    .HasForeignKey(d => d.ProcedureId)
                    .HasConstraintName("fk_form_procedure");*/
            });

            builder.Entity<FormProcedure>(entity =>
            {
                entity.ToTable("form_procedure");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.ProcessId).HasColumnName("process_id");

                entity.Property(e => e.SectionId).HasColumnName("section_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.FormProcedure)
                    .HasForeignKey(d => d.ProcessId)
                    .HasConstraintName("fk_qc_process");
            });

            builder.Entity<FormTool>(entity =>
            {
                entity.ToTable("form_tool");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.ProcessId).HasColumnName("process_id");

                entity.Property(e => e.SectionId).HasColumnName("section_id");

                entity.Property(e => e.Qty)
                    .HasColumnName("qty")
                    .HasColumnType("numeric");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.ToolId).HasColumnName("tool_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.FormTool)
                    .HasForeignKey(d => d.ProcessId)
                    .HasConstraintName("fk_qc_process");
            });

            builder.Entity<ItemBatchNumber>(e =>
            {
                e.ToTable("item_batch_number");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemId).HasColumnName("item_id");

                e.Property(e => e.ObjectStatus).HasColumnName("object_status");

                e.Property(e => e.BatchNumber).HasColumnName("batch_number");

                e.Property(e => e.ExpireDate).HasColumnName("expire_date");

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.Quantity)
                   .HasColumnName("quantity")
                   .HasMaxLength(20);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ItemGroups>(e =>
            {
                e.ToTable("item_group");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemGroupCode).HasColumnName("item_group_code");

                e.Property(e => e.ItemGroupName).HasColumnName("item_group_name");

                e.Property(e => e.ObjectStatus).HasColumnName("object_status");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<SamplingPoint>(e =>
            {
                e.ToTable("sampling_point");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.RoomId).HasColumnName("room_id");

                e.Property(e => e.ToolId).HasColumnName("tool_id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<Room>(e =>
            {
                e.ToTable("room");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");

                e.Property(e => e.Code).HasColumnName("code");
                e.Property(e => e.PosId).HasColumnName("pos_id");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.RowStatus).HasColumnName("row_status");
                e.Property(e => e.ObjectStatus).HasColumnName("object_status");

                e.Property(e => e.OrganizationId).HasColumnName("organization_id");

                e.Property(e => e.BuildingId).HasColumnName("building_id");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(e => e.Floor)
                   .HasColumnName("floor")
                   .HasColumnType("character varying");

                e.Property(e => e.Area).HasColumnName("area");
                e.Property(e => e.Ahu).HasColumnName("ahu");
                e.Property(e => e.TemperatureOperator).HasColumnName("temperature_operator");
                e.Property(e => e.TemperatureValue).HasColumnName("temperature_value");
                e.Property(e => e.TemperatureValueTo).HasColumnName("temperature_value_to");
                e.Property(e => e.TemperatureValueFrom).HasColumnName("temperature_value_from");
                e.Property(e => e.HumidityOperator).HasColumnName("humidity_operator");
                e.Property(e => e.HumidityValue).HasColumnName("humidity_value");
                e.Property(e => e.HumidityValueTo).HasColumnName("humidity_value_to");
                e.Property(e => e.HumidityValueFrom).HasColumnName("humidity_value_from");
                e.Property(e => e.PressureOperator).HasColumnName("pressure_operator");
                e.Property(e => e.PressureValue).HasColumnName("pressure_value");
                e.Property(e => e.PressureValueTo).HasColumnName("pressure_value_to");
                e.Property(e => e.PressureValueFrom).HasColumnName("pressure_value_from");
                e.Property(e => e.AirChangeOperator).HasColumnName("air_change_operator");
                e.Property(e => e.AirChangeValue).HasColumnName("air_change_value");
                e.Property(e => e.AirChangeValueTo).HasColumnName("air_change_value_to");
                e.Property(e => e.AirChangeValueFrom).HasColumnName("air_change_value_from");

            });

            builder.Entity<TestParameter>(e =>
            {
                e.ToTable("test_parameter");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.TestGroupId).HasColumnName("test_group_id");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.ShortName).HasColumnName("short_name");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.OrgId).HasColumnName("organization_id");

                e.Property(e => e.QcProcessId).HasColumnName("qc_process_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<TestScenario>(e =>
            {
                e.ToTable("test_scenario");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.Label).HasColumnName("label");

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<GradeRoom>(e =>
            {
                e.ToTable("grade_room");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.TestGroupId).HasColumnName("test_group_id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.Name).HasColumnName("name");
                e.Property(e => e.GradeRoomDefault).HasColumnName("grade_room_default");

                e.Property(e => e.RowStatus).HasColumnName("row_status");
                e.Property(e => e.ObjectStatus).HasColumnName("object_status");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<StorageTemperature>(e =>
            {
                e.ToTable("storage_temperature");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemId).HasColumnName("item_id");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.TresholdOperator).HasColumnName("treshold_operator");

                e.Property(e => e.TresholdValue).HasColumnName("treshold_value");

                e.Property(e => e.TresholdMin).HasColumnName("treshold_min");

                e.Property(e => e.TresholdMax).HasColumnName("treshold_max");

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<TestVariable>(e =>
            {
                e.ToTable("test_variable");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.TestParameterId).HasColumnName("test_parameter_id");

                e.Property(e => e.VariableName).HasColumnName("variable_name");

                e.Property(e => e.TresholdOperator).HasColumnName("treshold_operator");

                e.Property(e => e.TresholdValue).HasColumnName("treshold_value");

                e.Property(e => e.TresholdMin).HasColumnName("threshold_value_to");

                e.Property(e => e.TresholdMax).HasColumnName("threshold_value_from");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<Personal>(e =>
            {
                e.ToTable("personal");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Username).HasColumnName("username")
                  .HasMaxLength(50);

                e.Property(e => e.Name).HasColumnName("name")
                  .HasMaxLength(200);

                e.Property(e => e.Nik).HasColumnName("nik")
                  .HasMaxLength(16);

                e.Property(e => e.OrgId).HasColumnName("org_id");

                e.Property(e => e.PosId).HasColumnName("pos_id");

                e.Property(e => e.Pin).HasColumnName("pin")
                  .HasMaxLength(100);

                e.Property(e => e.Email).HasColumnName("email")
                    .HasMaxLength(50);

                e.Property(e => e.NoHandphone).HasColumnName("no_handphone")
                    .HasMaxLength(50);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<Position>(e =>
            {
                e.ToTable("position");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.PosCode).HasColumnName("pos_code")
                    .HasMaxLength(20);

                e.Property(e => e.Name).HasColumnName("name")
                    .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<EmProductionPhase>(e =>
            {
                e.ToTable("em_production_phase");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.ParentId).HasColumnName("parent_id");

                e.Property(e => e.QcProduct).HasColumnName("qc_product");

                e.Property(e => e.QcEm).HasColumnName("qc_em");

                e.Property(e => e.RoomId).HasColumnName("room_id");

                e.Property(e => e.FacilityId).HasColumnName("facility_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<SamplingTestParam>(e =>
            {
                e.ToTable("sampling_test_parameter");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.SamplingPointId).HasColumnName("sampling_point_id");

                e.Property(e => e.TestParameterId).HasColumnName("test_parameter_id");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelGradeRoomScenario>(e =>
            {
                e.ToTable("rel_grade_room_scenario");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");

                e.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelTestScenarioParam>(e =>
            {
                e.ToTable("rel_test_scenario_param");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.TestParameterId).HasColumnName("test_parameter_id");

                e.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelSamplingTestParam>(e =>
            {
                e.ToTable("rel_sampling_test_param");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.SamplingPointId).HasColumnName("sampling_point_id");

                e.Property(e => e.TestScenarioParamId).HasColumnName("test_scenario_param_id");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelItemTestScenario>(e =>
            {
                e.ToTable("rel_item_test_scenario");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemId).HasColumnName("item_id");

                e.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RequestSampling>(e =>
            {
                e.ToTable("qc_request_sampling");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.TypeRequestId).HasColumnName("type_request_id");

                e.Property(e => e.TypeSamplingId).HasColumnName("type_sampling_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcSampling>(e =>
            {
                e.ToTable("qc_sampling");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.RequestQcsId).HasColumnName("request_qcs_id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.SamplingDateFrom).HasColumnName("sampling_date_from");

                e.Property(e => e.SamplingDateTo).HasColumnName("sampling_date_to");

                e.Property(e => e.SamplingTypeId).HasColumnName("sampling_type_id");

                e.Property(e => e.SamplingTypeName).HasColumnName("sampling_type_name");

                e.Property(e => e.Status).HasColumnName("status");

                e.Property(e => e.WorkflowStatus).HasColumnName("workflow_status");

                e.Property(e => e.ShipmentNote).HasColumnName("shipment_note");

                e.Property(e => e.ShipmentApprovalDate).HasColumnName("shipment_approval_date");

                e.Property(e => e.ShipmentApprovalBy).HasColumnName("shipment_approval_by");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.ReceiptDate)
                .HasColumnName("receipt_date");

                e.Property(e => e.Note)
                .HasColumnName("note");

                e.Property(e => e.AttchmentFile)
                .HasColumnName("attchment_file");

                e.Property(e => e.ProductDate)
                .HasColumnName("product_date");

                e.Property(e => e.ProductMethodId)
                .HasColumnName("product_method_id");

                e.Property(e => e.ProductShipmentTemperature)
                .HasColumnName("product_shipment_temperature");

                e.Property(e => e.ProductShipmentDate)
                .HasColumnName("product_shipment_date");

                e.Property(e => e.ProductDataLogger)
                .HasColumnName("product_data_logger");
            });

            builder.Entity<WorkflowQcSampling>(e =>
            {
                e.ToTable("workflow_qc_sampling");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcSamplingId).HasColumnName("qc_sampling_id");

                e.Property(e => e.IsInWorkflow).HasColumnName("is_in_workflow");

                e.Property(e => e.WorkflowDocumentCode).HasColumnName("workflow_document_code");

                e.Property(e => e.WorkflowCode).HasColumnName("workflow_code");

                e.Property(e => e.WorkflowStatus).HasColumnName("workflow_status");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                //  e.HasOne(d => d.MasterQcSampling)
                //   .WithMany(p => p.WorkflowQcSampling)
                //   .HasForeignKey(d => d.QcSamplingId)
                //   .HasConstraintName("fk_qc_sampling");
            });

            builder.Entity<WorkflowQcTransactionGroup>(entity =>
            {
                entity.ToTable("workflow_qc_transaction_group");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.IsInWorkflow).HasColumnName("is_in_workflow");

                entity.Property(e => e.QcTransactionGroupId).HasColumnName("qc_transaction_group_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150);

                entity.Property(e => e.WorkflowCode)
                    .HasColumnName("workflow_code")
                    .HasMaxLength(20);

                entity.Property(e => e.WorkflowDocumentCode)
                    .HasColumnName("workflow_document_code")
                    .HasMaxLength(20);

                entity.Property(e => e.WorkflowStatus)
                    .HasColumnName("workflow_status")
                    .HasMaxLength(50);

                entity.HasOne(d => d.QcTransactionGroup)
                    .WithMany(p => p.WorkflowQcTransactionGroup)
                    .HasForeignKey(d => d.QcTransactionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk__qc_transaction_group");
            });

            builder.Entity<QcProcess>(entity =>
            {
                entity.ToTable("qc_process");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.IsInputForm).HasColumnName("is_input_form");

                entity.Property(e => e.AddSampleLayoutType).HasColumnName("add_sample_layout_type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<QcSamplingTools>(e =>
            {
                e.ToTable("qc_sampling_tools");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcSamplingId).HasColumnName("qc_sampling_id");

                e.Property(e => e.ToolId).HasColumnName("tool_id");

                e.Property(e => e.ToolCode).HasColumnName("tool_code");

                e.Property(e => e.ToolName).HasColumnName("tool_name");

                e.Property(e => e.ToolGroupId).HasColumnName("tool_group_id");

                e.Property(e => e.ToolGroupName).HasColumnName("tool_group_name");

                e.Property(e => e.ToolGroupLabel).HasColumnName("tool_group_label");

                e.Property(e => e.EdValidation).HasColumnName("ed_validation");

                e.Property(e => e.EdCalibration).HasColumnName("ed_calibration");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcSamplingMaterial>(e =>
            {
                e.ToTable("qc_sampling_material");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcSamplingId).HasColumnName("qc_sampling_id");

                e.Property(e => e.ItemId).HasColumnName("item_id");

                e.Property(e => e.ItemName).HasColumnName("item_name");

                e.Property(e => e.ItemBatchId).HasColumnName("item_batch_id");

                e.Property(e => e.NoBatch).HasColumnName("no_batch");

                e.Property(e => e.ExpireDate).HasColumnName("expire_date");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcSample>(e =>
            {
                e.ToTable("qc_sample");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcSamplingId).HasColumnName("qc_sampling_id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.ParentId).HasColumnName("parent_id");

                e.Property(e => e.SampleSequence).HasColumnName("sample_sequence");

                e.Property(e => e.SamplingPointId).HasColumnName("sampling_point_id");

                e.Property(e => e.SamplingPointCode).HasColumnName("sampling_point_code");

                e.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");

                e.Property(e => e.GradeRoomName).HasColumnName("grade_room_name");

                e.Property(e => e.ToolId).HasColumnName("tool_id");

                e.Property(e => e.ToolCode).HasColumnName("tool_code");

                e.Property(e => e.ToolName).HasColumnName("tool_name");

                e.Property(e => e.ToolGroupId).HasColumnName("tool_gorup_id");

                e.Property(e => e.ToolGroupName).HasColumnName("tool_group_name");

                e.Property(e => e.ToolGroupLabel).HasColumnName("tool_group_label");

                e.Property(e => e.TestParamId).HasColumnName("test_param_id");

                e.Property(e => e.TestParamName).HasColumnName("test_param_name");

                //e.Property(e => e.TestTypeId).HasColumnName("test_type_id");

                //e.Property(e => e.TestTypeName).HasColumnName("test_type_name");

                //e.Property(e => e.TestTypeMethodId).HasColumnName("test_method_id");

                //e.Property(e => e.TestTypeMethodName).HasColumnName("test_method_name");

                e.Property(e => e.PersonalId).HasColumnName("personal_id");

                e.Property(e => e.PersonalInitial).HasColumnName("personal_initial");

                e.Property(e => e.PersonalName).HasColumnName("personal_name");

                e.Property(e => e.SamplingDateTimeFrom).HasColumnName("sampling_date_time_from");

                e.Property(e => e.SamplingDateTimeTo).HasColumnName("sampling_date_time_to");

                e.Property(e => e.ParticleVolume).HasColumnName("particle_volume");

                e.Property(e => e.AttchmentFile).HasColumnName("attchment_file");

                e.Property(e => e.Note).HasColumnName("note");
                e.Property(e => e.ReviewQaNote).HasColumnName("review_qa_note");

                e.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");
                e.Property(e => e.IsDefault).HasColumnName("is_default");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");

                e.Property(e => e.QcSamplingToolsId)
                .HasColumnName("qc_sampling_tools_id");

                e.Property(e => e.QcSamplingMaterialsId)
                .HasColumnName("qc_sampling_materials_id");
            });


            builder.Entity<ToolGroup>(e =>
            {
                e.ToTable("tool_group");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.Label).HasColumnName("label");

                e.Property(e => e.ObjectStatus).HasColumnName("object_status");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
                ;
            });

            builder.Entity<Tool>(e =>
            {
                e.ToTable("tool");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ToolCode).HasColumnName("tool_code");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.ToolGroupId).HasColumnName("tool_group_id");

                e.Property(e => e.RoomId).HasColumnName("room_id");

                e.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");

                e.Property(e => e.FacilityId).HasColumnName("facility_id");
                e.Property(e => e.ObjectStatus).HasColumnName("object_status");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.SerialNumberId).HasColumnName("serial_number_id")
                   .HasMaxLength(20);

                e.Property(e => e.MachineId).HasColumnName("machine_id");
            });

            builder.Entity<ToolGroup>(e =>
            {
                e.ToTable("tool_group");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.Label).HasColumnName("label");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ToolActivity>(e =>
            {
                e.ToTable("tool_activity");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ToolId).HasColumnName("tool_id");

                e.Property(e => e.ActivityId).HasColumnName("activity_id");

                e.Property(e => e.ActivityCode).HasColumnName("activity_code");

                e.Property(e => e.ActivityDate).HasColumnName("activity_date");

                e.Property(e => e.ExpiredDate).HasColumnName("expired_date");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<Activity>(e =>
            {
                e.ToTable("activity");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelRoomSampling>(e =>
            {
                e.ToTable("rel_room_sampling_point");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.RoomPurposeId).HasColumnName("room_purpose_id");

                e.Property(e => e.SamplingPointId).HasColumnName("sampling_poin_id");

                e.Property(e => e.ScenarioLabel).HasColumnName("scenario_label");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelSamplingTool>(e =>
            {
                e.ToTable("rel_sampling_tool");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.SamplingPointId).HasColumnName("sampling_poin_id");

                e.Property(e => e.ToolPurposeId).HasColumnName("tool_purpose_id");

                e.Property(e => e.ScenarioLabel).HasColumnName("scenario_label");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ToolPurpose>(entity =>
            {
                entity.ToTable("tool_purpose");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.ToolId).HasColumnName("tool_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<ToolPurposeToMasterPurpose>(entity =>
            {
                entity.ToTable("tool_purpose_to_master_purpose");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.ToolPurposeId).HasColumnName("tool_purpose_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<ToolSamplingPointLayout>(entity =>
            {
                entity.ToTable("tool_sampling_point_layout");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AttachmentFile)
                    .IsRequired()
                    .HasColumnName("attachment_file")
                    .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("file_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.FileType)
                    .HasColumnName("file_type")
                    .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.ToolPurposeId).HasColumnName("tool_purpose_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<QcPersonel>(e =>
            {
                e.ToTable("qc_personel");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.PersonelCode4).HasColumnName("personel_code4");

                e.Property(e => e.PersonelCode8).HasColumnName("personel_code8");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.Initial).HasColumnName("initial");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });



            builder.Entity<QcRequestType>(e =>
            {
                e.ToTable("qc_request_type");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Code).HasColumnName("code")
                .HasMaxLength(20);

                e.Property(e => e.Name).HasColumnName("name")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcSamplingType>(e =>
            {
                e.ToTable("qc_sampling_type");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Code).HasColumnName("code")
                .HasMaxLength(20);

                e.Property(e => e.Name).HasColumnName("name")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });


            builder.Entity<QcSamplingShipment>(e =>
            {
                e.ToTable("qc_sampling_shipment");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcSamplingId).HasColumnName("qc_sampling_id");

                e.Property(e => e.QrCode).HasColumnName("qr_code")
                .HasMaxLength(20);

                e.Property(e => e.NoRequest).HasColumnName("no_request")
                .HasMaxLength(25);

                e.Property(e => e.TestParamId).HasColumnName("test_param_id");

                e.Property(e => e.TestParamName).HasColumnName("test_param_name");

                e.Property(e => e.FromOrganizationId).HasColumnName("from_organization_id");

                e.Property(e => e.FromOrganizationName).HasColumnName("from_organization_name");

                e.Property(e => e.ToOrganizationId).HasColumnName("to_organization_id");

                e.Property(e => e.ToOrganizationName).HasColumnName("to_organization_name");

                e.Property(e => e.StartDate).HasColumnName("start_date");

                e.Property(e => e.EndDate).HasColumnName("end_date");

                e.Property(e => e.EndDate).HasColumnName("end_date");

                e.Property(e => e.IsLateTransfer).HasColumnName("is_late_transfer");

                e.Property(e => e.Status).HasColumnName("status")
                   .HasMaxLength(10);

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcSamplingShipmentTracker>(e =>
            {
                e.ToTable("qc_sampling_shipment_tracker");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcSamplingShipmentId).HasColumnName("qc_sampling_shipment_id");

                e.Property(e => e.QrCode).HasColumnName("qr_code")
                .HasMaxLength(20);

                e.Property(e => e.Type).HasColumnName("type");

                //e.Property(e => e.IdLogger).HasColumnName("id_logger");

                //e.Property(e => e.Temperature).HasColumnName("temperature");
                
                e.Property(e => e.processAt).HasColumnName("processed_at");

                e.Property(e => e.UserNik).HasColumnName("user_nik");

                e.Property(e => e.UserName).HasColumnName("user_name");

                e.Property(e => e.OrganizationId).HasColumnName("organization_id");

                e.Property(e => e.OrganizationName).HasColumnName("organization_name");

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<AuthenticatedUserBiohr>(entity =>
            {
                entity.ToTable("authenticated_user_biohr");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Token).HasColumnName("token");

                entity.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(100);

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasMaxLength(10);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100);
            });

            builder.Entity<QcTransactionGroup>(e =>
            {
                e.ToTable("qc_transaction_group");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Code).HasColumnName("code")
                .HasMaxLength(20);

                e.Property(e => e.QcProcessId).HasColumnName("qc_process_id");

                e.Property(e => e.QcProcessName).HasColumnName("qc_process_name")
                .HasMaxLength(200);

                e.Property(e => e.TestDate).HasColumnName("test_date");

                e.Property(e => e.PersonelNik).HasColumnName("personel_nik");

                e.Property(e => e.PersonelName).HasColumnName("personel_name")
                .HasMaxLength(200);

                e.Property(e => e.PersonelPairingNik).HasColumnName("personel_pairing_nik");

                e.Property(e => e.PersonelPairingName).HasColumnName("personel_pairing_name")
                .HasMaxLength(200);

                e.Property(e => e.StatusProses).HasColumnName("status_proses");

                e.Property(e => e.Status).HasColumnName("status");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.WorkflowStatus).HasColumnName("workflow_status")
                .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcTransactionGroupSample>(e =>
            {
                e.ToTable("qc_transaction_sample");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcTransactionGroupId).HasColumnName("qc_transaction_group_id");

                e.Property(e => e.QcTransactionSamplingId).HasColumnName("qc_transaction_sampling_id");

                e.Property(e => e.QcSamplingId).HasColumnName("qc_sampling_id");

                e.Property(e => e.QcSampleId).HasColumnName("qc_sample_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcTransactionGroupSampling>(e =>
            {
                e.ToTable("qc_transaction_sampling");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcTransactionGroupId).HasColumnName("qc_transaction_group_id");

                e.Property(e => e.QcSamplingId).HasColumnName("qc_sampling_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcTransactionGroupSampleValue>(e =>
            {
                e.ToTable("qc_transaction_group_sample_value");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcTransactionGroupFormParameterId)
                   .HasColumnName("qc_transaction_group_form_parameter_id");


                e.Property(e => e.QcTransactionSampleId).HasColumnName("qc_transaction_sample_id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.Value).HasColumnName("value");

                e.Property(e => e.AttchmentFile).HasColumnName("attachment");

                e.Property(e => e.QcTransactionGroupFromMaterialId).HasColumnName("qc_transaction_group_form_material_id");

                e.Property(e => e.QcTransactionGroupFromToolId).HasColumnName("qc_transaction_group_form_tool_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<DigitalSignature>(e =>
            {

                e.ToTable("digital_signature");

                e.Property(e => e.BeginDate).HasColumnName("begin_date");

                e.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                e.Property(e => e.EndDate).HasColumnName("end_date");

                e.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                e.Property(e => e.Nik)
                    .HasColumnName("nik")
                    .HasColumnType("character varying");

                e.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasColumnName("serial_number")
                    .HasColumnType("character varying");

                e.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                e.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");
            });
            builder.Entity<QcTransactionGroupProcess>(e =>
            {
                e.ToTable("qc_transaction_group_process");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcTransactionGroupId).HasColumnName("qc_transaction_group_id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.ParentId).HasColumnName("parent_id");

                e.Property(e => e.RoomId).HasColumnName("room_id");

                e.Property(e => e.IsInputForm).HasColumnName("is_input_form");

                e.Property(e => e.QcProcessId).HasColumnName("qc_process_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcTransactionGroupFormProcedure>(e =>
            {
                e.ToTable("qc_transaction_group_form_procedure");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcTransactionGroupProcessId).HasColumnName("qc_transaction_group_process_id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.Description).HasColumnName("description");

                e.Property(e => e.FormProcedureId).HasColumnName("form_procedure_id");

                e.Property(e => e.QcTransactionGroupSectionId).HasColumnName("qc_transaction_group_section_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcTransactionGroupFormParameter>(e =>
            {
                e.ToTable("qc_transaction_group_form_parameter");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcTransactionGroupFormProcedureId).HasColumnName("qc_transaction_group_form_procedure_id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.Label).HasColumnName("label");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.InputType).HasColumnName("input_type");

                e.Property(e => e.Uom).HasColumnName("uom");

                e.Property(e => e.ThresholdOperator).HasColumnName("threshold_operator");

                e.Property(e => e.ThresholdValue).HasColumnName("threshold_value");

                e.Property(e => e.ThresholdValueTo).HasColumnName("threshold_value_to");

                e.Property(e => e.ThresholdValueFrom).HasColumnName("threshold_value_from");

                e.Property(e => e.NeedAttachment).HasColumnName("need_attachment");

                e.Property(e => e.Note).HasColumnName("note");

                e.Property(e => e.FormProcedureId).HasColumnName("form_procedure_id");

                e.Property(e => e.IsForAllSample).HasColumnName("is_for_all_sample");

                e.Property(e => e.IsResult).HasColumnName("is_result");

                e.Property(e => e.DefaultValue).HasColumnName("default_value");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcTransactionGroupFormMaterial>(e =>
            {
                e.ToTable("qc_transaction_group_form_material");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcTransactionGroupProcessId).HasColumnName("qc_transaction_group_process_id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.ItemId).HasColumnName("item_id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.DefaultPackageQty).HasColumnName("default_package_qty");

                e.Property(e => e.UomPackage).HasColumnName("uom_package");

                e.Property(e => e.DefaultQty).HasColumnName("default_qty");

                e.Property(e => e.Uom).HasColumnName("uom");

                e.Property(e => e.QcProcessId).HasColumnName("qc_process_id");

                e.Property(e => e.QcTransactionGroupSectionId).HasColumnName("qc_transaction_group_section_id");

                e.Property(e => e.GroupName).HasColumnName("group_name");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcTransactionGroupFormTool>(e =>
            {
                e.ToTable("qc_transaction_group_form_tool");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcTransactionGroupProcessId).HasColumnName("qc_transaction_group_process_id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.ToolId).HasColumnName("tools_id");

                e.Property(e => e.ItemId).HasColumnName("item_id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.Quantity).HasColumnName("quantity");

                e.Property(e => e.QcProcessId).HasColumnName("qc_process_id");

                e.Property(e => e.QcTransactionGroupSectionId).HasColumnName("qc_transaction_group_section_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcTransactionGroupValue>(e =>
            {
                e.ToTable("qc_transaction_group_value");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcTransactionGroupFormParameterId).HasColumnName("qc_transaction_group_form_parameter_id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.Value).HasColumnName("value");

                e.Property(e => e.AttchmentFile).HasColumnName("attachment");

                e.Property(e => e.QcTransactionGroupFormMaterialId).HasColumnName("qc_transaction_group_form_material_id");

                e.Property(e => e.QcTransactionGroupFormToolId).HasColumnName("qc_transaction_group_form_tool_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<InputType>(e =>
            {
                e.ToTable("input_type");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.TypeId).HasColumnName("type_id");

                e.Property(e => e.Label).HasColumnName("label");

                e.Property(e => e.Reference).HasColumnName("reference");

                e.Property(e => e.ReferenceType).HasColumnName("reference_type");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<Uom>(e =>
            {
                e.ToTable("uom");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.UomId).HasColumnName("uom_id");

                e.Property(e => e.Label).HasColumnName("label");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<Microflora>(e =>
            {
                e.ToTable("microflora");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.ObjectStatus).HasColumnName("object_status");

                e.Property(e => e.MicrobaCategory).HasColumnName("microba_category");
                e.Property(e => e.MicrobaId).HasColumnName("microba_id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcResult>(e =>
            {
                e.ToTable("qc_result");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ParentId).HasColumnName("parent_id");

                e.Property(e => e.SampleId).HasColumnName("qc_sample_id");

                e.Property(e => e.Value).HasColumnName("value");

                e.Property(e => e.TestVariableConclusion).HasColumnName("test_variabel_conclusion");

                e.Property(e => e.TestVariableId).HasColumnName("test_variabel_id");

                e.Property(e => e.Note).HasColumnName("note");

                e.Property(e => e.AttchmentFile).HasColumnName("attchment_file");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<Building>(entity =>
            {
                entity.ToTable("building");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                   .IsRequired()
                   .HasColumnName("building_id")
                   .HasColumnType("character varying");

                entity.Property(e => e.Name)
                   .IsRequired()
                   .HasColumnName("building_name")
                   .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                   .IsRequired()
                   .HasColumnName("created_by")
                   .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                   .HasColumnName("row_status")
                   .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                   .IsRequired()
                   .HasColumnName("updated_by")
                   .HasColumnType("character varying");
            });

            builder.Entity<Facility>(entity =>
            {
                entity.ToTable("facility");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                   .IsRequired()
                   .HasColumnName("created_by")
                   .HasColumnType("character varying");

                entity.Property(e => e.Code)
                   .IsRequired()
                   .HasColumnName("facility_code")
                   .HasColumnType("character varying");

                entity.Property(e => e.Name)
                   .IsRequired()
                   .HasColumnName("facility_name")
                   .HasColumnType("character varying");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
                entity.Property(e => e.ObjectStatus).HasColumnName("object_status");

                entity.Property(e => e.RowStatus)
                   .HasColumnName("row_status")
                   .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                   .IsRequired()
                   .HasColumnName("updated_by")
                   .HasColumnType("character varying");
            });

            builder.Entity<Purpose>(e =>
            {
                e.ToTable("purposes");

                e.Property(e => e.Id).HasColumnName("id");

                //e.Property(e => e.RequestTypeId).HasColumnName("request_type_id");

                e.Property(e => e.Code).HasColumnName("code");

                e.Property(e => e.Name).HasColumnName("name");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(150);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(150);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<PurposesPersonel>(entity =>
            {
                entity.ToTable("purposes_personel");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.Nik)
                    .IsRequired()
                    .HasColumnName("nik")
                    .HasMaxLength(20);

                entity.Property(e => e.PersonelName)
                    .IsRequired()
                    .HasColumnName("personel_name")
                    .HasMaxLength(150);

                entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150);
            });

            builder.Entity<RequestPurpose>(e =>
            {
                e.ToTable("request_purposes");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcRequestId).HasColumnName("qc_request_id");

                e.Property(e => e.PurposeId).HasColumnName("purpose_id");

                e.Property(e => e.PurposeCode).HasColumnName("purpose_code");

                e.Property(e => e.PurposeName).HasColumnName("purpose_name");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(150);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(150);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<FormSection>(e =>
            {
                e.ToTable("form_section");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.TypeId).HasColumnName("type_id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.Label).HasColumnName("label");

                e.Property(e => e.Icon).HasColumnName("icon");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(150);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(150);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcTransactionGroupFormSection>(e =>
            {
                e.ToTable("qc_transaction_group_form_section");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.SectionId).HasColumnName("section_id");

                e.Property(e => e.SectionTypeId).HasColumnName("section_type_id");

                e.Property(e => e.QcProcessId).HasColumnName("qc_process_id");

                e.Property(e => e.Sequence).HasColumnName("sequence");

                e.Property(e => e.Label).HasColumnName("label");

                e.Property(e => e.Icon).HasColumnName("icon");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(150);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(150);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RequestRoom>(e =>
            {
                e.ToTable("request_room");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcRequestId).HasColumnName("qc_request_id");

                e.Property(e => e.RoomId).HasColumnName("room_id");

                e.Property(e => e.RoomCode).HasColumnName("room_code");

                e.Property(e => e.RoomName).HasColumnName("room_name");

                e.Property(e => e.AhuId).HasColumnName("ahu_id");

                e.Property(e => e.AhuCode).HasColumnName("ahu_code");

                e.Property(e => e.AhuName).HasColumnName("ahu_name");

                e.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");

                e.Property(e => e.GradeRoomCode).HasColumnName("grade_room_code");

                e.Property(e => e.GradeRoomName).HasColumnName("grade_room_name");

                e.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");

                e.Property(e => e.TestScenarioName).HasColumnName("test_scenario_name");

                e.Property(e => e.TestScenarioLabel).HasColumnName("test_scenario_label");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(150);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(150);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelEmProdPhaseToRoom>(e =>
            {
                e.ToTable("rel_em_prod_phase_to_room");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.EmPhaseId).HasColumnName("em_prod_phase_id");

                e.Property(e => e.RoomId).HasColumnName("room_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(150);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(150);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RoomFacility>(e =>
            {
                e.ToTable("facility_room");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.FacilityId).HasColumnName("facility_id");

                e.Property(e => e.RoomId).HasColumnName("room_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(150);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(150);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RequestAhu>(e =>
            {
                e.ToTable("request_ahu");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcRequestId).HasColumnName("qc_request_id");

                e.Property(e => e.AhuId).HasColumnName("ahu_id");

                e.Property(e => e.AhuCode).HasColumnName("ahu_code");

                e.Property(e => e.AhuName).HasColumnName("ahu_name");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);

                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(150);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(150);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<QcSamplingAttachment>(e =>
            {
                e.ToTable("qc_sampling_attachment");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.QcSamplingId).HasColumnName("qc_sampling_id");

                e.Property(e => e.AttachmentFileName).HasColumnName("attachment_file_name");

                e.Property(e => e.AttachmentFileLink).HasColumnName("attachment_file_link");

                e.Property(e => e.AttachmentStorageName).HasColumnName("attachment_storage_name");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(150);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RoomSamplingPointLayout>(e =>
            {
                e.ToTable("room_sampling_point_layout");

                e.Property(e => e.Id).HasColumnName("id");

                //e.Property(e => e.RoomId).HasColumnName("room_id");
                e.Property(e => e.RoomPurposeId).HasColumnName("room_purpose_id");

                e.Property(e => e.AttachmentFile).HasColumnName("attachment_file");

                e.Property(e => e.FileName).HasColumnName("file_name");

                e.Property(e => e.FileType).HasColumnName("file_type");

                e.Property(e => e.RowStatus).HasColumnName("row_status");

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelSamplingPurposeToolGroup>(e =>
            {
                e.ToTable("rel_sampling_purpose_tool_group");

                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.PurposeId).HasColumnName("purpose_id");

                e.Property(e => e.ToolGroupId).HasColumnName("tool_group_id");

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedBy)
                  .HasColumnName("created_by")
                  .HasMaxLength(100);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(100);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ItemDosageForm>(e =>
            {
                e.ToTable("item_dosage_form");
                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemDosageFormCode).HasColumnName("item_dosage_form_code")
                .HasMaxLength(20);

                e.Property(e => e.ItemDosageFormName).HasColumnName("item_dosage_form_name")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);


                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ItemProductGroup>(e =>
            {
                e.ToTable("item_product_group");
                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemProductGroupCode).HasColumnName("item_product_group_code")
                .HasMaxLength(20);

                e.Property(e => e.ItemProductGroupName).HasColumnName("item_product_group_name")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);


                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelProductProdPhaseToRoom>(e =>
            {
                e.ToTable("rel_product_prod_phase_to_room");
                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ProductProductionPhasesId).HasColumnName("product_production_phases_id")
                .HasMaxLength(20);

                e.Property(e => e.RoomId).HasColumnName("room_id")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);


                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ItemPresentation>(e =>
            {
                e.ToTable("item_presentation");
                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemPresentationCode).HasColumnName("item_presentation_code")
                .HasMaxLength(20);

                e.Property(e => e.ItemPresentationName).HasColumnName("item_presentation_name")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);


                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<RelItemsItemPresentation>(e =>
            {
                e.ToTable("rel_items_item_presentation");
                e.Property(e => e.Id).HasColumnName("id");

                e.Property(e => e.ItemId).HasColumnName("item_id")
                .HasMaxLength(20);

                e.Property(e => e.ItemPresentationId).HasColumnName("item_presentation_id")
                .HasMaxLength(200);

                e.Property(e => e.RowStatus).HasColumnName("row_status")
                .HasMaxLength(10);


                e.Property(e => e.CreatedBy)
                   .HasColumnName("created_by")
                   .HasMaxLength(50);

                e.Property(e => e.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(e => e.UpdatedBy)
                   .HasColumnName("updated_by")
                   .HasMaxLength(50);

                e.Property(e => e.UpdatedAt)
                   .HasColumnName("updated_at")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            builder.Entity<ProductProductionPhasesPersonel>(entity =>
            {
                entity.ToTable("product_production_phases_personel");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.PersonelNik)
                    .IsRequired()
                    .HasColumnName("personel_nik")
                    .HasMaxLength(20);

                entity.Property(e => e.PersonelName)
                    .IsRequired()
                    .HasColumnName("personel_name")
                    .HasMaxLength(150);

                entity.Property(e => e.ProductProductionPhasesId).HasColumnName("product_production_phases_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150);
            });

            builder.Entity<QcSamplingPersonel>(entity =>
            {
                entity.ToTable("qc_sampling_personel");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.PersonelNik)
                    .IsRequired()
                    .HasColumnName("personel_nik")
                    .HasMaxLength(20);

                entity.Property(e => e.PersonelName)
                    .IsRequired()
                    .HasColumnName("personel_name")
                    .HasMaxLength(150);

                entity.Property(e => e.QcSamplingId).HasColumnName("qc_sampling_id");

                entity.Property(e => e.ProductProductionPhasesPersonelId).HasColumnName("product_production_phases_personel_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150);
            });

            builder.Entity<RoomPurpose>(entity =>
            {
                entity.ToTable("room_purpose");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                // entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                // entity.HasOne(d => d.Purpose)
                //     .WithMany(p => p.RoomPurpose)
                //     .HasForeignKey(d => d.PurposeId)
                //     .HasConstraintName("room_purpose_fk");
            });

            builder.Entity<RequestGroupRoomPurpose>(entity =>
            {
                entity.ToTable("request_group_room_purpose");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoomPurposeId).HasColumnName("room_purpose_id");
                entity.Property(e => e.RequestQcsId).HasColumnName("request_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                // entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                // entity.HasOne(d => d.Purpose)
                //     .WithMany(p => p.RoomPurpose)
                //     .HasForeignKey(d => d.PurposeId)
                //     .HasConstraintName("room_purpose_fk");
            });

            builder.Entity<RequestGroupToolPurpose>(entity =>
            {
                entity.ToTable("request_group_tool_purpose");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ToolPurposeId).HasColumnName("tool_purpose_id");
                entity.Property(e => e.RequestQcsId).HasColumnName("request_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                // entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                // entity.HasOne(d => d.Purpose)
                //     .WithMany(p => p.RoomPurpose)
                //     .HasForeignKey(d => d.PurposeId)
                //     .HasConstraintName("room_purpose_fk");
            });

            builder.Entity<RoomPurposeToMasterPurpose>(entity =>
            {
                entity.ToTable("room_purpose_to_master_purpose");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.RoomPurposeId).HasColumnName("room_purpose_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<TransactionActivity>(entity =>
            {
                entity.ToTable("transaction_activity", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);
            });

            builder.Entity<TransactionBatch>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("transaction_batch_pk");

                entity.ToTable("transaction_batch", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AttachmentNotes)
                    .HasMaxLength(200)
                    .HasColumnName("attachment_notes");
                entity.Property(e => e.RequestQcsId).HasColumnName("request_qcs_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<TransactionBatchAttachment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("transaction_batch_attachments_pk");

                entity.ToTable("transaction_batch_attachments", "transaction");
                entity.Property(e => e.AttachmentStorageName).HasColumnName("attachment_storage_name");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AttachmentFile)
                    .HasColumnType("character varying")
                    .HasColumnName("attachment_file");
                entity.Property(e => e.FileName)
                    .HasColumnType("character varying")
                    .HasColumnName("file_name");
                entity.Property(e => e.FileType)
                    .HasColumnName("file_type")
                    .HasMaxLength(20);
                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .HasColumnName("title");
                entity.Property(e => e.TrsBatchId).HasColumnName("trs_batch_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<TransactionBatchLine>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("transaction_batch_lines_pk");

                entity.ToTable("transaction_batch_lines", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ItemId).HasColumnName("item_id");
                entity.Property(e => e.ItemName)
                    .HasMaxLength(200)
                    .HasColumnName("item_name");
                entity.Property(e => e.NoBatch)
                    .HasMaxLength(100)
                    .HasColumnName("no_batch");
                entity.Property(e => e.Notes)
                    .HasColumnType("character varying")
                    .HasColumnName("notes");
                entity.Property(e => e.TrsBatchId).HasColumnName("trs_batch_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<TransactionFacility>(entity =>
            {
                entity.ToTable("transaction_facility", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.FacilityCode)
                    .IsRequired()
                    .HasColumnName("facility_code")
                    .HasColumnType("character varying");

                entity.Property(e => e.FacilityName)
                    .IsRequired()
                    .HasColumnName("facility_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.TransactionFacility)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("fk_transaction_facility_organization");
            });

            builder.Entity<TransactionFacilityRoom>(entity =>
            {
                entity.ToTable("transaction_facility_room", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.FacilityId).HasColumnName("facility_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.TransactionFacilityRoom)
                    .HasForeignKey(d => d.FacilityId)
                    .HasConstraintName("fk_transaction_facility_room");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.TransactionFacilityRoom)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("transaction_facility_room_fk");
            });

            builder.Entity<TransactionGradeRoom>(entity =>
            {
                entity.ToTable("transaction_grade_room", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.GradeRoomDefault)
                    .HasColumnName("grade_room_default")
                    .HasColumnType("character varying");

                entity.Property(e => e.GradeRoomIdentificationTest).HasColumnName("grade_room_identification_test");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.ObjectStatus).HasColumnName("object_status");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasMaxLength(50);

                entity.Property(e => e.TestGroupId).HasColumnName("test_group_id");
            });

            builder.Entity<TransactionOrganization>(entity =>
            {
                entity.ToTable("transaction_organization", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BiohrOrganizationId).HasColumnName("biohr_organization_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.OrgCode)
                    .IsRequired()
                    .HasColumnName("org_code")
                    .HasMaxLength(20);
            });

            builder.Entity<TransactionPurposes>(entity =>
            {
                entity.ToTable("transaction_purposes", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);
            });

            builder.Entity<TransactionRelGradeRoomScenario>(entity =>
            {
                entity.ToTable("transaction_rel_grade_room_scenario", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");

                entity.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");

                entity.HasOne(d => d.GradeRoom)
                    .WithMany(p => p.TransactionRelGradeRoomScenario)
                    .HasForeignKey(d => d.GradeRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transaction_grade_room");

                entity.HasOne(d => d.TestScenario)
                    .WithMany(p => p.TransactionRelGradeRoomScenario)
                    .HasForeignKey(d => d.TestScenarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transaction_test_scenario");
            });

            builder.Entity<TransactionRelRoomSamplingPoint>(entity =>
            {
                entity.ToTable("transaction_rel_room_sampling_point", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.RoomPurposeId).HasColumnName("room_purpose_id");

                entity.Property(e => e.SamplingPoinId).HasColumnName("sampling_poin_id");

                entity.Property(e => e.ScenarioLabel)
                    .HasColumnName("scenario_label")
                    .HasMaxLength(200);

                entity.HasOne(d => d.SamplingPoin)
                    .WithMany(p => p.TransactionRelRoomSamplingPoint)
                    .HasForeignKey(d => d.SamplingPoinId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transaction_rel_room_sampling_point_fk_1");
            });

            builder.Entity<TransactionRelSamplingPurposeToolGroup>(entity =>
            {
                entity.ToTable("transaction_rel_sampling_purpose_tool_group", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.ToolGroupId).HasColumnName("tool_group_id");

                // entity.HasOne(d => d.Purpose)
                //     .WithMany(p => p.TransactionRelSamplingPurposeToolGroup)
                //     .HasForeignKey(d => d.PurposeId)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("transaction_rel_sprtg_purposes_fk");

                // entity.HasOne(d => d.ToolGroup)
                //     .WithMany(p => p.TransactionRelSamplingPurposeToolGroup)
                //     .HasForeignKey(d => d.ToolGroupId)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("transaction_rel_sprtg_tool_group_fk");
            });

            builder.Entity<TransactionRelSamplingTestParam>(entity =>
            {
                entity.ToTable("transaction_rel_sampling_test_param", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.SamplingPointId).HasColumnName("sampling_point_id");

                entity.Property(e => e.TestScenarioParamId).HasColumnName("test_scenario_param_id");

                entity.HasOne(d => d.SamplingPoint)
                    .WithMany(p => p.TransactionRelSamplingTestParam)
                    .HasForeignKey(d => d.SamplingPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transaction_sampling_point");

                entity.HasOne(d => d.TestScenarioParam)
                    .WithMany(p => p.TransactionRelSamplingTestParam)
                    .HasForeignKey(d => d.TestScenarioParamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transaction_test_scenario_param");
            });

            builder.Entity<TransactionRelSamplingTool>(entity =>
            {
                entity.ToTable("transaction_rel_sampling_tool", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.SamplingPoinId).HasColumnName("sampling_poin_id");

                entity.Property(e => e.ScenarioLabel)
                    .HasColumnName("scenario_label")
                    .HasMaxLength(200);

                entity.Property(e => e.ToolId).HasColumnName("tool_id");

                entity.Property(e => e.ToolPurposeId).HasColumnName("tool_purpose_id");

                entity.HasOne(d => d.SamplingPoin)
                    .WithMany(p => p.TransactionRelSamplingTool)
                    .HasForeignKey(d => d.SamplingPoinId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transaction_rel_sampling_tool_fk");
            });

            builder.Entity<TransactionRelTestScenarioParam>(entity =>
            {
                entity.ToTable("transaction_rel_test_scenario_param", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.TestParameterId).HasColumnName("test_parameter_id");

                entity.Property(e => e.TestScenarioId).HasColumnName("test_scenario_id");

                entity.HasOne(d => d.TestParameter)
                    .WithMany(p => p.TransactionRelTestScenarioParam)
                    .HasForeignKey(d => d.TestParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transaction_test_parameter");

                entity.HasOne(d => d.TestScenario)
                    .WithMany(p => p.TransactionRelTestScenarioParam)
                    .HasForeignKey(d => d.TestScenarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transaction_test_scenario");
            });

            builder.Entity<TransactionRoom>(entity =>
            {
                entity.ToTable("transaction_room", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ahu).HasColumnName("ahu");

                entity.Property(e => e.AirChangeOperator).HasColumnName("air_change_operator");

                entity.Property(e => e.AirChangeValue)
                    .HasColumnName("air_change_value")
                    .HasColumnType("numeric");

                entity.Property(e => e.AirChangeValueFrom)
                    .HasColumnName("air_change_value_from")
                    .HasColumnType("numeric");

                entity.Property(e => e.AirChangeValueTo)
                    .HasColumnName("air_change_value_to")
                    .HasColumnType("numeric");

                entity.Property(e => e.Area).HasColumnName("area");

                entity.Property(e => e.BuildingId).HasColumnName("building_id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.Floor)
                    .HasColumnName("floor")
                    .HasColumnType("character varying");

                entity.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");

                entity.Property(e => e.HumidityOperator).HasColumnName("humidity_operator");

                entity.Property(e => e.HumidityValue)
                    .HasColumnName("humidity_value")
                    .HasColumnType("numeric");

                entity.Property(e => e.HumidityValueFrom)
                    .HasColumnName("humidity_value_from")
                    .HasColumnType("numeric");

                entity.Property(e => e.HumidityValueTo)
                    .HasColumnName("humidity_value_to")
                    .HasColumnType("numeric");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.ObjectStatus)
                    .HasColumnName("object_status")
                    .HasDefaultValueSql("3");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.PosId)
                    .HasColumnName("pos_id")
                    .HasColumnType("character varying");

                entity.Property(e => e.PressureOperator).HasColumnName("pressure_operator");

                entity.Property(e => e.PressureValue)
                    .HasColumnName("pressure_value")
                    .HasColumnType("numeric");

                entity.Property(e => e.PressureValueFrom)
                    .HasColumnName("pressure_value_from")
                    .HasColumnType("numeric");

                entity.Property(e => e.PressureValueTo)
                    .HasColumnName("pressure_value_to")
                    .HasColumnType("numeric");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasMaxLength(50);

                entity.Property(e => e.TemperatureOperator).HasColumnName("temperature_operator");

                entity.Property(e => e.TemperatureValue)
                    .HasColumnName("temperature_value")
                    .HasColumnType("numeric");

                entity.Property(e => e.TemperatureValueFrom)
                    .HasColumnName("temperature_value_from")
                    .HasColumnType("numeric");

                entity.Property(e => e.TemperatureValueTo)
                    .HasColumnName("temperature_value_to")
                    .HasColumnType("numeric");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.TransactionRoom)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("fk_transaction_room_1");
            });

            builder.Entity<TransactionRoomPurpose>(entity =>
            {
                entity.ToTable("transaction_room_purpose", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");
            });

            builder.Entity<TransactionRoomPurposeToMasterPurpose>(entity =>
            {
                entity.ToTable("transaction_room_purpose_to_master_purpose", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.RoomPurposeId).HasColumnName("room_purpose_id");
            });

            builder.Entity<TransactionRoomSamplingPointLayout>(entity =>
            {
                entity.ToTable("transaction_room_sampling_point_layout", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AttachmentFile)
                    .IsRequired()
                    .HasColumnName("attachment_file")
                    .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("file_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.FileType)
                    .HasColumnName("file_type")
                    .HasColumnType("character varying");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.RoomPurposeId).HasColumnName("room_purpose_id");
            });

            builder.Entity<TransactionSamplingPoint>(entity =>
            {
                entity.ToTable("transaction_sampling_point", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.ToolId).HasColumnName("tool_id");
            });

            builder.Entity<TransactionTestParameter>(entity =>
            {
                entity.ToTable("transaction_test_parameter", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.QcProcessId).HasColumnName("qc_process_id");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(10);

                entity.Property(e => e.TestGroupId).HasColumnName("test_group_id");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.TransactionTestParameter)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("transaction_test_parameter_fk2");

                //  entity.HasOne(d => d.QcProcess)
                //      .WithMany(p => p.TransactionTestParameter)
                //      .HasForeignKey(d => d.QcProcessId)
                //      .HasConstraintName("transaction_test_parameter_fk_2");

                //  entity.HasOne(d => d.TestGroup)
                //      .WithMany(p => p.TransactionTestParameter)
                //      .HasForeignKey(d => d.TestGroupId)
                //      .OnDelete(DeleteBehavior.ClientSetNull)
                //      .HasConstraintName("transaction_test_parameter_fk");
            });

            builder.Entity<TransactionTestScenario>(entity =>
            {
                entity.ToTable("transaction_test_scenario", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.Label)
                    .HasColumnName("label")
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasMaxLength(50);
            });

            builder.Entity<TransactionTestVariable>(entity =>
            {
                entity.ToTable("transaction_test_variable", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasMaxLength(50);

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.TestParameterId).HasColumnName("test_parameter_id");

                entity.Property(e => e.ThresholdValueFrom)
                    .HasColumnName("threshold_value_from")
                    .HasColumnType("numeric");

                entity.Property(e => e.ThresholdValueTo)
                    .HasColumnName("threshold_value_to")
                    .HasColumnType("numeric");

                entity.Property(e => e.TresholdOperator).HasColumnName("treshold_operator");

                entity.Property(e => e.TresholdValue)
                    .HasColumnName("treshold_value")
                    .HasColumnType("numeric");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(150);

                entity.Property(e => e.VariableName)
                    .IsRequired()
                    .HasColumnName("variable_name")
                    .HasMaxLength(200);

                entity.HasOne(d => d.TestParameter)
                    .WithMany(p => p.TransactionTestVariable)
                    .HasForeignKey(d => d.TestParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transaction_test_variable_fk");
            });

            builder.Entity<TransactionTool>(entity =>
            {
                entity.ToTable("transaction_tool", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.FacilityId)
                    .HasColumnName("facility_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GradeRoomId).HasColumnName("grade_room_id");

                entity.Property(e => e.MachineId).HasColumnName("machine_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.ObjectStatus)
                    .HasColumnName("object_status")
                    .HasDefaultValueSql("3");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.SerialNumberId)
                    .HasColumnName("serial_number_id")
                    .HasMaxLength(20);

                entity.Property(e => e.ToolCode)
                    .IsRequired()
                    .HasColumnName("tool_code")
                    .HasMaxLength(20);

                entity.Property(e => e.ToolGroupId).HasColumnName("tool_group_id");

                // entity.HasOne(d => d.ToolGroup)
                //     .WithMany(p => p.TransactionTool)
                //     .HasForeignKey(d => d.ToolGroupId)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("transaction_tool_fk_2");
            });

            builder.Entity<TransactionToolActivity>(entity =>
            {
                entity.ToTable("transaction_tool_activity", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActivityCode)
                    .IsRequired()
                    .HasColumnName("activity_code")
                    .HasMaxLength(20);

                entity.Property(e => e.ActivityDate)
                    .HasColumnName("activity_date")
                    .HasColumnType("date");

                entity.Property(e => e.ActivityId).HasColumnName("activity_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.ExpiredDate)
                    .HasColumnName("expired_date")
                    .HasColumnType("date");

                entity.Property(e => e.ToolId).HasColumnName("tool_id");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.TransactionToolActivity)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transaction_tool_activity_fk2");

                entity.HasOne(d => d.Tool)
                    .WithMany(p => p.TransactionToolActivity)
                    .HasForeignKey(d => d.ToolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transaction_tool_activity_fk");
            });

            builder.Entity<TransactionToolGroup>(entity =>
            {
                entity.ToTable("transaction_tool_group", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(150);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasColumnName("label")
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);
            });

            builder.Entity<TransactionToolPurpose>(entity =>
            {
                entity.ToTable("transaction_tool_purpose", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.ToolId).HasColumnName("tool_id");
            });

            builder.Entity<TransactionToolPurposeToMasterPurpose>(entity =>
            {
                entity.ToTable("transaction_tool_purpose_to_master_purpose", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

                entity.Property(e => e.ToolPurposeId).HasColumnName("tool_purpose_id");
            });

            builder.Entity<TransactionToolSamplingPointLayout>(entity =>
            {
                entity.ToTable("transaction_tool_sampling_point_layout", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AttachmentFile)
                    .IsRequired()
                    .HasColumnName("attachment_file")
                    .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("file_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.FileType)
                    .HasColumnName("file_type")
                    .HasColumnType("character varying");

                entity.Property(e => e.ToolPurposeId).HasColumnName("tool_purpose_id");
            });

            builder.Entity<TransactionBuilding>(entity =>
            {
                entity.ToTable("transaction_building", "transaction");

                entity.HasIndex(e => e.BuildingId)
                    .HasName("transaction_building_idx_building_on_building_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BuildingId)
                    .IsRequired()
                    .HasColumnName("building_id")
                    .HasColumnType("character varying");

                entity.Property(e => e.BuildingName)
                    .IsRequired()
                    .HasColumnName("building_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<QcSamplingTemplate>(entity =>
            {
                entity.ToTable("qc_sampling_template");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.name).IsRequired().HasColumnName("name");
                entity.Property(e => e.ValidityPeriodStart).IsRequired().HasColumnName("validity_period_start");
                entity.Property(e => e.ValidityPeriodEnd).IsRequired().HasColumnName("validity_period_end");
                entity.Property(e => e.MethodId).IsRequired().HasColumnName("method_id");
                entity.Property(e => e.TestTypeId).IsRequired().HasColumnName("test_type_id");
                entity.Property(e => e.Status).IsRequired().HasColumnName("status");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            builder.Entity<TransactionTesting>(entity =>
            {
                entity.ToTable("transaction_testing", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.TestingDate).IsRequired().HasColumnName("testing_date");
                entity.Property(e => e.ObjectStatus).IsRequired().HasColumnName("object_status");
                entity.Property(e => e.CreatedBy).IsRequired().HasColumnName("created_by");
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
                entity.Property(e => e.UpdatedBy).IsRequired().HasColumnName("updated_by");
                entity.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
                entity.Property(e => e.RowStatus).HasColumnName("row_status");
                entity.Property(e => e.TestTypeNameIdn).HasColumnName("test_type_name_idn");
                entity.Property(e => e.TestTypeNameEn).HasColumnName("test_type_name_en");
                entity.Property(e => e.TestTypeCode).HasColumnName("test_type_code");
                entity.Property(e => e.TestTypeId).HasColumnName("test_type_id");
                entity.Property(e => e.TestTypeMethodName).HasColumnName("test_type_method_name");
                entity.Property(e => e.TestTypeMethodCode).HasColumnName("test_type_method_code");
                entity.Property(e => e.TestTypeMethodId).HasColumnName("test_type_method_id");
                entity.Property(e => e.TestTemplateId).HasColumnName("test_template_id");
                entity.Property(e => e.TestingStartDate).HasColumnName("test_start_date");
                entity.Property(e => e.TestingEndtDate).HasColumnName("test_end_date");
            });

            builder.Entity<TransactionTestingProcedure>(entity =>
            {
                entity.ToTable("transaction_testing_procedure", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TransactionTestTypeMethodId).HasColumnName("transaction_test_type_method_id");
                entity.Property(e => e.TestTypeMethodCode).HasColumnName("test_type_method_code").HasColumnType("character varying");
                entity.Property(e => e.Title).HasColumnName("title").IsRequired().HasColumnType("character varying");
                entity.Property(e => e.Instruction).HasColumnName("instruction").IsRequired().HasColumnType("text");
                entity.Property(e => e.RowStatus).HasColumnName("row_status").HasColumnType("character varying");
                entity.Property(e => e.Sequence).HasColumnName("sequence");
                entity.Property(e => e.AttachmentStorageName).HasColumnName("attachment_storage_name").HasColumnType("character varying");
                entity.Property(e => e.AttachmentFile).HasColumnName("attachment_file").HasColumnType("character varying");
                entity.Property(e => e.IsEachSample).HasColumnName("is_each_sample");
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
                entity.Property(e => e.Status).HasColumnName("status").HasColumnType("integer");
                entity.Property(e => e.CreatedBy).IsRequired().HasColumnName("created_by").HasColumnType("character varying");
                entity.Property(e => e.UpdatedBy).IsRequired().HasColumnName("updated_by").HasColumnType("character varying");
                entity.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
            });

            builder.Entity<TransactionTestingProcedureParameter>(entity =>
            {
                entity.ToTable("transaction_testing_procedure_parameter", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.InputTypeId).HasColumnName("input_type_id");
                entity.Property(e => e.IsNullable).HasColumnName("isnullable");
                entity.Property(e => e.TransactionTestingProcedureId).HasColumnName("transaction_testing_procedure_id");
                entity.Property(e => e.TestTypeProcedureCode).HasColumnName("test_type_procedure_code");
                entity.Property(e => e.Properties).HasColumnName("properties").HasColumnType("jsonb");
                entity.Property(e => e.PropertiesValue).HasColumnName("properties_value").HasColumnType("jsonb");
                entity.Property(e => e.RowStatus).HasColumnName("row_status").HasColumnType("character varying");
                entity.Property(e => e.HasAttachment).HasColumnName("has_attachment");
                entity.Property(e => e.IsDeviation).HasColumnName("is_deviation");
                entity.Property(e => e.DeviationLevel).HasColumnName("deviation_level");
                entity.Property(e => e.DeviationNote).HasColumnName("deviation_note");
                entity.Property(e => e.Sequence).HasColumnName("sequence");
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).IsRequired().HasColumnName("created_by").HasColumnType("character varying");
                entity.Property(e => e.UpdatedBy).IsRequired().HasColumnName("updated_by").HasColumnType("character varying");
                entity.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
                entity.HasOne(d => d.Procedure)
                    .WithMany(p => p.ProcedureParameter)
                    .HasForeignKey(d => d.TransactionTestingProcedureId)
                    .HasConstraintName("procedure_procedure_parameter_fk");
            });

            builder.Entity<TransactionTestingProcedureParameterAttachment>(entity =>
            {
                entity.ToTable("transaction_testing_procedure_parameter_attachment", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasColumnName("filename")
                    .HasColumnType("character varying");

                entity.Property(e => e.MediaLink)
                    .IsRequired()
                    .HasColumnName("media_link")
                    .HasColumnType("character varying");

                entity.Property(e => e.Ext)
                    .IsRequired()
                    .HasColumnName("ext")
                    .HasColumnType("character varying");

                entity.Property(e => e.ExecutorName)
                    .HasColumnName("executor_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ExecutorNik)
                    .HasColumnName("executor_nik")
                    .HasColumnType("character varying");

                entity.Property(e => e.ExecutorPosition)
                    .HasColumnName("executor_position")
                    .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("character varying");

                entity.Property(e => e.TransactionTestingProcedureParameterId).HasColumnName("transaction_testing_procedure_parameter_id");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.ProcedureParameter)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.TransactionTestingProcedureParameterId)
                    .HasConstraintName("procedure_parameter_attachment_fk");

                entity.Property(e => e.TransactionTestingSamplingId)
                    .HasColumnName("transaction_testing_sampling_id")
                    .HasColumnType("integer");
            });

            builder.Entity<TransactionTestingProcedureParameterNote>(entity =>
            {
                entity.ToTable("transaction_testing_procedure_parameter_note", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Position)
                    .HasColumnName("position")
                    .HasColumnType("character varying");

                entity.Property(e => e.TransactionTestingProcedureParameterId).HasColumnName("transaction_testing_procedure_parameter_id");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.ProcedureParameter)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.TransactionTestingProcedureParameterId)
                    .HasConstraintName("procedure_parameter_note_fk");

                entity.Property(e => e.TransactionTestingSamplingId)
                    .HasColumnName("transaction_testing_sampling_id")
                    .HasColumnType("integer");
            });

            builder.Entity<TransactionTestingResultSample>(entity =>
            {
                entity.ToTable("transaction_testing_result_sample", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.NeedAttachment).HasColumnName("need_attachment");
                entity.Property(e => e.RowStatus).HasColumnName("row_status");
                entity.Property(e => e.Properties).HasColumnName("properties").HasColumnType("jsonb");
                entity.Property(e => e.PropertiesValue).HasColumnName("properties_value").HasColumnType("jsonb");
                entity.Property(e => e.TransactionTestTypeMethodResultParameterCode).HasColumnName("transaction_test_type_method_result_parameter_code");
                entity.Property(e => e.TransactionTestTypeMethodResultParameterId).HasColumnName("transaction_test_type_method_result_parameter_id");
                entity.Property(e => e.TransactionTestingId).HasColumnName("transaction_testing_id");
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).IsRequired().HasColumnName("created_by").HasColumnType("character varying");
                entity.Property(e => e.UpdatedBy).IsRequired().HasColumnName("updated_by").HasColumnType("character varying");
                entity.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
            });

            builder.Entity<TemplateTestingPersonnel>(entity =>
            {
                entity.ToTable("transaction_tmplt_testing_personnel", "transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NewNik)
                .HasColumnName("newnik");

                entity.Property(e => e.Name)
                .HasColumnName("nama");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

                entity.Property(e => e.TemplateTestingCode)
                .HasColumnName("testing_code");

                entity.Property(e => e.Nik)
                .HasColumnName("nik");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.TemplateTestingId)
                .HasColumnName("testing_id");

                entity.Property(e => e.PositionCode)
                .HasColumnName("posisi_code");

                entity.Property(e => e.PositionId)
                .HasColumnName("posisi_id");

                entity.Property(e => e.UpdatedBy)
               .HasColumnName("updated_by");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Position)
                .HasColumnName("posisi");
                entity.Property(e => e.CheckIn).HasColumnName("check_in");

                entity.Property(e => e.CheckOut).HasColumnName("check_out");

            });

            builder.Entity<TemplateTestingNote>(entity =>
            {
                entity.ToTable("transaction_tmplt_testing_note", "transaction");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.Note)
                .HasColumnName("note")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.TemplateTestingId)
                .HasColumnName("tmplt_testing_id");

                entity.Property(e => e.TemplateTestingCode)
                .HasColumnName("tmplt_testing_code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Position)
                .HasColumnName("position")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");
            });

            builder.Entity<TemplateTestingAttachment>(entity =>
            {
                entity.ToTable("transaction_tmplt_testing_attachment", "transaction");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.Filename)
                .HasColumnName("filename")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.ExecutorNik)
                .HasColumnName("executor_nik")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.ExecutorName)
                .HasColumnName("executor_name")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.ExecutorPosition)
                .HasColumnName("executor_position")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.MediaLink)
                .HasColumnName("media_link")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.TemplateTestingId)
                .HasColumnName("tmplt_testing_id");

                entity.Property(e => e.TemplateTestingCode)
                .HasColumnName("tmplt_testing_code")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.Ext)
                .HasColumnName("ext")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");
            });

            builder.Entity<TemplateOperatorTesting>(entity =>
            {
                entity.ToTable("transaction_template_testing", "transaction");
                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.TestingDate).HasColumnName("testing_date");
                entity.Property(e => e.ObjectStatus).HasColumnName("object_status");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by").HasColumnType("character varying"); ;
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired()
                .HasColumnType("character varying");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.ValidityPeriodStart)
                .HasColumnName("validity_period_start");
                entity.Property(e => e.ValidityPeriodEnd)
                .HasColumnName("validity_period_end");
                entity.Property(e => e.IdTemplate)
                .HasColumnName("id_template");
                entity.Property(e => e.RowStatus)
                .HasColumnName("row_status")
                .HasColumnType("character varying");
                entity.Property(e => e.TestTypeNameIdn)
                .HasColumnName("test_type_name_idn")
                .HasColumnType("character varying");

                entity.Property(e => e.TestTypeNameEn)
                .HasColumnName("test_type_name_en")
                .HasColumnType("character varying");

                entity.Property(e => e.TestTypeCode)
               .HasColumnName("test_type_code")
               .HasColumnType("character varying");

                entity.Property(e => e.TestTypeId).HasColumnName("test_type_id");

                entity.Property(e => e.TestTypeMethodName)
               .HasColumnName("test_type_method_name")
               .HasColumnType("character varying");

                entity.Property(e => e.TestTypeMethodCode)
               .HasColumnName("test_type_method_code")
               .HasColumnType("character varying");

                entity.Property(e => e.TestTypeMethodId).HasColumnName("test_type_method_id");
            });

            builder.Entity<TransactionTestingSampling>(entity =>
            {
                entity.ToTable("transaction_testing_sampling", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreatedBy).IsRequired().HasColumnName("created_by");
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
                entity.Property(e => e.UpdatedBy).IsRequired().HasColumnName("updated_by");
                entity.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
                entity.Property(e => e.RowStatus).HasColumnName("row_status");
                entity.Property(e => e.SampleId).HasColumnName("sample_id");
                entity.Property(e => e.TestingId).HasColumnName("testing_id");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.Attachment).HasColumnName("attachment");
                entity.Property(e => e.TestingCode).HasColumnName("testing_code");
                entity.Property(e => e.SampleName).HasColumnName("sample_name");
            });


            builder.Entity<QcTransactionTemplateTestingType>(entity =>
                {
                    entity.ToTable("transaction_tmplt_testing_type", "transaction");
                    entity.Property(e => e.Id).HasColumnName("id");
                    entity.Property(e => e.Code).HasColumnName("code");
                    entity.Property(e => e.NameId).HasColumnName("name_id");
                    entity.Property(e => e.ObjectStatus).HasColumnName("object_status");
                    entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                    entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
                    entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                    entity.Property(e => e.NameEn).HasColumnName("name_en");
                    entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                    entity.Property(e => e.RowStatus).HasColumnName("row_status");
                    entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                });
            builder.Entity<QcTransactionTemplateTestingTypeMethod>(entity =>
            {
                entity.ToTable("transaction_tmplt_testing_type_method", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.RowStatus).HasColumnName("row_status");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity.Property(e => e.TestTypeId).HasColumnName("test_type_id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.StandardProcedureNumber).HasColumnName("standard_procedure_number");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.TestTypeCode).HasColumnName("test_type_code");
            });
            builder.Entity<QcTransactionTemplateTestingTypeMethodResultParameter>(entity =>
            {
                entity.ToTable("transaction_tmplt_testing_type_method_result_parameter", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.Existing).HasColumnName("existing");
                entity.Property(e => e.InputTypeId).HasColumnName("input_type_id");
                entity.Property(e => e.IsExisting).HasColumnName("is_existing");
                entity.Property(e => e.MethodCode).HasColumnName("method_code");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.NeedAttachment).HasColumnName("need_attachment");
                entity.Property(e => e.ParameterIdExisting).HasColumnName("parameter_id_existing");
                entity.Property(e => e.Properties).HasColumnName("properties");
                entity.Property(e => e.PropertiesValue).HasColumnName("properties_value");
                entity.Property(e => e.RowStatus).HasColumnName("row_status");
                entity.Property(e => e.Sequence).HasColumnName("sequence");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });
            builder.Entity<QcTransactionTemplateTestingTypeMethodValidationParameter>(entity =>
            {
                entity.ToTable("transaction_tmplt_testing_type_method_validation_parameter", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.NeedAttachment).HasColumnName("need_attachment");
                entity.Property(e => e.Properties).HasColumnName("properties");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity.Property(e => e.Value).HasColumnName("value");
                entity.Property(e => e.TestTypeMethodCode).HasColumnName("test_type_method_code");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.IsNullable).HasColumnName("is_nullable");
                entity.Property(e => e.InputTypeId).HasColumnName("input_type_id");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.Sequence).HasColumnName("sequence");
                entity.Property(e => e.TestTypeMethodId).HasColumnName("test_type_method_id");
                entity.Property(e => e.RowStatus).HasColumnName("row_status");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.IsInstruction).HasColumnName("is_instruction");
                entity.Property(e => e.AttachmentFile).HasColumnName("attachment_file");
                entity.Property(e => e.Instruction).HasColumnName("instruction");
                entity.Property(e => e.IsExisting).HasColumnName("is_existing");
                entity.Property(e => e.TestTypeMethodId).HasColumnName("test_type_process_procedure_parameter_id");
                entity.Property(e => e.ValidationResult).HasColumnName("validation_result");
                entity.Property(e => e.TestTypeProcessProcedureParameterCode).HasColumnName("test_type_process_procedure_parameter_code");
            });

            builder.Entity<TransactionHtrTestingProcedureParameter>(entity =>
            {
                entity.ToTable("transaction_htr_testing_procedure_parameter", "transaction_history");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.InputTypeId).HasColumnName("input_type_id");
                entity.Property(e => e.IsNullable).HasColumnName("isnullable");
                entity.Property(e => e.Properties).HasColumnName("properties").HasColumnType("jsonb");
                entity.Property(e => e.PropertiesValue).HasColumnName("properties_value").HasColumnType("jsonb");
                entity.Property(e => e.RowStatus).HasColumnName("row_status").HasColumnType("character varying");
                entity.Property(e => e.IsDeviation).HasColumnName("is_deviation");
                entity.Property(e => e.DeviationLevel).HasColumnName("deviation_level");
                entity.Property(e => e.DeviationNote).HasColumnName("deviation_note");
                entity.Property(e => e.Sequence).HasColumnName("sequence");
                entity.Property(e => e.HasAttachment).HasColumnName("has_attachment");
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).IsRequired().HasColumnName("created_by").HasColumnType("character varying");
                entity.Property(e => e.UpdatedBy).IsRequired().HasColumnName("updated_by").HasColumnType("character varying");
                entity.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
                entity.Property(e => e.ParameterId).IsRequired().HasColumnName("transaction_testing_procedure_parameter_id").HasColumnType("integer");
                entity.Property(e => e.ExecutorName).HasColumnName("executor_name").HasColumnType("character varying");
                entity.Property(e => e.ExecutorNik).HasColumnName("executor_nik").HasColumnType("character varying");
                entity.Property(e => e.ExecutorPosition).HasColumnName("executor_position").HasColumnType("character varying");
                entity.HasOne(p => p.Paramater).WithMany(d => d.Histories).HasForeignKey(p => p.ParameterId);
            });

            builder.Entity<TransactionHtrProcessProcedureParameterAttachment>(entity =>
            {
                entity.ToTable("transaction_htr_testing_procedure_parameter_attachment", "transaction_history");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .IsRequired();

                entity.Property(e => e.TestingProcedureParameterId)
                    .HasColumnName("testing_procedure_parameter_id")
                    .IsRequired();

                entity.Property(e => e.Filename)
                    .HasColumnName("filename")
                    .IsRequired();

                entity.Property(e => e.MediaLink)
                    .HasColumnName("media_link")
                    .IsRequired();

                entity.Property(e => e.Ext)
                    .HasColumnName("ext")
                    .IsRequired();

                entity.Property(e => e.Note)
                    .HasColumnName("note");

                entity.Property(e => e.Action)
                    .HasColumnName("action");

                entity.Property(e => e.ExecutorName)
                    .HasColumnName("executor_name");

                entity.Property(e => e.ExecutorPosition)
                    .HasColumnName("executor_position");

                entity.Property(e => e.ExecutorNik)
                    .HasColumnName("executor_nik");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .IsRequired();

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .IsRequired();

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status");
            });

            builder.Entity<TransactionTestTypeMethodValidationParameter>(entity =>
            {
                entity.ToTable("transaction_test_type_method_validation_parameter", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.NeedAttachment).HasColumnName("need_attachment");
                entity.Property(e => e.Properties).HasColumnName("properties");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.IsNullable).HasColumnName("is_nullable");
                entity.Property(e => e.InputTypeId).HasColumnName("input_type_id");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.Sequence).HasColumnName("sequence");
                entity.Property(e => e.TestTypeMethodCode).HasColumnName("test_type_method_code");
                entity.Property(e => e.RowStatus).HasColumnName("row_status");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.IsInstruction).HasColumnName("is_instruction");
                entity.Property(e => e.AttachmentFile).HasColumnName("attachment_file");
                entity.Property(e => e.Instruction).HasColumnName("instruction");
                entity.Property(e => e.IsExisting).HasColumnName("is_existing");
                entity.Property(e => e.TestTypeProcessProcedureParameterCode).HasColumnName("test_type_process_procedure_parameter_code");
                entity.Property(e => e.ValidationResult).HasColumnName("validation_result");
                entity.Property(e => e.TransactionTestingProcedureParameterId).HasColumnName("transaction_testing_procedure_parameter_id");
                entity.Property(e => e.TestingId).HasColumnName("testing_id");
            });
            builder.Entity<TransactionTestingTypeMethodResultparameter>(entity =>
            {
                entity.ToTable("transaction_testing_type_method_result_parameter", "transaction");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.Existing).HasColumnName("existing");
                entity.Property(e => e.InputTypeId).HasColumnName("input_type_id");
                entity.Property(e => e.IsExisting).HasColumnName("is_existing");
                entity.Property(e => e.MethodCode).HasColumnName("method_code");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.NeedAttachment).HasColumnName("need_attachment");
                entity.Property(e => e.ParameterIdExisting).HasColumnName("parameter_id_existing");
                entity.Property(e => e.Properties).HasColumnName("properties");
                entity.Property(e => e.PropertiesValue).HasColumnName("properties_value");
                entity.Property(e => e.RowStatus).HasColumnName("row_status");
                entity.Property(e => e.Sequence).HasColumnName("sequence");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
                entity.Property(e => e.TestingId).HasColumnName("testing_id");
                entity.Property(e => e.TransactionTestingProcedureParameterId).HasColumnName("transaction_testing_procedure_parameter_id");
            });
        }
    }
}
