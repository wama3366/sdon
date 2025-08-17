namespace SchoolDonations.ApplicationServices;

public interface IApplicationMapper<TDomain, TDto> where TDto : Dto
{
    TDomain ToDomain(TDto dto);
    void CopyFromDomain(TDomain domain, TDto dto);
    TDto FromDomain(TDomain domain);
    List<TDomain> ToDomain(IEnumerable<TDto> dtos);
    List<TDto> FromDomain(IEnumerable<TDomain> domains);
}