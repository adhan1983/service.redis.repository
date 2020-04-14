using AOM.Redis.Service.API.Models;
using AOM.Redis.Service.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;

namespace AOM.Redis.Service.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly RedisConfiguration _redis;
        private readonly IDistributedCache _cache;
        private readonly IRedisConnectionFactory _fact;
        private RedisVoteService<Person> _redisVoteService;
        public WeatherForecastController(IOptions<RedisConfiguration> redis, IDistributedCache cache, IRedisConnectionFactory factory) 
        {            
            _redis = redis.Value;
            _cache = cache;
            _fact = factory;
            _redisVoteService = new RedisVoteService<Person>(_fact);
        }

        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();

        //    _cache.SetString("CacheTest", "Redis is awesome");

        //    var _value = _cache.GetString("CacheTest");

        //    var db = _fact.Connection().GetDatabase();

        //    db.StringSet("StackExchange.Redis.Key", "Stack Exchange Redis is Awesome");

        //    _value = db.StringGet("StackExchange.Redis.Key");            



        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpPost]
        [Route("create")]
        public string Post([FromBody]Person person)
        {
            var rng = new Random();

            Person model = new Person();
            model.Name = person.Name;
            model.Id = rng.Next(1, 55).ToString();

            _redisVoteService.Save(model.Id.ToString(), model);

            return model.Id;
        }

        [HttpGet]
        [Route("id")]
        public Person Get([FromQuery]string id)
        {
            _redisVoteService = new RedisVoteService<Person>(this._fact);
            
            var model = _redisVoteService.Get(id);

            return model;
        }

        [HttpDelete]
        [Route("/{id}")]
        public bool Delete([FromRoute]string id)         
        {
            _redisVoteService.Delete(id);
            
            return true;            
        }
    }
}
