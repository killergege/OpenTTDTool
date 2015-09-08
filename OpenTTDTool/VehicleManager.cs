using OpenTTDTool.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool
{
    public class VehicleManager
    {
        #region Singleton
        private static VehicleManager instance;
        public static VehicleManager Instance
        {
            get { return instance = instance ?? new VehicleManager(); } ///instance == null ? instance = new VehicleManager() : instance; }
        }
        #endregion

        public Dictionary<int, Vehicle> Vehicles { get; private set; }

        private VehicleManager()
        {
            Vehicles = new Dictionary<int, Vehicle>();
        }

        public void SetProperty(int rowNumber, int idVehicle, Features feature, int code, object value)
        {
            Vehicle vehicle;
            if (Vehicles.ContainsKey(idVehicle))
            {
                vehicle = Vehicles[idVehicle];
            }
            else
            {
                switch (feature)
                {
                    case Features.Trains:
                        vehicle = new Train();
                        Vehicles.Add(idVehicle, vehicle);
                        break;
                    default:
                        break;
                }                
            }
            Vehicles[idVehicle].SetProperty(rowNumber,code, value);

        }
    }
}
