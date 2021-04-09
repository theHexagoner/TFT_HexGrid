
using System.Text.Json.Serialization;

namespace HexGridInterfaces.Structs
{
    public struct SvgMegagon
    {
        [JsonInclude]
        public readonly int Id;
        
        [JsonInclude]
        public readonly string D; //e.g. <path d = "M20,230 Q40,205 50,230 T90,230" />

        [JsonConstructor]
        public SvgMegagon(int id, string d)
        {
            Id = id;
            D = d;
        }

    }
}