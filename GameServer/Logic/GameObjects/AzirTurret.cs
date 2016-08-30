﻿using InibinSharp;
using LeagueSandbox.GameServer.Core.Logic;
using LeagueSandbox.GameServer.Core.Logic.RAF;
using LeagueSandbox.GameServer.Logic.Enet;

namespace LeagueSandbox.GameServer.Logic.GameObjects
{
    public class AzirTurret : Unit
    {
        private const float TURRET_RANGE = 905.0f;
        public string Name { get; private set; }
        public Unit Owner { get; private set; }
        private float _globalGold = 250.0f;
        private float _globalExp = 0.0f;

        public AzirTurret(Game game, Unit owner, uint id, string name, string model, float x = 0, float y = 0, TeamId team = TeamId.TEAM_BLUE)
               : base(game, id, model, new Stats(), 50, x, y, 1200)
        {
            this.Name = name;
            this.Owner = owner;

            BuildAzirTurret();

            setTeam(team);
        }

        public void BuildAzirTurret()
        {
            Inibin inibin;
            if (!RAFManager.getInstance().readInibin("DATA/Characters/" + model + "/" + model + ".inibin", out inibin))
            {
                Logger.LogCoreError("couldn't find turret stats for " + model);
                return;
            }

            stats.HealthPoints.BaseValue = inibin.getFloatValue("Data", "BaseHP");
            stats.CurrentHealth = stats.HealthPoints.Total;
            stats.ManaPoints.BaseValue = inibin.getFloatValue("Data", "BaseMP");
            stats.CurrentMana = stats.ManaPoints.Total;
            stats.AttackDamage.BaseValue = inibin.getFloatValue("DATA", "BaseDamage");
            stats.Range.BaseValue = TURRET_RANGE;//inibin.getFloatValue("DATA", "AttackRange");
            stats.MoveSpeed.BaseValue = inibin.getFloatValue("DATA", "MoveSpeed");
            stats.Armor.BaseValue = inibin.getFloatValue("DATA", "Armor");
            stats.MagicResist.BaseValue = inibin.getFloatValue("DATA", "SpellBlock");
            stats.HealthRegeneration.BaseValue = inibin.getFloatValue("DATA", "BaseStaticHPRegen");
            stats.ManaRegeneration.BaseValue = inibin.getFloatValue("DATA", "BaseStaticMPRegen");
            stats.AttackSpeedFlat = 0.625f / (1 + inibin.getFloatValue("DATA", "AttackDelayOffsetPercent"));

            stats.HealthPerLevel = inibin.getFloatValue("DATA", "HPPerLevel");
            stats.ManaPerLevel = inibin.getFloatValue("DATA", "MPPerLevel");
            stats.AdPerLevel = inibin.getFloatValue("DATA", "DamagePerLevel");
            stats.ArmorPerLevel = inibin.getFloatValue("DATA", "ArmorPerLevel");
            stats.MagicResistPerLevel = inibin.getFloatValue("DATA", "SpellBlockPerLevel");
            stats.HealthRegenerationPerLevel = inibin.getFloatValue("DATA", "HPRegenPerLevel");
            stats.ManaRegenerationPerLevel = inibin.getFloatValue("DATA", "MPRegenPerLevel");
            stats.GrowthAttackSpeed = inibin.getFloatValue("DATA", "AttackSpeedPerLevel");

            setMelee(inibin.getBoolValue("DATA", "IsMelee"));
            setCollisionRadius(inibin.getIntValue("DATA", "PathfindingCollisionRadius"));

            Inibin autoAttack = RAFManager.getInstance().GetAutoAttackData(model);

            if (autoAttack != null)
            {
                autoAttackDelay = autoAttack.getFloatValue("SpellData", "castFrame") / 30.0f;
                autoAttackProjectileSpeed = autoAttack.getFloatValue("SpellData", "MissileSpeed");
            }
        }

        public void CheckForTargets()
        {
            var objects = _game.GetMap().GetObjects();
            Unit nextTarget = null;
            int nextTargetPriority = 10;

            foreach (var it in objects)
            {
                var u = it.Value as Unit;

                if (u == null || u.isDead() || u.getTeam() == getTeam() || distanceWith(u) > TURRET_RANGE)
                    continue;

                // Note: this method means that if there are two champions within turret range,
                // The player to have been added to the game first will always be targeted before the others
                if (targetUnit == null)
                {
                    var priority = classifyTarget(u);
                    if (priority < nextTargetPriority)
                    {
                        nextTarget = u;
                        nextTargetPriority = priority;
                    }
                }
                else
                {
                    var targetIsChampion = targetUnit as Champion;

                    // Is the current target a champion? If it is, don't do anything
                    if (targetIsChampion != null)
                    {
                        // Find the next champion in range targeting an enemy champion who is also in range
                        var enemyChamp = u as Champion;
                        if (enemyChamp != null && enemyChamp.getTargetUnit() != null)
                        {
                            var enemyChampTarget = enemyChamp.getTargetUnit() as Champion;
                            if (enemyChampTarget != null && // Enemy Champion is targeting an ally
                                enemyChamp.distanceWith(enemyChampTarget) <= enemyChamp.GetStats().Range.Total && // Enemy within range of ally
                                distanceWith(enemyChampTarget) <= TURRET_RANGE)
                            {                                     // Enemy within range of this turret
                                nextTarget = enemyChamp; // No priority required
                                break;
                            }
                        }
                    }
                }
            }
            if (nextTarget != null)
            {
                targetUnit = nextTarget;
                _game.PacketNotifier.notifySetTarget(this, nextTarget);
            }
        }

        public override void update(long diff)
        {
            if (!isAttacking)
            {
                CheckForTargets();
            }

            // Lose focus of the unit target if the target is out of range
            if (targetUnit != null && distanceWith(targetUnit) > TURRET_RANGE)
            {
                setTargetUnit(null);
                _game.PacketNotifier.notifySetTarget(this, null);
            }

            base.update(diff);
        }

        public override void refreshWaypoints()
        {
        }

        public override void die(Unit killer)
        {
            foreach (var player in _game.GetMap().GetAllChampionsFromTeam(killer.getTeam()))
            {
                var goldEarn = _globalGold;

                // Champions in Range within TURRET_RANGE * 1.5f will gain 150% more (obviously)
                if (player.distanceWith(this) <= (TURRET_RANGE * 1.5f) && !player.isDead())
                {
                    goldEarn = _globalGold * 2.5f;
                    if(_globalExp > 0)
                        player.GetStats().Experience += _globalExp;
                }

                player.GetStats().Gold += goldEarn;
                _game.PacketNotifier.notifyAddGold(player, this, goldEarn);
            }
            _game.PacketNotifier.notifyUnitAnnounceEvent(UnitAnnounces.TurretDestroyed, this, killer);
            base.die(killer);
        }

        public override float getMoveSpeed()
        {
            return 0;
        }
    }
}
