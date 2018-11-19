using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HTML2PDF.Models
{
    public class Dokument
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa organizacji")]
        public string Nazwa { get; set; }

        [Required]
        [Display(Name = "Adres organizacji")]
        public string Adres { get; set; }

        [Required]
        [Display(Name = "Nagłówek")]
        public string Naglowek { get; set; }

        [Required]
        [Display(Name = "Treść")]
        public string Tresc { get; set; }

    }
}