using System;
using System.ComponentModel.DataAnnotations;

namespace CrossSolar.Models
{
    public class OneHourElectricityModel
    {
        public int Id { get; set; }

        [Range(0, 9999999.999)]
        public double KiloWatt { get; set; }

        public DateTime DateTime { get; set; }
    }
}