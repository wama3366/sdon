using AutoMapper;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Values;

namespace SchoolDonations.ApplicationServices.Services.Customers;

public class CustomerApplicationMappingProfile : Profile
{
    public CustomerApplicationMappingProfile()
    {
        CreateMap<Customer, CustomerApplicationDto>()
            .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.Name.FirstName))
            .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.Name.LastName))
            .ForMember(d => d.BillingAddressLine1, opt => opt.MapFrom(s => s.BillingAddress.AddressLine1))
            .ForMember(d => d.BillingAddressLine2, opt => opt.MapFrom(s => s.BillingAddress.AddressLine2))
            .ForMember(d => d.BillingCity, opt => opt.MapFrom(s => s.BillingAddress.City))
            .ForMember(d => d.BillingState, opt => opt.MapFrom(s => s.BillingAddress.State.Abbreviation))
            .ForMember(d => d.BillingZipCode, opt => opt.MapFrom(s => s.BillingAddress.ZipCode.ZipCodeValue))
            .ForMember(d => d.BillingCountry, opt => opt.MapFrom(s => s.BillingAddress.Country))
            .ForMember(d => d.ShippingAddressLine1, opt => opt.MapFrom(s => s.ShippingAddress.AddressLine1))
            .ForMember(d => d.ShippingAddressLine2, opt => opt.MapFrom(s => s.ShippingAddress.AddressLine2))
            .ForMember(d => d.ShippingCity, opt => opt.MapFrom(s => s.ShippingAddress.City))
            .ForMember(d => d.ShippingState, opt => opt.MapFrom(s => s.ShippingAddress.State.Abbreviation))
            .ForMember(d => d.ShippingZipCode, opt => opt.MapFrom(s => s.ShippingAddress.ZipCode.ZipCodeValue))
            .ForMember(d => d.ShippingCountry, opt => opt.MapFrom(s => s.ShippingAddress.Country));

        CreateMap<CustomerApplicationDto, Customer>()
            .ConstructUsing(dto => new Customer(new CustomerId(dto.Id)))
            .ForPath(d => d.Name.FirstName, opt => opt.MapFrom(s => s.FirstName))
            .ForPath(d => d.Name.LastName, opt => opt.MapFrom(s => s.LastName))
            .ForPath(d => d.BillingAddress.AddressLine1, opt => opt.MapFrom(s => s.BillingAddressLine1))
            .ForPath(d => d.BillingAddress.AddressLine2, opt => opt.MapFrom(s => s.BillingAddressLine2))
            .ForPath(d => d.BillingAddress.City, opt => opt.MapFrom(s => s.BillingCity))
            .ForPath(d => d.BillingAddress.State, opt => opt.MapFrom(s => UsState.GetByAbbreviation(s.BillingState)))
            .ForPath(d => d.BillingAddress.ZipCode, opt => opt.MapFrom(s => new ZipCode(s.BillingZipCode)))
            .ForPath(d => d.BillingAddress.Country, opt => opt.MapFrom(s => s.BillingCountry))
            .ForPath(d => d.ShippingAddress.AddressLine1, opt => opt.MapFrom(s => s.ShippingAddressLine1))
            .ForPath(d => d.ShippingAddress.AddressLine2, opt => opt.MapFrom(s => s.ShippingAddressLine2))
            .ForPath(d => d.ShippingAddress.City, opt => opt.MapFrom(s => s.ShippingCity))
            .ForPath(d => d.ShippingAddress.State, opt => opt.MapFrom(s => UsState.GetByAbbreviation(s.ShippingState)))
            .ForPath(d => d.ShippingAddress.ZipCode, opt => opt.MapFrom(s => new ZipCode(s.ShippingZipCode)))
            .ForPath(d => d.ShippingAddress.Country, opt => opt.MapFrom(s => s.ShippingCountry));
    }
}

