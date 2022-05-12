using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class WillToControlCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops while in the Crimson/Corruption";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return info.player.ZoneCorrupt || info.player.ZoneCrimson;

            return false;
        }
    }
}
