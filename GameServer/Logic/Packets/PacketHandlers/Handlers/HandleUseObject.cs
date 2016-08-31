﻿using ENet;
using LeagueSandbox.GameServer.Logic.Packets;
using LeagueSandbox.GameServer.Logic.Players;
using Ninject;

namespace LeagueSandbox.GameServer.Core.Logic.PacketHandlers.Packets
{
    class HandleUseObject : IPacketHandler
    {
        private Logger _logger = Program.ResolveDependency<Logger>();
        private PlayerManager _playerManager = Program.ResolveDependency<PlayerManager>();

        public bool HandlePacket(Peer peer, byte[] data)
        {
            var parsedData = new UseObject(data);
            _logger.LogCoreInfo("Object " + _playerManager.GetPeerInfo(peer).GetChampion().getNetId() + " is trying to use (right clicked) " + parsedData.targetNetId);

            return true;
        }
    }
}
