using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Models
{
    public class ProxyIns
    {
        CancellationTokenSource ctsTcp = new CancellationTokenSource();
        TcpListener tcpListener ;
        List<TcpClient> ToClients ;
        IpPort ipPort = null;
        
        public async Task StartTcpProxyAsync(IpPort pIpPort)
        {
            ipPort = pIpPort;
            try
            {
                if (ipPort.ToIps.Count!=ipPort.ToPorts.Count ||ipPort.ToPorts.Count==0)
                {
                    return;
                }
                if (tcpListener == null)
                {
                    if (string.IsNullOrEmpty(ipPort.FromIp))
                    {
                        tcpListener = new TcpListener(IPAddress.Any, ipPort.FromPort);
                    }
                    else
                    {
                        IPAddress ipa = null;
                        if (IPAddress.TryParse(ipPort.FromIp, out ipa))
                        {
                            tcpListener = new TcpListener(ipa, ipPort.FromPort);
                        }
                    }
                }
                if (tcpListener==null)
                {
                    return;
                }
                ToClients = new List<TcpClient>();
                for (int i = 0; i < ipPort.ToPorts.Count; i++)
                {
                    TcpClient toClient = new TcpClient();
                    ToClients.Add(toClient);
                    toClient.Connect(ipPort.ToIps[i], ipPort.ToPorts[i]);
                }
                tcpListener.Start();
                UtilesHelper.CWshow("Tcp开始侦听："+ipPort.FromIp+":"+ipPort.FromPort);
                var tocken= ctsTcp.Token;
                tcpListener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback),tcpListener);


                while (!tocken.IsCancellationRequested)
                {
                    TcpClient client =await tcpListener.AcceptTcpClientAsync();
                    for (int i = 0; i < ToClients.Count; i++)
                    {
                        if (!ToClients[i].Connected)
                        {
                            ToClients[i].ConnectAsync(ipPort.ToIps[i], ipPort.ToPorts[i]);
                        }
                    }
                    ProxyTcp(client);
                }
                tcpListener.Stop();
            }
            catch (Exception ex)
            {
                UtilesHelper.Error(ex);
                UtilesHelper.CWshow("StartTcpProxy-" + ex.Message);
            }
            
        }

        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(ar);

        }

        private void ProxyTcp(TcpClient client)
        {
            try
            {
                
                client.NoDelay = true;
                NetworkStream stream = client.GetStream();
                
                if (stream.CanRead)
                {
                    byte[] myReadBuffer = new byte[1024];
                    StringBuilder sb = new StringBuilder();
                    int numberOfBytesRead = 0;
                    do
                    {
                        numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                        foreach (var item in ToClients)
                        {
                            
                        }
                    } while (stream.DataAvailable);

                }
            }
            catch (Exception ex)
            {
                UtilesHelper.CWshow("ProxyTcp--" + ex.Message);
                //throw;
            }
        }
    }

    public class IpPort
    {
        public string FromIp;
        public int FromPort;
        public List<string> ToIps;
        public List<int> ToPorts;
    }
}
