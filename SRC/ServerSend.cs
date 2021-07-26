using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ChessServer
{
    public class ServerSend
    {
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                SendTCPData(_toClient, _packet);
            }
        }
        public static void Move(int _fromClient, short _move)
        {
            using (Packet _packet = new Packet((int)ServerPackets.move))
            {
                _packet.Write(_move);
                _packet.Write(_fromClient);
                SendTCPDataToAll(_packet); //TODO All except sender
            }
        }
        public static void Fen(int _fromClient, string _fen, bool _isWhite)
        {
            using (Packet _packet = new Packet((int)ServerPackets.fen))
            {
                _packet.Write(_fen);
                _packet.Write(_fen.Length);
                _packet.Write(_isWhite);
                _packet.Write(_fromClient);
                SendTCPDataToAll(_packet); //TODO All except sender
            }
        }

        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            foreach (int id in Server.clients.Keys)
            {
                _packet.WriteLength();
                Server.clients[id].tcp.SendData(_packet);

            }
        }

        private static void SendTCPDataToAllExcept(int _exceptClient, Packet _packet)
        {
            foreach (int id in Server.clients.Keys)
            {
                if (id != _exceptClient)
                {
                    _packet.WriteLength();
                    Server.clients[id].tcp.SendData(_packet);
                }

            }
        }
    }
}