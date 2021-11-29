using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure.Persisence.Configurations
{
    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.BillingAdress);
            builder.OwnsOne(o => o.Payment);
        }
    }
}
