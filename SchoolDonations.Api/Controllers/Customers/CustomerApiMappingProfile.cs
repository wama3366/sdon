using AutoMapper;
using SchoolDonations.ApplicationServices.Services.Customers;
using SchoolDonations.CoreDomain.Aggregates.Customers;

namespace SchoolDonations.API.Controllers.Customers;

public class CustomerApiMappingProfile : Profile
{
    public CustomerApiMappingProfile()
    {
        CreateMap<CustomerApplicationDto, CustomerApiDto>().ReverseMap();

        CreateMap<Customer, CustomerApiDto>()
            .ConvertUsing((src, dest, ctx) => ctx.Mapper.Map<CustomerApiDto>(ctx.Mapper.Map<CustomerApplicationDto>(src)));

        CreateMap<CustomerApiDto, Customer>()
            .ConvertUsing((src, dest, ctx) => ctx.Mapper.Map<Customer>(ctx.Mapper.Map<CustomerApplicationDto>(src)));
    }
}

