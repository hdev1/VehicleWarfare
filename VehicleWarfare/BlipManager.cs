using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;

namespace VehicleWarfare
{
    class BlipManager
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
            foreach (var savedVehicle in VehicleTracker.SavedVehicles)
            {
                if (_blips.ContainsKey(savedVehicle.GameVehicle.GetHashCode().ToString()))
                {
                    _blips[savedVehicle.GameVehicle.GetHashCode().ToString()].Position = savedVehicle.GameVehicle.Position;

                }
            }
        }

    }
}
