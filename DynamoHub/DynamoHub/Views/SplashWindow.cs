using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Resources;

namespace DynaHub.Views
{
    class SplashWindow
    {
        private Form form = null;

        public SplashWindow()
        {
            form = new Form();

            form.Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.30);
            form.Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.12);

            Uri uri = new Uri("pack://application:,,,/DynaHub;component/Resources/verification.png", UriKind.RelativeOrAbsolute);

            StreamResourceInfo info = System.Windows.Application.GetResourceStream(uri);

            Image myImage = Image.FromStream(info.Stream);

            form.BackgroundImage = myImage;

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
