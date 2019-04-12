using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using NativeUI;
using VehicleWarfare.Menus;

namespace VehicleWarfare
{
    public static class MenuManager
    {
        public static TimerBarPool TimerBarPool;
        public static MenuPool MenuPool;
        public static UIMenu WeaponPurchase;
        public static UIMenu WeaponShop;
        public static UIMenu MainMenu;
        public static UIMenu VehicleMenu;

        public static void Init()
        {
            // Set up menus
            MenuPool = new MenuPool();

            MainMenu = new MainMenu().GetMenu();
            WeaponShop = new WeaponShop().GetMenu();
            WeaponPurchase = new WeaponPurchase().GetMenu();
            VehicleMenu = new VehicleMenu().GetMenu();

            MenuPool.Add(MainMenu);
            MenuPool.Add(WeaponPurchase);
            MenuPool.Add(WeaponShop);
            MenuPool.Add(VehicleMenu);

            MenuPool.RefreshIndex();

            // Other UI
            TimerBarPool = new TimerBarPool();
            VehicleTracker.Init();
        }

        public static void Update()
        {
            foreach (UIMenu menu in MenuPool.ToList().ToList())
            {
                menu.Draw();
                menu.ProcessMouse();
                menu.ProcessControl();
            }
        }
    }
}
