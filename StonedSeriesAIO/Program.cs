using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace StonedSeriesAIO
{
    internal class Program
    {
       private static void Main(string[] args)
        {
           string ChampionSwitch = ObjectManager.Player.ChampionName.ToLowerInvariant();

           switch(ChampionSwitch)
           {
               case "amumu":
                new Amumu();
                Game.PrintChat("<font color='#FF00BF'>Stoned Series {0} Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>",ChampionSwitch);
                break;

               case "drmundo":
                new DrMundo();
                Game.PrintChat("<font color='#FF00BF'>Stoned Series {0} Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>", ChampionSwitch);
                break;

              case "jarvaniv":
                new JarvanIV();
                Game.PrintChat("<font color='#FF00BF'>Stoned Series {0} Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>", ChampionSwitch);
                break;

               case "ryze":
                new Ryze();
                Game.PrintChat("<font color='#FF00BF'>Stoned Series {0} Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>", ChampionSwitch);
                break;

               case "volibear":
                new Volibear();
                Game.PrintChat("<font color='#FF00BF'>Stoned Series {0} Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>", ChampionSwitch);
                break;

               default:
                Game.PrintChat("{0} not supported in Stoned Series",ChampionSwitch);
                   break;
           }
           
           
        }
    }
}
