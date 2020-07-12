using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;

namespace CarRental.Model.DomainModelLayer.Models
{
    public class Area : ValueObject
    {
        public Area(List<PositionDTO> points)
        {
            AddPointsToPolygon(points);
        }

        public List<Position> Polygon { get; set; } = new List<Position>();

        public void AddPointsToPolygon(List<PositionDTO> points)
        {
            if (Polygon.Count + points.Count < 3) throw new Exception("Shape has to have at least 3 points!");
            foreach (var point in points) Polygon.Add(new Position(point.Latitude, point.Longitude));
        }

        public Distance GetDistanceFromNearestPoint(Position toPoint, DistanceUnit distanceUnit)
        {
            Position nearestPoint = null;
            var minDist = double.MaxValue;
            foreach (var p in Polygon)
            {
                var dist = p.GetDistanceTo(toPoint);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestPoint = p;
                }
            }

            if (nearestPoint == null) throw new Exception("Could not find any point!");
            var distanceTo = nearestPoint.GetDistanceTo(toPoint, distanceUnit);
            return distanceTo;
        }

        /// <summary>
        ///     Reference https://stackoverflow.com/questions/924171/geo-fencing-point-inside-outside-polygon/7739297#7739297
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public bool IsPointInPolygon(double latitude, double longitude)
        {
            var isInIntersection = false;
            var actualPointIndex = 0;
            var pointIndexBeforeActual = Polygon.Count - 1;

            var offset = calculateLonOffsetFromDateLine(Polygon);
            longitude = longitude < 0.0 ? longitude + offset : longitude;

            foreach (var actualPointPosition in Polygon)
            {
                var p1Lat = actualPointPosition.Latitude;
                var p1Lon = actualPointPosition.Longitude;

                var p0Lat = Polygon[pointIndexBeforeActual].Latitude;
                var p0Lon = Polygon[pointIndexBeforeActual].Longitude;

                if (p1Lon < 0.0) p1Lon += offset;
                if (p0Lon < 0.0) p0Lon += offset;

                // Jordan curve theorem - odd even rule algorithm
                if (isPointLatitudeBetweenPolyLine(p0Lat, p1Lat, latitude)
                    && isPointRightFromPolyLine(p0Lat, p0Lon, p1Lat, p1Lon, latitude, longitude))
                    isInIntersection = !isInIntersection;

                pointIndexBeforeActual = actualPointIndex;
                actualPointIndex++;
            }

            return isInIntersection;
        }

        private double calculateLonOffsetFromDateLine(List<Position> polygon)
        {
            var offset = 0.0;
            var maxLonPoly = polygon.Max(x => x.Longitude);
            var minLonPoly = polygon.Min(x => x.Longitude);
            if (Math.Abs(minLonPoly - maxLonPoly) > 180) offset = 360.0;

            return offset;
        }

        private bool isPointLatitudeBetweenPolyLine(double polyLinePoint1Lat, double polyLinePoint2Lat, double poiLat)
        {
            return polyLinePoint2Lat <= poiLat && poiLat < polyLinePoint1Lat ||
                   polyLinePoint1Lat <= poiLat && poiLat < polyLinePoint2Lat;
        }

        private bool isPointRightFromPolyLine(double polyLinePoint1Lat, double polyLinePoint1Lon,
            double polyLinePoint2Lat, double polyLinePoint2Lon, double poiLat, double poiLon)
        {
            // lon <(lon1-lon2)*(latp-lat2)/(lat1-lat2)+lon2
            return poiLon < (polyLinePoint1Lon - polyLinePoint2Lon) * (poiLat - polyLinePoint2Lat) /
                (polyLinePoint1Lat - polyLinePoint2Lat) + polyLinePoint2Lon;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}