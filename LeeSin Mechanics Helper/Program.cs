using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace LeeSin_Mechanics_Helper
{
    class Program
    {
        public static Spell Q;
        public static Spell R;
        private static Obj_AI_Hero Player;
        private static Menu _cfg;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            Console.WriteLine(Player.ChampionName);
        }

        private static void Game_OnGameLoad(EventArgs args)
        {

            Player = ObjectManager.Player;
            if (Player.ChampionName != "LeeSin") return;

            Q = new Spell(SpellSlot.Q, 1100);
            R = new Spell(SpellSlot.R, 375);

           R.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth,Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);

           _cfg = new Menu("Leesin Mechanics helper","Lee Mechanics Helper", true);

           var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
           TargetSelector.AddToMenu(targetSelectorMenu);
           _cfg.AddSubMenu(targetSelectorMenu);

           _cfg.AddSubMenu(new Menu("RQQ Settings", "RQQ Settings"));
           _cfg.SubMenu("RQQ Settings").AddItem(new MenuItem("ActiveRQQ", "RQQ").SetValue(new KeyBind(32, KeyBindType.Press)));
           _cfg.SubMenu("RQQ Settings").AddItem(new MenuItem("move", "Move to mouse").SetValue(false));
           //_cfg.SubMenu("RQQ Settings").AddItem(new MenuItem("UseQ1", "Use Q1").SetValue(true));
           _//cfg.SubMenu("RQQ Settings").AddItem(new MenuItem("UseQ2", "Use Q2").SetValue(true));

           _cfg.AddSubMenu(new Menu("Flash R Settings", "Flash R Settings"));
           _cfg.SubMenu("Flash R Settings").AddItem(new MenuItem("ActiveFR", "Flash R").SetValue(new KeyBind(40, KeyBindType.Press)));

            _cfg.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;

           
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Combo == false && _cfg.Item("move").IsActive() && _cfg.Item("ActiveRQQ").GetValue<KeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
            if (_cfg.Item("ActiveRQQ").GetValue<KeyBind>().Active)
            {
                Rqq();
            }

            if (_cfg.Item("ActiveFR").GetValue<KeyBind>().Active)
            {
                FlashR();
            }
        }

        private static void FlashR()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);

            if (target.Distance(Player) <= R.Range && R.IsReady() && ObjectManager.Player.GetSpellSlot("SummonerFlash").IsReady())
            {
                R.CastOnUnit(target);
                ObjectManager.Player.Spellbook.CastSpell(ObjectManager.Player.GetSpellSlot("SummonerFlash"), Game.CursorPos);
            }

        }
        public static bool Combo = false;
        private static void Rqq()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);

            if (target.Distance(Player) <= R.Range && R.IsReady() && Q.IsReady() && target.IsValidTarget())
            {
                Combo = true;
                R.CastOnUnit(target);

                Q.Cast(target.ServerPosition);

                Q.Cast();

                Combo = false;
            }

        }
    }
}
