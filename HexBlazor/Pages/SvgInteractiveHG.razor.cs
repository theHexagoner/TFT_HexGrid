using HexBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;

namespace HexBlazor.Pages
{
    public partial class SvgInteractiveHG : ComponentBase
    {
        #region private fields

        private const int DPI = 96;

        private ElementReference _divRef;
        private BSvg _svgRef;
        private ISvgGrid _grid;
        //private Map _map;

        private bool _saveDisabled = true;
        private bool _isShowingMap = false;

        #region the SVG

        public double _widthInches = 8.5d;
        public double _heightInches = 11d;
        private SvgViewBox _viewBox = new(-408d, -528d, 816d, 1056d);

        private void SetViewBox()
        {
            double pxH = _heightInches * DPI;
            double pxW = _widthInches * DPI;

            double transW = -pxW / 2;
            double transH = -pxH / 2;

            _viewBox = new SvgViewBox(transW, transH, pxW, pxH);
        }

        #endregion

        #region the Grid

        private int _rowCount = 23;
        private int _colCount = 21;
        private double _size = .25d;

        private string _styleText = "Flat";
        private bool _isStylePointy = false;

        private string _offsetText = "Even";
        private bool _isOffsetOdd = false;
        
        private string _skewText = "Left";
        private bool _isSkewRight = false;

        private string _bgFill = "#E0E0E0";

        private float _hexStrokeWidth = 1f;
        private string _hexStroke = "#000000";
        private string _hexFill = "#FFFFFF";

        private float _megaStrokeWidth = 3f;
        private string _megaStroke = "#000000";
        
        private bool _showStars = false;

        #endregion

        #endregion

        #region Offset Scheme

        private void ToggleStyle()
        {
            _isStylePointy = !_isStylePointy;
            _styleText = _isStylePointy ? "Pointy" : " Flat ";
        }

        private void ToggleOffset()
        {
            _isOffsetOdd = !_isOffsetOdd;
            _offsetText = _isOffsetOdd ? "Odd" : "Even";
        }

        private void ToggleSkew()
        {
            _isSkewRight = !_isSkewRight;
            _skewText = _isSkewRight ? "Right" : "Left";
        }

        #endregion

        /// <summary>
        /// instantiate the grid and it's content, then pass its geometry into the SVG
        /// </summary>
        private async Task GenerateTheGrid()
        {
            try
            {
                // show a spinner here
                await Task.Delay(1);

                var origin = new GridPoint(0.5d, .5d);
                var schema = new OffsetSchema(_isStylePointy, _isOffsetOdd, _isSkewRight);
                var pxRadius = (_size / Math.Sqrt(3)) * DPI;
                var size = new GridPoint(pxRadius,pxRadius);

                _grid = gridBuilder.Build(_rowCount, _colCount, size, origin, schema, _viewBox);

                //_map = _grid.InitMap();

                _svgRef.SetGeometry(_grid.SvgHexagons, _grid.SvgMegagons);
                _saveDisabled = false;

                // hide the spinner here
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // hide the spinner here
            }
        }

        /// <summary>
        /// respond to mouse clicks to select/deselect hexagons
        /// </summary>
        /// <param name="eventArgs">mouse event arguments for the click</param>
        /// <returns>Task result</returns>
        private async Task SvgOnClick(MouseEventArgs eventArgs)
        {
            await Task.Delay(1);

            //if (_grid != null && _map != null && _isShowingMap == false)
            //{
            //    // get the actual size of the DIV
            //    string data = await jsRuntime.InvokeAsync<string>("getDivDimensions", new object[] { _divRef });
            //    JObject dimensions = (JObject)JsonConvert.DeserializeObject(data);

            //    var divWidth = dimensions.Value<double>("width");
            //    var divHeight = dimensions.Value<double>("height");
            //    var oLeft = dimensions.Value<double>("offsetLeft");
            //    var oTop = dimensions.Value<double>("offsetTop");

            //    // calculate the factor by which to multiply the TRANSLATE_ vars
            //    // Width and Height should be same factor?

            //    var scaleW = _viewBox.Width / divWidth;
            //    var scaleH = _viewBox.Height / divHeight;

            //    // get the actual coordinates of the mouse click relative to the div:
            //    double mouseX = eventArgs.ClientX - oLeft;
            //    double mouseY = eventArgs.ClientY - oTop;

            //    // translate these for the 0,0 origin being located at center
            //    // translation factor must be scaled relative to actual size of div as displayed on screen
            //    var translatedX = (mouseX * scaleW) + _viewBox.OriginX;
            //    var translatedY = (mouseY * scaleH) + _viewBox.OriginY;

            //    // get the grid hex the user clicked on, if any:
            //    var hex = _grid.GetHexAt(new GridPoint(translatedX, translatedY));

            //    // left-click to add the hex to the map if not already present
            //    if (hex != null && eventArgs.Button == 0)
            //    {
            //        // update the look of the grid hexagon in case we need to redraw it from scratch later
            //        _grid.SvgHexagons[hex.ID].IsSelected = true;

            //        // update the current view
            //        await _svgRef.UpdateHexIsSelected(hex.ID, true);

            //        // if the map does not contain the hexagon, add it to the map
            //        _map.AddHexagon(hex.ID);
            //    }

            //    // right-click to remove the hex from the map if it is present
            //    if (hex != null && eventArgs.Button == 2)
            //    {
            //        // update the look of the grid hexagon in case we need to redraw it from scratch later
            //        _grid.SvgHexagons[hex.ID].IsSelected = false;

            //        // actually update the current view
            //        await _svgRef.UpdateHexIsSelected(hex.ID, false);

            //        // if the map contains the hex, remove it from the map
            //        _map.RemoveHexagon(hex.ID);
            //    }
            //}
        }

        /// <summary>
        /// swap between showing the grid or the map
        /// </summary>
        private void Swap()
        {
            if (!_isShowingMap)
            {
                //_svgRef.SetGeometry(_map.SvgHexagons, _map.SvgMegagons);
            }
            else
            {
                //_svgRef.SetGeometry(_grid.SvgHexagons, _grid.SvgMegagons);
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
                var filename = string.Format("{0}r_{1}c_{2}_hexgrid.svg", _rowCount, _colCount, _size);
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
