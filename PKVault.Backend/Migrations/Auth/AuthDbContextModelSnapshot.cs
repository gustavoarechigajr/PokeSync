using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PKVault.Backend.auth;

#nullable disable

namespace PKVault.Backend.Migrations.Auth;

[DbContext(typeof(AuthDbContext))]
partial class AuthDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "10.0.2");

        modelBuilder.Entity("PKVault.Backend.auth.entity.UserEntity", b =>
        {
            b.Property<string>("Id").HasColumnType("TEXT");
            b.Property<DateTime>("CreatedAt").HasColumnType("TEXT");
            b.Property<string>("PasswordHash").IsRequired().HasColumnType("TEXT");
            b.Property<string>("Username").IsRequired().HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("Username").IsUnique();
            b.ToTable("Users");
        });

        modelBuilder.Entity("PKVault.Backend.android.entity.AndroidVaultEntity", b =>
        {
            b.Property<string>("Id").HasColumnType("TEXT");
            b.Property<string>("UserId").IsRequired().HasColumnType("TEXT");
            b.Property<int>("Box").HasColumnType("INTEGER");
            b.Property<int>("Slot").HasColumnType("INTEGER");
            b.Property<int>("SpeciesId").HasColumnType("INTEGER");
            b.Property<string>("SpeciesName").IsRequired().HasColumnType("TEXT");
            b.Property<string>("Nickname").IsRequired().HasColumnType("TEXT");
            b.Property<bool>("IsNicknamed").HasColumnType("INTEGER");
            b.Property<int>("Level").HasColumnType("INTEGER");
            b.Property<bool>("IsShiny").HasColumnType("INTEGER");
            b.Property<bool>("IsEgg").HasColumnType("INTEGER");
            b.Property<int>("Gender").HasColumnType("INTEGER");
            b.Property<string>("Nature").IsRequired().HasColumnType("TEXT");
            b.Property<string>("Ball").IsRequired().HasColumnType("TEXT");
            b.Property<int>("Generation").HasColumnType("INTEGER");
            b.Property<int>("Move1").HasColumnType("INTEGER");
            b.Property<int>("Move2").HasColumnType("INTEGER");
            b.Property<int>("Move3").HasColumnType("INTEGER");
            b.Property<int>("Move4").HasColumnType("INTEGER");
            b.Property<DateTime>("ImportedAt").HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("UserId");
            b.ToTable("AndroidVault");
        });
#pragma warning restore 612, 618
    }
}
