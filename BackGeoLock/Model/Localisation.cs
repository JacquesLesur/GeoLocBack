using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackGeoLock.Model
{
    public class Localisation
    {
        public string adrDevice { get; set; }
        public string pseudo { get; set; }
        public Position position { get; set; }
        public DateTime date { get; set; }


        public Localisation()
        {
        }

        public Localisation(string adrDevice, string pseudo, double longitude, double latitude, DateTime now)
        {
            this.adrDevice = adrDevice;
            this.pseudo = pseudo;
            position = new Position(latitude, longitude);
            date = now;
           
        }
    }
}
