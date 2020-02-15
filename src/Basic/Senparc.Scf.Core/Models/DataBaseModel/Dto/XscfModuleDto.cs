using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Scf.Core.Models.DataBaseModel
{
    public class XscfModuleDto : DtoBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
        public string MenuName { get; set; }
        public bool AllowRemove { get; set; }
        public XscfModules_State State { get; set; }
    }

    public class CreateOrUpdate_XscfModuleDto : DtoBase
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, StringLength(100)]
        public string Uid { get; set; }
        [Required, StringLength(100)]
        public string MenuName { get; set; }
        [Required]
        public bool AllowRemove { get; set; }
        [Required]
        public XscfModules_State State { get; set; }

    }

}
