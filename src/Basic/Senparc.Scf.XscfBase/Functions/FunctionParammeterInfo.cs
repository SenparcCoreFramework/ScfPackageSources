using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    public enum ParammeterType
    {
        Text,
        SingleSelection,
        MultipleSelection,
    }

    /// <summary>
    /// FunctionParammeter 信息
    /// </summary>
    public class FunctionParammeterInfo
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public string SystemType { get; set; }


        /// <summary>
        /// 参数类型
        /// </summary>
        public ParammeterType ParammeterType { get; set; } = ParammeterType.Text;
        /// <summary>
        /// 选项
        /// </summary>
        public string[] SelectionItems { get; set; }

        public FunctionParammeterInfo()
        {
        }

        public FunctionParammeterInfo(string name, string title, string description,
            bool isRequired, string systemType, ParammeterType parammeterType, string[] selectionItems)
        {
            Name = name;
            Title = title;
            Description = description;
            IsRequired = isRequired;
            SystemType = systemType;
            SelectionItems = selectionItems;
            ParammeterType = parammeterType;
        }
    }
}
