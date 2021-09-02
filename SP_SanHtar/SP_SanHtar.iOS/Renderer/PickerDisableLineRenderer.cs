using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SP_SanHtar.Controls;
using SP_SanHtar.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(customDropdown), typeof(PickerDisableLineRenderer))]
namespace SP_SanHtar.iOS.Renderer
{
    class PickerDisableLineRenderer : PickerRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (this.Element == null) return;
            if (Control != null)
            {
                Picker ele = (Picker)Element;
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Font = UIFont.FromName("Sukhumvit Set", float.Parse(ele.FontSize.ToString()));
            }
        }

    }
}