using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PitangVac.Entity.Entities;

namespace PitangVac.Repository.Map
{
    public class PatientMap : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("tb_paciente");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("id_paciente")
                   .IsRequired();

            builder.Property(x => x.Name)
                   .HasColumnName("dsc_nome")
                   .IsRequired();

            builder.Property(x => x.Login)
                   .HasColumnName("lgn_paciente")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .HasColumnName("dsc_email")
                   .IsRequired();

            builder.Property(x => x.PasswordHash)
                   .HasColumnName("psw_hash")
                   .IsRequired();

            builder.Property(x => x.PasswordSalt)
                   .HasColumnName("psw_salt")
                   .IsRequired();

            builder.Property(x => x.BirthDate)
                   .HasColumnName("dat_nascimento")
                   .IsRequired();

            builder.Property(x => x.CreateAt)
                   .HasColumnName("dat_criacao")
                   .IsRequired();

        }
    }
}
