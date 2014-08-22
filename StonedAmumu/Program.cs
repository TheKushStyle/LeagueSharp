#region
using System;
using System.Collections;
using System.Linq;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using System.Collections.Generic;
using System.Threading;
#endregion

//Todo: nothing ATM feel free to suggest


namespace StonedAmumu
{
    internal class Program
    {

       

        private const string Champion = "Amumu";

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

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }


        static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
            if (ObjectManager.Player.BaseSkinName != Champion) return;

            Q = new Spell(SpellSlot.Q, 1000);
            W = new Spell(SpellSlot.W, 300);
            E = new Spell(SpellSlot.E, 350);
            R = new Spell(SpellSlot.R, 525);

            Q.SetSkillshot(0.250f, 80, 2000, true, Prediction.SkillshotType.SkillshotLine);

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

            //Menu Amumu
            Config = new Menu(Champion, "StonedAmumu", true);

            //Ts
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
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
            Config.SubMenu("Combo").AddItem(new MenuItem("AutoR", "Auto R")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("CountR", "Num of Enemy in Range to Ult").SetValue(new Slider(1, 5, 0)));
            Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            //JungleClear
            Config.AddSubMenu(new Menu("Jungle Clear", "Jungle"));
            Config.SubMenu("Jungle").AddItem(new MenuItem("UseQClear", "Use Q")).SetValue(true);
            Config.SubMenu("Jungle").AddItem(new MenuItem("UseWClear", "Use W")).SetValue(true);
            Config.SubMenu("Jungle").AddItem(new MenuItem("UseEClear", "Use E")).SetValue(true);
            Config.SubMenu("Jungle").AddItem(new MenuItem("ActiveClear", "Jungle Key").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            
            //Drawings
            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawR", "Draw R")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            Config.AddToMainMenu();

            Game.OnGameUpdate += OnGameUpdate;
            Drawing.OnDraw += OnDraw;



            Game.PrintChat("<font color='#FF00BF'>Stoned Amumu Loaded By</font> <font color='#FF0000'>The</font><font color='#FFFF00'>Kush</font><font color='#40FF00'>Style</font>");
        }

        private static void OnGameUpdate(EventArgs args)
        {
            Player = ObjectManager.Player;


            Orbwalker.SetAttacks(true);
            if (Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            if (Config.Item("ActiveClear").GetValue<KeyBind>().Active)
            {
                JungleClear();
            }

        }

        private static void JungleClear()
        { //credits to xQx
            var JungleFarmActive = Config.Item("JungleClear").GetValue<KeyBind>().Active;

            if (JungleFarmActive)
            {
                var useQ = Config.Item("UseQClear").GetValue<bool>();
                var useW = Config.Item("UseWClear").GetValue<bool>();
                var useE = Config.Item("UseEClear").GetValue<bool>();

                var mobs = MinionManager.GetMinions(Player.ServerPosition, Q.Range,
                    MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

                if (mobs.Count > 0)
                {
                    if (Q.IsReady() && useQ && IsPositionSafe(mobs[0], Q))
                        Q.CastOnUnit(mobs[0]);

                    if (W.IsReady() && useW)
                        W.Cast();

                    if (E.IsReady() && useE)
                        E.CastOnUnit(mobs[0]);
                }
            }
        }

        public static bool IsPositionSafe(Obj_AI_Base vTarget, Spell vSpell)
        { //Credits to xQx
            Vector2 predPos = vSpell.GetPrediction(vTarget).Position.To2D();
            Vector2 myPos = ObjectManager.Player.Position.To2D();
            Vector2 newPos = (vTarget.Position.To2D() - myPos);
            newPos.Normalize();

            Vector2 checkPos = predPos + newPos * (vSpell.Range - Vector2.Distance(predPos, myPos));
            Obj_Turret closestTower = null;

            foreach (Obj_Turret tower in ObjectManager.Get<Obj_Turret>().Where(tower => tower.IsValid && !tower.IsDead && tower.Health != 0 && tower.IsEnemy))
            {
                if (Vector3.Distance(tower.Position, ObjectManager.Player.Position) < 1450)
                    closestTower = tower;
            }

            if (closestTower == null)
                return true;

            if (Vector2.Distance(closestTower.Position.To2D(), checkPos) <= 910)
                return false;

            return true;
        }


        private static void Combo()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target == null) return;

            //Combo
            if (Player.Distance(target) <= Q.Range && Q.IsReady() && (Config.Item("UseQCombo").GetValue<bool>()))
            {
                Q.Cast(target);
            }
            if (W.IsReady() && (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1) && (Config.Item("UseWCombo").GetValue<bool>()))
                if (Player.ServerPosition.Distance(target.Position) < W.Range)
                {
                    W.Cast();
                }
            if (W.IsReady() && (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 2) && (Config.Item("UseWCombo").GetValue<bool>()))
                if (Player.ServerPosition.Distance(target.Position) > W.Range)
                {
                    W.Cast();
                }
            if (Player.Distance(target) <= E.Range && E.IsReady() && (Config.Item("UseECombo").GetValue<bool>()))
            {
                E.Cast();
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
            if (Config.Item("AutoR").GetValue<bool>())
            {

                if (GetNumberHitByR(target) >= Config.Item("CountR").GetValue<Slider>().Value)
                {
                    R.Cast();
                }
            }


        }

        private static int GetNumberHitByR(Obj_AI_Base target) // Credits to Trelli For helping me with this one!
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
            if (Config.Item("CircleLag").GetValue<bool>()) // Credits to SKOBOL
            {
                if (Config.Item("DrawQ").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("DrawW").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, W.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("DrawE").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, E.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("DrawR").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, R.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
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
