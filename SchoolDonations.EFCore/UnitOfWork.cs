using DDD.Concepts.Interfaces;
using SchoolDonations.CoreDomain.Aggregates.Customers.Persistence;
using SchoolDonations.CoreDomain.Dependencies.Infrastructure.Persistence;

namespace SchoolDonations.EFCore;

public class UnitOfWork : IUnitOfWork, IDisposable
 {
    private AppDbContext AppDbContext { get; }

    #region Repositories

    public ICustomerRepository CustomerRepository { get; }
    public IDomainEventRepository DomainEventRepository { get; }

    #endregion Repositories

    #region Construction

    public UnitOfWork(AppDbContext context,
        ICustomerRepository customerRepository,
        IDomainEventRepository domainEventRepository)
	{
		AppDbContext = context ?? throw new ArgumentNullException(nameof(context));
        CustomerRepository =  customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        DomainEventRepository = domainEventRepository ?? throw new ArgumentNullException(nameof(domainEventRepository));
    }

    #endregion Construction

    #region Methods

    public async Task<int> SaveChangesAsync()
    {
        await using var transaction = await AppDbContext.Database.BeginTransactionAsync();

        try
        {
            var result = await AppDbContext.SaveChangesAsync();

            await HandleDomainEventsAsync();

            await transaction.CommitAsync();
            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task HandleDomainEventsAsync()
    {
        var domainEntities = AppDbContext.ChangeTracker
            .Entries<ISupportsDomainEvents>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        // TODO: If this fails abort
        await DomainEventRepository.AddAsync(domainEvents);

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        // TODO: Add domain events to the event store

        // TODO: Read events in bulk from event store and publish to RabbitMQ
    }

    #region Dispose

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                AppDbContext.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion Dispose

    #endregion Methods
}
