using LeagueSharp;
using LeagueSharp.Common;
using System;


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
                Game.PrintChat("<font color='#FF0000'>Feel free to donate to TheKushStyle@gmail.com <3 </font>");
                switch (cs)
                {
                    case "Aatrox":
                        new Aatrox();
                        Game.PrintChat("<font color='#00FF15'>Kappa Series Loaded : {0}", cs);
                        break;

                    case "Ahri":
                        new Ahri();
                        Game.PrintChat("<font color='#00FF15'>Kappa Series Loaded : {0}", cs);
                        break;

                    default:
                        Game.PrintChat("<font color='#00FF15'>Kappa Series Doesn't Support : {0}", cs);
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
