using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace XMLConvertTool
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private static MainWindow mainWin;
        public static MainWindow MainWin
        {
            get { return mainWin; }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Process[] myProcesses = Process.GetProcessesByName("XMLConvertTool");
                if (myProcesses.Length > 1)
                {
                    MessageBox.Show("프로그램이 이미 실행중입니다");
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {

            }
            mainWin = XMLConvertTool.MainWindow.MainWinObj;
            mainWin.Show();
        }
    }
}
