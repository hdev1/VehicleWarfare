using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace VehicleWarfare
{
    public class SavedVehicle
    {
        public Vehicle GameVehicle;
        public float ArmorLevel = 1;
        public Vector3 LastPosition;
        public int HashCode;
        public string DisplayName;
        public Model Model;
        public bool IsSpawned;
    }
}
