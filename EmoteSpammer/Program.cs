#region
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading;
using SharpDX;
using LeagueSharp;
using LeagueSharp.Common;
#endregion

namespace EmoteSpammer
{
    internal class Program
    {
        private static Menu Config;
        private double tick;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }
        
        private static void Game_OnGameLoad(EventArgs args)
        {
            Config = new Menu("EmoteSpammer", "EmoteSpammer", true);
            Config.AddSubMenu(new Menu("On/Off", "On/Off"));
            Config.SubMenu("On/Off").AddItem(new MenuItem("EmotePress", "Emote On Key press")).SetValue(new KeyBind(32, KeyBindType.Press));
            Config.SubMenu("On/Off").AddItem(new MenuItem("EmoteToggable", "Toggleable Emote")).SetValue(new KeyBind('h', KeyBindType.Toggle));
            Config.SubMenu("On/Off").AddItem(new MenuItem("Type", "Wich Emote to spam?")).SetValue(new Slider(1, 3, 1));

            Config.AddToMainMenu();

            Game.OnGameUpdate += OnGameUpdate;        


            Game.PrintChat("<font color='#FF00BF'>EMOTE SPAMMER Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>");
            Game.PrintChat("1 = Laugh, 2 = Taunt, 3 = Joke.");

            //thanks for helping me Fluxy and Brian
        }

        private static void OnGameUpdate(EventArgs args)
        {
            double tick = 0;
            tick = TimeSpan.FromSeconds(Environment.TickCount).Minutes;

            if (ObjectManager.Player.HasBuff("Recall")) return;
            
            //Game.PrintChat(tick.ToString());
            {
                if ((Config.Item("EmotePress").GetValue<KeyBind>().Active) && tick == 59 )
                {
                    SPAM();
                }
                if (Config.Item("EmoteToggable").GetValue<KeyBind>().Active && tick == 59)
                {
                    SPAM();
                }
            }
        }



        private static void SPAM()
        {
            
           if ((Config.Item("Type").GetValue<Slider>().Value) == 1)
            {
               Packet.C2S.Emote.Encoded(new Packet.C2S.Emote.Struct(2)).Send();
               Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(Game.CursorPos.X, Game.CursorPos.Y)).Send();
            }
           if ((Config.Item("Type").GetValue<Slider>().Value) == 2)
           {
               Packet.C2S.Emote.Encoded(new Packet.C2S.Emote.Struct(1)).Send();
               Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(Game.CursorPos.X, Game.CursorPos.Y)).Send();
           }
           if ((Config.Item("Type").GetValue<Slider>().Value) == 3)
           {
               Packet.C2S.Emote.Encoded(new Packet.C2S.Emote.Struct(3)).Send();
               Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(Game.CursorPos.X, Game.CursorPos.Y)).Send();
           }
        }
    }
}
