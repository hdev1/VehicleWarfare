using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;
using VehicleWarfare;
using VehicleWarfare.Menus;

public class Main : Script
{

    ScriptSettings config;
    Keys OpenMenu;

    public Main()
        {
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            Tick += OnTick;

            UIManager.Init();
            BlipManager.Init();
            DebugInfo.Init();
            
            config = ScriptSettings.Load("scripts\\settings.ini");
            OpenMenu = config.GetValue<Keys>("Options", "OpenMenu", Keys.F7);

            World.DestroyAllCameras();

            KeyDown += (o, e) =>
            {
                if (e.KeyCode == OpenMenu && !UIManager.MenuPool.IsAnyMenuOpen())
                    UIManager.MainMenu.Visible = !UIManager.MainMenu.Visible;
            };
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Z)
        {
            //Game.Player.Character.CurrentVehicle.IsPersistent = true;
            Game.Player.Character.CurrentVehicle.ApplyForceRelative(new Vector3(0, 10, 10));
        }

        /* foreach (var vehicle in VehicleTracker.SavedVehicles)
        {
            if (vehicle.GameVehicle.GetHashCode() == Game.Player.Character.CurrentVehicle.GetHashCode())
            {
                vehicle.NitrousActivated = false;
            }
        }*/
        if (e.KeyCode == Keys.X)
        {
            /*
            foreach (var vehicle in VehicleTracker.SavedVehicles)
            {
                if (vehicle.GameVehicle.GetHashCode() == Game.Player.Character.CurrentVehicle.GetHashCode())
                {
                    vehicle.NitrousActivated = true;
                }
            }
            */
            DebugInfo.Bar1.Text = GTA.UI.WorldToScreen(Game.Player.LastVehicle.Position).ToString();   
        }

        if (e.KeyCode == Keys.N)
        {
            World.ShootBullet(Game.Player.Character.Position,
                World.GetClosestVehicle(Game.Player.Character.Position, 500.0f).Position,
                Game.Player.Character, new Model("w_lr_homing_rocket"), 20);
        }

        // Testh
        if (e.KeyCode == Keys.H) {
            /* var camera = World.CreateCamera(new Vector3(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z), new Vector3(180, 180, 180), 180.0f);
            World.RenderingCamera = camera;
            //Game.DisableControl(0, Game.Player);
            Game.Player.CanControlCharacter = false;
            Game.FadeScreenOut(100);
            Game.FadeScreenIn(100);*/
            Garage.Enter();
        } else if (e.KeyCode == Keys.T) {
            Garage.Leave();
        }

        //if (e.KeyCode == Keys.W) CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X - 0.1f, CameraManager.CurrentCamera.Position.Y, CameraManager.CurrentCamera.Position.Z);
        //if (e.KeyCode == Keys.S) CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X + 0.1f, CameraManager.CurrentCamera.Position.Y, CameraManager.CurrentCamera.Position.Z);
        //if (e.KeyCode == Keys.A) CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X, CameraManager.CurrentCamera.Position.Y - 0.1f, CameraManager.CurrentCamera.Position.Z);
        //if (e.KeyCode == Keys.D) CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X, CameraManager.CurrentCamera.Position.Y + 0.1f, CameraManager.CurrentCamera.Position.Z);
        
        /* if (e.KeyCode == Keys.J) {
            CameraManager.CurrentCamera.Rotation = new Vector3(CameraManager.CurrentCamera.Rotation.X + 1.0f, CameraManager.CurrentCamera.Rotation.Y, CameraManager.CurrentCamera.Rotation.Z);
        } else if (e.KeyCode == Keys.K) {
            CameraManager.CurrentCamera.Rotation = new Vector3(CameraManager.CurrentCamera.Rotation.X, CameraManager.CurrentCamera.Rotation.Y + 1.0f, CameraManager.CurrentCamera.Rotation.Z);
        } else 
        */if (e.KeyCode == Keys.L) {
            //CameraManager.CurrentCamera.Rotation = new Vector3(CameraManager.CurrentCamera.Rotation.X, CameraManager.CurrentCamera.Rotation.Y, CameraManager.CurrentCamera.Rotation.Z + 1.0f);
            //new NativeUI.Elements.NativeRectangle(new Point(300, 300), new Size(100, 20), Color.FromArgb(150, 0, 0, 0)).Draw();
            var v = Game.Player.Character.CurrentVehicle;
            uint hash = 3204302209;
            //World.ShootBullet(v.Position, World.GetNearbyVehicles(Game.Player.Character, 100.0f)[0].GetBoneCoord(0), Game.Player.Character, WeaponHash.AssaultRifle, 100);
            
        }


        if (e.KeyCode == Keys.I) {
            //CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X, CameraManager.CurrentCamera.Position.Y, CameraManager.CurrentCamera.Position.Z + 0.1f);
                
            // Create a campfire model
            var model = new Model("prop_beach_fire");
            model.Request(250);

            // Check the model is valid
            if (model.IsInCdImage && model.IsValid)
            {
                // Ensure the model is loaded before we try to create it in the world
                while (!model.IsLoaded) Script.Wait(50);

                // Create the prop in the world

                World.CreateProp(model, (new Vector3(Game.Player.Character.CurrentVehicle.Position.X + (Game.Player.Character.CurrentVehicle.Model.GetDimensions().X / 2),
                                                    Game.Player.Character.CurrentVehicle.Position.Y + (Game.Player.Character.CurrentVehicle.Model.GetDimensions().Y / 2),
                                                    Game.Player.Character.CurrentVehicle.Position.Z + (Game.Player.Character.CurrentVehicle.Model.GetDimensions().Z / 2))), true, true);
            }

            // Mark the model as no longer needed to remove it from memory.
            model.MarkAsNoLongerNeeded();
        }
        if (e.KeyCode == Keys.O) {
            CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X, CameraManager.CurrentCamera.Position.Y, CameraManager.CurrentCamera.Position.Z - 0.1f);

        }
    }

    private void OnKeyUp(object sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.X)
        {
            foreach (var vehicle in VehicleTracker.SavedVehicles)
            {
                if (vehicle.GameVehicle.GetHashCode() == Game.Player.Character.CurrentVehicle.GetHashCode())
                {
                    vehicle.NitrousActivated = false;
                }
            }
        }
    }
    private void OnTick(object sender, EventArgs e)
    {
        // Process menus
        UIManager.Update();
        KillTracker.Update();
        VehicleTracker.Update();
        BlipManager.Update();
        UIManager.TimerBarPool.Draw();
        HUD.Update();
        if (CameraManager.CurrentCamera != null) {
            //DebugInfo.Bar1.Text = Math.Round(CameraManager.CurrentCamera.Rotation.X, 2).ToString() + ", " + Math.Round(CameraManager.CurrentCamera.Rotation.Y, 2).ToString() + ", " + Math.Round(CameraManager.CurrentCamera.Rotation.Z, 2).ToString();
            //DebugInfo.Bar2.Text = Math.Round(CameraManager.CurrentCamera.Position.X, 2).ToString() + ", " + Math.Round(CameraManager.CurrentCamera.Position.Y, 2).ToString() + ", " + Math.Round(CameraManager.CurrentCamera.Position.Z, 2).ToString();
        }

    }
}

