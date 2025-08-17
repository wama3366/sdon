using SchoolDonations.CoreDomain.Aggregates.Customers;

namespace SchoolDonations.CoreDomain.Dependencies.Integrations.AntiCorruption;

public interface ISampleExternalService
{
    Customer GetProductFromExternalService(long aggregateId);
}
