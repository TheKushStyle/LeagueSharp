﻿#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using System.Threading;
#endregion

namespace StonedSeriesAIO
{
    internal class Volibear
    {
        private const string Champion = "Volibear";

        private static Orbwalking.Orbwalker Orbwalker;

        private static List<Spell> SpellList = new List<Spell>();

        private static Spell Q;

        private static Spell W;

        private static Spell E;

        private static Spell R;

        private static Menu Config;

        private static Items.Item RDO;

        private static Items.Item DFG;

        private static Items.Item YOY;

        private static Items.Item BOTK;

        private static Items.Item HYD;

        private static Items.Item CUT;

        private static Obj_AI_Hero Player;

        public Volibear()
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {

            Q = new Spell(SpellSlot.Q, 600);
            W = new Spell(SpellSlot.W, 405);
            E = new Spell(SpellSlot.E, 400);
            R = new Spell(SpellSlot.R, 125);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            RDO = new Items.Item(3143, 490f);
            HYD = new Items.Item(3074, 175f);
            DFG = new Items.Item(3128, 750f);
            YOY = new Items.Item(3142, 185f);
            BOTK = new Items.Item(3153, 450f);
            CUT = new Items.Item(3144, 450f);
            

            //Menu Volibear
            Config = new Menu(Champion, "StonedVolibear", true);

            //ts
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            //orb
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            //Combo Menu
            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseItems", "Use Items")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("CountW", "Min % enemy hp for W use").SetValue(new Slider(100, 0, 100)));
            Config.SubMenu("Combo").AddItem(new MenuItem("AutoR", "Use Auto R in Combo")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("CountR", "Num of Enemy in Range to Ult").SetValue(new Slider(1, 5, 0)));
            Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            //Harass Menu
            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("HarassE", "Use E in Harass")).SetValue(true);
            Config.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass!").SetValue(new KeyBind(97, KeyBindType.Press)));

            //Drawings
            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawR", "Draw R")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            Config.AddToMainMenu();

            Game.OnGameUpdate += GameOnGameUpdate;
            Drawing.OnDraw += OnDraw;
            
            Game.PrintChat("<font color='#FF00BF'>Stoned Volibear Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>");
        }

        private static void GameOnGameUpdate(EventArgs args)
        {
            Player = ObjectManager.Player;

            Orbwalker.SetAttack(true);
            if (Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }

            if (Config.Item("ActiveHarass").GetValue<KeyBind>().Active)
            {
                Harass();
            }
        
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(1100, TargetSelector.DamageType.Physical);
            if (target == null) return;

            if (Config.Item("HarassE").GetValue<bool>() && Player.Distance(target) <= E.Range && E.IsReady())
            {
                E.Cast();
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(1100, TargetSelector.DamageType.Physical);
            if (target == null) return;

            //Combo
            if (Player.Distance(target) <= Q.Range && Q.IsReady() && (Config.Item("UseQCombo").GetValue<bool>()))
            {
                Q.Cast();
            }
            if  (Player.Distance(target) <= E.Range && E.IsReady() && (Config.Item("UseECombo").GetValue<bool>()))
            {
                E.Cast();
            }
            //WLogic
           float health = target.Health;
           float maxhealth = target.MaxHealth;
           float wcount = Config.Item("CountW").GetValue<Slider>().Value;
            if (health < ((maxhealth * wcount)/100))
            {
                if (Config.Item("UseWCombo").GetValue<bool>() && W.IsReady())
                {
                    W.Cast(target);
                }
            }
            if (Config.Item("AutoR").GetValue<bool>() && R.IsReady() && (GetNumberHitByR(target) >= Config.Item("CountR").GetValue<Slider>().Value))
            {
                R.Cast();
            }
            if (Config.Item("UseItems").GetValue<bool>())
            {
                if (Player.Distance(target) <= RDO.Range)
                {
                    RDO.Cast(target);
                }
                if (Player.Distance(target) <= HYD.Range)
                {
                    HYD.Cast(target);
                }
                if (Player.Distance(target) <= DFG.Range)
                {
                    DFG.Cast(target);
                }
                if (Player.Distance(target) <= BOTK.Range)
                {
                    BOTK.Cast(target);
                }
                if (Player.Distance(target) <= CUT.Range)
                {
                    CUT.Cast(target);
                }
                if (Player.Distance(target) <= 125f)
                {
                    YOY.Cast();
                }
            }
            
        }

        private static int GetNumberHitByR(Obj_AI_Base target)
        {
            int totalHit = 0;
            foreach (Obj_AI_Hero current in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (current.IsEnemy && Vector3.Distance(Player.ServerPosition, current.ServerPosition) <= R.Range)
                {
                    totalHit = totalHit + 1;
                }
            }
            return totalHit;
        }

        private static void OnDraw(EventArgs args)
        {            
                if (Config.Item("CircleLag").GetValue<bool>())
                {
                    if (Config.Item("DrawQ").GetValue<bool>())
                    {
                        Render.Circle.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.White,
                            Config.Item("CircleThickness").GetValue<Slider>().Value);
                    }
                    if (Config.Item("DrawW").GetValue<bool>())
                    {
                        Render.Circle.DrawCircle(ObjectManager.Player.Position, W.Range, System.Drawing.Color.White,
                            Config.Item("CircleThickness").GetValue<Slider>().Value);
                    }
                    if (Config.Item("DrawE").GetValue<bool>())
                    {
                        Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, System.Drawing.Color.White,
                            Config.Item("CircleThickness").GetValue<Slider>().Value);
                    }
                    if (Config.Item("DrawR").GetValue<bool>())
                    {
                        Render.Circle.DrawCircle(ObjectManager.Player.Position, R.Range, System.Drawing.Color.White,
                            Config.Item("CircleThickness").GetValue<Slider>().Value);
                    }
                }
                else
                {
                    if (Config.Item("DrawQ").GetValue<bool>())
                    {
                        Drawing.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.White);
                    }
                    if (Config.Item("DrawW").GetValue<bool>())
                    {
                        Drawing.DrawCircle(ObjectManager.Player.Position, W.Range, System.Drawing.Color.White);
                    }
                    if (Config.Item("DrawE").GetValue<bool>())
                    {
                        Drawing.DrawCircle(ObjectManager.Player.Position, E.Range, System.Drawing.Color.White);
                    }
                    if (Config.Item("DrawR").GetValue<bool>())
                    {
                        Drawing.DrawCircle(ObjectManager.Player.Position, R.Range, System.Drawing.Color.White);
                    }

                }  
        }

    
    }
}
