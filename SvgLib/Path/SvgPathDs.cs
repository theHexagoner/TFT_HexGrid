using System;
using System.Collections.Generic;
using System.Text;
using HexGridInterfaces.Factories;
using HexGridInterfaces.Structs;

namespace SvgLib.Paths
{
    /// <summary>
    /// factory for returning SVG Path D for various graphic elements
    /// </summary>
    public sealed class SvgPathDFactory
    {
        private static readonly SvgPathDFactory instance = new SvgPathDFactory();
        private readonly Dictionary<Type, ISvgPathDGetter> dictionary;

        /// <summary>
        /// identify the specific SVG drawing being requested
        /// other members could be hex labels, counters/figure art, etc.
        /// </summary>
        public enum Type
        {
            Star
        }

        private SvgPathDFactory()
        {
            dictionary = new Dictionary<Type, ISvgPathDGetter>
            {
                { Type.Star, new StarSvgPathDGetter() }
            };
        }

        /// <summary>
        /// return the singleton instance of the factory
        /// </summary>
        public static SvgPathDFactory Instance
        {
            get
            {
                return instance ?? new SvgPathDFactory();
            }
        }

        /// <summary>
        /// return SVG by type, located at origin and scaled according to requested size
        /// </summary>
        /// <param name="type">what kind of SVG is being requested</param>
        /// <param name="origin">absolute location where SVG will be drawn on parent</param>
        /// <param name="size">how big to draw it, in user units</param>
        /// <returns></returns>
        public string GetPathD(Type type, GridPoint origin, double size)
        {
            return (dictionary[type].GetPathD(origin, size));
        }

    }

    /// <summary>
    /// base class for path getter helper classes
    /// </summary>
    public abstract class SvgPathDGetter : ISvgPathDGetter
    {
        public abstract string GetPathD(GridPoint origin, double size);

        protected static string GetPathD(GridPoint[] points)
        {
            // use a string builder to build up the D for the path
            var sb = new StringBuilder();

            // move to the first point
            sb.Append(string.Format("M{0},{1} ", points[0].X, points[0].Y));

            // draw lines to the rest
            for (int i = 1; i < points.Length; i++)
            {
                sb.Append(string.Format("L{0},{1} ", points[i].X, points[i].Y));
            }

            sb.Append(" Z");

            return sb.ToString();
        }

    }

    /// <summary>
    /// helper class to return SVG for 5 pointed star
    /// </summary>
    internal sealed class StarSvgPathDGetter : SvgPathDGetter
    {
        public override string GetPathD(GridPoint center, double outerRadius)
        {
            double innerRadius = outerRadius * Math.Sqrt(3.5 - (1.5 * Math.Sqrt(5)));

            // conversions to radians
            double Ang36 = Math.PI / 5.0;   // 36Â° x PI/180
            double Ang72 = 2.0 * Ang36;     // 72Â° x PI/180

            // some sine and cosine values we need
            double Sin36 = Math.Sin(Ang36);
            double Sin72 = Math.Sin(Ang72);
            double Cos36 = Math.Cos(Ang36);
            double Cos72 = Math.Cos(Ang72);

            // this star has 10 points
            GridPoint[] points = new GridPoint[10];

            // top off the star, or on a clock this is 0:00 hours
            points[0] = new GridPoint(center.X, center.Y - outerRadius);
            points[1] = new GridPoint(center.X + (innerRadius * Sin36), center.Y - (innerRadius * Cos36)); // 0:06 hours
            points[2] = new GridPoint(center.X + (outerRadius * Sin72), center.Y - (outerRadius * Cos72)); // 0:12 hours
            points[3] = new GridPoint(center.X + (innerRadius * Sin72), center.Y + (innerRadius * Cos72)); // 0:18
            points[4] = new GridPoint(center.X + (outerRadius * Sin36), center.Y + (outerRadius * Cos36)); // 0:24 
            points[5] = new GridPoint(center.X, center.Y + innerRadius);
            points[6] = new GridPoint(center.X - (outerRadius * Sin36), center.Y + (outerRadius * Cos36)); // 0:36 
            points[7] = new GridPoint(center.X - (innerRadius * Sin72), center.Y + (innerRadius * Cos72)); // 0:42
            points[8] = new GridPoint(center.X - (outerRadius * Sin72), center.Y - (outerRadius * Cos72)); // 0:48 
            points[9] = new GridPoint(center.X - (innerRadius * Sin36), center.Y - (innerRadius * Cos36)); // 0:54 hours

            return GetPathD(points);

        }
    }

}