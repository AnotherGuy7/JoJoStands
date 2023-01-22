using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace JoJoStands.Buffs.ItemBuff
{
    public class FinalPush : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Final Push");
            Description.SetDefault("Those attempting to knock you of your place will be met with a tireless fight.");
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.statLifeMax2 = (int)(player.statLifeMax * 0.3f);
            if (player.buffTime[buffIndex] <= 2)
            {
                string genderPhrase = "him";        //Or should I call it a pronoun?
                if (!player.Male)
                    genderPhrase = "her";

                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + "'s determination did not keep " + genderPhrase + " alive."), player.statLife + 1, player.direction);
                player.ClearBuff(Type);
            }
        }
    }
}