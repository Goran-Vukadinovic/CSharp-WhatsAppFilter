#region using

using System;
using System.Runtime.InteropServices;

#endregion

namespace Blaster
{
    internal class _Program
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();
        [STAThread]
        public static void Main()
        {
            try
            {
                //AllocConsole();
                ApplicationManager.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[!] Exception :\n" + ex);
            }
        }
    }
}