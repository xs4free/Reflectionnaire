using System;
using System.Collections.Generic;

namespace Reflectionnaire.Shared
{
    public class ReflectionnaireAnswers
    {
        public string ReflectionnaireId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
