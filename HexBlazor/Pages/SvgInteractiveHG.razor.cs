using HexBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TFT_HexGrid.Coordinates;
using TFT_HexGrid.Grids;
using TFT_HexGrid.Maps;

namespace HexBlazor.Pages
{
    public partial class SvgInteractiveHG : ComponentBase
    {
        #region private fields

        private int _rowCount = 23;
        private int _colCount = 21;
        private double _sideLen = 24d;
        private OffsetScheme _offsetScheme = OffsetScheme.Even_Q;

        private float _hexStrokeWidth = 1f;
        private string _hexStroke = "#000000";

        private float _megaStrokeWidth = 3f;
        private string _megaStroke = "#000000";

        private string _hexLabel = string.Empty;

        private const double VBOX_H = 1056d;
        private const double VBOX_W = 816d;
        private const double TRANSLATE_H = 528d; // half width in pixels @ 96ppi
        private const double TRANSLATE_W = 408d; // half height in pixels @ 96ppi

        private ElementReference _divRef;
        private BSvg _svgRef;

        private Grid _grid;
        private Map _map;

        private bool _saveDisabled = true;
        private bool _isShowingMap = false;

        private bool _showStars = false;

        #endregion

        /// <summary>
        /// instantiate the grid and it's content, then pass its geometry into the SVG
        /// </summary>
        private async Task GenerateTheGrid()
        {
            try
            {
                _hexLabel = "Loading...";
                await Task.Delay(1);

                var size = new GridPoint(_sideLen, _sideLen);
                var origin = new GridPoint(0.5d, .5d);

                _grid = new Grid(_rowCount, _colCount, size, origin, _offsetScheme);
                _map = _grid.InitMap();

                _svgRef.SetGeometry(_grid.SvgHexagons, _grid.SvgMegagons);
                _saveDisabled = false;
                _hexLabel = "Clicked Row, Col: ";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _hexLabel = "Error!";
            }
        }

        /// <summary>
        /// respond to mouse clicks to select/deselect hexagons
        /// </summary>
        /// <param name="eventArgs">mouse event arguments for the click</param>
        /// <returns>Task result</returns>
        private async Task SvgOnClick(MouseEventArgs eventArgs)
        {
            if (_grid != null && _map != null && _isShowingMap == false)
            {
                // get the actual size of the DIV
                string data = await jsRuntime.InvokeAsync<string>("getDivDimensions", new object[] { _divRef });
                JObject dimensions = (JObject)JsonConvert.DeserializeObject(data);

                var divWidth = dimensions.Value<double>("width");
                var divHeight = dimensions.Value<double>("height");
                var oLeft = dimensions.Value<double>("offsetLeft");
                var oTop = dimensions.Value<double>("offsetTop");

                // calculate the factor by which to multiply the TRANSLATE_ vars
                // Width and Height should be same factor?

                var scaleW = VBOX_W / divWidth;
                var scaleH = VBOX_H / divHeight;

                // get the actual coordinates of the mouse click relative to the div:
                double mouseX = eventArgs.ClientX - oLeft;
                double mouseY = eventArgs.ClientY - oTop;

                // translate these for the 0,0 origin being located at center
                // translation factor must be scaled relative to actual size of div as displayed on screen
                var translatedX = (mouseX * scaleW) - TRANSLATE_W;
                var translatedY = (mouseY * scaleH) - TRANSLATE_H;

                //_mouseLabel = string.Format(" {0}, {1}", mouseX, mouseY);
                //_translateLabel = string.Format(" {0}, {1}", translatedX, translatedY);

                // get the grid hex the user clicked on, if any:
                var hex = _grid.GetHexAt(new GridPoint(translatedX, translatedY));
                _hexLabel = hex != null ? string.Format("Clicked Row, Col: {0}, {1}", hex.OffsetLocation.Row, hex.OffsetLocation.Col) : "none";

                // left-click to add the hex to the map if not already present
                if (hex != null && eventArgs.Button == 0)
                {
                    // update the look of the grid hexagon in case we need to redraw it from scratch later
                    _grid.SvgHexagons[hex.ID] = new TFT_HexGrid.SvgHelpers.SvgHexagon(hex.ID, hex.Points, true);

                    // actually update the current view
                    await _svgRef.UpdateHexIsSelected(hex.ID, true);

                    // if the map does not contain the hexagon, add it to the map
                    if (_map.Hexagons.ContainsKey(hex.ID) == false)
                        _map.Hexagons.Add(hex.ID, hex);
                }

                // right-click to remove the hex from the map if it is present
                if (hex != null && eventArgs.Button == 2)
                {
                    // update the look of the grid hexagon in case we need to redraw it from scratch later
                    _grid.SvgHexagons[hex.ID] = new TFT_HexGrid.SvgHelpers.SvgHexagon(hex.ID, hex.Points, false);

                    // actually update the current view
                    await _svgRef.UpdateHexIsSelected(hex.ID, false);

                    // if the map contains the hex, remove it from the map
                    if (_map.Hexagons.ContainsKey(hex.ID))
                        _map.Hexagons.Remove(hex.ID);
                }
            }
        }

        /// <summary>
        /// swap between showing the grid or the map
        /// </summary>
        private void Swap()
        {
            if (!_isShowingMap)
            {
                _svgRef.SetGeometry(_map.SvgHexagons, _map.SvgMegagons);
            }
            else
            {
                _svgRef.SetGeometry(_grid.SvgHexagons, _grid.SvgMegagons);
            }

            _isShowingMap = !_isShowingMap;
        }

        /// <summary>
        /// save the grid as an SVG document
        /// </summary>
        /// <returns>Task result</returns>
        private async Task SaveMe()
        {
            if (!_saveDisabled)
            {
                var filename = string.Format("{0}r_{1}c_{2}_hexgrid.svg", _rowCount, _colCount, _sideLen);
                var paramz = new object[] { _svgRef.Svg, filename };
                try
                {
                    await jsRuntime.InvokeVoidAsync("saveSvg", paramz);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
