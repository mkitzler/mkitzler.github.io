using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToeGame;

namespace SignalRTest.Server.Hubs
{
    public class TTTHub : Hub
    {
        private List<string> games = new List<string>();
        private Dictionary<string, string> connWith = new Dictionary<string, string>();

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (connWith.ContainsKey(Context.ConnectionId))
            {
                await Clients.Client(connWith[Context.ConnectionId]).SendAsync("Disconnected", Context.ConnectionId);
                connWith.Remove(connWith[Context.ConnectionId]);
                connWith.Remove(Context.ConnectionId);
            }
        }

        public async Task CreateGame()
        {
            await Clients.All.SendAsync("GameListUpdate", games);
        }

        public async Task GetGames()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("GameListUpdate", games);
        }

        #region Connection
        public async Task JoinGame(string ConnectionId)
        {
            if (ConnectionId != Context.ConnectionId)
                await Clients.Client(ConnectionId).SendAsync("PlayerJoining", Context.ConnectionId);
        }
        public async Task AcceptJoin(string ConnectionId)
        {
            await Clients.Client(ConnectionId).SendAsync("JoinAccepted", Context.ConnectionId);
        }
        /*public async Task Ping(string ConnectionId)
        {
            await Clients.Client(ConnectionId).SendAsync("PingRecv", Context.ConnectionId);
        }
        public async Task ACK(string ConnectionId)
        {
            await Clients.Client(ConnectionId).SendAsync("ACKRecv", Context.ConnectionId);
        }*/
        public async Task Disconnect(string ConnectionId)
        {
            /*if (connWith.ContainsKey(Context.ConnectionId))   //TODO: hinzufügen, entfernen
            {
                await Clients.Client(connWith[Context.ConnectionId]).SendAsync("Disconnected", Context.ConnectionId);
                connWith.Remove(connWith[Context.ConnectionId]);
                connWith.Remove(Context.ConnectionId);
            }*/
            await Clients.Client(ConnectionId).SendAsync("Disconnected", Context.ConnectionId);
        }
        #endregion

        #region Game
        public async Task Reset(string ConnectionId, Player currentPlayer)
        {
            await Clients.Client(ConnectionId).SendAsync("Reset", Context.ConnectionId, currentPlayer);
        }
        public async Task MakeMove(string ConnectionId, int x, int y)
        {
            await Clients.Client(ConnectionId).SendAsync("MadeMove", Context.ConnectionId, x, y);
        }
        #endregion
    }
}
