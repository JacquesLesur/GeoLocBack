using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BackGeoLock.Model;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackGeoLock.Controllers
{
    [Produces("application/json")]
    [Route("api/Localisation")]
    public class LocalisationController : Controller
    {
        // GET: api/Localisation
        [HttpGet("{longitude}/{latitude}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<Localisation> Get(string longitude, string latitude)
        {
            
            longitude = longitude.Replace(".", ",");
            latitude = latitude.Replace(".", ",");
            
            List<Localisation> listLoc = Redis.GetLocalisations();
            
            List<Localisation> listLocProx = new List<Localisation>();
            double longitudeUser = Convert.ToDouble(longitude);
            double latitudeUser = Convert.ToDouble(latitude);
            foreach (Localisation loca in listLoc)
            {
                bool longOk = loca.position.lng > longitudeUser - 0.01  && loca.position.lng  < longitudeUser + 0.01;
                bool latOk = loca.position.lat > latitudeUser - 0.01 && loca.position.lat < latitudeUser + 0.01;
                if (longOk && latOk)
                {
                    listLocProx.Add(loca);
                }
            }
            return listLocProx;
        }

       
        // GET: api/Localisation
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<Localisation> Get()
        {
            return Redis.GetLocalisations();
        }
        
        // POST: api/Localisation
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void Post([FromBody]Localisation localisation)
        {

            try
            {
                if (localisation.adrDevice == null || localisation.pseudo == null || localisation.position.lat == 0 || localisation.position.lng == 0)
                {
                    throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);
                }
                else
                {
                    Redis.addLocalisation(localisation);

                }
            }
            catch (ArgumentException e)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotAcceptable);
            }
            
        }

    }
}
