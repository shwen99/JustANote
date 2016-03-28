using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JustANote
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static readonly Guid CLSID_ActiveDesktop = new Guid("{75048700-EF1F-11D0-9888-006097DEACF9}");

        public static IActiveDesktop GetActiveDesktop()
        {
            Type typeActiveDesktop = Type.GetTypeFromCLSID(CLSID_ActiveDesktop);
            return (IActiveDesktop)Activator.CreateInstance(typeActiveDesktop);
        }

        public static void Test()
        {
            var sb = new StringBuilder(1024);

            var ad = GetActiveDesktop();
            var ret = ad.GetWallpaper(sb, 1024, Constants.AD_GETWP_LAST_APPLIED);

            Console.WriteLine(ret);
            Console.WriteLine(sb);

            //ret = ad.SetWallpaper(@"C:\Windows\Web\Wallpaper\Theme1\img1.jpg", 0);
            ret = ad.SetWallpaper(@"C:\Windows\web\wallpaper\Windows\img0.jpg", 0);
            Console.WriteLine(ret);

            ret = ad.ApplyChanges(AD_Apply.ALL);
            Console.WriteLine(ret);

            ret = ad.GetWallpaper(sb, 1024, Constants.AD_GETWP_LAST_APPLIED);
            Console.WriteLine(ret);
            Console.WriteLine(sb);

        }
    }
}
