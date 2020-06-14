using System;
using System.Collections.Generic;
using DDD.Base.DomainModelLayer.Models;

namespace CarRental.Model.DomainModelLayer.Models
{
    public class Distance : ValueObject
    {
        public static readonly DistanceUnit DefaultDistanceUnit = DistanceUnit.KM;

        public Distance(double value)
        {
            Unit = DefaultDistanceUnit;
            Value = value;
        }

        public Distance(double value, DistanceUnit unit) : this(value)
        {
            Unit = unit;
        }

        public double Value { get; set; }
        public DistanceUnit Unit { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Unit;
            yield return Math.Round(Value, 2);
        }

        public void SwitchDistanceUnit()
        {
            Unit = Unit == DistanceUnit.KM ? DistanceUnit.MI : DistanceUnit.KM;
            Value = Units.SwitchUnits(Value, Unit);
        }

        public static Distance operator +(Distance d1, Distance d2)
        {
            if (!AreCompatibleUnits(d1, d2)) throw new ArgumentException("Units mismatch");
            return new Distance(d1.Value + d2.Value, d1.Unit);
        }

        public static Distance operator -(Distance d1, Distance d2)
        {
            if (!AreCompatibleUnits(d1, d2)) throw new ArgumentException("Units mismatch");
            return new Distance(d1.Value - d2.Value, d1.Unit);
        }

        private static bool AreCompatibleUnits(Distance d1, Distance d2)
        {
            return d1.Value == 0 || d1.Value == 0 || d1.Unit.Equals(d2.Unit);
        }


        public static bool operator <(Distance d1, Distance d2)
        {
            if (!AreCompatibleUnits(d1, d2)) throw new ArgumentException("Units mismatch");
            return d1.Value.CompareTo(d2.Value) < 0;
        }

        public static bool operator >(Distance d1, Distance d2)
        {
            if (!AreCompatibleUnits(d1, d2)) throw new ArgumentException("Units mismatch");
            return d1.Value.CompareTo(d2.Value) > 0;
        }

        public static bool operator >=(Distance d1, Distance d2)
        {
            if (!AreCompatibleUnits(d1, d2)) throw new ArgumentException("Units mismatch");
            return d1.Value.CompareTo(d2.Value) >= 0;
        }

        public static bool operator <=(Distance d1, Distance d2)
        {
            if (!AreCompatibleUnits(d1, d2)) throw new ArgumentException("Units mismatch");
            return d1.Value.CompareTo(d2.Value) <= 0;
        }

        public override string ToString()
        {
            return $"{Value}.2f {Unit}";
        }
    }
}