using CarRental.Model.DomainModelLayer.Interfaces;

namespace CarRental.Model.DomainModelLayer.Policies
{
    public class VipFreeMinutesPolicy : IFreeMinutesPolicy
    {
        private const double FreeMinutesPercent = 0.3;

        public VipFreeMinutesPolicy()
        {
            Name = "Vip Free Minutes Policy";
        }

        public string Name { get; protected set; }
        public PoliciesEnum PolicyType { get; protected set; } = PoliciesEnum.Vip;

        public double CalculateFreeMinutes(double numOfMinutes)
        {
            return FreeMinutesPercent * numOfMinutes;
        }
    }
}