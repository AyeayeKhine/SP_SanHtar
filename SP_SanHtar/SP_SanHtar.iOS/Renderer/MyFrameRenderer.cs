using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using SP_SanHtar.Controls;
using SP_SanHtar.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(MyFrameRenderer))]
namespace SP_SanHtar.iOS.Renderer
{
    public class MyFrameRenderer : FrameRenderer
    {
        public static void Initialize()
        {
            // empty, but used for beating the linker
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;
            UpdateShadow();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Elevation")
            {
                UpdateShadow();
            }
        }

        public void UpdateShadow()
        {

            var materialFrame = (CustomFrame)Element;

            // Update shadow to match better material design standards of elevation
            Layer.ShadowRadius = materialFrame.Elevation;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(-2, 2);
            Layer.ShadowOpacity = 0.80f;
            //Layer.ShadowPath = UIBezierPath.FromRect(Layer.ContentsRect).CGPath;
            Layer.MasksToBounds = false;

        }
    }
}