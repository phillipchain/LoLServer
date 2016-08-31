﻿using LeagueSandbox.GameServer.Core.Logic.PacketHandlers;
using LeagueSandbox.GameServer.Logic;
using LeagueSandbox.GameServer.Logic.Enet;
using LeagueSandbox.GameServer.Logic.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LeagueSandbox
{
    static class Extensions
    {
        public static float SqrLength(this Vector2 v)
        {
            return (v.X * v.X + v.Y * v.Y);
        }
        public static long ElapsedNanoSeconds(this Stopwatch watch)
        {
            return watch.ElapsedTicks * 1000000000 / Stopwatch.Frequency;
        }
        public static long ElapsedMicroSeconds(this Stopwatch watch)
        {
            return watch.ElapsedTicks * 1000000 / Stopwatch.Frequency;
        }
        public static void Add(this List<byte> list, int val)
        {
            list.AddRange(PacketHelper.intToByteArray(val));
        }
        public static void Add(this List<byte> list, string val)
        {
            list.AddRange(Encoding.BigEndianUnicode.GetBytes(val));
        }
        public static void fill(this BinaryWriter list, byte data, int length)
        {
            for (var i = 0; i < length; ++i)
            {
                list.Write(data);
            }
        }
    }

    static class EnumParser
    {
        public static SummonerSpellIds ParseSummonerSpell(string spellName)
        {
            if (spellName == "FLASH")
            {
                return SummonerSpellIds.SPL_Flash;
            }
            else if (spellName == "IGNITE")
            {
                return SummonerSpellIds.SPL_Ignite;
            }
            else if (spellName == "HEAL")
            {
                return SummonerSpellIds.SPL_Heal;
            }
            else if (spellName == "BARRIER")
            {
                return SummonerSpellIds.SPL_Barrier;
            }
            else if (spellName == "SMITE")
            {
                return SummonerSpellIds.SPL_Smite;
            }
            else if (spellName == "GHOST")
            {
                return SummonerSpellIds.SPL_Ghost;
            }
            else if (spellName == "REVIVE")
            {
                return SummonerSpellIds.SPL_Revive;
            }
            else if (spellName == "CLEANSE")
            {
                return SummonerSpellIds.SPL_Cleanse;
            }
            else if (spellName == "TELEPORT")
            {
                return SummonerSpellIds.SPL_Teleport;
            }

            return 0;
        }
    }

    public class PairList<TKey, TValue> : List<Pair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            Add(new Pair<TKey, TValue>(key, value));
        }
        public bool ContainsKey(TKey key)
        {
            foreach (var v in this)
                if (v.Item1.Equals(key))
                    return true;

            return false;
        }
        public TValue this[TKey key]
        {
            get
            {
                foreach (var v in this)
                    if (v.Item1.Equals(key))
                        return v.Item2;

                return default(TValue);
            }
            set
            {
                foreach (var v in this)
                    if (v.Item1.Equals(key))
                        v.Item2 = value;
            }
        }
    }
    public class CustomConvert
    {
        public static TeamId toTeamId(int i)
        {
            switch (i)
            {
                case 0:
                case (int)TeamId.TEAM_BLUE:
                    return TeamId.TEAM_BLUE;
                case 1:
                case (int)TeamId.TEAM_PURPLE:
                    return TeamId.TEAM_PURPLE;
                default:
                    return (TeamId)2;
            }
        }
        public static int fromTeamId(TeamId team)
        {
            switch (team)
            {
                case TeamId.TEAM_BLUE:
                    return 0;
                case TeamId.TEAM_PURPLE:
                    return 1;
                default:
                    return 2;
            }
        }

        public static TeamId getEnemyTeam(TeamId team)
        {
            switch (team)
            {
                case TeamId.TEAM_BLUE:
                    return TeamId.TEAM_PURPLE;
                case TeamId.TEAM_PURPLE:
                    return TeamId.TEAM_BLUE;
            }
            return (TeamId)2;
        }
    }
}
