using DevEvents.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevEvents.API.Persistence
{
    public class DevEventsDbContext :DbContext
    {
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext>options):base(options)
        {
            
        }
        public DbSet<DevEvent> DevEvents {  get; set; }
        public DbSet<DevEventsSpeaker> DevEventsSpeaker {  get; set; }


        protected override void OnModelCreating(ModelBuilder Builder)
        {
            Builder.Entity<DevEvent>(e =>
            {
                e.HasKey(de => de.ID);

                //IsRequired(false) PERMITE VALORES NULL
                e.Property(de => de.Title).IsRequired(false);

                e.Property(de=>de.Description)
                            .HasMaxLength(200)
                            .HasColumnName("varchar(200)");
                //HasMaxLength vem como um bloqueio logico
                //HasColumnName vem como um bloqueio diretamennto na DB

                e.Property(de => de.StartDate)
                    .HasColumnName("Start_Date");

                e.Property(de => de.EndDate)
                    .HasColumnName("End_Date");


                e.HasMany(de => de.Speakers)
                    .WithOne()
                    .HasForeignKey(s => s.DevEventID);



            });
            Builder.Entity<DevEventsSpeaker>(e =>
            {
                e.HasKey(de => de.ID);
            });
        }


            ///base.OnModelCreating(Builder);
    }

}
