using Microsoft.EntityFrameworkCore;
using SP_SanHtar.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.ContextDB
{
    public partial class OnlineContext : DbContext
    {
        public OnlineContext()
        {

        }

        public OnlineContext(DbContextOptions<OnlineContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tb_Myanamr> Tb_Myanamrs { get; set; }
        public virtual DbSet<Tb_Myanmar_Detail> Tb_Myanmar_Details { get; set; }
        public virtual DbSet<Tb_Chemistry> Tb_Chemistrys { get; set; }
        public virtual DbSet<Tb_Chemistry_Detail> Tb_Chemistry_Details { get; set; }
        public virtual DbSet<Tb_User> Tb_Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-251TL74;Database=SPSH;Persist Security Info=False;User ID=sa;Password=P@ssw0rd;Trusted_Connection=True;Encrypt=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tb_User>(entity =>
            {
                entity.ToTable("tbl_User", "dbo");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.UserID).ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.ChemistryID).HasColumnName("ChemistryID");

                entity.Property(e => e.PartID).HasColumnName("PartID");

                entity.Property(e => e.UserName).HasColumnName("UserName");

                entity.Property(e => e.FirstName).HasColumnName("FirstName");

                entity.Property(e => e.LastName).HasColumnName("LastName");

                entity.Property(e => e.Password).HasColumnName("Password");

                entity.Property(e => e.Email).HasColumnName("Email");

                entity.Property(e => e.PersonalContactNumber).HasColumnName("PersonalContactNumber");

                entity.Property(e => e.OtherContactNumber).HasColumnName("OtherContactNumber");

                entity.Property(e => e.Sex).HasColumnName("Sex");

                entity.Property(e => e.UserType).HasColumnName("UserType");

                entity.Property(e => e.Active).HasColumnName("Active");

                entity.Property(e => e.Enabled).HasColumnName("Enabled");

                entity.Property(e => e.SaltHash).HasColumnName("SaltHash");

                entity.Property(e => e.SaltAes).HasColumnName("SaltAes");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");

                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

            });
            modelBuilder.Entity<Tb_Myanamr>(entity =>
            {
                entity.ToTable("Tb_Myanmar", "dbo");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.MyanmarID).ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Main_Title).HasColumnName("Main_Title");

                entity.Property(e => e.Title).HasColumnName("Title");

                entity.Property(e => e.Sub_Title).HasColumnName("Sub_Title");

                entity.Property(e => e.Teachar_Name).HasColumnName("Teachar_Name");

                entity.Property(e => e.Name).HasColumnName("Name");

                entity.Property(e => e.ContentType).HasColumnName("ContentType");

                entity.Property(e => e.Data).HasColumnName("Data");

                entity.Property(e => e.Photo_Name).HasColumnName("Photo_Name");

                entity.Property(e => e.Photo_ContentType).HasColumnName("Photo_ContentType");

                entity.Property(e => e.Photo_Data).HasColumnName("Photo_Data");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");

                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

                entity.Property(e => e.Active).HasColumnName("Active");
                entity.Property(e => e.Enabled).HasColumnName("Enabled");

            });           
            modelBuilder.Entity<Tb_Myanmar_Detail>(entity =>
            {
                entity.ToTable("Tb_Myanmar_Detail", "dbo");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.MyanmarDetailID).ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.MyanmarID).HasColumnName("MyanmarID");

                entity.Property(e => e.Main_Title).HasColumnName("Main_Title");

                entity.Property(e => e.Chapter).HasColumnName("Chapter");

                entity.Property(e => e.Title).HasColumnName("Title");

                entity.Property(e => e.Sub_Title).HasColumnName("Sub_Title");

                entity.Property(e => e.Teachar_Name).HasColumnName("Teachar_Name");

                entity.Property(e => e.Name).HasColumnName("Name");

                entity.Property(e => e.ContentType).HasColumnName("ContentType");

                entity.Property(e => e.Data).HasColumnName("Data");

                entity.Property(e => e.Photo_Name).HasColumnName("Photo_Name");

                entity.Property(e => e.Photo_ContentType).HasColumnName("Photo_ContentType");

                entity.Property(e => e.Photo_Data).HasColumnName("Photo_Data");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");

                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

                entity.Property(e => e.Active).HasColumnName("Active");

                entity.Property(e => e.Enabled).HasColumnName("Enabled");

            });
            modelBuilder.Entity<Tb_Chemistry>(entity =>
            {
                entity.ToTable("tbl_Chemistry", "dbo");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.ChemistryID).ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Chapter).HasColumnName("Chapter");

                entity.Property(e => e.Photo_Path).HasColumnName("Photo_Path");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");

                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

                entity.Property(e => e.Active).HasColumnName("Active");
                entity.Property(e => e.Enabled).HasColumnName("Enabled");

            });
            modelBuilder.Entity<Tb_Chemistry_Detail>(entity =>
            {
                entity.ToTable("tbl_ChemistryDetail", "dbo");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.ChemistryDetailID).ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.ChemistryID).HasColumnName("ChemistryID");

                entity.Property(e => e.Part).HasColumnName("Part");

                entity.Property(e => e.Title).HasColumnName("Title");

                entity.Property(e => e.Video_Path).HasColumnName("Video_Path");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");

                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

                entity.Property(e => e.Active).HasColumnName("Active");

                entity.Property(e => e.Enabled).HasColumnName("Enabled");

            });
        }
    }
}
