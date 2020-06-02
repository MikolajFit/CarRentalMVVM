using System;
using System.Collections.Generic;
using System.Text;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;

namespace DDD.CarRentalLib.DomainModelLayer.Policies
{
    public class VipFreeMinutesPolicy:IFreeMinutesPolicy
    {
        private const double FreeMinutesPercent = 0.3;
        public string Name { get; protected set; }

        public VipFreeMinutesPolicy()
        {
            this.Name = "Vip Free Minutes Policy";
        }
        public double CalculateFreeMinutes(double numOfMinutes)
        {
            return FreeMinutesPercent * numOfMinutes;
        }
    }
}
