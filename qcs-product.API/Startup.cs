using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using qcs_product.API.BusinessProviders;
using qcs_product.API.BusinessProviders.Collection;
using qcs_product.API.DataProviders;
using qcs_product.API.DataProviders.Collection;
using qcs_product.API.Infrastructure;
using qcs_product.Constants;
using qcs_product.API.SettingModels;
using qcs_product.API.EventHandlers;
using qcs_product.API.Mappers;
using Q100Library.Authorization;
using Q100Library.Authorization.Infrastructure;
using Q100Library.Authorization.SettingModels;
using Q100Library.EventBus.Base;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.EventBus.GooglePubSub;
using Q100Library.IntegrationEvents;
using Q100Library.Authorization.EventHandlers;
using Autofac;

namespace qcs_product.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(ApplicationConstant.APPSETTING_PATH, optional: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<QcsProductContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString(ApplicationConstant.DB_CONTEXT_CONECTION_STRING_SECTION)
                )
            );

            services.AddDbContext<QualityAssuranceSystemServiceContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("QASConnection")
                )
            );

            services.AddDbContext<q100_authorizationContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString(ApplicationConstant.DB_CONTEXT_CONECTION_STRING_SECTION)
                )
            );

            services.AddTransient<IUserTestingBusinessProvider, UserTestingBusinessProvider>();
            services.AddTransient<IProductionPhaseBusinessProvider, ProductionPhaseBusinessProvider>();
            services.AddTransient<IProductProductionPhaseBusinessProvider, ProductProductionPhaseBusinessProvider>();
            services.AddTransient<IQcRequestBusinessProvider, QcRequestBusinessProvider>();
            services.AddTransient<ITypeFormBusinessProvider, TypeFormBusinessProvider>();
            services.AddTransient<ITestTypeBusinessProvider, TestTypeBusinessProvider>();
            services.AddTransient<IEnumConstantBusinessProvider, EnumConstantBusinessProvider>();
            services.AddTransient<INotificationServiceBusinessProvider, NotificationServiceBusinessProvider>();
            services.AddTransient<IItemBusinessProvider, ItemBusinessProvider>();
            services.AddTransient<IEmProductionPhaseBusinessProvider, EmProductionPhaseBusinessProvider>();
            services.AddTransient<IRoomBusinessProvider, RoomBusinessProvider>();
            services.AddTransient<IQcSamplingBusinessProvider, QcSamplingBusinessProvider>();
            services.AddTransient<IToolBusinessProvider, ToolBusinessProvider>();
            services.AddTransient<IPersonelBusinessProvider, PersonelBusinessProvider>();
            services.AddTransient<ITestParameterBusinessProvider, TestParameterBusinessProvider>();
            services.AddTransient<IUploadFilesBusinessProvider, UploadFilesBusinessProvider>();
            services.AddTransient<ISamplingShipmentBusinessProvider, SamplingShipmentBusinessProvider>();
            services.AddTransient<IBaseApiBioServiceBusinessProviders, BaseApiBioServiceBusinessProviders>();
            services.AddTransient<IBioHRIntegrationBussinesProviders, BioHRIntegrationBussinesProviders>();
            services.AddTransient<IQcProcessBusinessProvider, QcProcessBusinessProvider>();
            services.AddTransient<IQcTestBusinessProvider, QcTestBusinessProvider>();
            services.AddTransient<IReviewBusinessProvider, ReviewBusinessProvider>();
            services.AddTransient<IBioHRIntegrationBussinesProviders, BioHRIntegrationBussinesProviders>();
            services.AddTransient<IMonitoringBusinessProvider, MonitoringBusinessProvider>();
            services.AddTransient<IMicrofloraBusinessProvider, MicrofloraBusinessProvider>();
            services.AddTransient<IWorkflowServiceBusinessProvider, WorkflowServiceBusinessProvider>();
            services.AddTransient<ITestScenarioBusinessProvider, TestScenarioBusinessProvider>();
            services.AddTransient<IPurposeBusinessProvider, PurposeBusinessProvider>();
            services.AddTransient<IAuditTrailBusinessProvider, AuditTrailBusinessProvider>();
            services.AddTransient<IAUAMServiceBusinessProviders, AUAMServiceBusinessProviders>();
            services.AddTransient<IFacilityBusinessProvider, FacilityBusinessProvider>();
            services.AddTransient<IQcSamplingTypeBusinessProvider, QcSamplingTypeBusinessProvider>();
            services.AddTransient<IOrganizationBusinessProvider, OrganizationBusinessProvider>();
            services.AddTransient<ITransactionBatchLineBusinessProvider, TransactionBatchLineBusinessProvider>();
            services.AddTransient<IQcSamplingTemplateBusinessProvider, QcSamplingTemplateBusinessProvider>();
            services.AddTransient<ITransactionTestingBusinessProvider, TransactionTestingBusinessProvider>();
            services.AddTransient<ITransactionTemplateTestTypeProcedureBusinessProvider, TransactionTemplateTestTypeProcedureBusinessProvider>();
            services.AddTransient<IReviewKasieBusinessProvider, ReviewKasieBusinessProvider>();
            services.AddTransient<IOperatorTestingBusinessProvider, OperatorTestingBusinessProvider>();
            services.AddTransient<ITransactionTestingResultBusinessProvider, TransactionTestingResultBusinessProvider>();
            services.AddTransient<IDeviationBusinessProvider, DeviationBusinessProvider>();
            services.AddTransient<ITemplateTestingInfoBusinessProvider, TemplateTestingInfoBusinessProvider>();
            services.AddTransient<ITemplateOperatorTestingBusinessProvider, TemplateOperatorTestingBusinessProvider>();
            services.AddTransient<ITransactionTestingSamplingBusinessProvider, TransactionTestingSamplingBusinessProvider>();
            services.AddTransient<ITransactionTestTypeBusinessProvider, TransactionTestTypeBusinessProvider>();
            services.AddTransient<ITransactionTestTypeMethodValidationParameterBusinessProvider, TransactionTestTypeMethodValidationParameterBusinessProvider>();
            services.AddTransient<ITransactionTestingOperatorResultBusinessProvider, TransactionTestingOperatorResultBusinessProvider>();
            services.AddTransient<ITransactionTestingDeviationBusinessProvider, TransactionTestingDeviationBusinessProvider>();

            services.AddTransient<IPurposeDataProvider, PurposeDataProvider>();
            services.AddTransient<IMicrofloraDataProvider, MicrofloraDataProvider>();
            services.AddTransient<IWorkflowHistoryDataProvider, WorkflowHistoryDataProvider>();
            services.AddTransient<IUserTestingDataProvider, UserTestingDataProvider>();
            services.AddTransient<IProductionPhaseDataProvider, ProductionPhaseDataProvider>();
            services.AddTransient<IProductProductionPhasesDataProvider, ProductProductionPhasesDataProvider>();
            services.AddTransient<IQcRequestDataProvider, QcRequestDataProvider>();
            services.AddTransient<ITypeFormDataProvider, TypeFormDataProvider>();
            services.AddTransient<ITestTypeDataProvider, TestTypeDataProvider>();
            services.AddTransient<IEnumConstantDataProvider, EnumConstantDataProvider>();
            services.AddTransient<IItemDataProvider, ItemDataProvider>();
            services.AddTransient<IEmProductionPhaseDataProvider, EmProductionPhaseDataProvider>();
            services.AddTransient<IRoomDataProvider, RoomDataProvider>();
            services.AddTransient<IQcSamplingDataProvider, QcSamplingDataProvider>();
            services.AddTransient<IToolDataProvider, ToolDataProvider>();
            services.AddTransient<IPersonelDataProvider, PersonelDataProvider>();
            services.AddTransient<ITestParameterDataProvider, TestParameterDataProvider>();
            services.AddTransient<ISamplingShipmentDataProvider, SamplingShipmentDataProvider>();
            services.AddTransient<IAuthenticatedUserBiohrDataProviders, AuthenticatedUserBiohrDataProviders>();
            services.AddTransient<IQcProcessDataProvider, QcProcessDataProvider>();
            services.AddTransient<IQcTestDataProvider, QcTestDataProvider>();
            services.AddTransient<IOrganizationDataProvider, OrganizationDataProvider>();
            services.AddTransient<IWorkflowQcSamplingDataProvider, WorkflowQcSamplingDataProvider>();
            services.AddTransient<IReviewDataProvider, ReviewDataProvider>();
            services.AddTransient<IDigitalSignatureDataProvider, DigitalSignatureDataProvider>();
            services.AddTransient<IGradeRoomDataProvider, GradeRoomDataProvider>();
            services.AddTransient<IToolGroupDataProvider, ToolGroupDataProvider>();
            services.AddTransient<IToolActivityDataProvider, ToolActivityDataProvider>();
            services.AddTransient<IActivityDataProvider, ActivityDataProvider>();
            services.AddTransient<IMonitoringDataProvider, MonitoringDataProvider>();
            services.AddTransient<IWorkflowQcTransactionGroupDataProvider, WorkflowQcTransactionGroupDataProvider>();
            services.AddTransient<IWorkflowServiceDataProvider, WorkflowServiceDataProvider>();
            services.AddTransient<IBuildingDataProvider, BuildingDataProvider>();
            services.AddTransient<IFacilityDataProvider, FacilityDataProvider>();
            services.AddTransient<ITestScenarioDataProvider, TestScenarioDataProvider>();
            services.AddTransient<IQcSamplingTypeDataProvider, QcSamplingTypeDataProvider>();
            services.AddTransient<ISamplingPointDataProvider, SamplingPointDataProvider>();
            services.AddTransient<ITestVariableDataProvider, TestVariableDataProvider>();
            services.AddSingleton<IGenerateQcProcessDataProvider, GenerateQcProcessDataProvider>();
            services.AddSingleton<IGenerateQcResultDataProvider, GenerateQcResultDataProvider>();
            services.AddSingleton<IFacilityRoomDataProvider, FacilityRoomDataProvider>();
            services.AddTransient<IRoomSamplingPointLayoutDataProvider, RoomSamplingPointLayoutDataProvider>();
            services.AddTransient<IToolPurposeDataProvider, ToolPurposeDataProvider>();
            services.AddTransient<IToolPurposeToMasterPurposeDataProvider, ToolPurposeToMasterPurposeDataProvider>();
            services.AddTransient<IToolSamplingPointLayoutDataProvider, ToolSamplingPointLayoutDataProvider>();
            services.AddTransient<IRelSamplingToolDataProvider, RelSamplingToolDataProvider>();
            services.AddTransient<IRelSamplingTestParamDataProvider, RelSamplingTestParamDataProvider>();
            services.AddTransient<IOrganizationDataProvider, OrganizationDataProvider>();
            services.AddTransient<IRoomPurposeDataProvider, RoomPurposeDataProvider>();
            services.AddTransient<IRoomPurposeToMasterPurposeDataProvider, RoomPurposeToMasterPurposeDataProvider>();
            services.AddTransient<ITransactionDataProvider, TransactionDataProvider>();
            services.AddTransient<ITransactionRoomDataProvider, TransactionRoomDataProvider>();
            services.AddTransient<ITransactionTestScenarioDataProvider, TransactionTestScenarioDataProvider>();
            services.AddTransient<ITransactionBatchDataProvider, TransactionBatchDataProvider>();
            services.AddTransient<ITransactionBatchLineDataProvider, TransactionBatchLineDataProvider>();
            services.AddTransient<IQcSamplingTemplateDataProvider, QcSamplingTemplateDataProvider>();
            services.AddTransient<ITransactionTestingDataProvider, TransactionTestingDataProvider>();
            services.AddTransient<IReviewKasieDataProvider, ReviewKasieDataProvider>();
            services.AddTransient<IOperatorTestingDataProvider, OperatorTestingDataProvider>();
            services.AddTransient<ITransactionTestingResultDataProvider, TransactionTestingResultDataProvider>();
            services.AddTransient<IDeviationDataProvider, DeviationDataProvider>();
            services.AddTransient<ITemplateTestingInfoDataProvider, TemplateTestingInfoDataProvider>();
            services.AddTransient<ITemplateOperatorTestingDataProvider, TemplateOperatorTestingDataProvider>();
            services.AddTransient<ITransactionTemplateTestTypeProcessProcedureDataProvider, TransactionTemplateTestTypeProcessProcedureDataProvider>();
            services.AddTransient<ITransactionTemplateTestTypeProcessProcedureParameterDataProvider, TransactionTemplateTestTypeProcessProcedureParameterDataProvider>();
            services.AddTransient<ITransactionTestingSamplingDataProvider, TransactionTestingSamplingDataProvider>();
            services.AddTransient<ITransactionTestTypeDataProvider, TransactionTestTypeDataProvider>();
            services.AddTransient<ITransactionTestTypeMethodValidationParameterDataProvider, TransactionTestTypeMethodValidationParameterDataProvider>();
          
            services.AddTransient<ITransactionTestingOperatorResultDataProvider, TransactionTestingOperatorResultDataProvider>();
            services.AddTransient<ITransactionTestingDeviationDataProvider, TransactionTestingDeviationDataProvider>();

            // inject EventBus
            services.AddSingleton<IGooglePubSubClient>(sp =>
            {
                string projectId = string.Empty;
                string topicId = string.Empty;
                string subscriptionId = string.Empty;

                ILogger<GooglePubSubClient> logger = sp.GetRequiredService<ILogger<GooglePubSubClient>>();
                if (!string.IsNullOrEmpty(Configuration[ApplicationConstant.EVENT_BUS_PUBSUB_PROJECT_ID_SECTION]))
                {
                    projectId = Configuration[ApplicationConstant.EVENT_BUS_PUBSUB_PROJECT_ID_SECTION];
                }
                if (!string.IsNullOrEmpty(Configuration[ApplicationConstant.EVENT_BUS_PUBSUB_TOPIC_ID_SECTION]))
                {
                    topicId = Configuration[ApplicationConstant.EVENT_BUS_PUBSUB_TOPIC_ID_SECTION];
                }
                if (!string.IsNullOrEmpty(Configuration[ApplicationConstant.EVENT_BUS_PUBSUB_SUBSCRIPTION_ID_SECTION]))
                {
                    subscriptionId = Configuration[ApplicationConstant.EVENT_BUS_PUBSUB_SUBSCRIPTION_ID_SECTION];
                }

                return new GooglePubSubClient(logger, projectId, topicId, subscriptionId);
            });

            // Add CORS POLICY
            List<string> allowedOrigins = Configuration.GetSection(ApplicationConstant.ALLOWED_ORIGIN_SETTING_SECTION).Get<List<string>>();

            services.AddCors(options => options.AddPolicy(ApplicationConstant.API_CORS_POLICY_NAME, builder =>
            {
                builder.WithOrigins(allowedOrigins.ToArray()).AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddHttpClient();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.MaxDepth = 0;
            });

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header from q100 AUAM"

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Inject Attribute Filter
            services.AddScoped<Q100AUAMAuthorizationFilter>();

            services.AddHttpClient();
            
            services.AddHealthChecks();

            _registerEventBus(services);
            _registerProviders(services);
            _registerSettings(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.RouteTemplate = ApplicationConstant.SWAGGER_ROUTE_TEMPLATE;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(ApplicationConstant.SWAGGER_PATH, ApplicationConstant.APPLICATION_NAME);
                c.RoutePrefix = ApplicationConstant.SWAGGER_ROUTE_PREFIX;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(ApplicationConstant.API_CORS_POLICY_NAME);

            app.UseHealthChecks(ApplicationConstant.HEALTH_CHECK_PATH);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            _configureEventBus(app);
        }


        /// <summary>
        /// register message broker using rabbitMQ
        /// </summary>
        /// <param name="services"></param>
        private void _registerEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusGooglePubSub>(sp =>
            {
                var googlePubSubClient = sp.GetRequiredService<IGooglePubSubClient>();
                var logger = sp.GetRequiredService<ILogger<EventBusGooglePubSub>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();

                return new EventBusGooglePubSub(logger, googlePubSubClient, eventBusSubcriptionsManager, iLifetimeScope);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            //services.AddTransient<NotificationEventHandler>();
            services.AddTransient<OrganizationEventHandler>();
            services.AddTransient<RoomEventHandler>();
            services.AddTransient<GradeRoomEventHandler>();
            services.AddTransient<ToolEventHandler>();
            services.AddTransient<DigitalSignatureEventHandler>();
            services.AddTransient<FacilityEventHandler>();
            services.AddTransient<BuildingEventHandler>();
            services.AddTransient<ItemEventHandler>();
            services.AddTransient<ItemBatchEventHandler>();
            services.AddTransient<MicrobaEventHandler>();
            services.AddTransient<ReminderReviewEventHandler>();

            // inject handler to proceed event from AUAM Service
            services.AddTransient<ApplicationEventHandler>();
            services.AddTransient<EndpointEventHandler>();
            services.AddTransient<RoleEventHandler>();
            services.AddTransient<RoleToEndpointEventHandler>();
            services.AddTransient<PositionToRoleEventHandler>();
        }

        private void _registerProviders(IServiceCollection services)
        {
            services.Configure<NotificationServiceSetting>(Configuration.GetSection(ApplicationConstant.NOTIFICATION_SECTION_SETTING));
            services.Configure<EnvironmentSetting>(Configuration.GetSection(ApplicationConstant.ENVIRONMENT_SETTING_SECTION));
        }

        private void _configureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrganizationIntegrationEvent, OrganizationEventHandler>();
            eventBus.Subscribe<RoomIntegrationEvent, RoomEventHandler>();
            eventBus.Subscribe<GradeRoomIntegrationEvent, GradeRoomEventHandler>();
            eventBus.Subscribe<ToolIntegrationEvent, ToolEventHandler>();
            eventBus.Subscribe<DigitalSignatureIntegrationEvent, DigitalSignatureEventHandler>();
            eventBus.Subscribe<BuildingIntegrationEvent, BuildingEventHandler>();
            eventBus.Subscribe<FacilityIntegrationEvent, FacilityEventHandler>();
            eventBus.Subscribe<MicrobaIntegrationEvent, MicrobaEventHandler>();
            eventBus.Subscribe<ItemIntegrationEvent, ItemEventHandler>();
            eventBus.Subscribe<ItemBatchIntegrationEvent, ItemBatchEventHandler>();
            eventBus.Subscribe<TestScenarioIntegrationEvent, TestScenarioEventHandler>();
            eventBus.Subscribe<ReminderReviewIntegrationEvent, ReminderReviewEventHandler>();
            eventBus.Subscribe<ApplicationIntegrationEvent, ApplicationEventHandler>();
            eventBus.Subscribe<EndPointIntegrationEvent, EndpointEventHandler>();
            eventBus.Subscribe<RoleIntegrationEvent, RoleEventHandler>();
            eventBus.Subscribe<RoleToEndPointIntegrationEvent, RoleToEndpointEventHandler>();
            eventBus.Subscribe<PositionToRoleIntegrationEvent, PositionToRoleEventHandler>();
            //eventBus.StartSubscribing();
        }

        private void _registerSettings(IServiceCollection services)
        {
            services.Configure<Q100AuthorizationSetting>(Configuration.GetSection(ApplicationConstant.Q100_AUTHORIZATION_SETTING_SECTION));
            services.Configure<BioHRServiceSetting>(Configuration.GetSection(ApplicationConstant.BIOHR_SERVICE_SETTING_SECTION));
            services.Configure<EnvironmentSetting>(Configuration.GetSection(ApplicationConstant.ENVIRONMENT_SETTING_SECTION));
        }
    }
}
