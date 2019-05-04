using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;
using NativeUI;
using VehicleWarfare.Menus;

namespace VehicleWarfare
{
    public class UIElementManager {
            public Action<object> CustomUpdate;
            public UIElement UIElement;
            public bool IsDrawable = true;
            public UIElementManager(UIElement uiElement, Action<object> customUpdate)
            {
                CustomUpdate = customUpdate;
                UIElement = uiElement;
            }

            public UIElementManager(UIElement uiElement)
            {
                UIElement = uiElement;
            }

            public void Update() {
                if (CustomUpdate != null) {
                    CustomUpdate.Invoke(this);
                }
                
                if (IsDrawable) {
                    UIElement.Draw();
                }
                
            }
        }

    public static class UIManager
    {
        public static TimerBarPool TimerBarPool;
        public static MenuPool MenuPool;
        public static UIMenu WeaponPurchase;
        public static UIMenu WeaponShop;
        public static UIMenu MainMenu;
        public static UIMenu VehicleMenu;
        public static Dictionary<string, UIElementManager> UIElements;        
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
            UIElements = new Dictionary<string, UIElementManager>();
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

            foreach (var uiElement in UIElements) {
                uiElement.Value.Update();
            }
        }

        public static void RemoveGroup(string groupName) {
            foreach (var uiElement in UIElements) {
                if (uiElement.Key.StartsWith(groupName)) {
                    UIElements.Remove(uiElement.Key);
                }
            }
        }
    }
}
