﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueSandbox.GameServer.Logic.Packets.PacketDefinitions.Requests
{
    public class AutoAttackOptionRequest
    {
        public int NetId { get; }
        public bool Activated { get; }

        public AutoAttackOptionRequest(int netId, bool activated)
        {
            NetId = netId;
            Activated = activated;
        }
    }
}
