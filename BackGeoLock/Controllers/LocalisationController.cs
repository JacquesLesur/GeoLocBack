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
            //getPlacesAround(longitude, latitude);

            longitude = longitude.Replace(".", ",");
            latitude = latitude.Replace(".", ",");
            //a remplacer post fini
            //Localisation loc = new Localisation("Surface-Jacques", "Ranger0310", -1.5616382,47.2168111, DateTime.Now );
            //Localisation loc2 = new Localisation("TitanXp", "Netoun", -1.564179, 47.233920, DateTime.Now);
            //Redis.addLocalisation(loc);
            //////////
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

        private void getPlacesAround(string longitude, string latitude)
        {
            String username = "test";
            String password = "test";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.openstreetmap.org/api/0.6/map?bbox=47.242145,-1.556662,47.372145,-1.426662");
            request.Method = "GET";
            request.ContentType = "application/json";
            String encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + encoded);
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();
                            Console.Out.WriteLine(response);
                        }
                    }
                }
            }
            catch (Exception e)
            {
               
            }
        }

        // GET: api/Localisation
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<Localisation> Get()
        {
            //a supprimer post fini
            //List<Localisation> listLoc = new List<Localisation>();
            //Localisation loc = new Localisation("Surface-Jacques", "Ranger0310", 47.2168111, -1.5616382, DateTime.Now);
            //Localisation loc2 = new Localisation("TitanXp", "Netoun", 47.233920, -1.564179, DateTime.Now);
            //Redis.addLocalisation(loc);
            //List<Localisation> localisations = Redis.GetLocalisations();
            //listLoc.Add(loc);
            //listLoc.Add(loc2);
            ///////
            return Redis.GetLocalisations();
        }

        // GET: api/Localisation/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
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
            catch(ArgumentException e)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotAcceptable);
            }
           

            
        }
        
        // PUT: api/Localisation/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
