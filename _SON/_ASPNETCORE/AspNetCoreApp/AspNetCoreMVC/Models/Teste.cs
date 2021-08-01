using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Models
{
    public class Teste
    {
        [Required]
        [MaxLength(60)]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }
    }
}
