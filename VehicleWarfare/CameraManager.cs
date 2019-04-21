using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;
using GTA.Native;

namespace VehicleWarfare
{
        public static class CameraManager {
            public static Camera CurrentCamera;

            public enum CameraLocation {
                DesertGarage
            }

            private static void MoveCamera(Vector3 newPosition, out Vector3 position, Vector3 newRotation, out Vector3 rotation) {
                position = newPosition;
                rotation = newRotation;
                Game.Player.Character.Position = new Vector3(newPosition.X, newPosition.Y, newPosition.Z - 10.0f);
                Game.Player.Character.FreezePosition = true;
            }

            public static void RemoveCamera() {
                World.RenderingCamera = null;
                Game.Player.Character.Position = new Vector3(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z + 10.0f);
                Game.Player.Character.FreezePosition = false;
            }
            public static void SetCamera(CameraLocation camera) {
                Vector3 position = new Vector3();
                Vector3 rotation = new Vector3();
                if (camera == CameraLocation.DesertGarage) {
                    //position = ;
                    MoveCamera(
                        new Vector3(1923.77f, 3747.29f, 32.94f), out position,
                        new Vector3(-5.0f, 0.0f, 157.0f), out rotation
                    );
                }
                // Teleport player to load world
                
                CurrentCamera = World.CreateCamera(position, rotation, 90.0f);
                World.RenderingCamera = CurrentCamera;
            }
    }
}
