namespace DDD.CarRentalLib.DomainModelLayer.Interfaces
{
    public interface IFreeMinutesPolicy
    {
        string Name { get; }
        double CalculateFreeMinutes(double numOfMinutes);
    }
}