namespace DDD.CarRentalLib.DomainModelLayer.Models
{
    public enum DistanceUnit
    {
        KM,
        MI
    }

    public static class Units
    {
        public static readonly double KMtoMi = 0.621371192;

        public static double SwitchUnits(double value, DistanceUnit targetUnit)
        {
            if (targetUnit == DistanceUnit.MI) return value * KMtoMi;

            return value * 1 / KMtoMi;
        }
    }
}