using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Caching.Controllers
{
    [Route("api/[controller]")]
    public class CachingController : Controller
    {
        private readonly IMemoryCache _memoryCach;

        public CachingController(IMemoryCache memoryCache)
        {
            _memoryCach = memoryCache;
        }

        [HttpGet]
        public IActionResult Get()
        {
            bool exist = _memoryCach.TryGetValue("CahchedItem", out string userName);
            if (!exist)
            {
                userName = "TestCaching";
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(1));
                _memoryCach.Set("CahchedItem", userName, cacheEntryOptions);
            }
            return Ok(userName);
        }


        [HttpPost]
        public IActionResult Post(string value)
        {
            _memoryCach.GetOrCreate("CahchedItem", data =>
            {
                data.SlidingExpiration = TimeSpan.FromDays(1);
                return value;
            });
            return Created("", "");
        }


        [HttpDelete]
        public void Delete(string key)
        {
            _memoryCach.Remove(key);
        }
    }
}
