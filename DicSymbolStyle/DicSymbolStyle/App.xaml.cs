using Esri.ArcGISRuntime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DicSymbolStyle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                // Deployed applications must be licensed at the Lite level or greater. 
                // See https://developers.arcgis.com/licensing for further details.

                // Initialize the ArcGIS Runtime before any components are created.
                ArcGISRuntimeEnvironment.Initialize();

                string liteLicenseKey = "runtimelite,1000,rud5725037879,none,NKMFA0PL4PL9LMZ59253";
              //  Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.SetLicense(liteLicenseKey);

                string licenseKey = "runtimebasic,1000,rud000532390,none,A3CZ04SZ8Y8JLMZ59156";
               // Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.SetLicense(licenseKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ArcGIS Runtime initialization failed.");

                // Exit application
                this.Shutdown();
            }
        }
    }
}
