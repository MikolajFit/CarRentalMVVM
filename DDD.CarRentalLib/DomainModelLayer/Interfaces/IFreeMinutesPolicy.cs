using DDD.CarRentalLib.DomainModelLayer.Policies;

namespace DDD.CarRentalLib.DomainModelLayer.Interfaces
{
    public interface IFreeMinutesPolicy
    {
        string Name { get; }
        double CalculateFreeMinutes(double numOfMinutes);
         PoliciesEnum PolicyType { get; }
    }
}