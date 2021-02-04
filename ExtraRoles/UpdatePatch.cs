﻿using HarmonyLib;
using Reactor.Extensions;
using Reactor.Unstrip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using static ExtraRolesMod.ExtraRoles;
using print = System.Console;

namespace ExtraRolesMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class HudUpdateManager
    {
        static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
            {
                KillButton = __instance.KillButton;
                PlayerTools.closestPlayer = PlayerTools.getClosestPlayer(PlayerControl.LocalPlayer);
                DistLocalClosest = PlayerTools.getDistBetweenPlayers(PlayerControl.LocalPlayer, PlayerTools.closestPlayer);
                if (JokerSettings.Joker != null)
                    JokerSettings.ClearTasks();
                if (rend != null)
                    rend.SetActive(false);
                bool sabotageActive = false;
                foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
                    if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy || task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic || task.TaskType == TaskTypes.FixComms)
                        sabotageActive = true;
                EngineerSettings.sabotageActive = sabotageActive;
                if (MedicSettings.Protected != null && MedicSettings.Protected.Data.IsDead)
                    BreakShield(true);
                if (MedicSettings.Protected != null && MedicSettings.Medic != null && MedicSettings.Medic.Data.IsDead)
                    BreakShield(true);
                if (MedicSettings.Medic == null && MedicSettings.Protected != null)
                    BreakShield(true);
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                    player.nameText.Color = Color.white;
                if (PlayerControl.LocalPlayer.Data.IsImpostor)
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                        if (player.Data.IsImpostor)
                            player.nameText.Color = Color.red;
                if (MedicSettings.Medic != null)
                    if (MedicSettings.Medic == PlayerControl.LocalPlayer || MedicSettings.showMedic)
                        MedicSettings.Medic.nameText.Color = ModdedPalette.medicColor;
                        if (MeetingHud.Instance != null)
                            foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                                if (player.NameText != null & MedicSettings.Medic.PlayerId == player.TargetPlayerId)
                                    player.NameText.Color = ModdedPalette.medicColor;
                if (OfficerSettings.Officer != null)
                    if (OfficerSettings.Officer == PlayerControl.LocalPlayer || OfficerSettings.showOfficer)
                        OfficerSettings.Officer.nameText.Color = ModdedPalette.officerColor;
                        if (MeetingHud.Instance != null)
                            foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                                if (player.NameText != null & OfficerSettings.Officer.PlayerId == player.TargetPlayerId)
                                    player.NameText.Color = ModdedPalette.officerColor;
                if (EngineerSettings.Engineer != null)
                    if (EngineerSettings.Engineer == PlayerControl.LocalPlayer || EngineerSettings.showEngineer)
                        EngineerSettings.Engineer.nameText.Color = ModdedPalette.engineerColor;
                        if (MeetingHud.Instance != null)
                            foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                                if (player.NameText != null & EngineerSettings.Engineer.PlayerId == player.TargetPlayerId)
                                    player.NameText.Color = ModdedPalette.engineerColor;
                if (JokerSettings.Joker != null)
                    if (JokerSettings.Joker == PlayerControl.LocalPlayer || JokerSettings.showJoker)
                        JokerSettings.Joker.nameText.Color = ModdedPalette.jokerColor;
                        if (MeetingHud.Instance != null)
                            foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                                if (player.NameText != null & JokerSettings.Joker.PlayerId == player.TargetPlayerId)
                                    player.NameText.Color = ModdedPalette.jokerColor;
                if (MedicSettings.Protected != null)
                    if (MedicSettings.Protected == PlayerControl.LocalPlayer || MedicSettings.showProtected)
                        MedicSettings.Protected.myRend.material.SetColor("_VisorColor", ModdedPalette.protectedColor);
                if (MedicSettings.Medic != null && MedicSettings.Medic.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                {
                    KillButton.renderer.sprite = shieldIco;
                    KillButton.gameObject.SetActive(true);
                    KillButton.isActive = true;
                    KillButton.SetCoolDown(0f, PlayerControl.GameOptions.KillCooldown + 15.0f);
                    if (DistLocalClosest < KMOGFLPJLLK.JMLGACIOLIK[PlayerControl.GameOptions.KillDistance] && MedicSettings.shieldUsed == false)
                    {
                        KillButton.SetTarget(PlayerTools.closestPlayer);
                        CurrentTarget = PlayerTools.closestPlayer;
                    }
                    else
                    {
                        KillButton.SetTarget(null);
                        CurrentTarget = null;
                    }
                }
                if (OfficerSettings.Officer != null && OfficerSettings.Officer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                {
                    KillButton.gameObject.SetActive(true);
                    KillButton.isActive = true;
                    KillButton.SetCoolDown(PlayerTools.GetOfficerKD(), PlayerControl.GameOptions.KillCooldown + 15.0f);
                    if (DistLocalClosest < KMOGFLPJLLK.JMLGACIOLIK[PlayerControl.GameOptions.KillDistance])
                    {
                        KillButton.SetTarget(PlayerTools.closestPlayer);
                        CurrentTarget = PlayerTools.closestPlayer;
                    }
                    else
                    {
                        KillButton.SetTarget(null);
                        CurrentTarget = null;
                    }
                }
                if (MedicSettings.Protected != null && MedicSettings.Protected.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                {
                    if (rend == null)
                    {
                        rend = new GameObject("Shield Icon", new Il2CppSystem.Type[] { SpriteRenderer.Il2CppType });
                        rend.GetComponent<SpriteRenderer>().sprite = smallShieldIco;
                    }
                    int scale;
                    if (Screen.width > Screen.height)
                        scale = Screen.width / 800;
                    else
                        scale = Screen.height / 600;
                    rend.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0 + (25 * scale), 0 + (25 * scale), -50f));
                    rend.SetActive(true);
                }
                if (EngineerSettings.Engineer != null && EngineerSettings.Engineer.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                {
                    KillButton.gameObject.SetActive(true);
                    KillButton.isActive = true;
                    KillButton.SetCoolDown(0f, 30f);
                    KillButton.renderer.sprite = repairIco;
                    KillButton.renderer.color = Palette.EnabledColor;
                    KillButton.renderer.material.SetFloat("_Desat", 0f);
                }
            }
        }
    }
}