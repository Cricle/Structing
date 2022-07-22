using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Idempotent.Models
{
    public class IdempotentResult<TRawResult> : IdempotentBase
    {
        public TRawResult RawResult { get; set; }
    }
}
