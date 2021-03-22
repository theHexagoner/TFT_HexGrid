//using System;
//using System.Diagnostics.CodeAnalysis;
//using TFT_HexGrid.Grids;

//namespace TFT_HexGrid.Coordinates
//{
//    /// <summary>
//    /// structure to store the cubic coordinates for a Hexagon, provide math operations, calculate distances, etc. within an arbitrary grid
//    /// </summary>
//    internal struct Cube : IEquatable<Cube>
//    {
//        public readonly int X;
//        public readonly int Y;
//        public readonly int Z;

//        public Cube(int x, int y, int z)
//        {
//            if (x + y + z != 0) throw new ArgumentException("Sum of x + y + z must be 0");

//            X = x;
//            Y = y;
//            Z = z;
//        }

//        /// <summary>
//        /// all the cubes that touch this one
//        /// </summary>
//        public static Cube[] GetAdjacents(Cube center)
//        {
//            Cube[] offsets = new Cube[]
//                {
//                    new Cube(1, -1, 0),
//                    new Cube(1, 0, -1),
//                    new Cube(0, 1, -1),
//                    new Cube(-1, 1, 0),
//                    new Cube(-1, 0, 1),
//                    new Cube(0, -1, 1)
//                };

//            var adjacents = new Cube[6];
//            for (int i = 0; i < 6; i++)
//            {
//                adjacents[i] = center + offsets[i];
//            }

//            return adjacents;
//        }

//        /// <summary>
//        /// returns the distance from this cube to a target
//        /// </summary>
//        /// <param name="target">the distant cube to check</param>
//        /// <returns>Int32</returns>
//        public int GetDistanceTo(Cube target)
//        {
//            return GetDistance(this, target);
//        }

//        /// <summary>
//        /// returns distance between two cubes
//        /// </summary>
//        /// <param name="source">starting cube</param>
//        /// <param name="target">ending cube</param>
//        /// <returns>Int32</returns>
//        public static int GetDistance(Cube source, Cube target)
//        {
//            // for each dimension get the absolute value of the difference between this cube and the target
//            // the maximum of these values is the distance to the target cube
//            return Math.Max(Math.Abs(source.X - target.X), Math.Max(Math.Abs(source.Y - target.Y), Math.Abs(source.Z - target.Z)));
//        }

//        // other useful algorithms:
//        // cubes within distance?
//        // cubes at an exact distance (ring)?
//        // path to distant cube?
//        // all cubes on a line between this and distant cube?

//        #region Cube Arithmetic

//        public static Cube operator +(Cube a) => a;

//        public static Cube operator -(Cube a) => new Cube(-a.X, -a.Y, -a.Z);

//        public static Cube operator +(Cube a, Cube b)
//            => new Cube(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

//        public static Cube operator -(Cube a, Cube b)
//            => new Cube(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

//        public static Cube operator *(Cube a, int k)
//        => new Cube(a.X * k, a.Y * k, a.Z * k);

//        #endregion

//        #region Conversions

//        private static readonly int EVEN = 1;
//        private static readonly int ODD = -1;

//        public static Cube GetCubeFromOffset(OffsetSchema schema, Offset hex)
//        {
//            return schema.Style switch
//            {
//                HexagonStyle.Flat => OffsetToCubeQ(schema.Offset == OffsetPush.Even ? EVEN : ODD, hex),
//                HexagonStyle.Pointy => OffsetToCubeR(schema.Offset == OffsetPush.Even ? EVEN : ODD, hex),
//                _ => throw new ArgumentException(string.Format("Invalid Style {0} specified for OffsetSchema", schema.Style))
//            };
//        }

//        private static Cube OffsetToCubeQ(int push, Offset h)
//        {
//            int x = h.Col;
//            int y = h.Row - (int)((h.Col + push * (h.Col & 1)) / 2);
//            int z = -x - y;
//            return new Cube(x, y, z);
//        }

//        private static Cube OffsetToCubeR(int push, Offset h)
//        {
//            int x = h.Col - (int)((h.Row + push * (h.Row & 1)) / 2);
//            int y = h.Row;
//            int z = -x - y;
//            return new Cube(x, y, z);
//        }

//        #endregion

//        #region Equality and Hashcode overrides

//        public override int GetHashCode()
//        {
//            return HashCode.Combine(X, Y, Z);
//        }

//        public override bool Equals(object obj)
//        {
//            return Equals((Cube)obj);
//        }

//        public bool Equals([AllowNull] Cube other)
//        {
//            return other.X == X &&
//                   other.Y == Y &&
//                   other.Z == Z;
//        }

//        public static bool operator ==(Cube left, Cube right)
//        {
//            return left.Equals(right);
//        }

//        public static bool operator !=(Cube left, Cube right)
//        {
//            return !(left == right);
//        }

//        #endregion

