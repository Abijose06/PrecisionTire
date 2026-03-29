using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGomas.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
    }
}