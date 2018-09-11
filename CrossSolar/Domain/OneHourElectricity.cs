using System;
using System.ComponentModel.DataAnnotations;

namespace CrossSolar.Domain
{
    public class OneHourElectricity
    {
        public int Id { get; set; }

        public string PanelId { get; set; }

        [Range(0, 9999999.999)]
        public double KiloWatt { get; set; }

        public DateTime DateTime { get; set; }
    }
}