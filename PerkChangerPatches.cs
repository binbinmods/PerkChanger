using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
// using static Obeliskial_Essentials.Essentials;
using System;
using static PerkChanger.Plugin;
using static PerkChanger.CustomFunctions;
using static PerkChanger.PerkChangerFunctions;
using System.Collections.Generic;
using static Functions;
using UnityEngine;
// using Photon.Pun;
using TMPro;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;
// using Unity.TextMeshPro;

// Make sure your namespace is the same everywhere
namespace PerkChanger
{

    [HarmonyPatch] // DO NOT REMOVE/CHANGE - This tells your plugin that this is part of the mod

    public class PerkChangerPatches
    {
        public static bool devMode = false; //DevMode.Value;
        public static bool bSelectingPerk = false;
        public static bool IsHost()
        {
            return (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()) || !GameManager.Instance.IsMultiplayer();
        }

        public static bool CanChangePerk()
        {
            bool singleplayerCanChange = EnablePerkChangeWhenever.Value || (EnablePerkChangeInTowns.Value && AtOManager.Instance.CharInTown());
            bool mpCanChange = EnablePerkChangeWhenever.Value || (EnablePerkChangeInTownsMP && AtOManager.Instance.CharInTown());
            return true; //IsHost() ? singleplayerCanChange : mpCanChange;
        }




        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkTree), "CanModify")]
        public static void CanModifyPostfix(ref bool __result)
        {
            if (CanChangePerk())
                __result = true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkTree), "SelectPerk")]
        public static void SelectPerkPrefix()
        {
            if (CanChangePerk())
                bSelectingPerk = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkTree), "SelectPerk")]
        public static void SelectPerkPostfix()
        {
            bSelectingPerk = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "CharInTown")]
        public static void CharInTownPostfix(ref bool __result)
        {
            if (bSelectingPerk)// && EnablePerkChangeWhenever.Value)
                __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GetTownTier")]
        public static void GetTownTierPostfix(ref int __result)
        {
            if (bSelectingPerk)// && EnablePerkChangeWhenever.Value)
                __result = 0;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SettingsManager), "IsActive")]
        public static void SettingsManagerIsActivePostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AlertManager), "IsActive")]
        public static void AlertManagerIsActivePostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MadnessManager), "IsActive")]
        public static void MadnessManagerIsActivePostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseUp")]
        public static void OnMouseUpPrefix(ref PerkNode __instance)
        {
            if (CanChangePerk())
            {
                Traverse.Create(__instance).Field("nodeLocked").SetValue(false);
                __instance.iconLock.gameObject.SetActive(false);
                bSelectingPerk = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseUp")]
        public static void OnMouseUpPostfix()
        {
            bSelectingPerk = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseEnter")]
        public static void OnMouseEnterPrefix(ref PerkNode __instance)
        {
            if (CanChangePerk())
            {
                bSelectingPerk = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseEnter")]
        public static void OnMouseEnterPostfix()
        {
            bSelectingPerk = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkTree), "Show")]
        public static void ShowPostfix(ref PerkTree __instance, ref int ___totalAvailablePoints)
        {
            if (CanChangePerk())
            {
                __instance.buttonReset.gameObject.SetActive(value: true);
                __instance.buttonImport.gameObject.SetActive(value: true);
                __instance.buttonExport.gameObject.SetActive(value: true);
                __instance.saveSlots.gameObject.SetActive(value: true);
                __instance.buttonConfirm.gameObject.SetActive(value: true);
                // __instance.buttonConfirm.Enable();
            }
            return;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "SetIconLock")]
        public static void SetIconLockPrefix(ref bool _state)
        {
            if (CanChangePerk())
                _state = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "SetLocked")]
        public static void SetLockedPrefix(ref bool _status)
        {
            if (CanChangePerk())
                _status = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetObeliskScore")]
        public static bool SetObeliskScorePrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetScore")]
        public static bool SetScorePrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetSingularityScore")]
        public static bool SetSingularityScorePrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetObeliskScoreLeaderboard")]
        public static bool SetObeliskScoreLeaderboardPrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetScoreLeaderboard")]
        public static bool SetScoreLeaderboardPrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetSingularityScoreLeaderboard")]
        public static bool SetSingularityScoreLeaderboardPrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }


    }
}