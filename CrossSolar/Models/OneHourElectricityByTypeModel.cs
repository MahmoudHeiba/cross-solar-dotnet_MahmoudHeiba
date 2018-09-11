using System;

namespace CrossSolar.Models
{
    public class OneHourElectricityByTypeModel
    {
        public int Id { get; set; }

        public double ElectricAmount { get; set; }

        public DateTime DateTime { get; set; }

        public ElectricUnitType UnitType { get; set; } = ElectricUnitType.KiloWatt;
    }
}