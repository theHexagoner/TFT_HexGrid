using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using SvgLib.Polygons;
using System.Collections.Generic;

namespace SvgLib.ViewModels
{
    //public class SvgMap : ISvgMap
    //{
    //    private readonly IMap _map;

    //    private SvgMap() { }

    //    internal SvgMap(IMap map, SvgViewBox viewBox)
    //    {
    //        _map = map;
    //        SvgViewBox = viewBox;

    //        // set the SvgHexagons
    //        _hexDict = new Dictionary<int, ISvgHexagon>();
    //        foreach (IHexagon h in _map.Hexagons.Values)
    //        {
    //            _hexDict.Add(h.ID, new SvgHexagon(h.ID, h.OffsetLocation.Row, h.OffsetLocation.Col, h.Points));
    //        }

    //        _map.OnMapDictionaryAddItem += OnAddingHexagon;
    //        _map.OnMapDictionaryRemoveItem += OnRemovingHexagon;

    //        // set the SvgMegagons
    //        _megaDict = new Dictionary<int, SvgMegagon>();
    //        foreach (IEdge edge in _map.Edges.Values)
    //        {
    //            if (edge.IsMegaLine)
    //            {
    //                _megaDict.Add(edge.ID, new SvgMegagon(edge.ID, edge.PathD));
    //            }
    //        }

    //        // hook up the event listeners for adding and removing

    //    }

    //    public SvgViewBox SvgViewBox { get; }

    //    private readonly IDictionary<int, SvgHexagon> _hexDict;
    //    public IEnumerable<KeyValuePair<int, SvgHexagon>> SvgHexagons 
    //    { 
    //        get
    //        {
    //            return _hexDict;
    //        }
    //    }

    //    private readonly IDictionary<int, SvgMegagon> _megaDict;
    //    public IEnumerable<KeyValuePair<int, SvgMegagon>> SvgMegagons 
    //    {
    //        get 
    //        {
    //            return _megaDict;
    //        } 
    //    }

    //    public void AddHexagon(int ID)
    //    {
    //        _map.AddHexagon(ID);
    //    }

    //    private void OnAddingHexagon(object sender, DictionaryChangingEventArgs<int, IHexagon> e)
    //    {
    //        var hex = e.Value;
    //        _hexDict.Add(hex.ID, new SvgHexagon(hex.ID, hex.OffsetLocation.Row, hex.OffsetLocation.Col,  hex.Points, true));

    //        foreach (IEdge edge in hex.Edges)
    //        {
    //            if (edge.IsMegaLine && _megaDict.ContainsKey(edge.ID) == false)
    //                _megaDict.Add(edge.ID, new SvgMegagon(edge.ID, edge.PathD));
    //        }
    //    }

    //    public void RemoveHexagon(int ID)
    //    {
    //        _map.RemoveHexagon(ID);
    //    }

    //    public void OnRemovingHexagon(object sender, DictionaryChangingEventArgs<int, IHexagon> e)
    //    {
    //        var hex = e.Value;
    //        _hexDict.Remove(hex.ID);

    //        foreach (IEdge edge in hex.Edges)
    //        {
    //            if (edge.IsMegaLine == false && _megaDict.ContainsKey(edge.ID))
    //                _megaDict.Remove(edge.ID);
    //        }
    //    }

    //}
}
