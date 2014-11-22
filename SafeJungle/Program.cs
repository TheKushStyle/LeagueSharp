#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion 

namespace SafeJungle
{
   internal class Program
    {
        private static Menu Config;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
                 
        Config = new Menu("SafeJungle", "SafeJungle", true);

        Config.AddSubMenu(new Menu("Walk to Safe Spot", "WalkSpot"));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("EnableWalk", "Enable Walk to Spot?")).SetValue(true);
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("TeamSwitch", "off = Purple").SetValue(true));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot1", "Red").SetValue(new KeyBind(35,KeyBindType.Press)));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot2", "Wraiths").SetValue(new KeyBind(40, KeyBindType.Press)));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot3", "Wolves").SetValue(new KeyBind(34, KeyBindType.Press)));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot4", "Blue").SetValue(new KeyBind(37, KeyBindType.Press)));
        Config.SubMenu("WalkSpot").AddItem(new MenuItem("WalkSpot5", "Golems").SetValue(new KeyBind(12, KeyBindType.Press)));

        Config.AddSubMenu(new Menu("Drawings", "Drawings"));
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot1Draw", "Draw Red")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot2Draw", "Draw Wraiths")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot3Draw", "Draw Wolves")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot4Draw", "Draw Blue")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("Spot5Draw", "Draw Golems")).SetValue(true);
        Config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
        Config.SubMenu("Drawings").AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
        Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(30, 100, 1)));
        Config.SubMenu("Drawings").AddItem(new MenuItem("Width", "Circle Width").SetValue(new Slider(30, 100)));
        
        Config.AddToMainMenu();
        
        Game.OnGameUpdate += OnGameUpdate;
        Drawing.OnDraw += OnDraw;
        Game.PrintChat("<font color='#FF00BF'>Safe Jungle Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>");
        }

        private static void OnDraw(EventArgs args)
        {
            if (Config.Item("CircleLag").GetValue<bool>() == true)
            {
                var Width = Config.Item("Width").GetValue<Slider>().Value;
                if (Config.Item("TeamSwitch").GetValue<bool>() == true)
                //blue
                {

                if (Config.Item("Spot1Draw").GetValue<bool>()) //red
                {
                    Utility.DrawCircle(new Vector3(7592f, 3150f, 52.58508f), Width, Color.Aqua ,Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("Spot2Draw").GetValue<bool>())//wraith
                {
                    Utility.DrawCircle(new Vector3(7164f,4610f,48.52682f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("Spot3Draw").GetValue<bool>())//wolves
                {
                    Utility.DrawCircle(new Vector3(4562f, 6168f, 51.48312f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("Spot4Draw").GetValue<bool>())//blue
                {
                    Utility.DrawCircle(new Vector3(3374f, 8620f, 10.76506f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("Spot5Draw").GetValue<bool>())//golem
                {
                    Utility.DrawCircle(new Vector3(7646f, 2130f, 51.16118f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
             
                }
                else //red
                {
                    if (Config.Item("Spot1Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(7316f, 11606f, 51.27026f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (Config.Item("Spot2Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(7280f, 10100f, 51.82149f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (Config.Item("Spot3Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(10218f, 9004f, 49.67321f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (Config.Item("Spot4Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(11548f, 6232f, 51.33665f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                    if (Config.Item("Spot5Draw").GetValue<bool>())
                    {
                        Utility.DrawCircle(new Vector3(7124f, 12806f, 56.4768f), Width, Color.Aqua, Config.Item("CircleThickness").GetValue<Slider>().Value,
                            Config.Item("CircleQuality").GetValue<Slider>().Value);
                    }
                }
            }
        }

        

        private static void OnGameUpdate(EventArgs args)
        {
            
            if (Config.Item("EnableWalk").GetValue<bool>() == true)
            {
               //Blue Side
               if (Config.Item("TeamSwitch").GetValue<bool>() == true)
                {
                   if      (Config.Item("WalkSpot1").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7592f,3150f)).Send();//red
                   }
                   else if (Config.Item("WalkSpot2").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7164f, 4610f)).Send();//wraith
                   }
                   else if (Config.Item("WalkSpot3").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(4562f, 6168f)).Send();//wolves
                   }
                   else if (Config.Item("WalkSpot4").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(3374f, 8620f)).Send();//blue
                   }
                   else if (Config.Item("WalkSpot5").GetValue<KeyBind>().Active)
                   {
                       Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7646f, 2130f)).Send();//golem
                   }
                   }
                //Purple Side
               else
                {
                    if      (Config.Item("WalkSpot1").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7316f, 11606f)).Send();//red
                    }
                    else if (Config.Item("WalkSpot2").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7280f, 10100f)).Send();//wraith
                    }
                    else if (Config.Item("WalkSpot3").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(10218f, 9004f)).Send();//wolves
                    }
                    else if (Config.Item("WalkSpot4").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(11548f, 6232f)).Send();//blue
                    }
                    else if (Config.Item("WalkSpot5").GetValue<KeyBind>().Active)
                    {
                        Packet.C2S.Move.Encoded(new Packet.C2S.Move.Struct(7124f, 12806f)).Send();//golem
                    }
                }
            }
        }
    }
}

