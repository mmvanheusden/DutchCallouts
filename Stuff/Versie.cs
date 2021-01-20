using System;
using Rage;
using System.Net;

namespace DutchCallouts.Versie
{
    public class Versie
    {
        public static bool isUpdateAvailable()
        {
            string curVersion = Settings.CalloutVersion;

            Uri latestVersionUri = new Uri("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=32010&textOnly=1"); //Use instead of "20730" your file number on lcpdfr.com
            WebClient webClient = new WebClient();
            string receivedData = string.Empty;

            try
            {
                receivedData = webClient.DownloadString("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=32010&textOnly=1").Trim(); //Use instead of "20730" your file number on lcpdfr.com
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~DutchCallouts Fout", "~y~Fout bij controleren op updates", "Controleer of je ~o~online~w~, bent.");

                Game.Console.Print();
                Game.Console.Print("================================================ DutchCallouts WARNING =====================================================");
                Game.Console.Print();
                Game.Console.Print("[LOG]: Failed to check for a update.");
                Game.Console.Print("[LOG]: Please check if you are online, or try to reload the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================ DutchCallouts WARNING =====================================================");
                Game.Console.Print();
            }
            if (receivedData != Settings.CalloutVersion)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~DutchCallouts Fout", "~y~Een nieuwe versie is beschikbaar!", "Huidige Versie: ~r~" + curVersion + "~w~<br>Nieuwe Versie: ~o~" + receivedData);

                Game.Console.Print();
                Game.Console.Print("================================================ DutchCallouts WARNING =====================================================");
                Game.Console.Print();
                Game.Console.Print("[LOG]: A new version of DutchCallouts is available! Update the Version, or play on your own risk.");
                Game.Console.Print("[LOG]: Current Version:  " + curVersion);
                Game.Console.Print("[LOG]: New Version:  " + receivedData);
                Game.Console.Print();
                Game.Console.Print("================================================ DutchCallouts WARNING =====================================================");
                Game.Console.Print();
                return true;
            }
            else
            {
                Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~DutchCallouts", "", "Nieuwste versie geïnstaleerd.<br>Huidige Versie: ~g~" + curVersion + "");
                return false;
            }
        }
    }
}