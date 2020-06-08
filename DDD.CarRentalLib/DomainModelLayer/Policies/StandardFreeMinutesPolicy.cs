using DDD.CarRentalLib.DomainModelLayer.Interfaces;

namespace DDD.CarRentalLib.DomainModelLayer.Policies
{
    public class StandardFreeMinutesPolicy : IFreeMinutesPolicy
    {
        private const double FreeMinutesPercent = 0.1;

        public StandardFreeMinutesPolicy()
        {
            Name = "Standard Free Minutes Policy";
        }

        public string Name { get; protected set; }
        public PoliciesEnum PolicyType { get; protected set; } = PoliciesEnum.Standard;

        public double CalculateFreeMinutes(double numOfMinutes)
        {
            return FreeMinutesPercent * numOfMinutes;
        }
    }
}