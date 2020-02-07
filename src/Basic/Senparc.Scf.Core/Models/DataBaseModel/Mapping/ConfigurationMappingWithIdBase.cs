using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Core.Models.DataBaseModel
{
    /// <summary>
    /// 包含 Id（Key）的 ConfigurationMapping 基类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class ConfigurationMappingWithIdBase<TEntity,TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : EntityBase<TKey>
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(z => z.Id);
            builder.Property(e => e.AddTime).HasColumnType("datetime").IsRequired();
            builder.Property(e => e.LastUpdateTime).HasColumnType("datetime").IsRequired();
        }
    }
}
