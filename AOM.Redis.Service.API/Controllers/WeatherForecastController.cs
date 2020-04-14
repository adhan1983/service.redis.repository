using AOM.Redis.Service.API.Models;
using AOM.Redis.Service.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOM.Redis.Service.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly RedisConfiguration _redis;
        private readonly IDistributedCache _cache;
        private readonly IRedisConnectionFactory _fact;
        public WeatherForecastController(IOptions<RedisConfiguration> redis, IDistributedCache cache, IRedisConnectionFactory factory) 
        {            
            _redis = redis.Value;
            _cache = cache;
            _fact = factory;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            _cache.SetString("CacheTest", "Redis is awesome");

            var _value = _cache.GetString("CacheTest");

            var db = _fact.Connection().GetDatabase();
            
            db.StringSet("StackExchange.Redis.Key", "Stack Exchange Redis is Awesome");

            _value = db.StringGet("StackExchange.Redis.Key");            

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public void Post([FromBody]Person person) 
        {
            var redis = new RedisVoteService<Person>(this._fact);

            redis.Save(person.Name, person);

            var model = redis.Get(person.Name);

            if (model != null) 
            {
                
            }
        }
    }
}
