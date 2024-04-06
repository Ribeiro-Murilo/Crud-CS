using DevEvents.API.Entities;
using DevEvents.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var devEvents = _context.DevEvents
                .Include(de => de.Speakers).Where(o => !o.IsDeleted).ToList();

            return Ok(devEvents);
        }
        [HttpGet("{ID}")]
        public IActionResult GetById(Guid ID)
        {
            var devEvents = _context.DevEvents
                .Include(de=> de.Speakers)
                .SingleOrDefault(o => o.ID == ID);
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
            _context.SaveChanges();
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

            _context.DevEvents.Update(devEvents);
            _context.SaveChanges();

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

            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("{ID}/speakers")]
        public IActionResult Postspeaker(Guid ID, DevEventsSpeaker speaker)
        {
            speaker.DevEventID = ID;

            var devEvent = _context.DevEvents.Any(o=>o.ID == ID);
            if(!devEvent)
            {
                return NotFound();
            }

            _context.DevEventsSpeaker.Add(speaker);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
