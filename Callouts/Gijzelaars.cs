﻿using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using Rage.Native;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace DutchCallouts.Callouts
{
    [CalloutInfo("Gijzelaars", CalloutProbability.Medium)]
    internal class Gijzelaars : Callout
    {
        private string[] wepList = new string[] { "WEAPON_PISTOL", "WEAPON_SMG", "WEAPON_MACHINEPISTOL", "WEAPON_PUMPSHOTGUN" };
        private Ped _AG1;
        private Ped _AG2;
        private Ped _V1;
        private Ped _V2;
        private Ped _V3;
        private Ped _V4;
        private Vector3 _searcharea;
        private Vector3 _SpawnPoint;
        private Vector3 _Location1;
        private Vector3 _Location2;
        private Vector3 _Location3;
        private Vector3 _Location4;
        private Blip _SpawnLocation;
        private int _scenario = 0;
        private bool _Scene1 = false;
        private bool _Scene2 = false;
        private bool _notificationDisplayed = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            _Location1 = new Vector3(-573.7833f, -1606.964f, 27.01079f);
            _Location2 = new Vector3(-589.1813f, -1626.09f, 33.01056f);
            _Location3 = new Vector3(976.6871f, -96.42852f, 74.84537f);
            _Location4 = new Vector3(1250.002f, -3014.48f, 9.319259f);

            Random random = new Random();
            List<string> list = new List<string>
            {
                "Location1",
                "Location2",
                "Location3",
                "Location4",
            };
            int num = random.Next(0, 4);
            if (list[num] == "Location1")
            {
                _SpawnPoint = _Location1;
                _AG1 = new Ped("mp_g_m_pros_01", _SpawnPoint, 0f);
                _AG2 = new Ped("mp_g_m_pros_01", _SpawnPoint, 0f);
                _V1 = new Ped(_SpawnPoint, 0f);
                _V2 = new Ped(_SpawnPoint, 0f);
                _V3 = new Ped(_SpawnPoint, 0f);
                _V4 = new Ped(_SpawnPoint, 0f);
            }
            if (list[num] == "Location2")
            {
                _SpawnPoint = _Location2;
                _AG1 = new Ped("mp_g_m_pros_01", _SpawnPoint, 0f);
                _AG2 = new Ped("mp_g_m_pros_01", _SpawnPoint, 0f);
                _V1 = new Ped(_SpawnPoint, 0f);
                _V2 = new Ped(_SpawnPoint, 0f);
                _V3 = new Ped(_SpawnPoint, 0f);
                _V4 = new Ped(_SpawnPoint, 0f);
            }
            if (list[num] == "Location3")
            {
                _SpawnPoint = _Location3;
                _AG1 = new Ped("mp_g_m_pros_01", _SpawnPoint, 0f);
                _AG2 = new Ped("mp_g_m_pros_01", _SpawnPoint, 0f);
                _V1 = new Ped(_SpawnPoint, 0f);
                _V2 = new Ped(_SpawnPoint, 0f);
                _V3 = new Ped(_SpawnPoint, 0f);
                _V4 = new Ped(_SpawnPoint, 0f);
            }
            if (list[num] == "Location4")
            {
                _SpawnPoint = _Location4;
                _AG1 = new Ped("mp_g_m_pros_01", _SpawnPoint, 0f);
                _AG2 = new Ped("mp_g_m_pros_01", _SpawnPoint, 0f);
                _V1 = new Ped(_SpawnPoint, 0f);
                _V2 = new Ped(_SpawnPoint, 0f);
                _V3 = new Ped(_SpawnPoint, 0f);
                _V4 = new Ped(_SpawnPoint, 0f);
            }
            _scenario = new Random().Next(0, 100);
            switch (new Random().Next(1, 3))
            {
                case 1:
                    _Scene1 = true;
                    break;
                case 2:
                    _Scene2 = true;
                    break;
                case 3:
                    _Scene2 = true;
                    break;
            }
            ShowCalloutAreaBlipBeforeAccepting(_SpawnPoint, 100f);
            AddMinimumDistanceCheck(10f, _SpawnPoint);
            CalloutMessage = "~y~FIB~w~: Een groep criminelen hebben gegijzeld.";
            CalloutPosition = _SpawnPoint;
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            _AG1.IsPersistent = true;
            _AG1.BlockPermanentEvents = true;
            _AG1.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
            _AG1.Health = 200;
            _AG1.Armor = 300;

            _AG2.IsPersistent = true;
            _AG2.BlockPermanentEvents = true;
            _AG2.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
            _AG2.Health = 200;
            _AG2.Armor = 300;

            _V1.BlockPermanentEvents = true;
            _V2.BlockPermanentEvents = true;
            _V3.BlockPermanentEvents = true;
            _V4.BlockPermanentEvents = true;
            _V1.IsPersistent = true;
            _V2.IsPersistent = true;
            _V3.IsPersistent = true;
            _V4.IsPersistent = true;
            _V1.Tasks.PlayAnimation("random@arrests@busted", "idle_a", 8.0F, AnimationFlags.Loop);
            _V2.Tasks.PlayAnimation("random@arrests@busted", "idle_a", 8.0F, AnimationFlags.Loop);
            _V3.Tasks.PlayAnimation("random@arrests@busted", "idle_a", 8.0F, AnimationFlags.Loop);
            _V4.Tasks.PlayAnimation("random@arrests@busted", "idle_a", 8.0F, AnimationFlags.Loop);

            NativeFunction.CallByName<uint>("TASK_AIM_GUN_AT_ENTITY", _AG1, _V1, -1, true);
            NativeFunction.CallByName<uint>("TASK_AIM_GUN_AT_ENTITY", _AG2, _V2, -1, true);

            new RelationshipGroup("AG");
            new RelationshipGroup("VI");
            _AG1.RelationshipGroup = "AG";
            _AG2.RelationshipGroup = "AG";
            _V1.RelationshipGroup = "VI";
            _V2.RelationshipGroup = "VI";
            _V3.RelationshipGroup = "VI";
            _V4.RelationshipGroup = "VI";
            Game.SetRelationshipBetweenRelationshipGroups("AG", "VI", Relationship.Hate);

            _searcharea = _SpawnPoint.Around2D(1f, 2f);
            _SpawnLocation = new Blip(_searcharea, 40f);
            _SpawnLocation.EnableRoute(Color.Yellow);
            _SpawnLocation.Color = Color.Yellow;
            _SpawnLocation.Alpha = 2f;

            Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT_03 CRIME_CIVILIAN_NEEDING_ASSISTANCE_IN IN_OR_ON_POSITION", _SpawnPoint);
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~FIB", "De politie heeft het ~y~Gijzelaar Reddingsteam~w~ gebelt om alle ~o~Gegijzelde~w~ te redden.");
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (_AG1.Exists()) _AG1.Delete();
            if (_AG2.Exists()) _AG2.Delete();
            if (_V1.Exists()) _V1.Delete();
            if (_V2.Exists()) _V2.Delete();
            if (_V3.Exists()) _V3.Delete();
            if (_V4.Exists()) _V4.Delete();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (_SpawnPoint.DistanceTo(Game.LocalPlayer.Character) < 25f && Game.LocalPlayer.Character.IsOnFoot && !_notificationDisplayed)
                {
                    if (_SpawnLocation.Exists()) _SpawnLocation.Delete();

                    Game.DisplayHelp("Het ~y~Gijzelaar Reddingsteam~w~ is ~g~aangekomen~w~. ");
                    _notificationDisplayed = true;
                }
                if (_AG1.DistanceTo(Game.LocalPlayer.Character) < 14f)
                {
                    if (_Scene1 == true && _Scene2 == false && _AG1.DistanceTo(Game.LocalPlayer.Character) < 18f)
                    {
                        Game.DisplaySubtitle("~y~Crimineel~w~ shhh....Ik hoor voetstappen!");

                        _AG1.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        _AG2.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    }
                    if (_Scene2 == true && _Scene1 == false && Game.LocalPlayer.Character.DistanceTo(_AG1) < 18f)
                    {
                        _AG1.Tasks.FightAgainstClosestHatedTarget(1000f);
                        _AG2.Tasks.FightAgainstClosestHatedTarget(1000f);

                        _AG1.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        _AG2.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    }
                }
                if (_AG1.IsDead && _AG2.IsDead) End();
                if (Functions.IsPedArrested(_AG1) && Functions.IsPedArrested(_AG2)) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (Game.LocalPlayer.Character.IsDead) End();
            }, "Gijzelaars [DutchCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (_AG1.Exists()) _AG1.Dismiss();
            if (_AG2.Exists()) _AG2.Dismiss();
            if (_V1.Exists()) _V1.Dismiss();
            if (_V2.Exists()) _V2.Dismiss();
            if (_V3.Exists()) _V3.Dismiss();
            if (_V4.Exists()) _V4.Dismiss();
            if (_SpawnLocation.Exists()) _SpawnLocation.Delete();
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Gijzelaars", "~b~CODE 4.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH WE_ARE_CODE FOUR NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }
    }
}