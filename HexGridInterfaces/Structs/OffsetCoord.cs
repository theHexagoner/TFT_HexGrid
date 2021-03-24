using System;
using System.Diagnostics.CodeAnalysis;

namespace HexGridInterfaces.Structs
{
    /// <summary>
    /// structure for conversions between offset and cubic coordinate systems
    /// </summary>
    public struct OffsetCoordinate : IEquatable<OffsetCoordinate>
    {
        public readonly int Row;
        public readonly int Col;

        public OffsetCoordinate(int row, int col)
        {
            Row = row;
            Col = col;
        }

        #region Equality/Hashcode overrides

        public override bool Equals(object obj)
        {
            return Equals((OffsetCoordinate)obj);
        }

        public bool Equals([AllowNull] OffsetCoordinate other)
        {
            return other.Row == Row &&
                   other.Col == Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(OffsetCoordinate left, OffsetCoordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OffsetCoordinate left, OffsetCoordinate right)
        {
            return !(left == right);
        }

        #endregion

    }

}