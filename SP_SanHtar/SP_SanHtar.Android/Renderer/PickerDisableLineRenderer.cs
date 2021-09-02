using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SP_SanHtar.Controls;
using SP_SanHtar.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(customDropdown), typeof(PickerDisableLineRenderer))]
namespace SP_SanHtar.Droid.Renderer
{
    class PickerDisableLineRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (this.Element == null) return;
            if (Control != null)
            {
                Control.Background = Resources.GetDrawable(Resource.Drawable.noBorderEditText);
                //var font = Typeface.CreateFromAsset(this.Context.Assets, "fonts/sukhumvit_set.ttf");
                //Control.Typeface = font;
                Control.SetPadding(0, 0, 0, 0);
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Picker.TitleProperty.PropertyName)
            {
                Control.Background = Resources.GetDrawable(Resource.Drawable.noBorderEditText);
                //var font = Typeface.CreateFromAsset(this.Context.Assets, "fonts/sukhumvit_set.ttf");
                //Control.Typeface = font;
                Control.SetPadding(0, 0, 0, 0);
            }
        }
    }
}