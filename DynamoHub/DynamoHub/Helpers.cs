using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CredentialManagement;
using System.Reflection;

namespace DynaHub
{
    class Helpers
    {
        internal static void ErrorMessage(string message)
        {
            MessageBox.Show(message,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        internal static void InfoMessage(string message)
        {
            MessageBox.Show(message,
                "FYI",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        internal static void SuccessMessage(string message)
        {
            AutoClosingMessageBox.Show(message, "Success", 3000);
        }

        internal static string GetTokenFromCredManager()
        {
            // Create credential object targeting the DynaHub credential in Credential Manager
            Credential credential = new Credential { Target = "DynaHub" };

            if (!credential.Exists()) return null;

            // Load ("open") the credential object to get the properties
            credential.Load();

            return credential.Password;
        }

        internal static string DecryptToken(string stringFromCredManager)
        {
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(GlobalSettings.decryptionDll);

            Assembly assembly = Assembly.Load(assemblyName);
            Type type = assembly.GetType("DynaHub_Crypto.Crypto");
            MethodInfo method = type.GetMethod("DecryptString");

            //TODO: Get rid of pass phrase
            return Convert.ToString(method.Invoke(null,
                new object[] { stringFromCredManager, "justtesting" }));
        }
    }

    // From https://stackoverflow.com/questions/23692127/auto-close-message-box
    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            MessageBox.Show(text, caption);
        }

        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }

        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow(null, _caption);
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
