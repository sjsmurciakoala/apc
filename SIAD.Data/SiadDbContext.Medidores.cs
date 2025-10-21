using Microsoft.EntityFrameworkCore;
using SIAD.Core.Entities;

namespace SIAD.Data;

public partial class SiadDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<configuracion_app_lectura_medidore>(entity =>
        {
            entity.HasKey(e => e.ide).HasName("configuracion_app_lectura_medidores_pkey");

            entity.ToTable("configuracion_app_lectura_medidores");

            entity.Property(e => e.ide).UseIdentityAlwaysColumn();
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.valor_letras).HasMaxLength(100);
            entity.Property(e => e.valor_numeros).HasPrecision(10, 2);
        });
    }
}
