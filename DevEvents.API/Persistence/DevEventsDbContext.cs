using DevEvents.API.Entities;

namespace DevEvents.API.Persistence
{
    public class DevEventsDbContext
    {
        public List<DevEvent> DevEvents {  get; set; }

        public DevEventsDbContext()
        {
            DevEvents = new List<DevEvent>();
        }
    }
}
