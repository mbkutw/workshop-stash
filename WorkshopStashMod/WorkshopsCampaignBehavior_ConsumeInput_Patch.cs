using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace WorkshopStashMod
{
    [HarmonyPatch(typeof(WorkshopsCampaignBehavior), "ConsumeInput")]
    public static class WorkshopsCampaignBehavior_ConsumeInput_Patch
    {
        public static bool Prefix(
            ItemCategory productionInput,
            Town town,
            Workshop workshop,
            bool doNotEffectCapital)
        {
            if (workshop.Owner != Hero.MainHero) { return true; }

            var stash = MBObjectManager.Instance.GetObject<TownWorkshopStash>(x => x.Town == town);

            if (stash == null || !stash.InputFromStash) { return true; }

            var index = stash.Stash.FindIndex(x => x.ItemCategory == productionInput);

            if (index >= 0 && stash.Stash[index].Amount > 0)
            {
                stash.Stash.AddToCountsAtIndex(index, -1);
                return false;
            }

            return true;
        }
    }
}
