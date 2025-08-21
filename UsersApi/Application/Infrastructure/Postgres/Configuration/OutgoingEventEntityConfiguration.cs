using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersApi.Application.Domain.Entities;

namespace UsersApi.Application.Infrastructure.Postgres.Configuration;

public class OutgoingEventEntityConfiguration: IEntityTypeConfiguration<OutgoingEventEntity>
{
    public void Configure(EntityTypeBuilder<OutgoingEventEntity> builder)
    {
        builder.HasKey(x => x.OutgoingEventId);
        builder.Property(x => x.EventType)
            .HasConversion<int>();
        builder.Property(x => x.EventData);
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
    }
}