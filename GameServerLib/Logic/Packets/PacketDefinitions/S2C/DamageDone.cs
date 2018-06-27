using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Packets.PacketHandlers;

namespace LeagueSandbox.GameServer.Logic.Packets.PacketDefinitions.S2C
{
    public class DamageDone : BasePacket
    {
        public DamageDone(AttackableUnit source, AttackableUnit target, float amount, DamageType type, DamageText damageText)
            : base(PacketCmd.PKT_S2_C_DAMAGE_DONE, target.NetId)
        {
            _buffer.Write((byte)damageText);
            _buffer.Write((short)((short)type << 8));
            _buffer.Write((float)amount);
            _buffer.Write((int)target.NetId);
            _buffer.Write((int)source.NetId);
        }
    }
}