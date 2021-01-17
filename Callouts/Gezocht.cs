using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using Rage.Native;
using Rage.Euphoria;
using Rage.Forms;

namespace DutchCallouts.Callouts
{
    [CalloutInfo("Gezocht", CalloutProbability.High)]
    public class Gezocht : Callout
    {
        private string[] wapens = new string[] { "WEAPON_BATTLEAXE", "WEAPON_BULLPUPRIFLE_MK2", "WEAPON_SWITCHBLADE", "WEAPON_SNSPISTOL_MK2" };
        private Ped _verdachte;
        private Vector3 _SpawnPunt;
        private Vector3 _zoekgebied;
        private Vector3 _Loc1;
        private Vector3 _Loc2;
        private Vector3 _Loc3;
        private Vector3 _Loc4;
        private Vector3 _Loc5;
        private Vector3 _Loc6;
        private Vector3 _Loc7;
        private Vector3 _Loc8;
        private Vector3 _Loc9;
        private Vector3 _Loc10;
        private Blip _Blip;
        private int storyLine = 1;
        private int _callOutMessage = 0;
        private bool _attack = false;
        private bool HasWeapon = false;
        private bool _wasClose = false;
        private bool _alreadySubtitleIntrod = false;
        public override bool OnBeforeCalloutDisplayed()
{
            _Loc1 = new Vector3(-73.264f, -28.95624f, 65.75121f);
            _Loc2 = new Vector3(-1123.261f, 483.8483f, 82.16084f);
            _Loc3 = new Vector3(967.7412f, -546.0015f, 59.36506f);
            _Loc4 = new Vector3(-109.5984f, -10.19665f, 70.51959f);
            _Loc5 = new Vector3(-10.93565f, -1434.329f, 31.11683f);
            _Loc6 = new Vector3(-1.838376f, 523.2645f, 174.6274f);
            _Loc7 = new Vector3(-801.5516f, 178.7447f, 72.83471f);
            _Loc8 = new Vector3(-812.7239f, 178.7438f, 76.74079f);
            _Loc9 = new Vector3(3.542758f, 526.8926f, 170.6218f);
            _Loc10 = new Vector3(-1155.698f, -1519.297f, 10.63272f);
            Random random = new Random();
            List<string> list = new List<string>
            {
                "Loc1",
                "Loc2",
                "Loc3",
                "Loc4",
                "Loc5",
                "Loc6",
                "Loc7",
                "Loc8",
                "Loc9",
                "Loc10",
            };
            int num = random.Next(0, 10);
            if (list[num] == "Loc1")
            {
                _SpawnPunt = _Loc1;
            }
            if (list[num] == "Loc2")
            {
                _SpawnPunt = _Loc2;
            }
            if (list[num] == "Loc3")
            {
                _SpawnPunt = _Loc3;
            }
            if (list[num] == "Loc4")
            {
                _SpawnPunt = _Loc4;
            }
            if (list[num] == "Loc5")
            {
                _SpawnPunt = _Loc5;
            }
            if (list[num] == "Loc6")
            {
                _SpawnPunt = _Loc6;
            }
            if (list[num] == "Loc7")
            {
                _SpawnPunt = _Loc7;
            }
            if (list[num] == "Loc8")
            {
                _SpawnPunt = _Loc8;
            }
            if (list[num] == "Loc9")
            {
                _SpawnPunt = _Loc9;
            }
            if (list[num] == "Loc10")
            {
                _SpawnPunt = _Loc10;
            }
            ShowCalloutAreaBlipBeforeAccepting(_SpawnPunt, 30f);
            switch (new Random().Next(1, 3))
            {
                case 1:
                    _attack = true;
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
            switch (new Random().Next(1, 3))
            {
                case 1:
                    CalloutMessage = "~b~Meldkamer:~s~ Gezocht persoon";
                    _callOutMessage = 1;
                    break;
                case 2:
                    CalloutMessage = "~b~Meldkamer:~s~ Gezocht persoon";
                    _callOutMessage = 2;
                    break;
                case 3:
                    CalloutMessage = "~b~Meldkamer:~s~ Gezocht persoon";
                    _callOutMessage = 3;
                    break;
            }
            CalloutPosition = _SpawnPunt;
            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS CRIME_SUSPECT_RESISTING_ARREST_01 IN_OR_ON_POSITION", _SpawnPunt);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("DutchCallouts Log: Gezocht callout accepted.");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Gezocht persoon", "~b~Meldkamer:~w~ Probeer met de verdachte te ~o~spreken~w~ en ~b~arresteer~w~ het gezochte persoon. Reageer met ~y~CODE 2");

            _verdachte = new Ped(_SpawnPunt);
            _verdachte.Position = _SpawnPunt;
            _verdachte.IsPersistent = true;
            _verdachte.BlockPermanentEvents = true;
            LSPD_First_Response.Engine.Scripting.Entities.Persona.FromExistingPed(_verdachte).Wanted = true;

            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Meldkamer", "~g~Informatie~w~ laden uit de ~y~LSPD Database~w~...");
            Functions.DisplayPedId(_verdachte, true);

            _zoekgebied = _SpawnPunt.Around2D(1f, 2f);
            _Blip = new Blip(_zoekgebied, 30f);
            _Blip.Color = Color.Yellow;
            _Blip.EnableRoute(Color.Yellow);
            _Blip.Alpha = 2f;
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (_verdachte.Exists()) _verdachte.Delete();
            if (_Blip.Exists()) _Blip.Delete();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (_verdachte.DistanceTo(Game.LocalPlayer.Character) < 20f)
                {
                    if (_attack == true && !HasWeapon)
                    {
                        _verdachte.Inventory.GiveNewWeapon(new WeaponAsset(wapens[new Random().Next((int)wapens.Length)]), 500, true);
                        HasWeapon = true;
                        _verdachte.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    }
                    if (_attack == false && _verdachte.DistanceTo(Game.LocalPlayer.Character) < 10f && Game.LocalPlayer.Character.IsOnFoot && _alreadySubtitleIntrod == false)
                    {
                        Game.DisplaySubtitle("Druk op ~y~Y ~w~om te spreken met de verdachte.", 5000);
                        _alreadySubtitleIntrod = true;
                        _wasClose = true;
                    }
                    if (_attack == false && _verdachte.DistanceTo(Game.LocalPlayer.Character) < 2f && Game.IsKeyDown(Settings.Dialog))
                    {
                        _verdachte.Face(Game.LocalPlayer.Character);
                        switch (storyLine)
                        {
                            case 1:
                                Game.DisplaySubtitle("~y~Verdachte: ~w~Hallo meneer, kan ik u ergens mee helpen?", 5000);
                                storyLine++;
                                break;
                            case 2:
                                Game.DisplaySubtitle("~b~Agent: ~w~Hallo, je wordt gezocht door de politie.", 5000);
                                storyLine++;
                                break;
                            case 3:
                                Game.DisplaySubtitle("~y~Verdachte: ~w~ik...? Weet je dat zeker?", 5000);
                                storyLine++;
                                break;
                            case 4:
                                if (_callOutMessage == 1)
                                    Game.DisplaySubtitle("~b~Agent: ~w~Ik moet je arresteren, omdat je gezocht wordt door de politie, kom met mij mee", 5000);
                                if (_callOutMessage == 2)
                                    Game.DisplaySubtitle("~b~Agent: ~w~Ik moet je arresteren, omdat je gezocht wordt door de politie.", 5000);
                                if (_callOutMessage == 3)
                                    Game.DisplaySubtitle("~b~Agent: ~w~Ik moet je arresteren, omdat je gezocht wordt door de politie.", 5000);
                                storyLine++;
                                break;
                            case 5:
                                if (_callOutMessage == 1)
                                {
                                    _verdachte.Tasks.PutHandsUp(-1, Game.LocalPlayer.Character);
                                    Game.DisplaySubtitle("~y~Verdachte: ~w~OK, OK, arresteer mij, ik heb iets vreselijks gedaan", 5000);
                                }
                                if (_callOutMessage == 2)
                                {
                                    Game.DisplaySubtitle("~y~Verdachte: ~w~Alsjeblieft, arresteer me niet!!!", 5000);
                                    _verdachte.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);
                                    Rage.Native.NativeFunction.CallByName<uint>("TASK_COMBAT_PED", _verdachte, Game.LocalPlayer.Character, 0, 16);
                                }
                                if (_callOutMessage == 3)
                                {
                                    Game.DisplaySubtitle("~y~Verdachte: ~w~Ik ben onschuldig!", 5000);
                                    _verdachte.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                                    Rage.Native.NativeFunction.CallByName<uint>("TASK_COMBAT_PED", _verdachte, Game.LocalPlayer.Character, 0, 16);
                                }
                                storyLine++;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (_verdachte.IsDead) End();
                if (Functions.IsPedArrested(_verdachte)) End();
            }, "Gezocht [DutchCallouts]");
            base.Process();
        }
        public override void End()
        {
            if (_verdachte.Exists()) _verdachte.Dismiss();
            if (_Blip.Exists()) _Blip.Delete();
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Gezocht persoon", "~b~CODE 4.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH WE_ARE_CODE FOUR NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }
    }
}