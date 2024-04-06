using DevEvents.API.Entities;
using DevEvents.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventControler : ControllerBase
    {
        private readonly DevEventsDbContext _context;
        public DevEventControler(DevEventsDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents.Where(o => !o.IsDeleted).ToList();

            return Ok(devEvents);
        }
        [HttpGet("{ID}")]
        public IActionResult GetById(Guid ID)
        {
            var devEvents = _context.DevEvents.SingleOrDefault(o => o.ID == ID);
            if (devEvents == null)
            {
                return NotFound();
            }
            return Ok(devEvents);
        }
        [HttpPost]
        public IActionResult Post(DevEvent devEvents)
        {
            _context.DevEvents.Add(devEvents);
            return CreatedAtAction(nameof(GetById), new { ID = devEvents.ID }, devEvents);
        }
        [HttpPut("{ID}")]
        public IActionResult Update(Guid ID, DevEvent Input)
        {
            var devEvents = _context.DevEvents.SingleOrDefault(o => o.ID == ID);
            if (devEvents == null)
            {
                return NotFound();
            }

            devEvents.Update(Input.Title, Input.Description, Input.StartDate, Input.EndDate);
            return NoContent();
        }

        [HttpDelete("{ID}")]
        public IActionResult Delete(Guid ID)
        {
            var devEvents = _context.DevEvents.SingleOrDefault(o => o.ID == ID);
            if (devEvents == null)
            {
                return NotFound();
            }
            devEvents.Delete();

            return NoContent();
        }

        [HttpPost("{ID}/speakers")]
        public IActionResult Postspeaker(Guid ID, DevEventsSpeaker speaker)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(o=>o.ID == ID);
            if(devEvent == null)
            {
                return NotFound();
            }
            devEvent.Speakers.Add(speaker);

            return NoContent();
        }
    }
}
