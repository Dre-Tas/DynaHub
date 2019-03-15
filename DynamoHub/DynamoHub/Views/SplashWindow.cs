using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Resources;

namespace DynaHub.Views
{
    class SplashWindow
    {
        private Form form = null;

        public SplashWindow(Uri uri)
        {
            form = new Form();

            StreamResourceInfo info = System.Windows.Application.GetResourceStream(uri);

            Image splashImage = Image.FromStream(info.Stream);

            // Get image dimensions
            form.Width = splashImage.Width;
            form.Height = splashImage.Height;            

            form.BackgroundImage = splashImage;

            form.BackgroundImageLayout = ImageLayout.Stretch;

            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.None;
            form.TopMost = true;

            form.Show();
        }

        internal void CloseSplash()
        {
            form.Close();
        }
    }
}
