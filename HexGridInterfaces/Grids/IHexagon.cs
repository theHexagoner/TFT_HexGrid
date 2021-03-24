using HexGridInterfaces.Structs;

namespace HexGridInterfaces.Grids
{
    /// <summary>
    /// identifies the location of a hex within a megahex
    /// </summary>
    public enum MegaLocation
    {
        N = -1, // not set
        X = 0,  // center
        A = 1,  // flat = lower-right   pointy = right
        B = 2,  // flat = upper-right   pointy = upper-right
        C = 3,  // flat = top           pointy = upper-left
        D = 4,  // flat = upper-left    pointy = left
        E = 5,  // flat = lower-left    pointy = lower-left
        F = 6   // flat = bottom        pointy = lower-right
    }

    public interface IHexagon
    {
        int ID { get; }
        int Row { get; }
        int Col { get; }

        GridPoint[] Points { get; }

        MegaLocation MegaLocation { get; }

        IEdge[] Edges { get; }

        CubicCoordinate CubicLocation { get; }

        OffsetCoordinate OffsetLocation { get; }

        void SetLocationInMegagon(MegaLocation locationInMegagon);

    }
}
