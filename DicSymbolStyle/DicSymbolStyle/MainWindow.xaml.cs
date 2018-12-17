using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
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
using System.IO;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Geometry;

namespace DicSymbolStyle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DirectoryInfo parent = Directory.GetParent(Environment.CurrentDirectory);
        SymbolStyle symbolStyle;
      

        public MainWindow()
        {
            InitializeComponent();

            // Open a shapefile stored locally and add it to the map as a feature layer
            Initialize();

         
        }

        private async void Initialize()
        {
            // Create a new map to display in the map view with a streets basemap

            Map myMap = new Map(Basemap.CreateStreets());

            string file_shapefile_location = parent.FullName + "\\Data\\Public_Art.shp";

            try
            {

                ShapefileFeatureTable myShapefile = await ShapefileFeatureTable.OpenAsync(file_shapefile_location);
              
                // Create a feature layer to display the shapefile
                FeatureLayer newFeatureLayer = new FeatureLayer(myShapefile);

                string file_to_stylx = parent.FullName + "\\Data\\style.stylx";
                symbolStyle = await SymbolStyle.OpenAsync(file_to_stylx);


                 IList<String> stringList = new List<string>();
                 stringList.Add("shoppingcenter01");
                 stringList.Add("marker01");


                 SimpleRenderer simpleRenderer = new SimpleRenderer(await symbolStyle.GetSymbolAsync(stringList));
                 newFeatureLayer.Renderer = simpleRenderer;             


                // Add the feature layer to the map
                myMap.OperationalLayers.Add(newFeatureLayer);

                // Zoom the map to the extent of the shapefile

                MyMapView.Map = myMap;
                await MyMapView.SetViewpointGeometryAsync(newFeatureLayer.FullExtent);
      

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

         }




        // Map initialization logic is contained in MapViewModel.cs
    }
}
