using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolDonations.EFCore.Customers;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerPersistenceDto>
{
    public void Configure(EntityTypeBuilder<CustomerPersistenceDto> builder)
    {
        builder.HasKey(e => e.Id);

        #region Data Columns

        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.FirstName).HasColumnName("person_first_name").HasMaxLength(50).IsRequired();
        builder.Property(x => x.LastName).HasColumnName("person_last_name").HasMaxLength(50).IsRequired();

        builder.Property(x => x.BillingAddressLine1).HasColumnName("billing_address_line_1").HasMaxLength(100);
        builder.Property(x => x.BillingAddressLine2).HasColumnName("billing_address_line_2").HasMaxLength(100);
        builder.Property(x => x.BillingCity).HasColumnName("billing_city").HasMaxLength(100);
        builder.Property(x => x.BillingState).HasColumnName("billing_state").HasMaxLength(50);
        builder.Property(x => x.BillingZipCode).HasColumnName("billing_zip_code").HasMaxLength(10);
        builder.Property(x => x.BillingCountry).HasColumnName("billing_country").HasMaxLength(50);

        builder.Property(x => x.ShippingAddressLine1).HasColumnName("shipping_address_line_1").HasMaxLength(100);
        builder.Property(x => x.ShippingAddressLine2).HasColumnName("shipping_address_line_2").HasMaxLength(100);
        builder.Property(x => x.ShippingCity).HasColumnName("shipping_city").HasMaxLength(100);
        builder.Property(x => x.ShippingState).HasColumnName("shipping_state").HasMaxLength(50);
        builder.Property(x => x.ShippingZipCode).HasColumnName("shipping_zip_code").HasMaxLength(10);
        builder.Property(x => x.ShippingCountry).HasColumnName("shipping_country").HasMaxLength(50);

        #endregion Data Columns

        #region Meta Data Columns

        // TODO: Add max length to string fields.
        builder.Property(x => x.ModifiedAt).HasColumnName("modified_at").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.ModifiedBy).HasColumnName("modified_by").HasMaxLength(50).IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("created_by").HasMaxLength(50).IsRequired();
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
        builder.Property(x => x.RowVersion).HasColumnName("row_version").IsRequired();

        #endregion Meta Data Columns
    }
}