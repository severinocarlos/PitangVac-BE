using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PitangVac.Entity.Entities;

namespace PitangVac.Repository.Map
{
    public class SchedulingMap : IEntityTypeConfiguration<Scheduling>
    {
        public void Configure(EntityTypeBuilder<Scheduling> builder)
        {

            builder.ToTable("tb_agendamento");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                   .HasColumnName("id_agendamento")
                   .IsRequired();

            builder.Property(e => e.PatientId)
                   .HasColumnName("id_paciente")
                   .IsRequired();

            builder.Property(e => e.SchedulingDate)
                   .HasColumnName("dat_agendamento")
                   .IsRequired();

            builder.Property(e => e.SchedulingTime)
                   .HasColumnName("hor_agendamento")
                   .IsRequired();

            builder.Property(e => e.Status)
                   .HasColumnName("dsc_status")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(e => e.CreateAt)
                   .HasColumnName("dat_criacao")
                   .IsRequired();

            builder.HasOne(e => e.Patient)
                   .WithMany(p => p.Schedulings)
                   .HasForeignKey(e => e.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
