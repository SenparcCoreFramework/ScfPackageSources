using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase.Functions
{
    /// <summary>
    /// 选项列表
    /// </summary>
    public class SelectionList : List<SelectionItem>
    {
        /// <summary>
        /// 选项类型
        /// </summary>
        public SelectionType SelectionType { get; set; }
        /// <summary>
        /// 已经选中的项的值
        /// </summary>
        public string[] SelectedValues { get; set; }

        public SelectionList() { }

        public SelectionList(SelectionType selectionType, IEnumerable<SelectionItem> collection = null) : base(collection)
        {
            SelectionType = selectionType;
        }
    }

    /// <summary>
    /// 选项
    /// </summary>
    public class SelectionItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        //public bool Selected { get; set; }
        public string Note { get; set; }

        public SelectionItem() { }

        public SelectionItem(string text, string value, /*bool selected = false,*/ string note = "")
        {
            Text = text;
            Value = value;
            //Selected = selected;
            Note = note;
        }

    }

    /// <summary>
    /// 选项集合类型
    /// </summary>
    public enum SelectionType
    {
        /// <summary>
        /// 下拉列表（单选）
        /// </summary>
        DropDownList,
        /// <summary>
        /// 复选框列表（多选）
        /// </summary>
        CheckBoxList,
        ///// <summary>
        ///// 单选列表（单选）
        ///// </summary>
        //RadioButtonList
    }
}
