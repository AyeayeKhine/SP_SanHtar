using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using SP_SanHtar.Controls;
using SP_SanHtar.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(MyFrameRenderer))]
namespace SP_SanHtar.Droid.Renderer
{
    class MyFrameRenderer : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        //float _defaultElevation = -1f;
        //float _defaultCornerRadius = -1f;

        //bool _disposed;
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (Element == null) return;
            if (e.NewElement != null && e.OldElement == null)
            {
                UpdateShadow();
            }
        }


        private void UpdateShadow()
        {
            //if (_disposed)
            //    return;

            //float elevation = _defaultElevation;

            //if (elevation == -1f)
            //    _defaultElevation = elevation = CardElevation;

            //if (Element.HasShadow)
            //{
            //    CardElevation = _defaultElevation;
            //}
            //else
            //    CardElevation = 0f;
            var materialFrame = (CustomFrame)Element;

            // we need to reset the StateListAnimator to override the setting of Elevation on touch down and release.
            Control.StateListAnimator = new Android.Animation.StateListAnimator();

            // set the elevation manually
            ViewCompat.SetElevation(this, materialFrame.Elevation);
            ViewCompat.SetElevation(Control, materialFrame.Elevation);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Elevation")
            {
                UpdateShadow();
            }
        }
    }
}