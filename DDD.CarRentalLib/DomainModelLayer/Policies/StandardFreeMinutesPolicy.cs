using System;
using System.Collections.Generic;
using System.Text;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;

namespace DDD.CarRentalLib.DomainModelLayer.Policies
{
    public class StandardFreeMinutesPolicy:IFreeMinutesPolicy
    {
        private const double FreeMinutesPercent = 0.1;
        public string Name { get; protected set; }

        public StandardFreeMinutesPolicy()
        {
            this.Name = "Standard Free Minutes Policy";
        }
        public double CalculateFreeMinutes(double numOfMinutes)
        {
            return FreeMinutesPercent * numOfMinutes;
        }
    }
}
