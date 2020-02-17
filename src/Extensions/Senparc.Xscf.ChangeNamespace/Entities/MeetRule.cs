using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Xscf.ChangeNamespace
{
    public class MeetRule
    {

        public MeetRule(string orignalKeyword, string replaceWord, string fileType = null)
        {
            OrignalKeyword = orignalKeyword;
            ReplaceWord = replaceWord;
            FileType = fileType;
        }

        public string OrignalKeyword { get; set; }
        public string ReplaceWord { get; set; }
        public string FileType { get; set; }


    }
}
