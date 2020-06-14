using CarRental.Model.DomainModelLayer.Policies;

namespace CarRental.Model.DomainModelLayer.Interfaces
{
    public interface IFreeMinutesPolicy
    {
        string Name { get; }
        double CalculateFreeMinutes(double numOfMinutes);
         PoliciesEnum PolicyType { get; }
    }
}