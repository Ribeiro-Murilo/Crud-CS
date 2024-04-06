namespace DevEvents.API.Entities
{
    public class DevEventsSpeaker
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string TalkTitle { get; set; }
        public string TalkDescription { get; set; }
        public string LinkedInProfile { get; set;}
        public Guid DevEventID { get; set; }
    }
}