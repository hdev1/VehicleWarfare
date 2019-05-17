using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;

namespace VehicleWarfare
{
    class BlipController
    {
        private static Dictionary<string, Blip> _blips;

        public static void Init()
        {
            _blips = new Dictionary<string, Blip>();
        }

        public static void Add(string name, Vector3 position, BlipSprite blipSprite)
        {
            var blip = World.CreateBlip(position);
            blip.Sprite = blipSprite;

            _blips.Add(name, blip);
        }

        public static bool ContainsKey(string name)
        {
            return _blips.ContainsKey(name);
        }

        public static void Remove(string name)
        {
            _blips[name].Remove();
            _blips.Remove(name);
        }

        public static void Update()
        {
            // Update vehicles
            foreach (var SaveableVehicle in VehicleController.SaveableVehicles)
            {
                if (_blips.ContainsKey(SaveableVehicle.GameVehicle.GetHashCode().ToString()))
                {
                    _blips[SaveableVehicle.GameVehicle.GetHashCode().ToString()].Position = SaveableVehicle.GameVehicle.Position;

                }
            }
        }

    }
}
