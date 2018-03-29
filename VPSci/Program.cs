using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VPSci;


namespace VPSci
{
    class Program
    {
        static void Main(string[] args)
        {
            int speed = 0;
            bool bRet = true;
            string errStr = string.Empty;
            Shaker ShakerDevice = new Shaker();
            if (args[0].ToLower() == "start")
            {
                if (args.GetUpperBound(0) > 0)
                {
                    try
                    {
                        speed = Convert.ToInt32(args[1]);
                        ShakerDevice.Start(speed);
                    }
                    catch (Exception)
                    {
                        errStr = "Could not convert speed value";
                    }
                }
            }
            if (args[0].ToLower() == "setport")
            {
                if (args.GetUpperBound(0) > 0)
                {
                    foreach (string item in args)
                    {
                        if (item.ToLower().StartsWith("com"))
                        {
                            bRet = ShakerDevice.SetComport(item);
                        }
                    }

                }
            }
        }
    }
}
