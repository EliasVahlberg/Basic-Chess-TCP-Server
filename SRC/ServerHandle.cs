using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ChessServer
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientId = _packet.ReadInt();
            string _userName = _packet.ReadString();
            if (_fromClient != _clientId)
                System.Console.WriteLine($"Player: \" {_userName} \" (ID: {_fromClient} has missmatched ID ({_clientId})");
            System.Console.WriteLine($"Welcome received id: {_fromClient} , Name: {_userName} ");
            //TODO Send player to game
        }
        public static void FenSelect(Packet _packet)
        {
            int _id = _packet.ReadInt();
            int _fenL = _packet.ReadInt();
            string _fen = _packet.ReadString();
            bool _isWhite = _packet.ReadBool();
            System.Console.WriteLine($"Fen: \n{_fen}, From: {_id}, IsWhite: {_isWhite}");
            ServerSend.Fen(_id, _fen, _isWhite);
        }
        public static void MoveRequest(Packet _packet)
        {
            int _id = _packet.ReadInt();
            short _move = _packet.ReadShort();
            System.Console.WriteLine($"Move: \n{_move}, From: {_id}");
            ServerSend.Move(_id, _move);
        }
    }
}