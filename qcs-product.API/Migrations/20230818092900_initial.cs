using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace qcs_product.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "transaction");

            migrationBuilder.CreateTable(
                name: "activity",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "authenticated_user_biohr",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authenticated_user_biohr", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "building",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    building_id = table.Column<string>(type: "character varying", nullable: false),
                    building_name = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_building", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "digital_signature",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    serial_number = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    nik = table.Column<string>(type: "character varying", nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    updated_by = table.Column<string>(type: "character varying", nullable: true),
                    begin_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_digital_signature", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "em_production_phase",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequence = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    parent_id = table.Column<int>(nullable: false),
                    qc_product = table.Column<string>(nullable: true),
                    qc_em = table.Column<string>(nullable: true),
                    room_id = table.Column<int>(nullable: false),
                    facility_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_em_production_phase", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "enum_constant",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_id = table.Column<int>(nullable: false),
                    key_group = table.Column<string>(maxLength: 50, nullable: true),
                    key_value_label = table.Column<string>(maxLength: 200, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enum_constant", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "facility",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    facility_code = table.Column<string>(type: "character varying", nullable: false),
                    facility_name = table.Column<string>(type: "character varying", nullable: false),
                    object_status = table.Column<int>(nullable: false),
                    organization_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facility", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "facility_room",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    facility_id = table.Column<int>(nullable: false),
                    room_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facility_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "form_section",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_id = table.Column<int>(nullable: false),
                    sequence = table.Column<int>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    icon = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_section", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grade_room",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_group_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    object_status = table.Column<int>(nullable: false),
                    grade_room_default = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grade_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "input_type",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_id = table.Column<int>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    reference = table.Column<string>(nullable: true),
                    reference_type = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_input_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item_batch_number",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_id = table.Column<int>(nullable: false),
                    object_status = table.Column<int>(nullable: false),
                    quantity = table.Column<int>(maxLength: 20, nullable: false),
                    batch_number = table.Column<string>(nullable: true),
                    expire_date = table.Column<DateTime>(nullable: false),
                    row_status = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_batch_number", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item_dosage_form",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_dosage_form_code = table.Column<string>(maxLength: 20, nullable: true),
                    item_dosage_form_name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_dosage_form", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_group_code = table.Column<string>(nullable: true),
                    item_group_name = table.Column<string>(nullable: true),
                    object_status = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item_presentation",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_presentation_code = table.Column<string>(maxLength: 20, nullable: true),
                    item_presentation_name = table.Column<string>(maxLength: 200, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    row_status = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_presentation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item_product_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_product_group_code = table.Column<string>(maxLength: 20, nullable: true),
                    item_product_group_name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_product_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_code = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    prod_form_id = table.Column<int>(nullable: false),
                    object_status = table.Column<int>(nullable: false),
                    temperature = table.Column<string>(maxLength: 10, nullable: true),
                    item_group_id = table.Column<int>(nullable: true),
                    item_group_name = table.Column<string>(nullable: true),
                    label_id = table.Column<int>(nullable: false),
                    org_id = table.Column<int>(nullable: false),
                    org_name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    product_group_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "microflora",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(nullable: true),
                    microba_category = table.Column<string>(nullable: true),
                    microba_id = table.Column<string>(nullable: true),
                    object_status = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_microflora", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organization",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    org_code = table.Column<string>(maxLength: 20, nullable: true),
                    biohr_organization_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "personal",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(maxLength: 50, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    nik = table.Column<string>(maxLength: 16, nullable: true),
                    org_id = table.Column<int>(nullable: false),
                    pos_id = table.Column<int>(nullable: false),
                    pin = table.Column<string>(maxLength: 100, nullable: true),
                    email = table.Column<string>(maxLength: 50, nullable: true),
                    no_handphone = table.Column<string>(maxLength: 50, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personal", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pos_code = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_forms",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    form_code = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_forms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_production_phases",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_prod_phase_code = table.Column<string>(maxLength: 20, nullable: true),
                    item_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    room_id = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_production_phases", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_production_phases_personel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_production_phases_id = table.Column<int>(nullable: false),
                    personel_nik = table.Column<string>(maxLength: 20, nullable: false),
                    personel_name = table.Column<string>(maxLength: 150, nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_production_phases_personel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_test_types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_id = table.Column<int>(nullable: false),
                    test_type_id = table.Column<int>(nullable: false),
                    sample_amount_count = table.Column<int>(nullable: false),
                    sample_amount_volume = table.Column<double>(nullable: false),
                    sample_amount_unit = table.Column<string>(maxLength: 10, nullable: true),
                    sample_amount_presentation = table.Column<string>(maxLength: 10, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_test_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "production_phases",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prod_phase_code = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_production_phases", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "purposes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purposes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "purposes_personel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    purpose_id = table.Column<int>(nullable: false),
                    nik = table.Column<string>(maxLength: 20, nullable: false),
                    personel_name = table.Column<string>(maxLength: 150, nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purposes_personel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_personel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    personel_code4 = table.Column<string>(nullable: true),
                    personel_code8 = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    initial = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_personel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_process",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequence = table.Column<int>(nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    parent_id = table.Column<int>(nullable: true),
                    room_id = table.Column<int>(nullable: true),
                    is_input_form = table.Column<int>(nullable: true),
                    add_sample_layout_type = table.Column<int>(nullable: true),
                    purpose_id = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_process", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_request_sampling",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_request_id = table.Column<int>(nullable: false),
                    type_sampling_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_request_sampling", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_request_type",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_request_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_result",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parent_id = table.Column<int>(nullable: true),
                    qc_sample_id = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    test_variabel_conclusion = table.Column<string>(nullable: true),
                    test_variabel_id = table.Column<int>(nullable: true),
                    note = table.Column<string>(nullable: true),
                    attchment_file = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_result", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_sampling",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_qcs_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    sampling_date_from = table.Column<DateTime>(nullable: true),
                    sampling_date_to = table.Column<DateTime>(nullable: true),
                    sampling_type_id = table.Column<int>(nullable: false),
                    sampling_type_name = table.Column<string>(nullable: true),
                    status = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    workflow_status = table.Column<string>(nullable: true),
                    receipt_date = table.Column<DateTime>(nullable: true),
                    attchment_file = table.Column<string>(nullable: true),
                    note = table.Column<string>(nullable: true),
                    shipment_note = table.Column<string>(nullable: true),
                    shipment_approval_date = table.Column<DateTime>(nullable: true),
                    shipment_approval_by = table.Column<string>(nullable: true),
                    product_date = table.Column<DateTime>(nullable: true),
                    product_method_id = table.Column<int>(nullable: true),
                    product_shipment_temperature = table.Column<string>(nullable: true),
                    product_shipment_date = table.Column<DateTime>(nullable: true),
                    product_data_logger = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_sampling", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_sampling_personel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_sampling_id = table.Column<int>(nullable: false),
                    product_production_phases_personel_id = table.Column<int>(nullable: true),
                    personel_nik = table.Column<string>(maxLength: 20, nullable: false),
                    personel_name = table.Column<string>(maxLength: 150, nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_sampling_personel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_sampling_shipment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_sampling_id = table.Column<int>(nullable: false),
                    qr_code = table.Column<string>(maxLength: 20, nullable: true),
                    no_request = table.Column<string>(maxLength: 25, nullable: true),
                    test_param_id = table.Column<int>(nullable: false),
                    test_param_name = table.Column<string>(nullable: true),
                    from_organization_id = table.Column<int>(nullable: true),
                    from_organization_name = table.Column<string>(nullable: true),
                    to_organization_id = table.Column<int>(nullable: true),
                    to_organization_name = table.Column<string>(nullable: true),
                    start_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true),
                    is_late_transfer = table.Column<bool>(nullable: false),
                    status = table.Column<int>(maxLength: 10, nullable: false),
                    row_status = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_sampling_shipment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_sampling_type",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_sampling_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(maxLength: 20, nullable: true),
                    qc_process_id = table.Column<int>(nullable: false),
                    qc_process_name = table.Column<string>(maxLength: 200, nullable: true),
                    test_date = table.Column<DateTime>(nullable: false),
                    personel_nik = table.Column<string>(nullable: true),
                    personel_name = table.Column<string>(maxLength: 200, nullable: true),
                    personel_pairing_nik = table.Column<string>(nullable: true),
                    personel_pairing_name = table.Column<string>(maxLength: 200, nullable: true),
                    workflow_status = table.Column<string>(maxLength: 50, nullable: true),
                    status = table.Column<int>(nullable: false),
                    status_proses = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_group_form_material",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_process_id = table.Column<int>(nullable: false),
                    sequence = table.Column<int>(nullable: false),
                    item_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    default_package_qty = table.Column<decimal>(nullable: false),
                    uom_package = table.Column<int>(nullable: false),
                    default_qty = table.Column<decimal>(nullable: false),
                    uom = table.Column<int>(nullable: false),
                    qc_process_id = table.Column<int>(nullable: false),
                    qc_transaction_group_section_id = table.Column<int>(nullable: false),
                    group_name = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_group_form_material", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_group_form_section",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    section_id = table.Column<int>(nullable: false),
                    section_type_id = table.Column<int>(nullable: false),
                    qc_process_id = table.Column<int>(nullable: false),
                    sequence = table.Column<int>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    icon = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_group_form_section", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_em_prod_phase_to_room",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    em_prod_phase_id = table.Column<int>(nullable: false),
                    room_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_em_prod_phase_to_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_grade_room_scenario",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grade_room_id = table.Column<int>(nullable: false),
                    test_scenario_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_grade_room_scenario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_item_test_scenario",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_id = table.Column<int>(nullable: false),
                    test_scenario_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_item_test_scenario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_items_item_presentation",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_id = table.Column<int>(maxLength: 20, nullable: false),
                    item_presentation_id = table.Column<int>(maxLength: 200, nullable: false),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    row_status = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_items_item_presentation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_product_prod_phase_to_room",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_production_phases_id = table.Column<int>(maxLength: 20, nullable: false),
                    room_id = table.Column<int>(maxLength: 200, nullable: false),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_product_prod_phase_to_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_room_sampling_point",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_purpose_id = table.Column<int>(nullable: true),
                    sampling_poin_id = table.Column<int>(nullable: false),
                    scenario_label = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_room_sampling_point", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_sampling_purpose_tool_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    purpose_id = table.Column<int>(nullable: false),
                    tool_group_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_sampling_purpose_tool_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_sampling_test_param",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sampling_point_id = table.Column<int>(nullable: false),
                    test_scenario_param_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_sampling_test_param", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_sampling_tool",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sampling_poin_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    tool_purpose_id = table.Column<int>(nullable: true),
                    scenario_label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_sampling_tool", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rel_test_scenario_param",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_parameter_id = table.Column<int>(nullable: false),
                    test_scenario_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rel_test_scenario_param", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "request_ahu",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_request_id = table.Column<int>(nullable: false),
                    ahu_id = table.Column<int>(nullable: false),
                    ahu_code = table.Column<string>(nullable: true),
                    ahu_name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_ahu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "request_group_room_purpose",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_id = table.Column<int>(nullable: false),
                    room_purpose_id = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_group_room_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "request_group_tool_purpose",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_id = table.Column<int>(nullable: false),
                    tool_purpose_id = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_group_tool_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "request_purposes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_request_id = table.Column<int>(nullable: false),
                    purpose_id = table.Column<int>(nullable: false),
                    purpose_code = table.Column<string>(nullable: true),
                    purpose_name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_purposes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "request_qcs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(nullable: false),
                    receipt_date = table.Column<DateTime>(nullable: true),
                    receipt_date_qa = table.Column<DateTime>(nullable: true),
                    receipt_date_kabag = table.Column<DateTime>(nullable: true),
                    no_request = table.Column<string>(nullable: true),
                    item_id = table.Column<int>(nullable: true),
                    item_name = table.Column<string>(nullable: true),
                    type_form_id = table.Column<int>(nullable: true),
                    type_form_name = table.Column<string>(nullable: true),
                    no_batch = table.Column<string>(nullable: true),
                    type_request = table.Column<string>(nullable: true),
                    product_form_id = table.Column<int>(nullable: true),
                    product_form_name = table.Column<string>(nullable: true),
                    product_group_id = table.Column<int>(nullable: true),
                    product_group_name = table.Column<string>(nullable: true),
                    product_presentation_id = table.Column<int>(nullable: true),
                    product_presentation_name = table.Column<string>(nullable: true),
                    product_phase_id = table.Column<int>(nullable: true),
                    product_phase_name = table.Column<string>(nullable: true),
                    product_temperature = table.Column<string>(nullable: true),
                    status = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    org_id = table.Column<int>(nullable: true),
                    org_name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(nullable: true),
                    storage_temperature_id = table.Column<int>(nullable: true),
                    storage_temperature_name = table.Column<string>(nullable: true),
                    purpose_id = table.Column<int>(nullable: true),
                    purpose_name = table.Column<string>(nullable: true),
                    em_room_id = table.Column<int>(nullable: true),
                    em_room_name = table.Column<string>(nullable: true),
                    em_room_grade_id = table.Column<int>(nullable: true),
                    em_room_grade_name = table.Column<string>(nullable: true),
                    workflow_status = table.Column<string>(nullable: true),
                    em_phase_id = table.Column<int>(nullable: true),
                    em_phase_name = table.Column<string>(nullable: true),
                    type_request_id = table.Column<int>(nullable: false),
                    test_scenario_id = table.Column<int>(nullable: true),
                    test_scenario_name = table.Column<string>(nullable: true),
                    test_scenario_label = table.Column<string>(nullable: true),
                    facility_id = table.Column<int>(nullable: true),
                    facility_code = table.Column<string>(nullable: true),
                    facility_name = table.Column<string>(nullable: true),
                    dev_number = table.Column<string>(nullable: true),
                    conclusion = table.Column<string>(nullable: true),
                    location = table.Column<string>(maxLength: 50, nullable: true),
                    process_date = table.Column<DateTime>(nullable: true),
                    process_id = table.Column<int>(nullable: true),
                    process_name = table.Column<string>(nullable: true),
                    item_temperature = table.Column<string>(maxLength: 20, nullable: true),
                    is_no_batch_editable = table.Column<bool>(nullable: false),
                    is_from_bulk_request = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_qcs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "request_room",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_request_id = table.Column<int>(nullable: false),
                    room_id = table.Column<int>(nullable: false),
                    room_code = table.Column<string>(nullable: true),
                    room_name = table.Column<string>(nullable: true),
                    ahu_id = table.Column<int>(nullable: true),
                    ahu_code = table.Column<string>(nullable: true),
                    ahu_name = table.Column<string>(nullable: true),
                    grade_room_id = table.Column<int>(nullable: true),
                    grade_room_code = table.Column<string>(nullable: true),
                    grade_room_name = table.Column<string>(nullable: true),
                    test_scenario_id = table.Column<int>(nullable: false),
                    test_scenario_name = table.Column<string>(nullable: true),
                    test_scenario_label = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "room",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grade_room_id = table.Column<int>(nullable: false),
                    organization_id = table.Column<int>(nullable: false),
                    building_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    pos_id = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    floor = table.Column<string>(type: "character varying", nullable: true),
                    temperature_operator = table.Column<int>(nullable: true),
                    temperature_value = table.Column<decimal>(nullable: true),
                    temperature_value_to = table.Column<decimal>(nullable: true),
                    temperature_value_from = table.Column<decimal>(nullable: true),
                    humidity_operator = table.Column<int>(nullable: true),
                    humidity_value = table.Column<decimal>(nullable: true),
                    humidity_value_to = table.Column<decimal>(nullable: true),
                    humidity_value_from = table.Column<decimal>(nullable: true),
                    pressure_operator = table.Column<int>(nullable: true),
                    pressure_value = table.Column<decimal>(nullable: true),
                    pressure_value_to = table.Column<decimal>(nullable: true),
                    pressure_value_from = table.Column<decimal>(nullable: true),
                    air_change_operator = table.Column<int>(nullable: true),
                    air_change_value = table.Column<decimal>(nullable: true),
                    air_change_value_to = table.Column<decimal>(nullable: true),
                    air_change_value_from = table.Column<decimal>(nullable: true),
                    ahu = table.Column<int>(nullable: true),
                    area = table.Column<int>(nullable: true),
                    object_status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "room_purpose",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "room_purpose_to_master_purpose",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_purpose_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true),
                    purpose_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_purpose_to_master_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "room_sampling_point_layout",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_purpose_id = table.Column<int>(nullable: false),
                    attachment_file = table.Column<string>(nullable: true),
                    file_name = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    row_status = table.Column<string>(nullable: true),
                    file_type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_sampling_point_layout", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sampling_point",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<int>(nullable: true),
                    tool_id = table.Column<int>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sampling_point", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sampling_test_parameter",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sampling_point_id = table.Column<int>(nullable: false),
                    test_parameter_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sampling_test_parameter", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "storage_temperature",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    item_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    treshold_operator = table.Column<string>(nullable: true),
                    treshold_value = table.Column<double>(nullable: false),
                    treshold_min = table.Column<double>(nullable: false),
                    treshold_max = table.Column<double>(nullable: false),
                    row_status = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storage_temperature", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "test_parameter",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_group_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    short_name = table.Column<string>(nullable: true),
                    sequence = table.Column<int>(nullable: false),
                    organization_id = table.Column<int>(nullable: false),
                    qc_process_id = table.Column<int>(nullable: true),
                    row_status = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_parameter", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "test_scenario",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    label = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_scenario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "test_types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    org_id = table.Column<int>(nullable: false),
                    org_name = table.Column<string>(nullable: true),
                    test_type_code = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "test_variable",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_parameter_id = table.Column<int>(nullable: false),
                    variable_name = table.Column<string>(nullable: true),
                    treshold_operator = table.Column<int>(nullable: false),
                    treshold_value = table.Column<long>(nullable: true),
                    threshold_value_to = table.Column<long>(nullable: true),
                    threshold_value_from = table.Column<long>(nullable: true),
                    sequence = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_variable", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tool_code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    tool_group_id = table.Column<int>(nullable: false),
                    room_id = table.Column<int>(nullable: false),
                    grade_room_id = table.Column<int>(nullable: false),
                    facility_id = table.Column<int>(nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    serial_number_id = table.Column<string>(maxLength: 20, nullable: true),
                    machine_id = table.Column<int>(nullable: true),
                    object_status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool_activity",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tool_id = table.Column<int>(nullable: false),
                    activity_id = table.Column<int>(nullable: false),
                    activity_code = table.Column<string>(nullable: true),
                    activity_date = table.Column<DateTime>(nullable: false),
                    expired_date = table.Column<DateTime>(nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_activity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    label = table.Column<string>(nullable: true),
                    object_status = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool_purpose",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tool_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool_purpose_to_master_purpose",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tool_purpose_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true),
                    purpose_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_purpose_to_master_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool_sampling_point_layout",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    attachment_file = table.Column<string>(type: "character varying", nullable: false),
                    file_name = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true),
                    file_type = table.Column<string>(type: "character varying", nullable: true),
                    tool_purpose_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_sampling_point_layout", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "type_forms",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_form_code = table.Column<string>(maxLength: 20, nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_type_forms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "uom",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uom_id = table.Column<int>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uom", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserTestings",
                columns: table => new
                {
                    id2 = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    jenis_kelamin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTestings", x => x.id2);
                });

            migrationBuilder.CreateTable(
                name: "workflow_history",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workflow_document_code = table.Column<string>(nullable: true),
                    action = table.Column<string>(maxLength: 50, nullable: true),
                    note = table.Column<string>(maxLength: 200, nullable: true),
                    workflow_status = table.Column<string>(nullable: true),
                    pic_nik = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_qc_sampling",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_sampling_id = table.Column<int>(nullable: false),
                    is_in_workflow = table.Column<bool>(nullable: false),
                    workflow_status = table.Column<string>(nullable: true),
                    workflow_document_code = table.Column<string>(nullable: true),
                    workflow_code = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_qc_sampling", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_activity",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(maxLength: 20, nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_activity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_batch",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_qcs_id = table.Column<int>(nullable: false),
                    attachment_notes = table.Column<string>(maxLength: 200, nullable: true),
                    created_by = table.Column<string>(type: "character varying", nullable: true),
                    created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("transaction_batch_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_batch_attachments",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trs_batch_id = table.Column<int>(nullable: false),
                    title = table.Column<string>(maxLength: 200, nullable: true),
                    attachment_file = table.Column<string>(type: "character varying", nullable: true),
                    attachment_storage_name = table.Column<string>(nullable: true),
                    file_name = table.Column<string>(type: "character varying", nullable: true),
                    file_type = table.Column<string>(maxLength: 20, nullable: true),
                    created_by = table.Column<string>(type: "character varying", nullable: true),
                    created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("transaction_batch_attachments_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_batch_lines",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trs_batch_id = table.Column<int>(nullable: false),
                    item_id = table.Column<int>(nullable: false),
                    item_name = table.Column<string>(maxLength: 200, nullable: true),
                    no_batch = table.Column<string>(maxLength: 100, nullable: true),
                    notes = table.Column<string>(type: "character varying", nullable: true),
                    created_by = table.Column<string>(type: "character varying", nullable: true),
                    created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("transaction_batch_lines_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_building",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    building_id = table.Column<string>(type: "character varying", nullable: false),
                    building_name = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_building", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_grade_room",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_group_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(maxLength: 50, nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    grade_room_identification_test = table.Column<bool>(nullable: true),
                    grade_room_default = table.Column<string>(type: "character varying", nullable: true),
                    object_status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_grade_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_organization",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    org_code = table.Column<string>(maxLength: 20, nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    created_by = table.Column<string>(maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    biohr_organization_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_organization", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_purposes",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(maxLength: 20, nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_purposes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_room_purpose",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_room_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_room_purpose_to_master_purpose",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_purpose_id = table.Column<int>(nullable: false),
                    purpose_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_room_purpose_to_master_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_room_sampling_point_layout",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<int>(nullable: false),
                    attachment_file = table.Column<string>(type: "character varying", nullable: false),
                    file_name = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    file_type = table.Column<string>(type: "character varying", nullable: true),
                    room_purpose_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_room_sampling_point_layout", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_sampling_point",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<int>(nullable: true),
                    tool_id = table.Column<int>(nullable: true),
                    code = table.Column<string>(maxLength: 50, nullable: false),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_sampling_point", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_test_scenario",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    label = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_test_scenario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_tool_group",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(maxLength: 20, nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    label = table.Column<string>(maxLength: 200, nullable: false),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_tool_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_tool_purpose",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tool_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_tool_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_tool_purpose_to_master_purpose",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tool_purpose_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    purpose_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_tool_purpose_to_master_purpose", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_tool_sampling_point_layout",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    attachment_file = table.Column<string>(type: "character varying", nullable: false),
                    file_name = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    file_type = table.Column<string>(type: "character varying", nullable: true),
                    tool_purpose_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_tool_sampling_point_layout", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "form_material",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequence = table.Column<int>(nullable: false),
                    item_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    batch_number = table.Column<string>(nullable: true),
                    default_package_qty = table.Column<decimal>(type: "numeric", nullable: false),
                    uom_package = table.Column<int>(nullable: false),
                    default_qty = table.Column<decimal>(type: "numeric", nullable: false),
                    uom = table.Column<int>(nullable: false),
                    process_id = table.Column<int>(nullable: false),
                    section_id = table.Column<int>(nullable: false),
                    group_name = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_material", x => x.id);
                    table.ForeignKey(
                        name: "fk_qc_process",
                        column: x => x.process_id,
                        principalTable: "qc_process",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_procedure",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequence = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    process_id = table.Column<int>(nullable: false),
                    section_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_procedure", x => x.id);
                    table.ForeignKey(
                        name: "fk_qc_process",
                        column: x => x.process_id,
                        principalTable: "qc_process",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_tool",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequence = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    tool_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    item_id = table.Column<int>(nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    process_id = table.Column<int>(nullable: false),
                    section_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    updated_by = table.Column<string>(type: "character varying", nullable: false),
                    row_status = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_tool", x => x.id);
                    table.ForeignKey(
                        name: "fk_qc_process",
                        column: x => x.process_id,
                        principalTable: "qc_process",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qc_sample",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_sampling_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    parent_id = table.Column<int>(nullable: true),
                    sample_sequence = table.Column<int>(nullable: true),
                    sampling_point_id = table.Column<int>(nullable: true),
                    sampling_point_code = table.Column<string>(nullable: true),
                    grade_room_id = table.Column<int>(nullable: true),
                    grade_room_name = table.Column<string>(nullable: true),
                    tool_id = table.Column<int>(nullable: true),
                    tool_code = table.Column<string>(nullable: true),
                    tool_name = table.Column<string>(nullable: true),
                    tool_gorup_id = table.Column<int>(nullable: true),
                    tool_group_name = table.Column<string>(nullable: true),
                    tool_group_label = table.Column<string>(nullable: true),
                    test_param_id = table.Column<int>(nullable: false),
                    test_param_name = table.Column<string>(nullable: true),
                    personal_id = table.Column<int>(nullable: true),
                    personal_initial = table.Column<string>(nullable: true),
                    personal_name = table.Column<string>(nullable: true),
                    sampling_date_time_from = table.Column<DateTime>(nullable: true),
                    sampling_date_time_to = table.Column<DateTime>(nullable: true),
                    particle_volume = table.Column<decimal>(nullable: true),
                    attchment_file = table.Column<string>(nullable: true),
                    note = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    test_scenario_id = table.Column<int>(nullable: true),
                    review_qa_note = table.Column<string>(nullable: true),
                    is_default = table.Column<bool>(nullable: false),
                    qc_sampling_tools_id = table.Column<int>(nullable: true),
                    qc_sampling_materials_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_sample", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_sample_qc_sampling_qc_sampling_id",
                        column: x => x.qc_sampling_id,
                        principalTable: "qc_sampling",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qc_sampling_attachment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_sampling_id = table.Column<int>(nullable: false),
                    attachment_file_name = table.Column<string>(nullable: true),
                    attachment_file_link = table.Column<string>(nullable: true),
                    attachment_storage_name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_sampling_attachment", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_sampling_attachment_qc_sampling_qc_sampling_id",
                        column: x => x.qc_sampling_id,
                        principalTable: "qc_sampling",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qc_sampling_material",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_sampling_id = table.Column<int>(nullable: false),
                    item_id = table.Column<int>(nullable: false),
                    item_name = table.Column<string>(nullable: true),
                    item_batch_id = table.Column<int>(nullable: true),
                    no_batch = table.Column<string>(nullable: true),
                    expire_date = table.Column<DateTime>(nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_sampling_material", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_sampling_material_qc_sampling_qc_sampling_id",
                        column: x => x.qc_sampling_id,
                        principalTable: "qc_sampling",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qc_sampling_tools",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_sampling_id = table.Column<int>(nullable: false),
                    tool_id = table.Column<int>(nullable: false),
                    tool_code = table.Column<string>(nullable: true),
                    tool_name = table.Column<string>(nullable: true),
                    tool_group_id = table.Column<int>(nullable: false),
                    tool_group_name = table.Column<string>(nullable: true),
                    tool_group_label = table.Column<string>(nullable: true),
                    ed_validation = table.Column<DateTime>(nullable: true),
                    ed_calibration = table.Column<DateTime>(nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_sampling_tools", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_sampling_tools_qc_sampling_qc_sampling_id",
                        column: x => x.qc_sampling_id,
                        principalTable: "qc_sampling",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qc_sampling_shipment_tracker",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_sampling_shipment_id = table.Column<int>(nullable: true),
                    qr_code = table.Column<string>(maxLength: 20, nullable: true),
                    type = table.Column<string>(nullable: true),
                    processed_at = table.Column<DateTime>(nullable: true),
                    user_nik = table.Column<string>(nullable: true),
                    user_name = table.Column<string>(nullable: true),
                    organization_id = table.Column<int>(nullable: true),
                    organization_name = table.Column<string>(nullable: true),
                    row_status = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_sampling_shipment_tracker", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_sampling_shipment_tracker_qc_sampling_shipment_qc_sampli~",
                        column: x => x.qc_sampling_shipment_id,
                        principalTable: "qc_sampling_shipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_group_process",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_id = table.Column<int>(nullable: false),
                    sequence = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    parent_id = table.Column<int>(nullable: true),
                    room_id = table.Column<int>(nullable: true),
                    is_input_form = table.Column<int>(nullable: true),
                    qc_process_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_group_process", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_transaction_group_process_qc_transaction_group_qc_transa~",
                        column: x => x.qc_transaction_group_id,
                        principalTable: "qc_transaction_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_sample",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_id = table.Column<int>(nullable: false),
                    qc_transaction_sampling_id = table.Column<int>(nullable: true),
                    qc_sampling_id = table.Column<int>(nullable: true),
                    qc_sample_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_sample", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_transaction_sample_qc_transaction_group_qc_transaction_g~",
                        column: x => x.qc_transaction_group_id,
                        principalTable: "qc_transaction_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_sampling",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_id = table.Column<int>(nullable: false),
                    qc_sampling_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_sampling", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_transaction_sampling_qc_transaction_group_qc_transaction~",
                        column: x => x.qc_transaction_group_id,
                        principalTable: "qc_transaction_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_qc_transaction_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_id = table.Column<int>(nullable: false),
                    workflow_status = table.Column<string>(maxLength: 50, nullable: true),
                    workflow_document_code = table.Column<string>(maxLength: 20, nullable: true),
                    workflow_code = table.Column<string>(maxLength: 20, nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    is_in_workflow = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_qc_transaction_group", x => x.id);
                    table.ForeignKey(
                        name: "fk__qc_transaction_group",
                        column: x => x.qc_transaction_group_id,
                        principalTable: "qc_transaction_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_test_type_qcs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_qcs_id = table.Column<int>(nullable: false),
                    test_type_id = table.Column<int>(nullable: false),
                    test_type_name = table.Column<string>(maxLength: 70, nullable: true),
                    test_parameter_id = table.Column<int>(nullable: false),
                    test_parameter_name = table.Column<string>(maxLength: 70, nullable: true),
                    sample_amount_count = table.Column<int>(nullable: false),
                    sample_amount_volume = table.Column<double>(nullable: false),
                    sample_amount_unit = table.Column<string>(maxLength: 10, nullable: true),
                    sample_amount_presentation = table.Column<string>(maxLength: 10, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    org_id = table.Column<int>(nullable: false),
                    org_name = table.Column<string>(maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_test_type_qcs", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_test_type_qcs_request_qcs_request_qcs_id",
                        column: x => x.request_qcs_id,
                        principalTable: "request_qcs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_facility",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    facility_code = table.Column<string>(type: "character varying", nullable: false),
                    facility_name = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false),
                    organization_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_facility", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_facility_organization",
                        column: x => x.organization_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_room",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grade_room_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(maxLength: 50, nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    organization_id = table.Column<int>(nullable: true),
                    floor = table.Column<string>(type: "character varying", nullable: true),
                    area = table.Column<int>(nullable: true),
                    temperature_operator = table.Column<int>(nullable: true),
                    temperature_value = table.Column<decimal>(type: "numeric", nullable: true),
                    temperature_value_to = table.Column<decimal>(type: "numeric", nullable: true),
                    temperature_value_from = table.Column<decimal>(type: "numeric", nullable: true),
                    humidity_operator = table.Column<int>(nullable: true),
                    humidity_value = table.Column<decimal>(type: "numeric", nullable: true),
                    humidity_value_to = table.Column<decimal>(type: "numeric", nullable: true),
                    humidity_value_from = table.Column<decimal>(type: "numeric", nullable: true),
                    pressure_operator = table.Column<int>(nullable: true),
                    pressure_value = table.Column<decimal>(type: "numeric", nullable: true),
                    pressure_value_to = table.Column<decimal>(type: "numeric", nullable: true),
                    pressure_value_from = table.Column<decimal>(type: "numeric", nullable: true),
                    air_change_operator = table.Column<int>(nullable: true),
                    air_change_value = table.Column<decimal>(type: "numeric", nullable: true),
                    air_change_value_to = table.Column<decimal>(type: "numeric", nullable: true),
                    air_change_value_from = table.Column<decimal>(type: "numeric", nullable: true),
                    ahu = table.Column<int>(nullable: true),
                    building_id = table.Column<int>(nullable: true),
                    object_status = table.Column<int>(nullable: true, defaultValueSql: "3"),
                    pos_id = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_room", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_room_1",
                        column: x => x.organization_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_test_parameter",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_group_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    sequence = table.Column<int>(nullable: false),
                    organization_id = table.Column<int>(nullable: true),
                    qc_process_id = table.Column<int>(nullable: true),
                    short_name = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_test_parameter", x => x.id);
                    table.ForeignKey(
                        name: "transaction_test_parameter_fk2",
                        column: x => x.organization_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transaction_test_parameter_qc_process_qc_process_id",
                        column: x => x.qc_process_id,
                        principalTable: "qc_process",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transaction_test_parameter_enum_constant_test_group_id",
                        column: x => x.test_group_id,
                        principalTable: "enum_constant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_rel_room_sampling_point",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<int>(nullable: false),
                    sampling_poin_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    room_purpose_id = table.Column<int>(nullable: true),
                    scenario_label = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_rel_room_sampling_point", x => x.id);
                    table.ForeignKey(
                        name: "transaction_rel_room_sampling_point_fk_1",
                        column: x => x.sampling_poin_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_sampling_point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_rel_sampling_tool",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sampling_poin_id = table.Column<int>(nullable: false),
                    tool_id = table.Column<int>(nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    tool_purpose_id = table.Column<int>(nullable: true),
                    scenario_label = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_rel_sampling_tool", x => x.id);
                    table.ForeignKey(
                        name: "transaction_rel_sampling_tool_fk",
                        column: x => x.sampling_poin_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_sampling_point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_rel_grade_room_scenario",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grade_room_id = table.Column<int>(nullable: false),
                    test_scenario_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_rel_grade_room_scenario", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_grade_room",
                        column: x => x.grade_room_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_grade_room",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transaction_test_scenario",
                        column: x => x.test_scenario_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_test_scenario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_rel_sampling_purpose_tool_group",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    purpose_id = table.Column<int>(nullable: false),
                    tool_group_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_rel_sampling_purpose_tool_group", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_rel_sampling_purpose_tool_group_transaction_pur~",
                        column: x => x.purpose_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_purposes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_rel_sampling_purpose_tool_group_transaction_too~",
                        column: x => x.tool_group_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_tool_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_tool",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tool_code = table.Column<string>(maxLength: 20, nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    tool_group_id = table.Column<int>(nullable: false),
                    room_id = table.Column<int>(nullable: true),
                    grade_room_id = table.Column<int>(nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    serial_number_id = table.Column<string>(maxLength: 20, nullable: true),
                    machine_id = table.Column<int>(nullable: true),
                    organization_id = table.Column<int>(nullable: true),
                    facility_id = table.Column<int>(nullable: true, defaultValueSql: "0"),
                    object_status = table.Column<int>(nullable: true, defaultValueSql: "3")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_tool", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_tool_transaction_tool_group_tool_group_id",
                        column: x => x.tool_group_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_tool_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_parameter",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequence = table.Column<int>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    input_type = table.Column<int>(nullable: false),
                    uom = table.Column<int>(nullable: true),
                    threshold_operator = table.Column<int>(nullable: true),
                    threshold_value = table.Column<decimal>(nullable: true),
                    threshold_value_to = table.Column<decimal>(nullable: true),
                    threshold_value_from = table.Column<decimal>(nullable: true),
                    need_attachment = table.Column<bool>(nullable: false),
                    note = table.Column<string>(nullable: true),
                    procedure_id = table.Column<int>(nullable: false),
                    is_for_all_sample = table.Column<bool>(nullable: false),
                    is_result = table.Column<bool>(nullable: false),
                    default_value = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 100, nullable: true),
                    row_status = table.Column<string>(nullable: true),
                    FormProcedureId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_parameter", x => x.id);
                    table.ForeignKey(
                        name: "FK_form_parameter_form_procedure_FormProcedureId",
                        column: x => x.FormProcedureId,
                        principalTable: "form_procedure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_group_form_procedure",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_process_id = table.Column<int>(nullable: false),
                    sequence = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    form_procedure_id = table.Column<int>(nullable: false),
                    qc_transaction_group_section_id = table.Column<int>(nullable: false),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_group_form_procedure", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_transaction_group_form_procedure_qc_transaction_group_pr~",
                        column: x => x.qc_transaction_group_process_id,
                        principalTable: "qc_transaction_group_process",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_group_form_tool",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_process_id = table.Column<int>(nullable: false),
                    sequence = table.Column<int>(nullable: false),
                    tools_id = table.Column<int>(nullable: false),
                    item_id = table.Column<int>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    quantity = table.Column<decimal>(nullable: false),
                    qc_process_id = table.Column<int>(nullable: false),
                    qc_transaction_group_section_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_group_form_tool", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_transaction_group_form_tool_qc_transaction_group_process~",
                        column: x => x.qc_transaction_group_process_id,
                        principalTable: "qc_transaction_group_process",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_facility_room",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    facility_id = table.Column<int>(nullable: false),
                    room_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_facility_room", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_facility_room",
                        column: x => x.facility_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_facility",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "transaction_facility_room_fk",
                        column: x => x.room_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_room",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_rel_test_scenario_param",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_parameter_id = table.Column<int>(nullable: false),
                    test_scenario_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_rel_test_scenario_param", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_test_parameter",
                        column: x => x.test_parameter_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_test_parameter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transaction_test_scenario",
                        column: x => x.test_scenario_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_test_scenario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_tool_activity",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tool_id = table.Column<int>(nullable: false),
                    activity_id = table.Column<int>(nullable: false),
                    activity_code = table.Column<string>(maxLength: 20, nullable: false),
                    activity_date = table.Column<DateTime>(type: "date", nullable: false),
                    expired_date = table.Column<DateTime>(type: "date", nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_tool_activity", x => x.id);
                    table.ForeignKey(
                        name: "transaction_tool_activity_fk2",
                        column: x => x.activity_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_activity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "transaction_tool_activity_fk",
                        column: x => x.tool_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_tool",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_group_form_parameter",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_form_procedure_id = table.Column<int>(nullable: false),
                    sequence = table.Column<int>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    input_type = table.Column<int>(nullable: false),
                    uom = table.Column<int>(nullable: true),
                    threshold_operator = table.Column<int>(nullable: true),
                    threshold_value = table.Column<decimal>(nullable: true),
                    threshold_value_to = table.Column<decimal>(nullable: true),
                    threshold_value_from = table.Column<decimal>(nullable: true),
                    need_attachment = table.Column<bool>(nullable: false),
                    note = table.Column<string>(nullable: true),
                    form_procedure_id = table.Column<int>(nullable: false),
                    is_for_all_sample = table.Column<bool>(nullable: false),
                    is_result = table.Column<bool>(nullable: false),
                    default_value = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_group_form_parameter", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_transaction_group_form_parameter_qc_transaction_group_fo~",
                        column: x => x.qc_transaction_group_form_procedure_id,
                        principalTable: "qc_transaction_group_form_procedure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_rel_sampling_test_param",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sampling_point_id = table.Column<int>(nullable: false),
                    test_scenario_param_id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_rel_sampling_test_param", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_sampling_point",
                        column: x => x.sampling_point_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_sampling_point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transaction_test_scenario_param",
                        column: x => x.test_scenario_param_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_rel_test_scenario_param",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_test_variable",
                schema: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_parameter_id = table.Column<int>(nullable: false),
                    treshold_operator = table.Column<int>(nullable: false),
                    treshold_value = table.Column<decimal>(type: "numeric", nullable: true),
                    threshold_value_to = table.Column<decimal>(type: "numeric", nullable: true),
                    threshold_value_from = table.Column<decimal>(type: "numeric", nullable: true),
                    row_status = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<string>(maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 150, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    sequence = table.Column<int>(nullable: true),
                    variable_name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_test_variable", x => x.id);
                    table.ForeignKey(
                        name: "transaction_test_variable_fk",
                        column: x => x.test_parameter_id,
                        principalSchema: "transaction",
                        principalTable: "transaction_rel_test_scenario_param",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_group_sample_value",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_form_parameter_id = table.Column<int>(nullable: false),
                    qc_transaction_sample_id = table.Column<int>(nullable: false),
                    sequence = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    attachment = table.Column<string>(nullable: true),
                    qc_transaction_group_form_material_id = table.Column<int>(nullable: true),
                    qc_transaction_group_form_tool_id = table.Column<int>(nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_group_sample_value", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_transaction_group_sample_value_qc_transaction_group_form~",
                        column: x => x.qc_transaction_group_form_parameter_id,
                        principalTable: "qc_transaction_group_form_parameter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qc_transaction_group_value",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qc_transaction_group_form_parameter_id = table.Column<int>(nullable: false),
                    sequence = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    attachment = table.Column<string>(nullable: true),
                    qc_transaction_group_form_material_id = table.Column<int>(nullable: true),
                    qc_transaction_group_form_tool_id = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_by = table.Column<string>(maxLength: 50, nullable: true),
                    row_status = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_transaction_group_value", x => x.id);
                    table.ForeignKey(
                        name: "FK_qc_transaction_group_value_qc_transaction_group_form_parame~",
                        column: x => x.qc_transaction_group_form_parameter_id,
                        principalTable: "qc_transaction_group_form_parameter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_form_material_process_id",
                table: "form_material",
                column: "process_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_parameter_FormProcedureId",
                table: "form_parameter",
                column: "FormProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_form_procedure_process_id",
                table: "form_procedure",
                column: "process_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_tool_process_id",
                table: "form_tool",
                column: "process_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_test_type_qcs_request_qcs_id",
                table: "product_test_type_qcs",
                column: "request_qcs_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_sample_qc_sampling_id",
                table: "qc_sample",
                column: "qc_sampling_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_sampling_attachment_qc_sampling_id",
                table: "qc_sampling_attachment",
                column: "qc_sampling_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_sampling_material_qc_sampling_id",
                table: "qc_sampling_material",
                column: "qc_sampling_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_sampling_shipment_tracker_qc_sampling_shipment_id",
                table: "qc_sampling_shipment_tracker",
                column: "qc_sampling_shipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_sampling_tools_qc_sampling_id",
                table: "qc_sampling_tools",
                column: "qc_sampling_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_transaction_group_form_parameter_qc_transaction_group_fo~",
                table: "qc_transaction_group_form_parameter",
                column: "qc_transaction_group_form_procedure_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_transaction_group_form_procedure_qc_transaction_group_pr~",
                table: "qc_transaction_group_form_procedure",
                column: "qc_transaction_group_process_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_transaction_group_form_tool_qc_transaction_group_process~",
                table: "qc_transaction_group_form_tool",
                column: "qc_transaction_group_process_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_transaction_group_process_qc_transaction_group_id",
                table: "qc_transaction_group_process",
                column: "qc_transaction_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_transaction_group_sample_value_qc_transaction_group_form~",
                table: "qc_transaction_group_sample_value",
                column: "qc_transaction_group_form_parameter_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_transaction_group_value_qc_transaction_group_form_parame~",
                table: "qc_transaction_group_value",
                column: "qc_transaction_group_form_parameter_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_transaction_sample_qc_transaction_group_id",
                table: "qc_transaction_sample",
                column: "qc_transaction_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_qc_transaction_sampling_qc_transaction_group_id",
                table: "qc_transaction_sampling",
                column: "qc_transaction_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_qc_transaction_group_qc_transaction_group_id",
                table: "workflow_qc_transaction_group",
                column: "qc_transaction_group_id");

            migrationBuilder.CreateIndex(
                name: "transaction_building_idx_building_on_building_id",
                schema: "transaction",
                table: "transaction_building",
                column: "building_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_facility_organization_id",
                schema: "transaction",
                table: "transaction_facility",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_facility_room_facility_id",
                schema: "transaction",
                table: "transaction_facility_room",
                column: "facility_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_facility_room_room_id",
                schema: "transaction",
                table: "transaction_facility_room",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_grade_room_scenario_grade_room_id",
                schema: "transaction",
                table: "transaction_rel_grade_room_scenario",
                column: "grade_room_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_grade_room_scenario_test_scenario_id",
                schema: "transaction",
                table: "transaction_rel_grade_room_scenario",
                column: "test_scenario_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_room_sampling_point_sampling_poin_id",
                schema: "transaction",
                table: "transaction_rel_room_sampling_point",
                column: "sampling_poin_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_sampling_purpose_tool_group_purpose_id",
                schema: "transaction",
                table: "transaction_rel_sampling_purpose_tool_group",
                column: "purpose_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_sampling_purpose_tool_group_tool_group_id",
                schema: "transaction",
                table: "transaction_rel_sampling_purpose_tool_group",
                column: "tool_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_sampling_test_param_sampling_point_id",
                schema: "transaction",
                table: "transaction_rel_sampling_test_param",
                column: "sampling_point_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_sampling_test_param_test_scenario_param_id",
                schema: "transaction",
                table: "transaction_rel_sampling_test_param",
                column: "test_scenario_param_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_sampling_tool_sampling_poin_id",
                schema: "transaction",
                table: "transaction_rel_sampling_tool",
                column: "sampling_poin_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_test_scenario_param_test_parameter_id",
                schema: "transaction",
                table: "transaction_rel_test_scenario_param",
                column: "test_parameter_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_rel_test_scenario_param_test_scenario_id",
                schema: "transaction",
                table: "transaction_rel_test_scenario_param",
                column: "test_scenario_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_room_organization_id",
                schema: "transaction",
                table: "transaction_room",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_test_parameter_organization_id",
                schema: "transaction",
                table: "transaction_test_parameter",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_test_parameter_qc_process_id",
                schema: "transaction",
                table: "transaction_test_parameter",
                column: "qc_process_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_test_parameter_test_group_id",
                schema: "transaction",
                table: "transaction_test_parameter",
                column: "test_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_test_variable_test_parameter_id",
                schema: "transaction",
                table: "transaction_test_variable",
                column: "test_parameter_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_tool_tool_group_id",
                schema: "transaction",
                table: "transaction_tool",
                column: "tool_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_tool_activity_activity_id",
                schema: "transaction",
                table: "transaction_tool_activity",
                column: "activity_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_tool_activity_tool_id",
                schema: "transaction",
                table: "transaction_tool_activity",
                column: "tool_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity");

            migrationBuilder.DropTable(
                name: "authenticated_user_biohr");

            migrationBuilder.DropTable(
                name: "building");

            migrationBuilder.DropTable(
                name: "digital_signature");

            migrationBuilder.DropTable(
                name: "em_production_phase");

            migrationBuilder.DropTable(
                name: "facility");

            migrationBuilder.DropTable(
                name: "facility_room");

            migrationBuilder.DropTable(
                name: "form_material");

            migrationBuilder.DropTable(
                name: "form_parameter");

            migrationBuilder.DropTable(
                name: "form_section");

            migrationBuilder.DropTable(
                name: "form_tool");

            migrationBuilder.DropTable(
                name: "grade_room");

            migrationBuilder.DropTable(
                name: "input_type");

            migrationBuilder.DropTable(
                name: "item_batch_number");

            migrationBuilder.DropTable(
                name: "item_dosage_form");

            migrationBuilder.DropTable(
                name: "item_group");

            migrationBuilder.DropTable(
                name: "item_presentation");

            migrationBuilder.DropTable(
                name: "item_product_group");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "microflora");

            migrationBuilder.DropTable(
                name: "organization");

            migrationBuilder.DropTable(
                name: "personal");

            migrationBuilder.DropTable(
                name: "position");

            migrationBuilder.DropTable(
                name: "product_forms");

            migrationBuilder.DropTable(
                name: "product_production_phases");

            migrationBuilder.DropTable(
                name: "product_production_phases_personel");

            migrationBuilder.DropTable(
                name: "product_test_type_qcs");

            migrationBuilder.DropTable(
                name: "product_test_types");

            migrationBuilder.DropTable(
                name: "production_phases");

            migrationBuilder.DropTable(
                name: "purposes");

            migrationBuilder.DropTable(
                name: "purposes_personel");

            migrationBuilder.DropTable(
                name: "qc_personel");

            migrationBuilder.DropTable(
                name: "qc_request_sampling");

            migrationBuilder.DropTable(
                name: "qc_request_type");

            migrationBuilder.DropTable(
                name: "qc_result");

            migrationBuilder.DropTable(
                name: "qc_sample");

            migrationBuilder.DropTable(
                name: "qc_sampling_attachment");

            migrationBuilder.DropTable(
                name: "qc_sampling_material");

            migrationBuilder.DropTable(
                name: "qc_sampling_personel");

            migrationBuilder.DropTable(
                name: "qc_sampling_shipment_tracker");

            migrationBuilder.DropTable(
                name: "qc_sampling_tools");

            migrationBuilder.DropTable(
                name: "qc_sampling_type");

            migrationBuilder.DropTable(
                name: "qc_transaction_group_form_material");

            migrationBuilder.DropTable(
                name: "qc_transaction_group_form_section");

            migrationBuilder.DropTable(
                name: "qc_transaction_group_form_tool");

            migrationBuilder.DropTable(
                name: "qc_transaction_group_sample_value");

            migrationBuilder.DropTable(
                name: "qc_transaction_group_value");

            migrationBuilder.DropTable(
                name: "qc_transaction_sample");

            migrationBuilder.DropTable(
                name: "qc_transaction_sampling");

            migrationBuilder.DropTable(
                name: "rel_em_prod_phase_to_room");

            migrationBuilder.DropTable(
                name: "rel_grade_room_scenario");

            migrationBuilder.DropTable(
                name: "rel_item_test_scenario");

            migrationBuilder.DropTable(
                name: "rel_items_item_presentation");

            migrationBuilder.DropTable(
                name: "rel_product_prod_phase_to_room");

            migrationBuilder.DropTable(
                name: "rel_room_sampling_point");

            migrationBuilder.DropTable(
                name: "rel_sampling_purpose_tool_group");

            migrationBuilder.DropTable(
                name: "rel_sampling_test_param");

            migrationBuilder.DropTable(
                name: "rel_sampling_tool");

            migrationBuilder.DropTable(
                name: "rel_test_scenario_param");

            migrationBuilder.DropTable(
                name: "request_ahu");

            migrationBuilder.DropTable(
                name: "request_group_room_purpose");

            migrationBuilder.DropTable(
                name: "request_group_tool_purpose");

            migrationBuilder.DropTable(
                name: "request_purposes");

            migrationBuilder.DropTable(
                name: "request_room");

            migrationBuilder.DropTable(
                name: "room");

            migrationBuilder.DropTable(
                name: "room_purpose");

            migrationBuilder.DropTable(
                name: "room_purpose_to_master_purpose");

            migrationBuilder.DropTable(
                name: "room_sampling_point_layout");

            migrationBuilder.DropTable(
                name: "sampling_point");

            migrationBuilder.DropTable(
                name: "sampling_test_parameter");

            migrationBuilder.DropTable(
                name: "storage_temperature");

            migrationBuilder.DropTable(
                name: "test_parameter");

            migrationBuilder.DropTable(
                name: "test_scenario");

            migrationBuilder.DropTable(
                name: "test_types");

            migrationBuilder.DropTable(
                name: "test_variable");

            migrationBuilder.DropTable(
                name: "tool");

            migrationBuilder.DropTable(
                name: "tool_activity");

            migrationBuilder.DropTable(
                name: "tool_group");

            migrationBuilder.DropTable(
                name: "tool_purpose");

            migrationBuilder.DropTable(
                name: "tool_purpose_to_master_purpose");

            migrationBuilder.DropTable(
                name: "tool_sampling_point_layout");

            migrationBuilder.DropTable(
                name: "type_forms");

            migrationBuilder.DropTable(
                name: "uom");

            migrationBuilder.DropTable(
                name: "UserTestings");

            migrationBuilder.DropTable(
                name: "workflow_history");

            migrationBuilder.DropTable(
                name: "workflow_qc_sampling");

            migrationBuilder.DropTable(
                name: "workflow_qc_transaction_group");

            migrationBuilder.DropTable(
                name: "transaction_batch",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_batch_attachments",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_batch_lines",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_building",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_facility_room",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_rel_grade_room_scenario",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_rel_room_sampling_point",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_rel_sampling_purpose_tool_group",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_rel_sampling_test_param",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_rel_sampling_tool",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_room_purpose",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_room_purpose_to_master_purpose",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_room_sampling_point_layout",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_test_variable",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_tool_activity",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_tool_purpose",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_tool_purpose_to_master_purpose",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_tool_sampling_point_layout",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "form_procedure");

            migrationBuilder.DropTable(
                name: "request_qcs");

            migrationBuilder.DropTable(
                name: "qc_sampling_shipment");

            migrationBuilder.DropTable(
                name: "qc_sampling");

            migrationBuilder.DropTable(
                name: "qc_transaction_group_form_parameter");

            migrationBuilder.DropTable(
                name: "transaction_facility",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_room",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_grade_room",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_purposes",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_sampling_point",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_rel_test_scenario_param",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_activity",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_tool",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "qc_transaction_group_form_procedure");

            migrationBuilder.DropTable(
                name: "transaction_test_parameter",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_test_scenario",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_tool_group",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "qc_transaction_group_process");

            migrationBuilder.DropTable(
                name: "transaction_organization",
                schema: "transaction");

            migrationBuilder.DropTable(
                name: "qc_process");

            migrationBuilder.DropTable(
                name: "enum_constant");

            migrationBuilder.DropTable(
                name: "qc_transaction_group");
        }
    }
}
