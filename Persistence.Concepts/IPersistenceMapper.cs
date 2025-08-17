namespace Persistence.Concepts;

public interface IPersistenceMapper<TDomain, TDto> where TDto : Dto
{
    TDomain ToDomain(TDto dto);
    void CopyFromDomain(TDomain domain, TDto dto);
    TDto FromDomain(TDomain domain);
    List<TDomain> ToDomain(IEnumerable<TDto> dtos);
    List<TDto> FromDomain(IEnumerable<TDomain> domains);

    /// <summary>
    /// Creates a lazy evaluated value that must only be accessed after SaveChanges is called.
    /// </summary>
    /// <param name="dto">The Dto to convert to a domain object.</param>
    /// <returns>A lazy evaluated value that must only be accessed after SaveChanges is called.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Lazy<TDomain> ToDomainLazy(TDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        return new Lazy<TDomain>(() =>
        {
            if (dto.Id <= 0)
                throw new InvalidOperationException("Id is not available before SaveChangesAsync.");

            var domain = ToDomain(dto);

            return domain;
        });
    }
}