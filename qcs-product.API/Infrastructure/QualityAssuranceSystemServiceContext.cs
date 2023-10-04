using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using qcs_product.API.MasterModels;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class QualityAssuranceSystemServiceContext : DbContext
    {
        public QualityAssuranceSystemServiceContext(DbContextOptions<QualityAssuranceSystemServiceContext> options) : base(options)
        {

        }

        public virtual DbSet<TestType> TestType { get; set; }
        public virtual DbSet<TestTypeMethod> TestTypeMethod { get; set; }
        public virtual DbSet<TestTypePersonnelQualification> TestTypePersonnelQualification { get; set; }
        public virtual DbSet<TestTypeProcess> TestTypeProcess { get; set; }
        public virtual DbSet<TestTypeProcessProcedure> TestTypeProcessProcedure { get; set; }
        public virtual DbSet<TestTypeProcessProcedureParameter> TestTypeProcessProcedureParameter { get; set; }
        public virtual DbSet<TestTypeProcessProcedureCondition> TestTypeProcessProcedureCondition { get; set; }
        public virtual DbSet<TestTypeMethodValidationInstruction> TestTypeMethodValidationInstruction { get; set; }
        public virtual DbSet<TestTypeMethodValidationParameter> TestTypeMethodValidationParameter { get; set; }
        public virtual DbSet<TestTypeMethodResultParameter> TestTypeMethodResultParameter { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TestType>(entity =>
            {
                entity.ToTable("test_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("character varying");

                entity.Property(e => e.NameId)
                    .HasColumnName("name_id")
                    .HasColumnType("character varying");

                entity.Property(e => e.NameEn)
                    .HasColumnName("name_en")
                    .HasColumnType("character varying");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id");

                entity.Property(e => e.ObjectStatus)
                    .HasColumnName("object_status");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

            });

            builder.Entity<TestTypeMethod>(entity =>
            {
                entity.ToTable("test_type_method");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.NameEn)
                    .HasColumnName("name_en")
                    .HasColumnType("character varying");

                entity.Property(e => e.StandardProcedureNumber)
                    .HasColumnName("standard_procedure_number")
                    .HasColumnType("character varying");

                entity.Property(e => e.TestTypeId).HasColumnName("test_type_id");
                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

            });

            builder.Entity<TestTypePersonnelQualification>(entity =>
            {
                entity.ToTable("test_type_personnel_qualification");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.TestTypeMethodId)
                    .HasColumnName("test_type_method_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.TestTypeId)
                    .HasColumnName("test_type_id");

            });

            builder.Entity<TestTypeMethodValidationInstruction>(entity =>
            {
                entity.ToTable("test_type_method_validation_instruction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TestTypeMethodId)
                .HasColumnName("test_type_method_id");
                entity.Property(e => e.TestTypeMethodName)
                .HasColumnName("test_type_method_name");
                entity.Property(e => e.TestTypeMethodCode)
                .HasColumnName("test_type_method_code");

                entity.Property(e => e.Order)
                .HasColumnName("order")
                .HasDefaultValueSql("1");

                entity.Property(e => e.OrderStatic)
                .HasColumnName("order_static")
                .HasDefaultValueSql("1");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.AttachmentUrl)
                .HasColumnName("attachment_url")
                .HasColumnType("character varying");

                entity.Property(e => e.Instructions)
                .HasColumnName("instruction")
                .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<TestTypeMethodValidationParameter>(entity =>
            {
                entity.ToTable("test_type_method_validation_parameter");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Order)
                .HasColumnName("order")
                .HasDefaultValueSql("1");
                entity.Property(e => e.OrderStatic)
                .HasColumnName("order_static")
                .HasDefaultValueSql("1");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.TestTypeMethodValidationInstructionId)
                .HasColumnName("test_type_method_validation_instruction_id");
                entity.Property(e => e.TestTypeMethodId)
                .HasColumnName("test_type_method_id");
                entity.Property(e => e.TestTypeMethodName)
                .HasColumnName("test_type_method_name");
                entity.Property(e => e.TestTypeMethodCode)
                .HasColumnName("test_type_method_code");
                entity.Property(e => e.TestTypeProcessPrecedureParameterId)
                .HasColumnName("test_type_process_procedure_parameter_id");
                entity.Property(e => e.TestTypeProcessPrecedureParameterCode)
                .HasColumnName("test_type_process_procedure_parameter_code");
                entity.Property(e => e.TestTypeProcessPrecedureParameterName)
                .HasColumnName("test_type_process_procedure_parameter_name");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<TestTypeProcess>(entity =>
            {
                entity.ToTable("test_type_process");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.MethodCode).HasColumnName("method_code");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.ParentTestTypeProcessId)
                    .HasColumnName("parent_test_type_process_id");

                entity.Property(e => e.IsEachSample)
                    .HasColumnName("is_each_sample")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.TestTypeMethodId)
                    .HasColumnName("test_type_method_id");

            });

            builder.Entity<TestTypeProcessProcedureParameter>(entity =>
            {
                entity.ToTable("test_type_process_procedure_parameter");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.InputTypeId).HasColumnName("input_type_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasColumnType("character varying");

                entity.Property(e => e.NeedAttachment).HasColumnName("need_attachment");

                entity.Property(e => e.IsNullable).HasColumnName("is_nullable");

                entity.Property(e => e.Properties)
                    .HasColumnName("properties")
                    .HasColumnType("jsonb");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.TestTypeProcessProcedureId).HasColumnName("test_type_process_procedure_id");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by");
            });

            builder.Entity<TestTypeProcessProcedure>(entity =>
            {
                entity.ToTable("test_type_process_procedure");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AttachmentFile)
                    .HasColumnName("attachment_file")
                    .HasColumnType("character varying");

                entity.Property(e => e.AttachmentStorageName)
                    .HasColumnName("attachment_storage_name");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.Instruction).HasColumnName("instruction");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.TestTypeProcessId)
                    .IsRequired()
                    .HasColumnName("test_type_process_id");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");
            });

            builder.Entity<TestTypeMethodResultParameter>(entity =>
            {
                entity.ToTable("test_type_method_result_parameter");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ParameterIdExisting).HasColumnName("parameter_id_existing");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.InputTypeId).HasColumnName("input_type_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name");

                entity.Property(e => e.IsExisting)
                    .HasColumnName("is_existing")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.NeedAttachment)
                    .HasColumnName("need_attachment")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Properties)
                    .HasColumnName("properties")
                    .HasColumnType("jsonb");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status")
                    .HasColumnType("character varying");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.MethodCode).HasColumnName("method_code");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by");
            });

            builder.Entity<TestTypeProcessProcedureCondition>(entity =>
            {
                entity.ToTable("test_type_process_procedure_condition");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.RowStatus)
                    .HasColumnName("row_status");

                entity.Property(e => e.TestTypeProcessProcedureId).HasColumnName("test_type_process_procedure_id");

                entity.Property(e => e.TestTypeProcessProcedureParameterId).HasColumnName("test_type_process_procedure_parameter_id");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasColumnType("character varying");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("jsonb");
            });



        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
    
}

