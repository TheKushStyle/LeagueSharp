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
            Notifications.AddNotification("Feel free to donate to TheKushStyle@gmail.com <3 ");
            Notifications.AddNotification("Leesin Mechanics Helper Loaded");
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
           _cfg.SubMenu("RQQ Settings").AddItem(new MenuItem("moveRQQ", "Move to mouse").SetValue(false));
           _cfg.SubMenu("RQQ Settings").AddItem(new MenuItem("UseQ1RQQ", "Use Q1").SetValue(true));
           _cfg.SubMenu("RQQ Settings").AddItem(new MenuItem("UseQ2RQQ", "Use Q2").SetValue(true));

           _cfg.AddSubMenu(new Menu("Flash R Settings", "Flash R Settings"));
           _cfg.SubMenu("Flash R Settings").AddItem(new MenuItem("ActiveFR", "Flash R").SetValue(new KeyBind(40, KeyBindType.Press)));
           _cfg.SubMenu("Flash R Settings").AddItem(new MenuItem("UseQ1RFQQ", "Use Q1").SetValue(true));
           _cfg.SubMenu("Flash R Settings").AddItem(new MenuItem("UseQ2RFQQ", "Use Q2").SetValue(true));
           _cfg.SubMenu("Flash R Settings").AddItem(new MenuItem("moveRFQQ", "Move to mouse").SetValue(false));
            _cfg.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;

           
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (ComboRqq == false && _cfg.Item("moveRQQ").IsActive() && _cfg.Item("ActiveRQQ").GetValue<KeyBind>().Active)
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
        public static bool ComboRfqq = false;
        private static void FlashR()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);

            if (ComboRfqq == false && _cfg.Item("moveRFQQ").IsActive() && _cfg.Item("ActiveFR").GetValue<KeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }

            if (target.Distance(Player) <= R.Range && R.IsReady() && ObjectManager.Player.GetSpellSlot("SummonerFlash").IsReady())
            {
                ComboRfqq = true;

                R.CastOnUnit(target);
                ObjectManager.Player.Spellbook.CastSpell(ObjectManager.Player.GetSpellSlot("SummonerFlash"), Game.CursorPos);
                if (_cfg.Item("UseQ1RFQQ").IsActive())
                {
                    Q.Cast(target.ServerPosition);
                }

                if (_cfg.Item("UseQ2RFQQ").IsActive())
                {
                    Q.Cast();
                }
                ComboRfqq = false;
            }

        }
        public static bool ComboRqq = false;
        private static void Rqq()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);

            if (target.Distance(Player) <= R.Range && R.IsReady() && Q.IsReady() && target.IsValidTarget())
            {
                ComboRqq = true;
                R.CastOnUnit(target);
                if (_cfg.Item("UseQ1RQQ").IsActive())
                {
                    Q.Cast(target.ServerPosition);
                }


                if (_cfg.Item("UseQ2RQQ").IsActive())
                {
                    Q.Cast();
                }

                ComboRqq = false;
            }

        }
    }
}
