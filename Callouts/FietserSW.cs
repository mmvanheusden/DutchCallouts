using System;
using Rage;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Drawing;
using System.Collections.Generic;

namespace DutchCallouts.Callouts
{
    [CalloutInfo("FietserSW", CalloutProbability.Medium)]
    public class FietserSW : Callout
    {
        private string[] pedList = new string[] { "A_F_Y_Hippie_01", "A_M_Y_Skater_01", "A_M_M_FatLatin_01", "A_M_M_EastSA_01", "A_M_Y_Latino_01", "G_M_Y_FamDNF_01", "G_M_Y_FamCA_01", "G_M_Y_BallaSout_01", "G_M_Y_BallaOrig_01", "G_M_Y_BallaEast_01", "G_M_Y_StrPunk_02", "S_M_Y_Dealer_01", "A_M_M_RurMeth_01", "A_M_Y_MethHead_01", "A_M_M_Skidrow_01", "S_M_Y_Dealer_01", "a_m_y_mexthug_01", "G_M_Y_MexGoon_03", "G_M_Y_MexGoon_02", "G_M_Y_MexGoon_01", "G_M_Y_SalvaGoon_01", "G_M_Y_SalvaGoon_02", "G_M_Y_SalvaGoon_03", "G_M_Y_Korean_01", "G_M_Y_Korean_02", "G_M_Y_StrPunk_01" };
        private string[] Bicycles = new string[] { "bmx", "Cruiser", "Fixter", "Scorcher", "tribike3", "tribike2", "tribike" };
        private Ped _subject;
        private Vehicle _Bike;
        private Vector3 _SpawnPoint;
        private Vector3 _Location1;
        private Vector3 _Location2;
        private Vector3 _Location3;
        private Vector3 _Location4;
        private Vector3 _Location5;
        private Blip _Blip;
        private LHandle _pursuit;
        private bool _IsStolen = false;
        private bool _startedPursuit = false;
        private bool _alreadySubtitleIntrod = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            _Location1 = new Vector3(1720.068f, 1535.201f, 84.72424f);
            _Location2 = new Vector3(2563.921f, 5393.056f, 44.55834f);
            _Location3 = new Vector3(-1826.79f, 4697.899f, 56.58701f);
            _Location4 = new Vector3(-1344.75f, -757.6135f, 11.10569f);
            _Location5 = new Vector3(1163.919f, 449.0514f, 82.59987f);
            Random random = new Random();
            List<string> list = new List<string>
            {
                "Location1",
                "Location2",
                "Location3",
                "Location4",
                "Location5",
            };
            int num = random.Next(0, 5);
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
            ShowCalloutAreaBlipBeforeAccepting(_SpawnPoint, 100f);
            switch (new Random().Next(1, 2))
            {
                case 1:
                    _IsStolen = true;
                    break;
                case 2:
                    break;
            }
            CalloutMessage = "~b~Meldkamer:~s~ Fietser op de Snelweg";
            CalloutPosition = _SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS SUSPICIOUS_PERSON IN_OR_ON_POSITION", _SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("UnitedCallouts Log: Bicycle callout accepted.");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Fietser op de snelweg", "~b~Meldkamer:~w~ Iemand heeft de politie gebeld omdat er iemand met een ~g~fiets~w~ rijdt op de ~o~snelweg~w~! Reageer met ~y~CODE 2");

            _subject = new Ped(this.pedList[new Random().Next((int)pedList.Length)], _SpawnPoint, 0f);
            _Bike = new Vehicle(this.Bicycles[new Random().Next((int)Bicycles.Length)], _SpawnPoint, 0f);
            _subject.WarpIntoVehicle(_Bike, -1);

            _Blip = _Bike.AttachBlip();
            _Blip.Color = Color.LightBlue;
            _Blip.EnableRoute(Color.Yellow);

            _subject.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.FollowTraffic);
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (_subject.Exists()) _subject.Delete();
            if (_Bike.Exists()) _Bike.Delete();
            if (_Blip.Exists()) _Blip.Delete();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (_subject.DistanceTo(Game.LocalPlayer.Character) < 20f)
                {
                    if (_IsStolen == true && _startedPursuit == false)
                    {
                        if (_Blip.Exists()) _Blip.Delete();
                        _pursuit = Functions.CreatePursuit();
                        Functions.AddPedToPursuit(_pursuit, _subject);
                        Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                        _startedPursuit = true;
                        _Bike.IsStolen = true;
                        Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts Computer", "~y~Meldkamer Informatie", "De ~g~fiets~w~ van de verdachte is een ~o~" + _Bike.Model.Name + "~w~. De ~g~fiets~w~ is ~r~gestolen~w~!");
                        GameFiber.Wait(2000);
                    }
                    if (_subject.DistanceTo(Game.LocalPlayer.Character) < 25f && Game.LocalPlayer.Character.IsOnFoot && _alreadySubtitleIntrod == false && _pursuit == null)
                    {
                        Game.DisplayNotification("Doe een normale aanhouding van de ~o~verdachte~w~.");
                        Game.DisplayNotification("~b~Meldkamer:~w~ Serienummer controleren.....");
                        GameFiber.Wait(600);
                        Game.DisplayNotification("~b~Meldkamer:~w~ We hebben het serienummer van de fiets nagetrokken.<br>Model: ~o~" + _Bike.Model.Name + "<br>~w~Serienummer: ~o~" + _Bike.LicensePlate + "");
                        _alreadySubtitleIntrod = true;
                        return;
                    }
                }
                if (_subject.Exists() && Functions.IsPedArrested(_subject) && _IsStolen && _subject.DistanceTo(Game.LocalPlayer.Character) < 15f)
                {
                    Game.DisplaySubtitle("~y~Verdachte: ~w~Laat me alsjeblieft gaan! ik breng de fiets direct terug!", 4000);
                }
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (_subject.IsDead) End();
                if (Functions.IsPedArrested(_subject)) End();
            }, "FietserSW [DutchCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (_subject.Exists()) _subject.Dismiss();
            if (_Bike.Exists()) _Bike.Dismiss();
            if (_Blip.Exists()) _Blip.Delete();
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Fiets op de snelweg", "~b~CODE 4");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH WE_ARE_CODE FOUR NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }
    }
}