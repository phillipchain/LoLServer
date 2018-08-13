﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServerCore.Logic.Enums;

namespace GameServerCore.Logic.Domain.GameObjects
{
    public interface IMinion : IObjAiBase
    {
        MinionSpawnPosition SpawnPosition { get; }
        MinionSpawnType MinionSpawnType { get; }
    }
}
