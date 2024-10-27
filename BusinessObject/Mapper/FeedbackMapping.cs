using AutoMapper;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;

namespace BusinessObject.Mapper
{
    public class FeedbackMapping : Profile
    {
        public FeedbackMapping()
        {
            CreateMap<Feedback, FeedbackResponseDTO>()
    .ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => src.FeedbackId))
    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
    .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
    .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateBy))
    .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
    .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdateBy))
    .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
    .ReverseMap();

            CreateMap<Feedback, FeedbackRequestDTO>()
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
              .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateBy))
              .ReverseMap();

            CreateMap<Feedback, ChangefeedbackStatusDTO>().ReverseMap();
        }

    }
}
