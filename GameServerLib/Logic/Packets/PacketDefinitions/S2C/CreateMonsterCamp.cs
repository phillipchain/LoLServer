using System.Text;
using LeagueSandbox.GameServer.Logic.Packets.PacketHandlers;

namespace LeagueSandbox.GameServer.Logic.Packets.PacketDefinitions.S2C
{
    public class CreateMonsterCamp : BasePacket
    {
        public CreateMonsterCamp(float x, float y, float z, string iconName, byte campId, byte campUnk, float unk)
            : base(PacketCmd.PKT_S2_C_CREATE_MONSTER_CAMP)
        {
            _buffer.Write((float)x);
            _buffer.Write((float)z);
            _buffer.Write((float)y);
            _buffer.Write(Encoding.Default.GetBytes(iconName));
            _buffer.Fill(0, 64 - iconName.Length);
            _buffer.Write((byte)campId);
            _buffer.Write((byte)campUnk);

            /*buffer.Write((byte)0x64); // <-|
            buffer.Write((byte)0x15); //   |
            buffer.Write((byte)0xFB); //   |-> Unk
            buffer.Write((byte)0x41); //   |
            buffer.Write((byte)0x0C); // <-|*/
            _buffer.Fill(0, 5);
            _buffer.Write((float)unk);
        }
    }
}