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
        private static readonly RedisClient redisClient = new RedisClient("163.172.144.237", 6379, "master123", 0);
        public static void addLocalisation(Localisation localisation)
        {             
                IRedisTypedClient<Localisation> localisationRedis = redisClient.As<Localisation>();
                IRedisList<Localisation> listUser = localisationRedis.Lists[$"Localisation:{localisation.pseudo}"];
                //expire list 5h
                redisClient.Custom("EXPIRE", $"Localisation:{localisation.pseudo}", "18000");
                listUser.Add(localisation);
              
        }
        public static List<Localisation> GetLocalisations()
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
