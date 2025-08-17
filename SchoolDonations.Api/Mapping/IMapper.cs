namespace SchoolDonations.API.Mapping;

public interface IApiMapper<TApplicationDto, TDto>
{
    TApplicationDto ToAppDto(TDto dto);
    void CopyFromAppDto(TApplicationDto domain, TDto dto);
    TDto FromAppDto(TApplicationDto appDto);
    List<TApplicationDto> ToAppDto(IEnumerable<TDto> dtos);
    List<TDto> FromAppDto(IEnumerable<TApplicationDto> appDtos);
}