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
    public class UIElementController {
            public Action<object> CustomUpdate;
            public UIElement UIElement;
            public bool IsDrawable = true;
            public UIElementController(UIElement uiElement, Action<object> customUpdate)
            {
                CustomUpdate = customUpdate;
                UIElement = uiElement;
            }

            public UIElementController(UIElement uiElement)
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

    public static class UIController
    {
        public static TimerBarPool TimerBarPool;
        public static MenuPool MenuPool;
        public static UIMenu WeaponPurchase;
        public static UIMenu WeaponShop;
        public static UIMenu MainMenu;
        public static UIMenu VehicleMenu;
        public static Dictionary<string, UIElementController> UIElements;        
        public static void Init()
        {
            // Set up menus
            MenuPool = new MenuPool();

            MainMenu = new MainMenu().GetMenu();
            WeaponShop = new WeaponShopMenu().GetMenu();
            WeaponPurchase = new WeaponPurchaseMenu().GetMenu();
            VehicleMenu = new VehicleMenu().GetMenu();

            MenuPool.Add(MainMenu);
            MenuPool.Add(WeaponPurchase);
            MenuPool.Add(WeaponShop);
            MenuPool.Add(VehicleMenu);

            MenuPool.RefreshIndex();

            // Other UI
            TimerBarPool = new TimerBarPool();
            UIElements = new Dictionary<string, UIElementController>();
            VehicleController.Init();
        }

        private static void UpdateHUD()
        {
            VehicleController.GetNearbyVehiclesByFilter(Game.Player.Character.Position, Filters.IsPoliceVehicle).ForEach(
                veh => {
                    if (veh.Exists() &&
                        !UIElements.ContainsKey(veh.GetHashCode().ToString() + "_bg") &&
                        veh.IsOnScreen
                    )
                    {
                        var background = new UIElementController(
                            new NativeUI.Elements.NativeSprite("vehicle_warfare", "vehicleHealthBarBackground", new Point(0, 0), new Size(96, 18)),
                            (that) => {
                                var parent = (UIElementController)that;
                                parent.IsDrawable = true;
                                float verticalOffset = 1.2f;

                                if (veh.Exists() && veh.IsDriveable)
                                {
                                    var distance = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                    if (distance > 100.0f)
                                    {
                                        parent.IsDrawable = false;
                                        return;
                                    }

                                    // Calculate verticaloffset based off distance
                                    if (distance > 20.0f)
                                    {
                                        verticalOffset = distance / 25.0f;
                                    }
                                    else if (distance < 20.0f)
                                    {
                                        verticalOffset = 1.2f;
                                    }

                                    parent.UIElement = new NativeUI.Elements.NativeSprite("vehicle_warfare", "vehicleHealthBarBackground", new Point(0, 0), new Size(48, 9));
                                    Function.Call(Hash.SET_DRAW_ORIGIN, veh.Position.X, veh.Position.Y, veh.Position.Z + verticalOffset, 0);
                                    parent.UIElement.Position = new Point(-24, 0);

                                }
                                else
                                {
                                    parent.IsDrawable = false;
                                }
                            }
                        );
                        var bar = new UIElementController(
                            new NativeUI.Elements.NativeRectangle(new Point(0, 0), new Size(88, 4), Color.White),
                            (that) => {
                                var parent = (UIElementController)that;
                                parent.IsDrawable = true;
                                float verticalOffset = 1.2f;

                                if (veh.Exists() && veh.IsDriveable)
                                {
                                    var distance = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                    if (distance > 100.0f)
                                    {
                                        parent.IsDrawable = false;
                                        return;
                                    }

                                    var width = (int)Math.Round(
                                        ((veh.Health + 100.0f) / (veh.MaxHealth + 100.0f)) * 44.0f,
                                        0
                                    );
                                    if (width < 0) width = 0;


                                    // Calculate verticaloffset based off distance
                                    if (distance > 30.0f)
                                    {
                                        verticalOffset = distance / 25.0f;
                                    }
                                    else if (distance < 30.0f)
                                    {
                                        verticalOffset = 1.2f;
                                    }

                                    parent.UIElement = new NativeUI.Elements.NativeRectangle(new Point(0, 0), new Size(width, 2), Color.White);
                                    GTA.Native.Function.Call(Hash.SET_DRAW_ORIGIN, veh.Position.X, veh.Position.Y, veh.Position.Z + verticalOffset, 0);
                                    parent.UIElement.Position = new Point(-22, 3);

                                }
                                else
                                {
                                    parent.IsDrawable = false;
                                }

                            }
                        );

                        UIElements.Add(veh.GetHashCode().ToString() + "_bg", background);
                        UIElements.Add(veh.GetHashCode().ToString() + "_bar", bar);

                    }

                }
            );
        }
        public static void Update()
        {
            MenuPool.ProcessMenus();
            UpdateHUD();
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
