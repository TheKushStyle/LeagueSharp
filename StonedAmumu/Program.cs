#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace StonedAmumu
{
    internal class Program
    {

    /* To do :
     * Auto R if enemies in range
     * Drawings
     * JungleClear
     */
        
        private const string Champion = "Amumu";
       
        private static Orbwalking.Orbwalker Orbwalker;
        
        private static List<Spell> SpellList = new List<Spell>();

        private static Spell Q;

        private static Spell W;

        private static Spell E;

        private static Spell R;

        private static Menu Config;

        private static Items.Item RDO;

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

            RDO = new Items.Item(3143, 490);
      

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
            //Config.SubMenu("Combo").AddItem(new MenuItem("AutoR", "Auto R")).SetValue(true);
            //Config.SubMenu("Combo").AddItem(new MenuItem("AutoRNUM", "Auto R if enemies in Range")).SetValue(new Slider(1, 5, 0));
            Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            //JungleClear
            /*Config.AddSubMenu(new Menu("Jungle Clear", "Jungle"));
            Config.SubMenu("Jungle").AddItem(new MenuItem("UseQClear", "Use Q")).SetValue(true);
            Config.SubMenu("Jungle").AddItem(new MenuItem("UseWClear", "Use W")).SetValue(true);
            Config.SubMenu("Jungle").AddItem(new MenuItem("UseEClear", "Use E")).SetValue(true);
            Config.SubMenu("Jungle").AddItem(new MenuItem("ActiveClear", "Jungle Key").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            */
            //Drawings
            /*Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));
            */
            Config.AddToMainMenu();

            Game.OnGameUpdate += OnGameUpdate;
            //Drawing.OnDraw += OnDraw;

            
            
            Game.PrintChat("StonedAmumu Loaded By TheKushStyle");
        }

        private static void OnGameUpdate(EventArgs args)
        {
            Player = ObjectManager.Player;

            Orbwalker.SetAttacks(true);
            if (Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            /*if (Config.Item("ActiveClear").GetValue<KeyBind>().Active)
            {
                JungleClear();
            }
            
            if (Config.Item("AutoR").GetValue<bool>())
            {
                StunR();
            }
            
        }

        private static void StunR()
        {
            if (R.IsReady() && Config.SubMenu("Combo").Item("AutoR").GetValue<bool>())
            {
                int reqHitNum = Config.SubMenu("Combo").Item("AutoRNUM").GetValue<Slider>().Value;
                int hitNum = 0;
                Vector2 castPosition = new Vector2();

                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                {
                    var prediction = R.GetPrediction(enemy);
                    if (prediction.HitChance == Prediction.HitChance.HighHitchance && prediction.TargetsHit > hitNum)
                    {
                        //hitNum = prediction.TargetsHit();
                        castPosition = prediction.CastPosition.To2D();
                    }
                }
                if (hitNum >= reqHitNum)
                    R.Cast(castPosition);
            }
             */ 
        }
        

        private static void Combo()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target == null) return;

            //Combo
            if (Player.Distance(target) <= Q.Range && Q.IsReady())
            {
                Q.Cast(target);
            }
            if (W.IsReady() && (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1))
                if (Player.ServerPosition.Distance(target.Position) < W.Range)
                {
                    W.Cast();
                }
            if (W.IsReady() && Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 2)
                if (Player.ServerPosition.Distance(target.Position) > W.Range)
                {
                    W.Cast();
                }
            if (Player.Distance(target) <= E.Range && E.IsReady())
            {
                E.Cast();
            }
            if (Config.Item("UseItems").GetValue<bool>())
            {
                if (Player.Distance(target) <= RDO.Range)
                {
                    RDO.Cast(target);
                }
            }
           

           
        }


     /*private static void JungleClear()
        {
            var mobs = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range, MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (mobs.Count > 0)
            {
                var mob = mobs[0];
                if (Q.IsReady () && Config.SubMenu("JungleClear").Item("UseQClear").GetValue<bool>())
                {
                    Q.CastOnUnit(mob);
                }
                if (W.IsReady() && (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1) && Config.SubMenu("JungleClear").Item("UseWClear").GetValue<bool>())
                {
                    W.CastOnUnit(mob);
                }
                if (W.IsReady() && (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 2) && Config.SubMenu("JungleClear").Item("UseWClear").GetValue<bool>())
                {
                    W.CastOnUnit(mob);
                }
                if (E.IsReady () && Config.SubMenu("JungleClear").Item("UseEClear").GetValue<bool>())
                {
                    E.CastOnUnit(mob);
                }

                
            }
        } */ 

        }
        /*     
        private static void OnDraw(EventArgs args)
        {
            throw new NotImplementedException();
        }
         */
    }

