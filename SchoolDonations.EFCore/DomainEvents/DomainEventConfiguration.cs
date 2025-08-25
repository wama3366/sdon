using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolDonations.EFCore.DomainEvents;

public class DomainConfiguration : IEntityTypeConfiguration<DomainEventDto>
{
    public void Configure(EntityTypeBuilder<DomainEventDto> builder)
    {
        builder.HasKey(e => e.Id);

        #region Data Columns

        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.UniqueEventId).HasColumnName("unique_event_id").HasMaxLength(50).IsRequired();
        builder.Property(x => x.EventData).HasColumnName("event_data").HasMaxLength(50).IsRequired();
        builder.Property(x => x.OccurredOn).HasColumnName("occurred_on").IsRequired();
        builder.Property(x => x.PublishedOn).HasColumnName("published_on");

        #endregion Data Columns
    }
}