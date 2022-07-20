using BiancasBikeShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiancasBikeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private IBikeRepository _bikeRepo;

        public BikeController(IBikeRepository bikeRepo)
        {
            _bikeRepo = bikeRepo;
        }

        // Uncomment this and finish these controller methods...
        // 
        [HttpGet]
        public IActionResult Get()
        {
            // add implementation here... 
            return Ok(_bikeRepo.GetAllBikes());
        }

        // 
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            //add implementation here... 
            var bike = _bikeRepo.GetBikeById(id);
            if (bike == null)
            {
                return NotFound();
            }
            return Ok(bike);
        }

        // 
        [HttpGet("count")]
        public IActionResult GetBikesInShopCount()
        {
            //add implementation here...
            return Ok(_bikeRepo.GetBikesInShopCount());
        }
    }
}
