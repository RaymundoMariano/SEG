using System.Collections.Generic;

namespace SEG.Domain.Models.Response
{
    public class ResultModel
    {
        public bool Succeeded { get; set; }
        public object ObjectRetorno { get; set; }        
        public int ObjectResult { get; set; }
        public List<string> Errors { get; set; }
    }
}
