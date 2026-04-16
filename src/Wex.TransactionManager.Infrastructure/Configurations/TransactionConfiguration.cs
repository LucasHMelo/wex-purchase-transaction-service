using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wex.TransactionManager.Domain.Entities;
using Wex.TransactionManager.Domain.ValueObjects;

namespace Wex.TransactionManager.Infrastructure.Configurations;

internal class TransactionConfiguration
    : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(transaction => transaction.Id);
        builder.Property(transaction => transaction.Description)
            .HasMaxLength(50);
        builder.OwnsOne(x => x.Amount, (OwnedNavigationBuilder<Transaction, Money> money) =>
        {
            money.Property(m => m.Value).HasColumnName("amount");
            money.Property(m => m.Currency).HasColumnName("currency");
        });
        builder.Property(transaction => transaction.TransactionDate);
        builder.Property(transaction => transaction.CreatedAt);

    }
}
