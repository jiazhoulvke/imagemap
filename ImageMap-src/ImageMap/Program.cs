using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ImageMap
{
    static class Program
    {
        public static int linkNum = 0;
        public static string result = "";
        public static string filename = "";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static int Main(string[] argv)
        {
            if (argv.Length > 0)
            {
                if (argv[0].Length > 0)
                    filename = argv[0];
                else
                    return 1;
            }
            else
            {
                return 1;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            if (linkNum > 0)
            {
                Console.Write(result);
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