//    }

//    /// <summary>
//    /// floating point value cubic coordinates, useful for rounding and linear interpolation
//    /// </summary>
//    internal struct CubeF
//    {
//        public readonly double X;
//        public readonly double Y;
//        public readonly double Z;

//        public CubeF(double x, double y, double z)
//        {
//            if (Math.Round(x + y + z) != 0) throw new ArgumentException("x + y + z must == 0");

//            X = x;
//            Y = y;
//            Z = z;
//        }

//        /// <summary>
//        /// use this to figure out which (integer) cube an arbitrary set of floating point cubic coordinates belongs to
//        /// </summary>
//        /// <returns>Cube</returns>
//        public Cube Round()
//        {
//            int x = (int)Math.Round(X);
//            int y = (int)Math.Round(Y);
//            int z = (int)Math.Round(Z);

//            double x_diff = Math.Abs(x - X);
//            double y_diff = Math.Abs(y - Y);
//            double z_diff = Math.Abs(z - Z);

//            if (x_diff > y_diff && x_diff > z_diff)
//            {
//                x = -y - z;
//            }
//            else if (y_diff > z_diff)
//            {
//                y = -x - z;
//            }
//            else
//            {
//                z = -x - y;
//            }

//            return new Cube(x, y, z);
//        }
//    }

//    /// <summary>
//    /// structure for conversions between offset and cubic coordinate systems
//    /// </summary>
//    internal struct Offset : IEquatable<Offset>
//    {
//        public readonly int Row;
//        public readonly int Col;

//        private static readonly int EVEN = 1;
//        private static readonly int ODD = -1;

//        public Offset(int row, int col)
//        {
//            Row = row;
//            Col = col;
//        }

//        /// <summary>
//        /// convert a cube to offset (row, column) coordinates
//        /// </summary>
//        /// <param name="schema">the schema of the parent grid</param>
//        /// <param name="hex">the hex for which you want the offset coordinates</param>
//        /// <returns>Offset</returns>
//        public static Offset GetOffset(OffsetSchema schema, Cube hex)
//        {
//            return schema.Style switch
//            {
//                HexagonStyle.Flat => GetOffsetQ(schema.Offset == OffsetPush.Even ? EVEN : ODD, hex),
//                HexagonStyle.Pointy => GetOffsetR(schema.Offset == OffsetPush.Even ? EVEN : ODD, hex),
//                _ => throw new ArgumentException(string.Format("Invalid Style {0} specified for OffsetSchema", schema.Style))
//            };
//        }

//        private static Offset GetOffsetQ(int push, Cube hex)
//        {
//            int col = hex.X;
//            int row = hex.Y + ((hex.X + push * (hex.X & 1)) / 2);
//            return new Offset(row, col);
//        }

//        private static Offset GetOffsetR(int push, Cube hex)
//        {
//            int col = hex.X + (hex.Y + push * (hex.Y & 1)) / 2;
//            int row = hex.Y;
//            return new Offset(col, row);
//        }

//        #region Equality/Hashcode overrides

//        public override bool Equals(object obj)
//        {
//            return Equals((Offset)obj);
//        }

//        public bool Equals([AllowNull] Offset other)
//        {
//            return other.Row == Row &&
//                   other.Col == Col;
//        }

//        public override int GetHashCode()
//        {
//            return HashCode.Combine(Row, Col);
//        }

//        public static bool operator ==(Offset left, Offset right)
//        {
//            return left.Equals(right);
//        }

//        public static bool operator !=(Offset left, Offset right)
//        {
//            return !(left == right);
//        }

//        #endregion

//    }

//    /// <summary>
//    /// structure for storing X-Y floating-point coordinates
//    /// </summary>
//    public struct GridPoint
//    {
//        public readonly double X;
//        public readonly double Y;

//        public GridPoint(double size)
//        {
//            X = size;
//            Y = size;
//        }

//        public GridPoint(double x, double y)
//        {
//            X = x;
//            Y = y;
//        }

//        public static GridPoint GetRound(GridPoint point, int places)
//        {
//            var x = (double)(decimal.Round((decimal)point.X, places));
//            var y = (double)(decimal.Round((decimal)point.Y, places));
//            return new GridPoint(x, y);
//        }

//        public static double GetDistance(double x1, double y1, double x2, double y2)
//        {
//            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
//        }

//        public static double GetDistance(GridPoint gridpointA, GridPoint gridpointB)
//        {
//            return Math.Sqrt(Math.Pow((gridpointB.X - gridpointA.X), 2) + Math.Pow((gridpointB.Y - gridpointA.Y), 2));
//        }

//        public double GetDistanceTo(GridPoint target)
//        {
//            return Math.Sqrt(Math.Pow((target.X - X), 2) + Math.Pow((target.Y - Y), 2));
//        }

//    }

//}
