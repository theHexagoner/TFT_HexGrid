
using System;

namespace HexGridInterfaces.Structs
{
    /// <summary>
    /// structure for storing X-Y floating-point coordinates
    /// </summary>
    public struct GridPoint
    {
        public readonly double X;
        public readonly double Y;

        public GridPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static GridPoint GetRound(GridPoint point, int places)
        {
            var x = (double)(decimal.Round((decimal)point.X, places));
            var y = (double)(decimal.Round((decimal)point.Y, places));
            return new GridPoint(x, y);
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public static double GetDistance(GridPoint gridpointA, GridPoint gridpointB)
        {
            return Math.Sqrt(Math.Pow((gridpointB.X - gridpointA.X), 2) + Math.Pow((gridpointB.Y - gridpointA.Y), 2));
        }

        public double GetDistanceTo(GridPoint target)
        {
            return Math.Sqrt(Math.Pow((target.X - X), 2) + Math.Pow((target.Y - Y), 2));
        }



    }
}