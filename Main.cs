using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using Rage;
using System.Windows.Forms;
using DutchCallouts;
using System.Reflection;
using DutchCallouts.Stuff;


namespace DutchCallouts
{
    public class Main : Plugin
    {
        //For further information and explanation please check the PDF file.
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " door Maarten is ingeladen.");
            Game.LogTrivial("Ga on duty om de plugin volledig in te laden.");
        }
        public override void Finally()
        {
            Game.LogTrivial("DutchCallouts is opgeschoond.");
        }

        static void OnOnDutyStateChangedHandler(bool onDuty)
        {
            if (onDuty)
                GameFiber.StartNew(delegate
                {
                    RegisterCallouts();
                    Game.DisplayNotification(
                        "web_lossantospolicedept", // You can find all logos/images in OpenIV
                        "web_lossantospolicedept", // You can find all logos/images in OpenIV
                        "DutchCallouts",
                        "",
                        "~b~succesvol ingeladen!");
                });
        }

        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.GestolenVoertuig));
            Functions.RegisterCallout(typeof(Callouts.KillerClownGezien));
            Functions.RegisterCallout(typeof(Callouts.FietserSW));
            Functions.RegisterCallout(typeof(Callouts.Gevecht));
            Functions.RegisterCallout(typeof(Callouts.Truck));
            Functions.RegisterCallout(typeof(Callouts.SuperCar));
            Functions.RegisterCallout(typeof(Callouts.Backup));
            Functions.RegisterCallout(typeof(Callouts.GezondheidCheck));
            Functions.RegisterCallout(typeof(Callouts.Onruststoker));
            Functions.RegisterCallout(typeof(Callouts.Inbraak));
            Functions.RegisterCallout(typeof(Callouts.Drug));
            Functions.RegisterCallout(typeof(Callouts.Schieter));
            Functions.RegisterCallout(typeof(Callouts.Gezocht));
        }
    }
}
