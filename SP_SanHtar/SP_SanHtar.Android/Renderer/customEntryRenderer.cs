using System.ComponentModel;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SP_SanHtar.Controls;
using SP_SanHtar.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(customEntry), typeof(customEntryRenderer))]
namespace SP_SanHtar.Droid.Renderer
{
    class customEntryRenderer : EntryRenderer
    {
        public customEntryRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (this.Element == null) return;
            if (Control != null)
            {
                // Control.KeyPress += CustomEntryRenderer_KeyPress;
                Control.Background = Resources.GetDrawable(Resource.Drawable.noBorderEditText);
                //Control.SetBackgroundColor(Android.Graphics.Color.White);
                //var font = Typeface.CreateFromAsset(this.Context.Assets, "fonts/sukhumvit_set.ttf");
                //Control.Typeface = font;
                //Control.SetPadding(0, 2, 0, 0);
                Control.SetPadding(0, 20, 0, 20);
                string Styleid = e.NewElement?.StyleId;
                var label = (TextView)Control; // for example
                //if (Styleid == "kanitmedium")
                //{
                //    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "kanit-medium.otf");
                //    label.Typeface = font;
                //}
                //if (Styleid == "kanitregular")
                //{
                //    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "kanit-regular.otf");
                //    label.Typeface = font;
                //}
                //if (Styleid == "SukhumvitSetBold")
                //{
                //    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "SukhumvitSet-Bold.ttf");
                //    label.Typeface = font;
                //}
                //if (Styleid == "SukhumvitSetLight")
                //{
                //    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "SukhumvitSet-Light.ttf");
                //    label.Typeface = font;
                //}
                //if (Styleid == "SukhumvitSetMedium")
                //{
                //    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "SukhumvitSet-Medium.ttf");
                //    label.Typeface = font;
                //}
                //if (Styleid == "SukhumvitSetSemiBold")
                //{
                //    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "SukhumvitSet-SemiBold.ttf");
                //    label.Typeface = font;
                //}
                //if (Styleid == "SukhumvitSetText")
                //{
                //    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "SukhumvitSet-Text.ttf");
                //    label.Typeface = font;
                //}
                //if (Styleid == "SukhumvitSetThin")
                //{
                //    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "SukhumvitSet-Thin.ttf");
                //    label.Typeface = font;
                //}

            }
            //if (e.NewElement != null)
            //{
            //    this.Control.ShowSoftInputOnFocus = false;
            //}
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
            {
                Control.Background = Resources.GetDrawable(Resource.Drawable.noBorderEditText);
                //Control.SetBackgroundColor(Android.Graphics.Color.White);
                //var font = Typeface.CreateFromAsset(this.Context.Assets, "fonts/sukhumvit_set.ttf");
                //Control.Typeface = font;
                //Control.SetPadding(0, 2, 0, 0);
            }
        }

        void CustomEntryRenderer_KeyPress(object sender, global::Android.Views.View.KeyEventArgs e)
        {
           
        }
    }
}