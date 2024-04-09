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
        /// <summary>
        /// Obter todos os eventos
        /// </summary>
        /// <returns>Coleção de enventos</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents
                .Include(de => de.Speakers).Where(o => !o.IsDeleted).ToList();

            return Ok(devEvents);
        }
        /// <summary>
        /// Obter um evento por ID
        /// </summary>
        /// <param name="ID">Identificador do evento</param>
        /// <returns>Dados do evento</returns>
        /// <respose code="200">Sucesso</respose>
        /// <respose code="404">Não encontrado</respose>
        [HttpGet("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Cadastrar um evento 
        /// </summary>
        /// <remarks>
        /// {}obj json
        /// </remarks>
        /// <param name="devEvents">Dados do Evento</param>
        /// <returns>Obj recem criado</returns>
        /// <response code="201">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(DevEvent devEvents)
        {
            _context.DevEvents.Add(devEvents);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { ID = devEvents.ID }, devEvents);
        }
        /// <summary>
        /// Atualizar um evento
        /// </summary>
        /// <remarks>Obj json</remarks>
        /// <param name="ID">Identificador do evento</param>
        /// <param name="Input">Dados do evento</param>
        /// <returns>Nada.</returns>
        /// <resonse name="404">Não encontrado</resonse>
        /// <resonse name="204">sucesso</resonse>
        [HttpPut("{ID}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Deletar um evento
        /// </summary>
        /// <param name="ID">Identificador do evento</param>
        /// <returns>Nada.</returns>
        /// <resonse name="404">Não encontrado</resonse>
        /// <resonse name="204">sucesso</resonse>
        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Cadastrar palestrante
        /// </summary>
        /// <remarks>Ojs Json</remarks>
        /// <param name="ID">Identificador do evento</param>
        /// <param name="speaker">Dados do palestrante</param>
        /// <returns>Nada.</returns>
        /// <resonse name="204">sucesso</resonse>
        /// <resonse name="404">Não encontrado</resonse>

        [HttpPost("{ID}/speakers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
