using BackGeoLock.Model;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackGeoLock.Controllers
{
    public static class Redis
    {
        
        public static void addLocalisation(Localisation localisation)
        {
            string host = "163.172.144.237";
            string elementKey = "master123";
            using (RedisClient redisClient = new RedisClient(host, 6379,elementKey))
            {
                
                IRedisTypedClient<Localisation> localisationRedis = redisClient.As<Localisation>();
                IRedisList<Localisation> listUser = localisationRedis.Lists[$"Localisation:{localisation.pseudo}"];
                listUser.Add(localisation);
                //localisationRedis.Store(localisation);

                //localisationRedis.de();

                //localisationRedis.Store(localisation);
                //localisationRedis.SetValue(localisation.pseudo,localisation);

                //var testGet = localisationRedis.GetById(localisation.Id);

            }
        }
        public static List<Localisation> GetLocalisations()
        {
            string host = "163.172.144.237";
            string elementKey = "master123";
            using (RedisClient redisClient = new RedisClient(host, 6379, elementKey, 0))
            {

                IRedisTypedClient<Localisation> localisationRedis = redisClient.As<Localisation>();
                List<string> keys = redisClient.GetAllKeys();
                List<Localisation> listLastLoc = new List<Localisation>();
                foreach (string key in keys)
                {
                    Localisation lastLoc = localisationRedis.Lists[key].ToList().Last();
                    listLastLoc.Add(lastLoc);
                    
                }
                
                return listLastLoc;
            }
        }

    }
}
