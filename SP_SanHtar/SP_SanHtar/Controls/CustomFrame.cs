using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SP_SanHtar.Controls
{
   public class CustomFrame : Frame
    {
        public static BindableProperty ElevationProperty = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(CustomFrame), 4.0f);

        public float Elevation
        {
            get { return (float)GetValue(ElevationProperty); }
            set { SetValue(ElevationProperty, value); }
        }
    }
}
