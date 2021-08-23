using LinenAndBird.DataAccess;
using LinenAndBird.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LinenAndBird.Models.Hat;

namespace LinenAndBird.Controllers
{
    [Route("api/hats")] // exposed at this endpoint
    [ApiController] // an api controller, so it returns json or xml
    public class HatsController : ControllerBase
    {
        private HatRepository _repo;

        public HatsController()
        {
            _repo = new HatRepository();
        }

        [HttpGet]
        public List<Hat> GetAllHats()
        {
            //var repo = new HatRepository(); // no longer need thanks to private HatRepository _repo
            return _repo.GetAll();
        }

        // GET /api/hats/styles/1 -> all open backed hats
        [HttpGet("styles/{style}")]
        public IEnumerable<Hat> GetHatsByStyle(HatStyle style)
        {
            return _repo.GetByStyle(style);
        }

        [HttpPost]
        public void AddAHat(Hat newHat)
        {
            //var repo = new HatRepository(); // no longer need thanks to private HatRepository _repo
            _repo.Add(newHat);
        }
    }
}
