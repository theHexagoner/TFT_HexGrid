using System;
using System.Text.Json.Serialization;

namespace HexGridInterfaces.Structs
{
    public enum HexagonStyle
    {
        Flat = 0,
        Pointy = 1
    }

    public enum MegagonSkew
    {
        Left = 0,
        Right = 1
    }

    public enum OffsetPush
    {
        Even = 0,
        Odd = 1
    }

    public struct OffsetSchema
    {
        [JsonInclude] public readonly HexagonStyle HexStyle;
        [JsonInclude] public readonly OffsetPush OffsetPush;
        [JsonInclude] public readonly MegagonSkew MegahexSkew;

        [JsonConstructor]
        public OffsetSchema(HexagonStyle hexStyle, OffsetPush offsetPush, MegagonSkew megahexSkew)
        {
            HexStyle = hexStyle;
            OffsetPush = offsetPush;
            MegahexSkew = megahexSkew;
        }

        public OffsetSchema(bool isPointy, bool isOdd, bool isRight)
        {
            HexStyle = isPointy ? HexagonStyle.Pointy : HexagonStyle.Flat;
            OffsetPush = isOdd ? OffsetPush.Odd : OffsetPush.Even;
            MegahexSkew = isRight ? MegagonSkew.Right : MegagonSkew.Left;
        }

        #region Conversions

        private static readonly int EVEN = 1;
        private static readonly int ODD = -1;

        #region Offset to Cubic Coords

        public CubicCoordinate GetCubicCoords(OffsetCoordinate hex)
        {
            return HexStyle switch
            {
                HexagonStyle.Flat => OffsetToCubeQ(OffsetPush == OffsetPush.Even ? EVEN : ODD, hex),
                HexagonStyle.Pointy => OffsetToCubeR(OffsetPush == OffsetPush.Even ? EVEN : ODD, hex),
                _ => throw new ArgumentException(string.Format("Invalid Style {0} specified for OffsetSchema", HexStyle))
            };
        }

        private static CubicCoordinate OffsetToCubeQ(int push, OffsetCoordinate h)
        {
            int x = h.Col;
            int y = h.Row - (h.Col + push * (h.Col & 1)) / 2;
            int z = -x - y;
            return new CubicCoordinate(x, y, z);
        }

        private static CubicCoordinate OffsetToCubeR(int push, OffsetCoordinate h)
        {
            int x = h.Col - (h.Row + push * (h.Row & 1)) / 2;
            int y = h.Row;
            int z = -x - y;
            return new CubicCoordinate(x, y, z);
        }

        #endregion

        #region Cubic to Offset Coords

        /// <summary>
        /// convert a cube to offset (row, column) coordinates
        /// </summary>
        /// <param name="hex">the hex for which you want the offset coordinates</param>
        /// <returns>Offset</returns>
        internal OffsetCoordinate GetOffsetCoords(CubicCoordinate hex)
        {
            return HexStyle switch
            {
                HexagonStyle.Flat => GetOffsetQ(OffsetPush == OffsetPush.Even ? EVEN : ODD, hex),
                HexagonStyle.Pointy => GetOffsetR(OffsetPush == OffsetPush.Even ? EVEN : ODD, hex),
                _ => throw new ArgumentException(string.Format("Invalid Style {0} specified for OffsetSchema", HexStyle))
            };
        }

        private static OffsetCoordinate GetOffsetQ(int push, CubicCoordinate hex)
        {
            int col = hex.X;
            int row = hex.Y + ((hex.X + push * (hex.X & 1)) / 2);
            return new OffsetCoordinate(row, col);
        }

        private static OffsetCoordinate GetOffsetR(int push, CubicCoordinate hex)
        {
            int col = hex.X + (hex.Y + push * (hex.Y & 1)) / 2;
            int row = hex.Y;
            return new OffsetCoordinate(col, row);
        }

        #endregion

        #endregion

    }
}