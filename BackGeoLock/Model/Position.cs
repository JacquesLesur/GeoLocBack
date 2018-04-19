using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackGeoLock.Model
{
    public class Position
    {
        
        public double lat { get; set; }
        public double lng { get; set; }

        public Position(double lng, double lat)
        {
            this.lat = lat;
            this.lng = lng;
        }


    }
}
