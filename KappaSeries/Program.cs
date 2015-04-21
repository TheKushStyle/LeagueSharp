using LeagueSharp;


namespace KappaSeries
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string CS = ObjectManager.Player.ChampionName.ToString();
            Game.PrintChat("<font color='#FF0000'>Feel free to donate to TheKushStyle@gmail.com <3 </font>");
            switch (CS)
            {
                case "Aatrox":
                    new Aatrox();
                    Game.PrintChat("<font color='#00FF15'>Kappa Series Loaded : {0}", CS);
                    break;

                default:
                    Game.PrintChat("<font color='#00FF15'>Kappa Series Doesn't Support : {0}", CS);
                    break;
            }
        }
    }
}
