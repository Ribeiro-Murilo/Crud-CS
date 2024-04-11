using AutoMapper;
using DevEvents.API.Model;
using DevEvents.API.Entities;

namespace DevEvents.API.Mappers
{
    public class DevEventsProfile:Profile
    {
        public DevEventsProfile()
        {
            CreateMap<DevEvent, DevEventsViewModel>();
            CreateMap<DevEventsSpeaker, DevEventsSpeakerViewModel>();
            

            CreateMap<DevEventsInputModel, DevEvent>();
            CreateMap<DevEventsSpeakerInputModel, DevEventsSpeaker>();
        }
    }
}
