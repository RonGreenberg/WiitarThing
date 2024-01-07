using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WiitarThing
{
    public class Program
    {
        private const string Unique = "wiitarthing-instance";

        [STAThread]
        public static void Main()
        {
            System.Threading.Thread.Sleep(250);

            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                // run the WiitarThing process in High priority, to prevent occasional delays in reading data from the guitar
                //System.Diagnostics.Process myProcess = System.Diagnostics.Process.GetCurrentProcess();
                //myProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
                //myProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;

                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                App.Main();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            //MessageBox.Show("WiitarThing is about to crash. Press OK to show the crash message.", "Sorry", MessageBoxButton.OK, MessageBoxImage.Error);

            Exception e = (Exception)args.ExceptionObject;

            WiitarDebug.Log($"ERROR:\n----------------------------------------\n{e.ToString()}\n----------------------------------------", WiitarDebug.LogLevel.Error);

            var box = new ErrorWindow(e);
            box.ShowDialog();

            SingleInstance<App>.Cleanup();
            //Current.Dispatcher.Invoke(new Action(() => 
            //{
            //    var box = new ErrorWindow(e);
            //    box.ShowDialog();
            //}));
        }
    }
}
