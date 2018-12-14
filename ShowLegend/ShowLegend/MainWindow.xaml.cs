using System;
using System.Linq;
using System.Windows;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;

namespace ShowLegend
{
   
    public partial class MainWindow : Window
    {
       private string imageLayerUri = "https://maps.raleighnc.gov/arcgis/rest/services/Sustainable/MapServer/";

        public MainWindow()
        {
            InitializeComponent();
             MyMapView.Loaded += MyMapView_Loaded;
         
        }

        private async void MyMapView_Loaded(object sender, RoutedEventArgs e)
        {
            Map myMap = new Map();
            myMap.Basemap = Basemap.CreateStreets();

            MyMapView.Map = myMap;
            await MyMapView.SetViewpointGeometryAsync(new Envelope(-78.691528, 35.799884, -78.601835, 35.760055, SpatialReferences.Wgs84));             

             ArcGISMapImageLayer imageLayer = new ArcGISMapImageLayer(new Uri(imageLayerUri));
             await imageLayer.LoadAsync();
             myMap.OperationalLayers.Add(imageLayer);

            for (int i = 0; i < imageLayer.Sublayers.Count; i++)
                 {
                     var legendLayer = imageLayer.SublayerContents[i];
                     legendLayer.ShowInLegend = true;

                     var layerLegendInfo = await legendLayer.GetLegendInfosAsync();
              
               
                foreach (var l in layerLegendInfo.ToList())
                {
                    legendTree.Items.Add(l);
                }
            }

        }
        
    }
}
