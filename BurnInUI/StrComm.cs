using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace BurnInUI
{
    public class StrComm
    {
        private SerialPort com = null;
        public StrComm(string portName, int baudrate=19200)
        {
            this.com = new SerialPort();
            this.com.BaudRate = baudrate;
            this.com.StopBits = StopBits.One;
            this.com.DataBits = 8;
            this.com.Parity = Parity.None;
            this.com.PortName = portName;
            this.com.ReadTimeout = 5000;
        }
        public string Query(string cmd, int sleep=0)
        {
            for (int k = 0; k < 3; k++)
            {
                try
                {
                    if (this.com.IsOpen == false)
                    {
                        this.com.Open();
                    }
                    this.com.Write(cmd);
                    bool receive = false;
                    this.com.DataReceived += (sender, e) => { receive = true; };
                    int cnt = 0;
                    while (!receive)
                    {
                        System.Threading.Thread.Sleep(20);
                        Application.DoEvents();
                        cnt += 20;
                        if (cnt > 5000)
                        {
                            throw new Exception("Response Timeout 5s.");
                        }
                    }
                    System.Threading.Thread.Sleep(sleep);
                    return this.com.ReadExisting();
                }
                catch
                {
                    
                }
                finally
                {
                    this.com.Close();
                }
            }
            throw new Exception(this.com.PortName+" error.");
        }
    }
}
