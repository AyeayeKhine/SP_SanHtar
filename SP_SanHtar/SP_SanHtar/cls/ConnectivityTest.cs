using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace SP_SanHtar.cls
{
    public class ConnectivityTest
    {
        public ConnectivityTest()
        {
            var current = Connectivity.NetworkAccess;

            switch (current)
            {
                case NetworkAccess.Internet:
                    // Connected to internet
                    break;
                case NetworkAccess.Local:
                    // Only local network access
                    break;
                case NetworkAccess.ConstrainedInternet:
                    // Connected, but limited internet access such as behind a network login page
                    break;
                case NetworkAccess.None:
                    // No internet available
                    break;
                case NetworkAccess.Unknown:
                    // Internet access is unknown
                    break;
            }
        }
        public void StartListening()
        {
            // Register for connectivity changes
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public void StopListening()
        {
            // Un-register listener for changes
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            // Update UI or notify the user
        }
    }
}
