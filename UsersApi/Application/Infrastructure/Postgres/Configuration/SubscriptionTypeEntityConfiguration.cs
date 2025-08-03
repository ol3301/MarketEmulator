using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersApi.Application.Domain;
using UsersApi.Application.Domain.Entities;

namespace UsersApi.Application.Infrastructure.Postgres.Configuration;

public class SubscriptionTypeEntityConfiguration : IEntityTypeConfiguration<SubscriptionTypeEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionTypeEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(new SubscriptionTypeEntity { Id = SubscriptionType.Free, Name = "Free" },
            new SubscriptionTypeEntity { Id = SubscriptionType.Trial, Name = "Trial" },
            new SubscriptionTypeEntity { Id = SubscriptionType.Super, Name = "Super" });
    }
}