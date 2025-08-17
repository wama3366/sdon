using SchoolDonations.CoreDomain.Aggregates.Customers.Persistence;

namespace SchoolDonations.CoreDomain.Dependencies.Infrastructure.Persistence;

public interface IUnitOfWork
{
    ICustomerRepository CustomerRepository { get; }
    IDomainEventRepository DomainEventRepository { get; }
    Task<int> SaveChangesAsync();
}