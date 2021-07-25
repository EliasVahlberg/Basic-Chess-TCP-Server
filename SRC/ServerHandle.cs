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
    }
}