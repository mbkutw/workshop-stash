using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace WorkshopStashMod
{
    public class WorkshopStashSubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (!(game.GameType is Campaign)) { return; }

            MBObjectManager.Instance.RegisterType<TownWorkshopStash>("TownWorkshopStash", "TownWorkshopStashes");

            if (gameStarterObject is CampaignGameStarter starter)
            {
                starter.AddBehavior(new WorkshopStashCampaignBehavior());
            }

            new HarmonyLib.Harmony("WorkshopStashMod.patcher").PatchAll();
        }
    }
}
