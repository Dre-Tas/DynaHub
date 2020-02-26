using System;
using System.Collections.Generic;
using System.IO;
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
            // This happens only after checking if ini and dll exist
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(IniConfigInfo.GetDllPath());
            Assembly assembly = Assembly.Load(assemblyName);
            // Get Decrypting method from dll
            Type type = null;
            if (IniConfigInfo.GetDllClass() != null)
                type = assembly.GetType(IniConfigInfo.GetDllClass());

            MethodInfo method = null;
            if (IniConfigInfo.GetDllMethod() != null && type != null)
                    method = type.GetMethod(IniConfigInfo.GetDllMethod());

            if (method != null)
            {
                return Convert.ToString(method.Invoke(null,
                    new object[] { stringFromCredManager }));
            }
            else
            {
                return null;
            }
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
