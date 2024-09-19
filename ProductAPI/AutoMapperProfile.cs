using App.Metrics.Counter;
using App.Metrics;
using AutoMapper;
using Entities.Dto;
using Entities.Models;

namespace ProductAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap().ForMember(x=> x.ProductImages, s => s.MapFrom(d => d.ProductImages)).ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }


    public class RegisterMetrics
    {
        public static CounterOptions GetWeatherForecasts => new CounterOptions
        {
            Name = "Getting Products",
            Context = "ProductController",
            MeasurementUnit = Unit.Calls
        };
    }
}
