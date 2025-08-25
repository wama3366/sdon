using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolDonations.CoreDomain.Aggregates.Customers;
using SchoolDonations.CoreDomain.Values;

namespace SchoolDonations.EFCore.Customers;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasConversion(id => id.Value, value => new CustomerId(value));

        builder.Property(c => c.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.OwnsOne(c => c.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName("person_first_name")
                .HasMaxLength(50)
                .IsRequired();
            name.Property(n => n.LastName)
                .HasColumnName("person_last_name")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.OwnsOne(c => c.BillingAddress, address =>
        {
            address.Property(a => a.AddressLine1)
                .HasColumnName("billing_address_line_1")
                .HasMaxLength(100);
            address.Property(a => a.AddressLine2)
                .HasColumnName("billing_address_line_2")
                .HasMaxLength(100);
            address.Property(a => a.City)
                .HasColumnName("billing_city")
                .HasMaxLength(100);
            address.Property(a => a.State)
                .HasColumnName("billing_state")
                .HasConversion(s => s.Abbreviation, v => UsState.GetByAbbreviation(v))
                .HasMaxLength(50);
            address.Property(a => a.ZipCode)
                .HasColumnName("billing_zip_code")
                .HasConversion(z => z.ZipCodeValue, v => new ZipCode(v))
                .HasMaxLength(10);
            address.Property(a => a.Country)
                .HasColumnName("billing_country")
                .HasMaxLength(50);
        });

        builder.OwnsOne(c => c.ShippingAddress, address =>
        {
            address.Property(a => a.AddressLine1)
                .HasColumnName("shipping_address_line_1")
                .HasMaxLength(100);
            address.Property(a => a.AddressLine2)
                .HasColumnName("shipping_address_line_2")
                .HasMaxLength(100);
            address.Property(a => a.City)
                .HasColumnName("shipping_city")
                .HasMaxLength(100);
            address.Property(a => a.State)
                .HasColumnName("shipping_state")
                .HasConversion(s => s.Abbreviation, v => UsState.GetByAbbreviation(v))
                .HasMaxLength(50);
            address.Property(a => a.ZipCode)
                .HasColumnName("shipping_zip_code")
                .HasConversion(z => z.ZipCodeValue, v => new ZipCode(v))
                .HasMaxLength(10);
            address.Property(a => a.Country)
                .HasColumnName("shipping_country")
                .HasMaxLength(50);
        });
    }
}
