using System;
using System.Net;
using System.Net.Sockets;


namespace ChessServer
{
    public class Client
    {
        public static int dataBufferSize = 4096;
        public int id;
        public TCP tcp;
        public Client(int _id)
        {
            id = _id;
            tcp = new TCP(id);
        }
        public class TCP
        {
            public TcpClient socket;
            private readonly int id;
            private NetworkStream stream;
            private Packet reccivePacket;
            private byte[] recceiveBuffer;

            public TCP(int _id)
            {
                id = _id;
            }
            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;
                stream = socket.GetStream();
                reccivePacket = new Packet();
                recceiveBuffer = new byte[dataBufferSize];
                stream.BeginRead(recceiveBuffer, 0, dataBufferSize, RecceiveCallback, null);
                //TODO Send welcome packet
                ServerSend.Welcome(id, "Welcome");
            }
            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    System.Console.WriteLine($"Exception during send data id:{id} , exception: {_ex}");
                    //Todo something
                }
            }
            private void RecceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        //TODO disconnect
                        return;
                    }
                    byte[] _data = new byte[_byteLength];
                    Array.Copy(recceiveBuffer, _data, _byteLength);
                    reccivePacket.Reset(HandleData(_data));

                    stream.BeginRead(recceiveBuffer, 0, dataBufferSize, RecceiveCallback, null);
                }
                catch (Exception _ex)
                {
                    System.Console.WriteLine($"Error recciving TCP data {_ex}");
                    //TODO disconnect
                }
            }
            private bool HandleData(byte[] _data)
            {
                int _packetLength = 0;
                reccivePacket.SetBytes(_data);
                if (reccivePacket.UnreadLength() >= 4)
                {
                    _packetLength = reccivePacket.ReadInt();
                    if (_packetLength <= 0)
                    { return true; }
                }
                while (_packetLength > 0 && _packetLength <= reccivePacket.UnreadLength())
                {
                    byte[] _packetBytes = reccivePacket.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            Server.packetHandlers[_packetId](id, _packet);
                        }
                    });
                    _packetLength = 0;
                    if (reccivePacket.UnreadLength() >= 4)
                    {
                        _packetLength = reccivePacket.ReadInt();
                        if (_packetLength <= 0)
                            return true;
                    }
                }
                if (_packetLength <= 1)
                {
                    return true;
                }
                return false;

            }
        }
    }
}