using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatePokemonJson.Models
{
    public class Result
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class GeneralResponse
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<Result> results { get; set; }
    }
}
