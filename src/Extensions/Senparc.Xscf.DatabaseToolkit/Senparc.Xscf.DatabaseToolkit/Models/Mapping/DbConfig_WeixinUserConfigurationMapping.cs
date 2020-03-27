using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Scf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Xscf.DatabaseToolkit.Models
{
    public class DbConfig_WeixinUserConfigurationMapping : ConfigurationMappingWithIdBase<DbConfig>
    {
        public override void Configure(EntityTypeBuilder<DbConfig> builder)
        {
            base.Configure(builder);
        }
    }
}
