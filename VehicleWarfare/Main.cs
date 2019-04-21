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

            MenuManager.Init();
            BlipManager.Init();
            DebugInfo.Init();
            
            config = ScriptSettings.Load("scripts\\settings.ini");
            OpenMenu = config.GetValue<Keys>("Options", "OpenMenu", Keys.F7);

            World.DestroyAllCameras();

            KeyDown += (o, e) =>
            {
                if (e.KeyCode == OpenMenu && !MenuManager.MenuPool.IsAnyMenuOpen())
                    MenuManager.MainMenu.Visible = !MenuManager.MainMenu.Visible;
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
            foreach (var vehicle in VehicleTracker.SavedVehicles)
            {
                if (vehicle.GameVehicle.GetHashCode() == Game.Player.Character.CurrentVehicle.GetHashCode())
                {
                    vehicle.NitrousActivated = true;
                }
            }
        }

        if (e.KeyCode == Keys.N)
        {
            World.ShootBullet(Game.Player.Character.Position,
                World.GetClosestVehicle(Game.Player.Character.Position, 500.0f).Position,
                Game.Player.Character, new Model("w_lr_homing_rocket"), 20);
        }

        // Test
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

        if (e.KeyCode == Keys.W) CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X - 0.1f, CameraManager.CurrentCamera.Position.Y, CameraManager.CurrentCamera.Position.Z);
        if (e.KeyCode == Keys.S) CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X + 0.1f, CameraManager.CurrentCamera.Position.Y, CameraManager.CurrentCamera.Position.Z);
        if (e.KeyCode == Keys.A) CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X, CameraManager.CurrentCamera.Position.Y - 0.1f, CameraManager.CurrentCamera.Position.Z);
        if (e.KeyCode == Keys.D) CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X, CameraManager.CurrentCamera.Position.Y + 0.1f, CameraManager.CurrentCamera.Position.Z);
        
        if (e.KeyCode == Keys.J) {
            CameraManager.CurrentCamera.Rotation = new Vector3(CameraManager.CurrentCamera.Rotation.X + 1.0f, CameraManager.CurrentCamera.Rotation.Y, CameraManager.CurrentCamera.Rotation.Z);
        } else if (e.KeyCode == Keys.K) {
            CameraManager.CurrentCamera.Rotation = new Vector3(CameraManager.CurrentCamera.Rotation.X, CameraManager.CurrentCamera.Rotation.Y + 1.0f, CameraManager.CurrentCamera.Rotation.Z);
        } else if (e.KeyCode == Keys.L) {
            CameraManager.CurrentCamera.Rotation = new Vector3(CameraManager.CurrentCamera.Rotation.X, CameraManager.CurrentCamera.Rotation.Y, CameraManager.CurrentCamera.Rotation.Z + 1.0f);
        }

        if (e.KeyCode == Keys.I) {
            CameraManager.CurrentCamera.Position = new Vector3(CameraManager.CurrentCamera.Position.X, CameraManager.CurrentCamera.Position.Y, CameraManager.CurrentCamera.Position.Z + 0.1f);
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
        MenuManager.Update();
        KillTracker.Update();
        VehicleTracker.Update();
        BlipManager.Update();
        MenuManager.TimerBarPool.Draw();
        if (CameraManager.CurrentCamera != null) {
            DebugInfo.Bar1.Text = Math.Round(CameraManager.CurrentCamera.Rotation.X, 2).ToString() + ", " + Math.Round(CameraManager.CurrentCamera.Rotation.Y, 2).ToString() + ", " + Math.Round(CameraManager.CurrentCamera.Rotation.Z, 2).ToString();
            DebugInfo.Bar2.Text = Math.Round(CameraManager.CurrentCamera.Position.X, 2).ToString() + ", " + Math.Round(CameraManager.CurrentCamera.Position.Y, 2).ToString() + ", " + Math.Round(CameraManager.CurrentCamera.Position.Z, 2).ToString();
        }

    }
}

