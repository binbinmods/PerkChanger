using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
// using Obeliskial_Content;
// using Obeliskial_Essentials;
using System.IO;
using static UnityEngine.Mathf;
using UnityEngine.TextCore.LowLevel;
using static PerkChanger.Plugin;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PerkChanger
{
    public class PerkChangerFunctions
    {

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


    }
}

