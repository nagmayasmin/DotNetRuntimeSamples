using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpatialFilter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Symbol _markerSymbol;
        private GraphicsOverlay _pointFeatureOverlay;
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private async void Initialize()
        {
            _markerSymbol = layoutGrid.Resources["markerSymbol"] as Symbol;
            _pointFeatureOverlay = MyMapView.GraphicsOverlays["pointFeatureOverlay"];

          
            // City Layer
            FeatureLayer cityLayer = new FeatureLayer(new Uri("https://sampleserver6.arcgisonline.com/arcgis/rest/services/USA/MapServer/0"));

            // State Layer
            FeatureLayer stateLayer = new FeatureLayer(new Uri("https://sampleserver6.arcgisonline.com/arcgis/rest/services/USA/MapServer/2"));

            MyMap.OperationalLayers.Add(cityLayer);
            MyMap.OperationalLayers.Add(stateLayer);
           
            // State Query
            QueryParameters stateQueryParameters = new QueryParameters();

            // only select one polygon from State layer
            stateQueryParameters.WhereClause = "state_name = 'California'";

           // Query the State feature table 
            FeatureQueryResult stateQueryResult = await stateLayer.FeatureTable.QueryFeaturesAsync(stateQueryParameters);
            var stateResult = stateQueryResult.ToList();

           // City Query
            QueryParameters cityQueryParameter = new QueryParameters();
            cityQueryParameter.WhereClause = "1=1";

            // Query the City feature table 
            FeatureQueryResult cityQueryResult = await cityLayer.FeatureTable.QueryFeaturesAsync(cityQueryParameter);
            var cities = cityQueryResult.Select(feature => feature.Geometry);

            var city_name = cityQueryResult.Select(feature => feature.Attributes);

            var citiesWithinState = cities
                   .Select(city => GeometryEngine.Intersection(city, stateResult[0].Geometry))
                   .Select(city => new Graphic(city, _markerSymbol));


            foreach (Graphic g in citiesWithinState)
            {
                _pointFeatureOverlay.Graphics.Add(g);
             
            }

          
            
        }

     }
}
