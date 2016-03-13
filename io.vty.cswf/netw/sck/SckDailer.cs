using io.vty.cswf.log;
using io.vty.cswf.netw.r;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace io.vty.cswf.netw.sck
{
    public class SckDailer
    {
        private static readonly ILog L = Log.New();
        public string Addr;
        public short Port;
        public SckDailer(string addr, short port)
        {
            this.Addr = addr;
            this.Port = port;
        }
        public NetwBase Dail()
        {
            L.D("Sck start connect to server({0}:{1}", this.Addr, this.Port);
            TcpClient client = new TcpClient();
            client.Connect(this.Addr, this.Port);
            Stream stream = client.GetStream();
            L.D("Sck connect to server({0}:{1} success", this.Addr, this.Port);
            return new NetwBaseImpl(stream);
        }
    }
}
