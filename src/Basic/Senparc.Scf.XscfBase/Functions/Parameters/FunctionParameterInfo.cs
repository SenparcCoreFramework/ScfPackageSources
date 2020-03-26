using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    public enum ParameterType
    {
        Text,
        SingleSelection,
        MultipleSelection,
    }

    /// <summary>
    /// FunctionParameter 信息
    /// </summary>
    public class FunctionParameterInfo
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public string SystemType { get; set; }
        public object Value { get; set; }


        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterType ParameterType { get; set; } = ParameterType.Text;
        /// <summary>
        /// 选项
        /// </summary>
        public string[] SelectionItems { get; set; }

        public FunctionParameterInfo()
        {
        }

        public FunctionParameterInfo(string name, string title, string description,
            bool isRequired, string systemType, ParameterType parameterType, string[] selectionItems, object value)
        {
            Name = name;
            Title = title;
            Description = description;
            IsRequired = isRequired;
            SystemType = systemType;
            SelectionItems = selectionItems;
            ParameterType = parameterType;
            Value = value;
        }
    }
}
