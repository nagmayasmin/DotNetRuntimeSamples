using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;



namespace Search_MVVM
{
    public class MapViewModel : INotifyPropertyChanged
    {
        string uri = "http://sampleserver6.arcgisonline.com/arcgis/rest/services/PoolPermits/FeatureServer/0";

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SearchCommand { get { return new SearchCommand(this); } }
        public FeatureLayer featureLayer;

        #region Properties

        private string _searchText;
        public string SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                _searchText = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchText"));
            }
        }

        private Map _map;
        public Map Map
        {
            get
            { return _map; }

            set
            {
                _map = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Map"));
            }
        }

        private Feature _feature;
        public Feature SelectedFeature
        {
            get { return _feature; }
            set
            {
                _feature = value;
                DoSelectFeature(_feature);
            }
        }

        private List<Feature> _featureList;
        public List<Feature> FeatureList
        {
            get { return _featureList; }

            set
            {
                _featureList = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FeatureList"));
            }
        }

        #endregion

        #region Constructor

        public MapViewModel()
        {
            Map = new Map(Basemap.CreateOpenStreetMap());
            LoadFeatureLayer();
            SearchText = "pool_permit = 0";
        }

        #endregion

        private async void LoadFeatureLayer()
        {
            MapPoint centralPoint = new MapPoint(-117.452684, 33.903562, SpatialReferences.Wgs84);
            Viewpoint startingViewpoint = new Viewpoint(centralPoint, 10000);

            Map.InitialViewpoint = startingViewpoint;
            featureLayer = new FeatureLayer(new Uri(uri));
            await featureLayer.LoadAsync();
            Map.OperationalLayers.Add(featureLayer);

        }


        private void DoSelectFeature(Feature feature)
        {
            featureLayer.ClearSelection();
            if (feature != null)
            {
                featureLayer.SelectFeature(feature);
                MessageBox.Show("Address of the parcel: " + _feature.Attributes["Address"].ToString());

            }
        }

        public async void DoSearch()
        {
            QueryParameters queryParameters = new QueryParameters()
            {
                WhereClause = SearchText
            };

            ServiceFeatureTable serviceFeatureTable = featureLayer.FeatureTable as ServiceFeatureTable;
            var featureQueryResults = await serviceFeatureTable.QueryFeaturesAsync(queryParameters, QueryFeatureFields.LoadAll);

            FeatureList = featureQueryResults.ToList();

         }
    }

    #region SearchCommand

    public class SearchCommand : ICommand
    {
        MapViewModel _vm = new MapViewModel();

        public SearchCommand(MapViewModel vm)
        {
            _vm = vm;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.DoSearch();
        }
    }

    #endregion
}
