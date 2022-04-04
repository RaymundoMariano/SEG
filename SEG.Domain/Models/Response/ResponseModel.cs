using System.Collections.Generic;

namespace SEG.Domain.Models.Response
{
    public class ResponseModel
    {
        public bool Succeeded { get; set; }
        public object ObjectRetorno { get; set; }        
        public List<string> Errors { get; set; }
    }
}
