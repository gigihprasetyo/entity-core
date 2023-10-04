using AutoMapper;
using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.Mappers
{
    public class ObjectMapperProfile : Profile
    {
        public ObjectMapperProfile()
        {
            #region 
            CreateMap<Organization, TransactionOrganization>()
                .ForMember(dest => dest.BiohrOrganizationId,
                    opt =>
                        opt.MapFrom(src => src.BIOHROrganizationId))
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
            #endregion

            #region 
            CreateMap<Activity, TransactionActivity>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
            #endregion

            #region 
            CreateMap<Facility, TransactionFacility>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
            #endregion

            #region 
            CreateMap<Facility, TransactionFacility>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
            #endregion

            #region 
            CreateMap<RoomFacility, TransactionFacilityRoom>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
            #endregion

            #region 
            CreateMap<GradeRoom, TransactionGradeRoom>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
            #endregion

            #region 
            CreateMap<Purpose, TransactionPurposes>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
            #endregion

            #region request batch
            CreateMap<InsertBatchRequestQcBindingModel, TransactionBatch>();
            CreateMap<InsertBatchLineRequestQcBindingModel, TransactionBatchLine>();
            CreateMap<InsertBatchAttachmentRequestQcBindingModel, TransactionBatchAttachment>();

            CreateMap<EditBatchRequestQcBindingModel, TransactionBatch>();
            CreateMap<EditBatchLineRequestQcBindingModel, TransactionBatchLine>();
            CreateMap<EditBatchAttachmentRequestQcBindingModel, TransactionBatchAttachment>();

            CreateMap<TransactionBatchLine, TransactionBatchLineViewModel>();
            #endregion
        }
    }
}