using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;

namespace VehicleWarfare
{
    public static class Garage
    {
        public static Vehicle RenderedVehicle;
        
        private static void ShowVehicle(VehicleHash vehicleHash) {
            if (RenderedVehicle != null) RenderedVehicle.Delete();
            
            RenderedVehicle = World.CreateVehicle(vehicleHash, new Vector3(1922.47f, 3745.19f, 32.94f));
            RenderedVehicle.PlaceOnGround();
        }

        private static void RemoveVehicle() {
            if (RenderedVehicle != null) RenderedVehicle.Delete();
        }
        
        public static void Enter() {
            // set up player
            Game.Player.CanControlCharacter = false;
            Game.Player.IgnoredByEveryone = true;
            Game.Player.IsInvincible = true;

            CameraManager.SetCamera(CameraManager.CameraLocation.DesertGarage);
            ShowVehicle(VehicleHash.Adder);
        }

        public static void Leave() {
            Game.Player.CanControlCharacter = true;
            Game.Player.IgnoredByEveryone = false;
            Game.Player.IsInvincible = false;
            CameraManager.RemoveCamera();
            RemoveVehicle();
        }
    }
}
