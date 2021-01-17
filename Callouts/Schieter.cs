using LSPD_First_Response.Mod.API;
using System;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using Rage.Native;
using System.Drawing;

namespace DutchCallouts.Callouts
{
    [CalloutInfo("Schieter", CalloutProbability.Medium)]
    public class Schieter : Callout
    {
        private string[] wepList = new string[] { "WEAPON_RAILGUN", "WEAPON_CARBINERIFLE_MK2", "WEAPON_BULLPUPRIFLE_MK2", "WEAPON_ADVANCEDRIFLE", "WEAPON_SPECIALCARBINE", "WEAPON_RAYCARBINE", "WEAPON_RAYMINIGUN" };
        private Ped _subject;
        private Ped _V1;
        private Ped _V2;
        private Ped _V3;
        private Vector3 _SpawnPoint;
        private Vector3 _searcharea;
        private Blip _Blip;
        private int _scenario = 0;
        private bool _hasBegunAttacking = false;
        private bool _isArmed = false;
        private bool _hasPursuitBegun = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            _scenario = new Random().Next(0, 100);
            ShowCalloutAreaBlipBeforeAccepting(_SpawnPoint, 100f);
            CalloutMessage = "~b~Meldkamer:~s~ Meldingen over een gepanserd persoon.";
            CalloutPosition = _SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", _SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("DutchCallouts Log: Schieter callout accepted.");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Gepanserd Persoon", "~b~Meldkamer: ~w~Het ~r~Gepanserd Persoon~w~ is gespot met een vuurwapen! Zoek in het ~y~gele gebied~w~ naar het Gepanserd Persoon. Reageer met ~r~CODE 3~w~!");

            _SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));
            _subject = new Ped("u_m_y_juggernaut_01", _SpawnPoint, 0f);
            _subject.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
            _subject.BlockPermanentEvents = true;
            _subject.IsPersistent = true;
            _subject.CanRagdoll = false;
            _subject.Armor = 1200;
            _subject.Health = 1200;
            _subject.MaxHealth = 1200;
            _subject.CanAttackFriendlies = true;
            _subject.Tasks.Wander();
            NativeFunction.Natives.SET_PED_SUFFERS_CRITICAL_HITS(_subject, false);
            NativeFunction.Natives.SetPedPathCanUseClimbovers(_subject, true);
            Functions.SetPedCantBeArrestedByPlayer(_subject, true);

            _V1 = new Ped(_SpawnPoint);
            _V2 = new Ped(_SpawnPoint);
            _V3 = new Ped(_SpawnPoint);
            _V1.Health = 150;
            _V2.Health = 150;
            _V3.Health = 150;
            _V1.Tasks.Wander();
            _V2.Tasks.Wander();
            _V3.Tasks.Wander();

            _searcharea = _SpawnPoint.Around2D(1f, 2f);
            _Blip = new Blip(_searcharea, 90f);
            _Blip.Color = Color.Yellow;
            _Blip.EnableRoute(Color.Yellow);
            _Blip.Alpha = 5f;
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (_Blip.Exists()) _Blip.Delete();
            if (_subject.Exists()) _subject.Delete();
            if (_V1.Exists()) _V1.Dismiss();
            if (_V2.Exists()) _V2.Dismiss();
            if (_V3.Exists()) _V3.Dismiss();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (_subject.Exists() && _subject.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 35f && !_isArmed)
                {
                    if (_Blip.Exists()) _Blip.Delete();
                    _subject.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
                    _isArmed = true;
                }
                if (_subject.Exists() && _subject.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 50f && !_hasBegunAttacking)
                {
                    if (_scenario > 40)
                    {
                        _subject.KeepTasks = true;
                        new RelationshipGroup("AG");
                        new RelationshipGroup("VI");
                        _subject.RelationshipGroup = "AG";
                        _V1.RelationshipGroup = "VI";
                        _V2.RelationshipGroup = "VI";
                        _V3.RelationshipGroup = "VI";
                        Game.SetRelationshipBetweenRelationshipGroups("AG", "VI", Relationship.Hate);
                        _subject.Tasks.FightAgainstClosestHatedTarget(1000f);
                        Game.DisplayNotification("~b~Meldkamer: ~w~Het Gepanserd Persoon is aan het ~r~schieten~w~ op mensen!");
                        GameFiber.Wait(2000);
                        _subject.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        _hasBegunAttacking = true;
                        GameFiber.Wait(600);
                    }
                    else
                    {
                        if (!_hasPursuitBegun)
                        {
                            _subject.KeepTasks = true;
                            _subject.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            _hasBegunAttacking = true;
                            GameFiber.Wait(2000);
                        }
                    }
                }
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (_subject.IsDead) End();
                if (Functions.IsPedArrested(_subject)) End();
            }, "Schieter [DutchCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (_subject.Exists()) _subject.Dismiss();
            if (_Blip.Exists()) _Blip.Delete();
            if (_V1.Exists()) _V1.Dismiss();
            if (_V2.Exists()) _V2.Dismiss();
            if (_V3.Exists()) _V3.Dismiss();
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "~y~Gepanserd Persoon", "~b~CODE 4.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH WE_ARE_CODE FOUR NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }
    }
}