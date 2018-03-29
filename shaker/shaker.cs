using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Xml.XPath;


namespace VPSci
{
    public class Shaker
    {
        SerialPort sp = new SerialPort();
        private static string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private string assemblyFilename = Path.GetFileNameWithoutExtension(assemblyLocation);
        private string assemblyPath = Path.GetDirectoryName(assemblyLocation);
        public string outTerminator { get; private set; }
        public string inTerminator { get; private set; }
        public string defaultComPort { get; private set; }
                
        public Shaker()
        {
            OpenConfigFile();
        }
        public void Start(int speed)
        {
            if (sp.IsOpen)
            {
                sp.Write($"s{speed.ToString().PadLeft(3,'0')}{outTerminator}");
            }
        }
        private void OpenConfigFile()
        {
            string filename = Path.Combine(assemblyPath, "shaker.xml");
            if (File.Exists(filename))
            {
                XDocument xConfigDoc = XDocument.Load(filename);
                outTerminator = GetElementValue(xConfigDoc, "outTerminator");
                outTerminator = DecodeTerminator(outTerminator);
                inTerminator = GetElementValue(xConfigDoc, "inTerminator");
                defaultComPort = GetElementValue(xConfigDoc, "lastComPort");
            }
            
        }

        private string DecodeTerminator(string outTerminator)
        {
            string returnValue = string.Empty;
            switch (outTerminator)
            {
                case "cr":
                    returnValue = '\r'.ToString();
                    break;
                case "lf":
                    returnValue = '\n'.ToString();
                    break;
                case "crlf":
                    returnValue = string.Format("\r\n");
                    break;
                default:
                    break;
            }

            return returnValue;
        }

        private string GetElementValue(XDocument xConfigDoc, string elementName)
        {
            IEnumerable<XElement> rows =
                from row in xConfigDoc.Descendants(elementName)
                select row;

            foreach (XElement xEle in rows)
            {
                if (elementName == xEle.Name)
                {
                    return xEle.Value;
                }
            }

            return null;
        }

        public bool SetComport(string comport, int baudRate = 9600, Parity par = Parity.None, int databits = 8, StopBits sb = StopBits.One)
        {
            
            string[] ports = SerialPort.GetPortNames();
            if (!ports.Contains<string>(comport))
            {
                Console.Write($"Cannot open comport {comport}, not valid on this computer");
            }

            sp = new SerialPort(comport, baudRate, par, databits, sb);

            sp.Handshake = Handshake.None;

            sp.Open();

            return sp.IsOpen;

        }

    }
}
