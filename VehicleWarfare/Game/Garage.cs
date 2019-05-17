using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;
using System.Drawing;

namespace VehicleWarfare
{
    public static class Garage
    {
        // Constants
        public readonly static Vector3 DesertGarageCameraPosition = new Vector3(1923.77f, 3747.29f, 32.94f);
        public readonly static Vector3 DesertGarageCameraRotation = new Vector3(-5.0f, 0.0f, 157.0f);
        public readonly static Vector3 DesertGarageCarPosition = new Vector3(1922.47f, 3745.19f, 32.94f);
        public readonly static float HighestVehicleSpeed = 400.0f;
        public readonly static float HighestVehicleArmor = 1200.0f;
        public readonly static float HighestVehicleTraction = 800.0f;
        public readonly static float HighestVehicleBraking = 800.0f;
        public readonly static string GarageUIT;
        public static bool IsEntered;

        private static string groupName = "Garage";
        private static bool showUI;
        enum Screens
        {
            SelectCar
        }

        private static Screens CurrentScreen;
        public static Vehicle RenderedVehicle;

        public static float GetSpeedStat(Vehicle vehicle) => vehicle.MaxTraction / HighestVehicleSpeed;
        public static float GetArmorStat(Vehicle vehicle) => vehicle.MaxHealth / HighestVehicleArmor;
        public static float GetHandlingStat(Vehicle vehicle) => ((vehicle.MaxTraction / HighestVehicleTraction) * 0.5f) + ((vehicle.MaxBraking / HighestVehicleBraking) * 0.5f);
        private static void ShowUI()
        {
            showUI = true;
            switch (CurrentScreen) {
                case Screens.SelectCar:
                    UIController.UIElements.Add(
                        groupName + "Test",
                        new UIElementController(
                            new NativeUI.Elements.NativeRectangle(new Point(300, 300), new Size(100, 20), Color.FromArgb(150, 0, 0, 0)
                        )
                    ));
                    break;     
                default:
                    break;
            }
        }

        private static void HideUI() {
            UIController.RemoveGroup(groupName);
            showUI = false;
        }
        private static void ClearUI()
        {
            UIController.RemoveGroup(groupName);
        }

        public static void ShowVehicle(VehicleHash vehicleHash)
        {
            if (RenderedVehicle != null) RenderedVehicle.Delete();

            RenderedVehicle = World.CreateVehicle(vehicleHash, DesertGarageCarPosition);
            RenderedVehicle.PlaceOnGround();
        }

        public static void RemoveVehicle()
        {
            if (RenderedVehicle != null) RenderedVehicle.Delete();
        }

        public static void Enter()
        {
            CameraController.SetCamera(CameraController.CameraLocation.DesertGarage, true);
            CurrentScreen = Screens.SelectCar;
            ShowUI();
            ShowVehicle(VehicleHash.Adder);
            IsEntered = true;
        }

        public static void Leave()
        {
            Game.Player.CanControlCharacter = true;
            Game.Player.IgnoredByEveryone = false;
            Game.Player.IsInvincible = false;

            CameraController.RemoveCamera();
            RemoveVehicle();
            HideUI();
            IsEntered = false;
        }
    }
}
