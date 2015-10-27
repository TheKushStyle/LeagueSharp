using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Drawing;


namespace KappaSeries
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
            
        }

        static void OnGameLoad(EventArgs args)
        {
            try
            {
                var cs = ObjectManager.Player.ChampionName;
                Notifications.AddNotification("Feel free to donate to TheKushStyle@gmail.com <3 ").SetTextColor(Color.MediumVioletRed);
                const string say = ("Kappa Series Loaded : ");
                const string def = ("Kappa Series Doesn't Support : ");
                switch (cs)
                {

                    case "Aatrox":
                        new Aatrox();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color.LawnGreen);
                        break;

                    case "Ahri":
                        new Ahri();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color.LawnGreen);
                        break;

                    case "Akali":
                        new Akali();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color.LawnGreen);
                        break;

                    case "Volibear":
                        new Volibear();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color.LawnGreen);
                        break;

                    case "DrMundo":
                        new DrMundo();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color.LawnGreen);
                        break;

                    default:
                        Notifications.AddNotification(say + def, 5000).SetTextColor(Color.Crimson);
                        break;
                }
            }
            catch (Exception e)
            {
            Console.WriteLine(e);
            }
        }
    }
}
