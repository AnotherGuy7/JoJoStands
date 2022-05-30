using JoJoStands.Projectiles.PlayerStands;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace JoJoStands
{
    public class CustomizableServerOptions : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(StandSyncFrequency.SemiFrequently)]
        [Label("Stand Sync Frequency")]
        [Tooltip("Throttles how often the Stand is synced. Can have negative effects on network stability on higher frequencies.")]
        public StandSyncFrequency standSyncFrequency;

        public enum StandSyncFrequency
        {
            Sparingly,          //3s
            SemiFrequently,     //1.5s
            Frequently,         //0.5s
            Always              //Always
        }

        public override void OnChanged()
        {
            switch (standSyncFrequency)
            {
                case StandSyncFrequency.Sparingly:
                    StandClass.StandNetworkUpdateTime = 3 * 60;
                    break;
                case StandSyncFrequency.SemiFrequently:
                    StandClass.StandNetworkUpdateTime = (int)(1.5f * 60);
                    break;
                case StandSyncFrequency.Frequently:
                    StandClass.StandNetworkUpdateTime = 30;
                    break;
                case StandSyncFrequency.Always:
                    StandClass.StandNetworkUpdateTime = 2;
                    break;
            }
        }
    }
}