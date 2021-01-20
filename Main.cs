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
using DutchCallouts.Versie;


namespace DutchCallouts
{
    public class Main : Plugin
    {
        //For further information and explanation please check the PDF file.
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
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
                       "DutchCallouts", // Title
                       "~y~v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                       " ~o~door Maarten", "~b~Succesvol ingeladen!!"); // Subtitle
                    Versie.Versie.isUpdateAvailable();
                });
        }

        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.KillerClownGezien));
            Functions.RegisterCallout(typeof(Callouts.FietserSW));
            Functions.RegisterCallout(typeof(Callouts.Gevecht));
            Functions.RegisterCallout(typeof(Callouts.Truck));
            Functions.RegisterCallout(typeof(Callouts.Backup));
            Functions.RegisterCallout(typeof(Callouts.GezondheidCheck));
            Functions.RegisterCallout(typeof(Callouts.Onruststoker));
            Functions.RegisterCallout(typeof(Callouts.Inbraak));
            Functions.RegisterCallout(typeof(Callouts.Drug));
            Functions.RegisterCallout(typeof(Callouts.Schieter));
            Functions.RegisterCallout(typeof(Callouts.Gezocht));
            Functions.RegisterCallout(typeof(Callouts.Gijzelaars));
            Functions.RegisterCallout(typeof(Callouts.GestolenFiets));
        }
    }
}
