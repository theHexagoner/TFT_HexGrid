using HexBlazor.Components;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using HexGridInterfaces.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace HexBlazor.Pages
{
    public partial class HexGridPage : ComponentBase
    {
        IHexGridPageVM viewModel;

        #region private fields

        private const int DPI = 96;

        private ElementReference _divRef;
        private BSvg _svgRef;
        private ISvgGrid _grid;
        private ISvgMap _map;

        private bool _saveDisabled = true;
        private bool _isShowingMap = false;

        #region the SVG

        public double _widthInches = 8.5d;
        public double _heightInches = 11d;
        private SvgViewBox _viewBox;

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

        private int _rowCount = 19;
        private int _colCount = 17;
        private double _size = .5d;

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

        #region Spinner

        private void SetShowSpinner(bool showIt)
        {
            if (showIt)
            {
                _spinnerClass = "mb-2 spinner-border";
                _svgClass = "d-none";
            }
            else
            {
                _spinnerClass = "mb-2 d-none";
                _svgClass = "";
            }
        }

        private string _spinnerClass = "mb-2 d-none";
        private string _svgClass = "";

        #endregion

        #region Component Life-cycle

        /// <summary>
        /// Sets parameters supplied by the component's parent in the render 
        /// tree or from route parameters.
        /// </summary>
        /// <param name="parameters">Represents a collection of parameters 
        /// supplied to an IComponent by its parent in the render tree
        /// </param>
        /// <returns>
        /// A Task that completes when the component has finished updating 
        /// and rendering itself.
        /// </returns>
        public override async Task SetParametersAsync(ParameterView parameters) 
        {
            // if base.SetParametersAsync isn't invoked, developer code can interpret the 
            // incoming parameters' values in any way required
            await base.SetParametersAsync(parameters);

            // If event handlers are provided in developer code, unhook them on disposal.
        }

        /// <summary>
        /// OnInitialized (or OnInitializedAsync) are invoked when the component 
        /// is initialized after having received its initial parameters in SetParametersAsync.
        /// </summary>
        protected override void OnInitialized() // or async Task OnInitializedAsync
        {
            SetViewBox();
            // If event handlers are provided in developer code, unhook them on disposal.
        }

        /// <summary>
        /// OnParametersSet (or OnParametersSetAsync) are called:
        ///     - after the component is initialized in OnInitialized (or OnInitializedAsync)
        ///     - when the parent component re-renders and supplies "changed" params
        /// </summary>
        protected override void OnParametersSet()
        {
            // If event handlers are provided in developer code, unhook them on disposal.
        }

        /// <summary>
        /// OnAfterRender and OnAfterRenderAsync are called after a component has finished 
        /// rendering. Element and component references are populated at this point. Use 
        /// this stage to perform additional initialization steps with the rendered content, 
        /// such as JS interop calls that interact with the rendered DOM elements.
        /// </summary>
        /// <param name="firstRender">True on first time component is rendered in browser</param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                // do something only once, after first render
            }

            // do something every time the component finished rendering

            // NOTE: Even if you return a Task from OnAfterRenderAsync, the framework doesn't 
            // schedule a further render cycle for your component once that task completes. 
            // This is to avoid an infinite render loop. This is different from the other 
            // lifecycle methods, which schedule a further render cycle once a returned 
            // Task completes.

            // If event handlers are provided in developer code, unhook them on disposal.
        }

        /// <summary>
        /// ShouldRender is called each time a component is rendered. Override ShouldRender to 
        /// manage UI refreshing. If the implementation returns true, the UI is refreshed. 
        /// NOTE: even if ShouldRender is overridden, the component is always initially rendered.
        /// </summary>
        /// <returns>True if the component should render itself, or false if render 
        /// should be suppressed</returns>
        protected override bool ShouldRender()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// instantiate the grid and it's content, then pass its geometry into the SVG
        /// </summary>
        private async Task GenerateTheGrid()
        {
            SetShowSpinner(true);
            await Task.Delay(100);

            try
            {
                var origin = new GridPoint(0.5d, .5d);
                var schema = new OffsetSchema(_isStylePointy, _isOffsetOdd, _isSkewRight);
                var pxSize = (_size / Math.Sqrt(3)) * DPI;
                var radius = new GridPoint(pxSize,pxSize);

                viewModel = vmBuilder.Build(_rowCount, _colCount, radius, origin, schema, _viewBox);

                _grid = viewModel.Grid;
                _map = viewModel.Map;

                _svgRef.SetGeometry(_grid.SvgHexagons, _grid.SvgMegagons);
                _saveDisabled = false;
                SetShowSpinner(false);
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SetShowSpinner(false);
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

                var scaleW = _viewBox.Width / divWidth;
                var scaleH = _viewBox.Height / divHeight;

                // get the actual coordinates of the mouse click relative to the div:
                double mouseX = eventArgs.ClientX - oLeft;
                double mouseY = eventArgs.ClientY - oTop;

                // translate these for the 0,0 origin being located at center
                // translation factor must be scaled relative to actual size of div as displayed on screen
                var translatedX = (mouseX * scaleW) + _viewBox.OriginX;
                var translatedY = (mouseY * scaleH) + _viewBox.OriginY;

                // get the grid hex the user clicked on, if any:
                var ID = viewModel.HitTester.HitTest(new GridPoint(translatedX, translatedY));
                var exists = _grid.TryGetHex(ID, out _);

                // left-click to add the hex to the map if not already present
                if (exists && eventArgs.Button == 0)
                {
                    // update the look of the grid hexagon in case we need to redraw it from scratch later
                    _grid.GetHex(ID.Value).IsSelected = true;

                    // update the current view
                    _svgRef.UpdateHexIsSelected(ID.Value, true);

                    // if the map does not contain the hexagon, add it to the map
                    _map.AddHexagon(ID.Value);
                }

                // right-click to remove the hex from the map if it is present
                if (exists && eventArgs.Button == 2)
                {
                    // update the look of the grid hexagon in case we need to redraw it from scratch later
                    _grid.GetHex(ID.Value).IsSelected = false;

                    // actually update the current view
                    _svgRef.UpdateHexIsSelected(ID.Value, false);

                    // if the map contains the hex, remove it from the map
                    _map.RemoveHexagon(ID.Value);
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
