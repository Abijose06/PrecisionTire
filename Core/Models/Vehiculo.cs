using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("tblVehiculo")]
    public class Vehiculo
    {
        [Key]
        public int IdVehiculo { get; set; }

        // Es vital tener el ID del cliente al que pertenece el carro
        public int IdCliente { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public short Año { get; set; }

        public string Placa { get; set; }
        [Column("Chassis")]
        public string Chasis { get; set; }

        public bool Estado { get; set; } = true;
    }
}