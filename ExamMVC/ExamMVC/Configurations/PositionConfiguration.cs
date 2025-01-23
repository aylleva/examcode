﻿using ExamMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamMVC.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(100)");
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
