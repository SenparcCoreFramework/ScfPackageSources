using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Scf.Core.Models.DataBaseModel
{
    public class XscfModuleDto : DtoBase
    {

        public int Id { get; }
        public string Name { get; }
        public string Uid { get; }
        public string MenuName { get; }
        public string Version { get; }
        public string Description { get; }
        public string UpdateLog { get; }
        public bool AllowRemove { get; }
        public string MenuId { get; }
        public string Icon { get; }
        public XscfModules_State State { get; }

        public XscfModuleDto(int id, string name, string uid, string menuName, string version, string description, string updateLog, bool allowRemove, string menuId,string icon, XscfModules_State state)
        {
            Id = id;
            Name = name;
            Uid = uid;
            MenuName = menuName;
            Version = version;
            Description = description;
            UpdateLog = updateLog;
            AllowRemove = allowRemove;
            MenuId = menuId;
            Icon = icon;
            State = state;
        }
    }

    public class CreateOrUpdate_XscfModuleDto : DtoBase
    {

        [Required, StringLength(100)]
        public string Name { get; }
        [Required, StringLength(100)]
        public string Uid { get; }
        [Required, StringLength(100)]
        public string MenuName { get; }
        [Required]
        public string Version { get; }
        [Required]
        public string Description { get; }

        [Required]
        public string UpdateLog { get; }
        [Required]
        public bool AllowRemove { get; }
        public string MenuId { get; }
        public string Icon { get; }
        [Required]
        public XscfModules_State State { get; }

        public CreateOrUpdate_XscfModuleDto(string name, string uid, string menuName, string version, string description, string updateLog, bool allowRemove, string menuId,string icon, XscfModules_State state)
        {
            Name = name;
            Uid = uid;
            MenuName = menuName;
            Version = version;
            Description = description;
            UpdateLog = updateLog;
            AllowRemove = allowRemove;
            MenuId = menuId;
            Icon = icon;
            State = state;
        }
    }

    public class UpdateVersion_XscfModuleDto : DtoBase
    {

        [Required, StringLength(100)]
        public string Name { get; }
        [Required, StringLength(100)]
        public string Uid { get; }
        [Required, StringLength(100)]
        public string MenuName { get; }
        [Required]
        public string Version { get; }
        [Required]
        public string Description { get; }

        public UpdateVersion_XscfModuleDto(string name, string uid, string menuName, string version, string description)
        {
            Name = name;
            Uid = uid;
            MenuName = menuName;
            Version = version;
            Description = description;
        }
    }

    /// <summary>
    /// 跟新菜单Id
    /// </summary>
    public class UpdateMenuId_XscfModuleDto : DtoBase
    {
        public string Uid { get; set; }

        public string MenuId { get; }

        public UpdateMenuId_XscfModuleDto(string uid, string menuId)
        {
            Uid = uid;
            MenuId = menuId;
        }
    }

}
