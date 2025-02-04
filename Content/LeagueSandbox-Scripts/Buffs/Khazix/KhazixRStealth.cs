﻿using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore;

namespace Buffs
{
    internal class KhazixRStealth : IBuffGameScript
    {
        Buff Stealth;
        ObjAIBase Khazix;
        Particle Invisible;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Stealth = buff;
            Khazix = ownerSpell.CastInfo.Owner as Champion;
            StatsModifier.MoveSpeed.PercentBonus += 0.4f;
            Khazix.AddStatModifier(StatsModifier);
            OverrideAnimation(Khazix, "run_haste", "RUN");
            Invisible = AddParticleTarget(Khazix, Khazix, "khazix_base_r_invisible", Khazix, buff.Duration);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Invisible);
            OverrideAnimation(Khazix, "RUN", "run_haste");
            AddParticleTarget(Khazix, Khazix, "khazix_base_r_end", Khazix);
        }
    }
}