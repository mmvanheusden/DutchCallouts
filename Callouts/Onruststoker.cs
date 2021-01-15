using System;
using Rage;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Drawing;
using System.Collections.Generic;

namespace DutchCallouts.Callouts
{
    [CalloutInfo("Onruststoker", CalloutProbability.Medium)]
    public class Onruststoker : Callout
    {
        private Ped _subject;
        private string[] pedList = new string[] { "s_m_y_dealer_01", "u_m_m_jesus_01", "u_m_y_militarybum", "u_m_y_proldriver_01", "a_m_o_soucent_03", "u_m_o_tramp_01",
                                                  "a_m_m_tramp_01", "a_m_o_tramp_01", "a_m_m_trampbeac_01" };
        private Vector3 _SpawnPoint;
        private Vector3 _searcharea;
        private Vector3 _Location1;
        private Vector3 _Location2;
        private Vector3 _Location3;
        private Vector3 _Location4;
        private Vector3 _Location5;
        private Vector3 _Location6;
        private Vector3 _Location7;
        private Vector3 _Location8;
        private Blip _Blip;
        private bool _attack = false;
        private bool _startedPursuit = false;
        private bool _wasClose = false;
        private bool _alreadySubtitleIntrod = false;
        private int _storyLine = 1;
        private int _callOutMessage = 0;

        public override bool OnBeforeCalloutDisplayed()
        {
            _Location1 = new Vector3(-292.7569f, -305.3849f, 10.06316f);
            _Location2 = new Vector3(-282.8204f, -326.8933f, 18.28812f);
            _Location3 = new Vector3(262.1429f, -1205.378f, 29.28906f);
            _Location4 = new Vector3(298.8077f, -1206.379f, 38.89511f);
            _Location5 = new Vector3(-854.7909f, -107.829f, 28.18498f);
            _Location6 = new Vector3(-824.4403f, -129.8238f, 28.17533f);
            _Location7 = new Vector3(-1359.522f, -472.9277f, 23.27035f);
            _Location8 = new Vector3(-1346.406f, -474.0514f, 15.04538f);
            Random random = new Random();
            List<string> list = new List<string>
            {
                "Location1",
                "Location2",
                "Location3",
                "Location4",
                "Location5",
                "Location6",
                "Location7",
                "Location8",
                "Location9",
                "Location10",
            };
            int num = random.Next(0, 8);
            if (list[num] == "Location1")
            {
                _SpawnPoint = _Location1;
            }
            if (list[num] == "Location2")
            {
                _SpawnPoint = _Location2;
            }
            if (list[num] == "Location3")
            {
                _SpawnPoint = _Location3;
            }
            if (list[num] == "Location4")
            {
                _SpawnPoint = _Location4;
            }
            if (list[num] == "Location5")
            {
                _SpawnPoint = _Location5;
            }
            if (list[num] == "Location6")
            {
                _SpawnPoint = _Location6;
            }
            if (list[num] == "Location7")
            {
                _SpawnPoint = _Location7;
            }
            if (list[num] == "Location8")
            {
                _SpawnPoint = _Location8;
            }
            ShowCalloutAreaBlipBeforeAccepting(_SpawnPoint, 30f);
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
                    this.CalloutMessage = "~b~Meldkamer:~s~ Onruststoker bij het Metro station";
                    _callOutMessage = 1;
                    break;
                case 2:
                    this.CalloutMessage = "~b~Meldkamer:~s~ Onruststoker bij het Metro station";
                    _callOutMessage = 2;
                    break;
                case 3:
                    this.CalloutMessage = "~b~Meldkamer:~s~ Onruststoker bij het Metro station";
                    _callOutMessage = 3;
                    break;
            }
            CalloutPosition = _SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS CRIME_SUSPECT_RESISTING_ARREST_01 IN_OR_ON_POSITION", _SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("DutchCallouts Log: Onruststoker callout accepted.");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Onruststoker bij het Metro station", "~b~Meldkamer:~w~ Zoek in het ~y~gele gebied bij het Metro station~w~ en probeer de onruststoker te ondervragen. Reageer met ~y~CODE 2");

            _subject = new Ped(pedList[new Random().Next((int)pedList.Length)], _SpawnPoint, 0f);
            _subject.Position = _SpawnPoint;
            _subject.IsPersistent = true;
            _subject.BlockPermanentEvents = true;
            _subject.Tasks.PlayAnimation("amb@world_human_bum_standing@drunk@base", "base", 5, AnimationFlags.None);

