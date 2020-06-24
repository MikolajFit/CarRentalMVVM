using CarRental.Model.DomainModelLayer.Policies;

namespace CarRental.Model.DomainModelLayer.Interfaces
{
    public interface IFreeMinutesPolicy
    {
        string Name { get; }
        PoliciesEnum PolicyType { get; }
        double CalculateFreeMinutes(double numOfMinutes);
    }
}