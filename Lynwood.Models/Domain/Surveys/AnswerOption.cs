using System;
using System.Collections.Generic;
using System.Text;

namespace Lynwood.Models.Domain.Surveys
{
    public class AnswerOption
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Values { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
