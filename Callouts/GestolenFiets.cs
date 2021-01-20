using Rage;
using System;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;

namespace DutchCallouts.Callouts
{
    [CalloutInfo("GestolenFiets", CalloutProbability.Medium)]
    public class GestolenFiets : Callout
    {
        private string[] GestolenFietsList = new string[] { "BMX", "CRUISER", "TRIBIKE", "FIXTER", "TRIBIKE3", "TRIBIKE2", "TRIBIKE", "SCORCHER" };
        private Ped _Suspect;
        private Vehicle _GestolenFiets;
        private Vector3 _SpawnPoint;
        private Blip _Blip;
        private LHandle _Pursuit;
        private bool _PursuitCreated = false;
        private bool _notificationDisplayed = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            _SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(_SpawnPoint, 50f);
            CalloutMessage = "~b~Meldkamer:~s~ Gestolen Fiets";
            CalloutPosition = _SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION", _SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("DutchCallouts Log: GestolenFiets callout accepted.");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Gestolen Fiets", "~b~Meldkamer: ~w~Een Fiets is gestolen. reageer met ~r~CODE 3");

            _GestolenFiets = new Vehicle(this.GestolenFietsList[new Random().Next((int)GestolenFietsList.Length)], _SpawnPoint);
            _GestolenFiets.IsPersistent = true;

            _Suspect = _GestolenFiets.CreateRandomDriver();
            _Suspect.BlockPermanentEvents = true;
            _Suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency);

            _Blip = _Suspect.AttachBlip();
            _Blip.IsFriendly = false;
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (_GestolenFiets.DistanceTo(Game.LocalPlayer.Character) < 20f && !_notificationDisplayed)
            {
                Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Meldkamer", " ~g~Informatie~w~ laden uit de ~y~LSPD Database~w~...");
                GameFiber.Wait(1000);
                Functions.DisplayVehicleRecord(_GestolenFiets, true);
                _notificationDisplayed = true;
            }
            if (!_PursuitCreated && Game.LocalPlayer.Character.DistanceTo(_Suspect.Position) < 30f)
            {
                _Pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(_Pursuit, _Suspect);
                Functions.SetPursuitIsActiveForPlayer(_Pursuit, true);
                _PursuitCreated = true;
            }
            if (_PursuitCreated && !Functions.IsPursuitStillRunning(_Pursuit))
            {
                End();
            }
            if (Game.LocalPlayer.Character.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();
            if (_Suspect.IsDead) End();
            if (Functions.IsPedArrested(_Suspect)) End();
            base.Process();
        }

        public override void End()
        {
            if (_Suspect.Exists()) { _Suspect.Dismiss(); }
            if (_GestolenFiets.Exists()) { _GestolenFiets.Dismiss(); }
            if (_Blip.Exists()) { _Blip.Delete(); }
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Gestolen Fiets", "~b~CODE 4.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH WE_ARE_CODE FOUR NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }
    }
}