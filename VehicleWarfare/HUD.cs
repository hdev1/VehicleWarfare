using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA.Math;
using GTA.Native;

namespace VehicleWarfare
{
    class HUD
    {
        public static void Init() {

        }

        public static void Update() {
            VehicleTracker.GetNearbyVehiclesByFilter(Game.Player.Character.Position, Filters.IsPoliceVehicle).ForEach(
                veh => {
                    if (veh.Exists() &&
                        !UIManager.UIElements.ContainsKey(veh.GetHashCode().ToString() + "_bg") && 
                        veh.IsOnScreen
                    ) {
                        var background = new UIElementManager(
                            new NativeUI.Elements.NativeSprite("vehicle_warfare", "vehicleHealthBarBackground", new Point(0,0), new Size(96, 18)),
                            (that) => {
                                var parent = (UIElementManager)that;
                                parent.IsDrawable = true;
                                float verticalOffset = 1.2f;

                                if (veh.Exists() && veh.IsDriveable) {
                                    var distance = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                    if (distance > 100.0f) {
                                        parent.IsDrawable = false;
                                        return;
                                    }
                                    
                                    if (distance > 20.0f) {
                                        parent.UIElement = new NativeUI.Elements.NativeSprite("vehicle_warfare", "vehicleHealthBarBackground", new Point(0,0), new Size(48, 9));
                                        verticalOffset = 2.4f;
                                        GTA.Native.Function.Call(Hash.SET_DRAW_ORIGIN, veh.Position.X, veh.Position.Y, veh.Position.Z + verticalOffset, 0);
                                        parent.UIElement.Position = new Point(-24,0);
                                    } else if (distance < 20.0f) {
                                        parent.UIElement = new NativeUI.Elements.NativeSprite("vehicle_warfare", "vehicleHealthBarBackground", new Point(0,0), new Size(96, 18));    
                                        verticalOffset = 1.2f;                                        
                                        GTA.Native.Function.Call(Hash.SET_DRAW_ORIGIN, veh.Position.X, veh.Position.Y, veh.Position.Z + verticalOffset, 0);
                                        parent.UIElement.Position = new Point(-48,0);
                                    }
                                    
                                    } else {
                                        parent.IsDrawable = false;
                                    }
                            }
                        );
                        var bar = new UIElementManager(
                            new NativeUI.Elements.NativeRectangle(new Point(0, 0), new Size(88, 4), Color.White),
                            (that) => {
                                var parent = (UIElementManager)that;
                                parent.IsDrawable = true;
                                float verticalOffset = 1.2f;

                                if (veh.Exists() && veh.IsDriveable) {
                                    var distance = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                    if (distance > 100.0f) {
                                        parent.IsDrawable = false;
                                        return;
                                    }
                                    
                                    var width = (int)Math.Round(
                                        (((float)veh.Health + 100.0f) / ((float)veh.MaxHealth + 100.0f))*44.0f,
                                        0
                                    );
                                    if (width < 0) width = 0;

                                    if (distance > 20.0f) {
                                        parent.UIElement = new NativeUI.Elements.NativeRectangle(new Point(0, 0), new Size(width,2), Color.White);
                                        verticalOffset = 2.4f;
                                        GTA.Native.Function.Call(Hash.SET_DRAW_ORIGIN, veh.Position.X, veh.Position.Y, veh.Position.Z + verticalOffset, 0);
                                        parent.UIElement.Position = new Point(-22,3);
                                    } else if (distance < 20.0f) {
                                        parent.UIElement = new NativeUI.Elements.NativeRectangle(new Point(0, 0), new Size(width,4), Color.White);                                
                                        verticalOffset = 1.2f;
                                        GTA.Native.Function.Call(Hash.SET_DRAW_ORIGIN, veh.Position.X, veh.Position.Y, veh.Position.Z + verticalOffset, 0);
                                        parent.UIElement.Position = new Point(-44,6);
                                    }

                                } else {
                                    parent.IsDrawable = false;
                                }
                                
                            }
                        );

                        UIManager.UIElements.Add(veh.GetHashCode().ToString() + "_bg", background);  
                        UIManager.UIElements.Add(veh.GetHashCode().ToString() + "_bar", bar);
                                
                    }

                }
            );      
        }
            
    }
}

