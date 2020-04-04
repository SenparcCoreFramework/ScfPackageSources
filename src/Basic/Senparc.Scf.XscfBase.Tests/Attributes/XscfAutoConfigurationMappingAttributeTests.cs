using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.NeuChar.Entities;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.XscfBase.Attributes;
using Senparc.Xscf.DatabaseToolkit;
using Senparc.Xscf.DatabaseToolkit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase.Tests.Attributes
{
    [TestClass]
    public class XscfAutoConfigurationMappingAttributeTests
    {

        [TestMethod]
        public void TypeTest()
        {
            var attr = new DbConfig_WeixinUserConfigurationMapping();
            var typedAttr = attr as IEntityTypeConfiguration<object>;
            IEntityTypeConfiguration<IEntityBase> m = (IEntityTypeConfiguration<IEntityBase>)attr;
            Assert.IsNotNull(typedAttr);

        }
    }
}
