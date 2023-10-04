using qcs_product.Auth.Authorization.Models;
using Microsoft.EntityFrameworkCore;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.Auth.Authorization.Infrastructure
{
    public partial class q100_authorizationContext : DbContext
    {
        public q100_authorizationContext()
        {
        }

        public q100_authorizationContext(DbContextOptions<q100_authorizationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Endpoint> Endpoint { get; set; }
        public virtual DbSet<PositionToRole> PositionToRole { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleToEndpoint> RoleToEndpoint { get; set; }
        public virtual DbSet<NowTimestamp> NowTimestamp { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("application", "auam");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplicationCode)
                    .IsRequired()
                    .HasColumnName("application_code")
                    .HasMaxLength(200);

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasColumnName("application_name")
                    .HasMaxLength(200);

                entity.Property(e => e.BeginDate)
                    .HasColumnName("begin_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(100);

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasDefaultValueSql("'9999-12-31 23:59:59'::timestamp without time zone");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Endpoint>(entity =>
            {
                entity.ToTable("endpoint", "auam");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplicationCode)
                    .IsRequired()
                    .HasColumnName("application_code")
                    .HasMaxLength(200);

                entity.Property(e => e.BeginDate)
                    .HasColumnName("begin_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(100);

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasDefaultValueSql("'9999-12-31 23:59:59'::timestamp without time zone");

                entity.Property(e => e.EndpointCode)
                    .IsRequired()
                    .HasColumnName("endpoint_code")
                    .HasMaxLength(200);

                entity.Property(e => e.EndpointName)
                    .IsRequired()
                    .HasColumnName("endpoint_name")
                    .HasMaxLength(200);

                entity.Property(e => e.EndpointPath)
                    .IsRequired()
                    .HasColumnName("endpoint_path")
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<PositionToRole>(entity =>
            {
                entity.ToTable("position_to_role", "auam");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplicationCode)
                    .IsRequired()
                    .HasColumnName("application_code")
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.PosId)
                    .IsRequired()
                    .HasColumnName("pos_id")
                    .HasMaxLength(200);

                entity.Property(e => e.RoleCode)
                    .IsRequired()
                    .HasColumnName("role_code")
                    .HasMaxLength(200);

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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role", "auam");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplicationCode)
                    .IsRequired()
                    .HasColumnName("application_code")
                    .HasMaxLength(200);

                entity.Property(e => e.BeginDate)
                    .HasColumnName("begin_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(100);

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasDefaultValueSql("'9999-12-31 23:59:59'::timestamp without time zone");

                entity.Property(e => e.RoleCode)
                    .IsRequired()
                    .HasColumnName("role_code")
                    .HasMaxLength(200);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<RoleToEndpoint>(entity =>
            {
                entity.ToTable("role_to_endpoint", "auam");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplicationCode)
                    .IsRequired()
                    .HasColumnName("application_code")
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(100);

                entity.Property(e => e.EndpointCode)
                    .IsRequired()
                    .HasColumnName("endpoint_code")
                    .HasMaxLength(200);

                entity.Property(e => e.RoleCode)
                    .IsRequired()
                    .HasColumnName("role_code")
                    .HasMaxLength(200);

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

            modelBuilder.Entity<NowTimestamp>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CurrentTimestamp)
                    .HasColumnName("current_timestamp");

                entity.ToView(nameof(NowTimestamp));
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
