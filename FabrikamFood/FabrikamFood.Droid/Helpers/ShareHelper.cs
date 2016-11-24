using System;
using Android.Content;
using FabrikamFood.Droid.Helpers;
using Xamarin.Forms;
using Android.Graphics;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(ShareHelper))]
namespace FabrikamFood.Droid.Helpers
{
    public class ShareHelper : FabrikamFood.Helpers.IShareable
    {
        public void Share(string message)
        {
            throw new NotImplementedException();
        }

        public void Share(string title, string content)
        {
            var myIntent = new Intent(Android.Content.Intent.ActionSend);
            myIntent.SetType("text/plain");
            myIntent.PutExtra(Intent.ExtraText, content);
            Forms.Context.StartActivity(Intent.CreateChooser(myIntent, title));
        }

        public void Share(string title, string content, string picName)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)|| string.IsNullOrEmpty(picName))
                return;
            int resourceId = (int)typeof(Resource.Drawable).GetField(picName).GetValue(null);

            Bitmap b = BitmapFactory.DecodeResource(Forms.Context.Resources, resourceId);

            var tempFilename = "temp.png";
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(sdCardPath, tempFilename);
            using (var os = new FileStream(filePath, FileMode.Create))
            {
                b.Compress(Bitmap.CompressFormat.Jpeg, 80, os);
            }
            b.Dispose();

            var imageUri = Android.Net.Uri.Parse($"file://{sdCardPath}/{tempFilename}");
            var sharingIntent = new Intent();
            sharingIntent.SetAction(Intent.ActionSend);
            sharingIntent.SetType("image/*");
            sharingIntent.PutExtra(Intent.ExtraText, content);
            sharingIntent.PutExtra(Intent.ExtraStream, imageUri);
            sharingIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
            Forms.Context.StartActivity(Intent.CreateChooser(sharingIntent, title));
        }
    }
}