            _searcharea = _SpawnPoint.Around2D(1f, 2f);
            _Blip = new Blip(_searcharea, 80f);
            _Blip.Color = Color.Yellow;
            _Blip.EnableRoute(Color.Yellow);
            _Blip.Alpha = 5f;
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (_subject.Exists()) _subject.Delete();
            if (_Blip.Exists()) _Blip.Delete();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (_subject.DistanceTo(Game.LocalPlayer.Character) < 20f)
                {
                    if (_attack == true && _startedPursuit == false)
                    {
                        _subject.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    }
                    if (_attack == false && _subject.DistanceTo(Game.LocalPlayer.Character) < 6f && Game.LocalPlayer.Character.IsOnFoot && _alreadySubtitleIntrod == false)
                    {
                        Game.DisplaySubtitle("Druk op ~y~Y ~w~om de praten met de Onruststoker.", 5000);
                        _subject.Face(Game.LocalPlayer.Character);
                        _alreadySubtitleIntrod = true;
                        _wasClose = true;
                        if (_Blip.Exists()) _Blip.Delete();
                    }
                    if (_subject.DistanceTo(Game.LocalPlayer.Character) < 5f && Game.LocalPlayer.Character.IsOnFoot && _alreadySubtitleIntrod == false)
                    {
                        _subject.Tasks.AchieveHeading(Game.LocalPlayer.Character.Heading);
                    }
                    if (_attack == false && _subject.DistanceTo(Game.LocalPlayer.Character) < 2f && Game.IsKeyDown(Settings.Dialog))
                    {
                        _subject.Face(Game.LocalPlayer.Character);
                        switch (_storyLine)
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Onruststoker: ~w~Hallo meneer, is er iets aan de hand? (1/5)", 5000);
                                _storyLine++;
                                break;
                            case 2:
                                Game.DisplaySubtitle("~b~Agent: ~w~Ben jij het persoon die mogelijk onrust veroorzaakt? (2/5)", 5000);
                                _storyLine++;
                                break;
                            case 3:
                                Game.DisplaySubtitle("~r~Onruststoker: ~w~ehm... Nee! (3/5)", 5000);
                                _storyLine++;
                                break;
                            case 4:
                                if (_callOutMessage == 1)
                                    Game.DisplaySubtitle("~b~Agent: ~w~We hebben een fimpje gekregen van iemand die zeer aggresief gedrag vertoont en u matcht de omschrijving. (4/5)", 5000);
                                if (_callOutMessage == 2)
                                    Game.DisplaySubtitle("~b~Agent: ~w~Oké, maar iemand heeft de politie gebeld tegen iemand die op jou lijkt. (4/5)", 5000);
                                if (_callOutMessage == 3)
                                    Game.DisplaySubtitle("~b~Agent: ~w~hmm, weet je dat zeker?? (4/5)", 5000);
                                _storyLine++;
                                break;
                            case 5:
                                if (_callOutMessage == 1)
                                {
                                    _subject.Tasks.PutHandsUp(-1, Game.LocalPlayer.Character);
                                    Game.DisplaySubtitle("~r~Onruststoker: ~w~Ik geef toe dat ik het gedaan heb. (5/5)", 5000);
                                }
                                if (_callOutMessage == 2)
                                {
                                    Game.DisplaySubtitle("~r~Onruststoker: ~w~Fuck off! (5/5)", 5000);
                                    _subject.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                                    Rage.Native.NativeFunction.CallByName<uint>("TASK_COMBAT_PED", _subject, Game.LocalPlayer.Character, 0, 16);
                                }
                                if (_callOutMessage == 3)
                                {
                                    Game.DisplaySubtitle("~r~Onruststoker: ~w~KLOOTZAK! (5/5)", 5000);
                                    _subject.Inventory.GiveNewWeapon("WEAPON_MOLOTOV", 500, true);
                                    Rage.Native.NativeFunction.CallByName<uint>("TASK_COMBAT_PED", _subject, Game.LocalPlayer.Character, 0, 16);
                                }
                                _storyLine++;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (_subject.IsDead) End();
                if (Functions.IsPedArrested(_subject)) End();
            }, "Onruststoker [DutchCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (_subject.Exists()) _subject.Dismiss();
            if (_Blip.Exists()) _Blip.Delete();
            _attack = false;
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Onruststoker bij het Metro station", "~b~CODE 4");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH WE_ARE_CODE FOUR NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }

    }
}