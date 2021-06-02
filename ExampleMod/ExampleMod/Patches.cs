using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CultOfClang.NuclearReactor
{
    [HarmonyPatch(typeof(SteamBoilerNode), "RunFixedUpdate")]
    public class SteamBoilerNode_RunFixedUpdate
    {
        public static bool Prefix(SteamBoilerNode __instance, float deltaTime)
        {
            if ((Block)__instance.GoverningBlock == (Block)null || !__instance.IsAlive)
                return false;
            float n = Math.Max(Math.Min((__instance.TargetAmount - __instance.StorageModule.Amount) / SteamConstants.SteamPerMaterial, __instance.MatPerSec * deltaTime), 0.0f);
            float materialsBurned = n;
            //__instance.MainConstruct.GetForce().Material.TakeAsMuchAsPossibleAndReturnWhatYouStillNeed(n)
            //__instance.LastBurned = materialsBurned / deltaTime;
            if ((double)materialsBurned > 0.0)
            {
                float amountToAdd = materialsBurned * SteamConstants.SteamPerMaterial;
                __instance.StorageModule.AddSteam(amountToAdd);
                float steam = amountToAdd;
                if (__instance.GoverningBlock.Stats != null)
                {
                    __instance.GoverningBlock.Stats.BoilerMatsBurned.Add(materialsBurned);
                    __instance.GoverningBlock.Stats.BoilerSteamCreated.Add(steam);
                }
            }
            __instance.GoverningBlock.GenerateHeat(materialsBurned);
            return false;
        }
    }
}
