using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Data.Entities.Account;
using Library.Data.Entities.Library;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Data;
using System.Data.Common;

namespace Library.Data.Services
{
    public partial class ApplicationDbContext : DbContext, IDbContext
    {
        public virtual DbSet<BookCarts> BookCarts { get; set; }
        public virtual DbSet<BookFavorites> BookFavorites { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Libraries> Libraries { get; set; }
        public virtual DbSet<PermissionGroupMembers> PermissionGroupMembers { get; set; }
        public virtual DbSet<PermissionGroups> PermissionGroups { get; set; }
        public virtual DbSet<Publishers> Publishers { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Titles> Titles { get; set; }
        public virtual DbSet<UserBookRequests> UserBookRequests { get; set; }
        public virtual DbSet<UserBooks> UserBooks { get; set; }
        public virtual DbSet<UserNotifications> UserNotifications { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookCarts>(entity =>
            {
                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookCarts)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookCarts__BookI__4AB81AF0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookCarts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookCarts__UserI__49C3F6B7");
            });

            modelBuilder.Entity<BookFavorites>(entity =>
            {
                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookFavorites)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookFavor__BookI__5070F446");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookFavorites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookFavor__UserI__4F7CD00D");
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasIndex(e => e.BookCode)
                    .HasName("UQ__Books__0A5FFCC77A927797")
                    .IsUnique();

                entity.Property(e => e.Amount).HasDefaultValueSql("((0))");

                entity.Property(e => e.AmountAvailable).HasDefaultValueSql("((0))");

                entity.Property(e => e.Author).HasMaxLength(250);

                entity.Property(e => e.BookCode)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.BookName).HasMaxLength(250);

                entity.Property(e => e.DateImport).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Format).HasMaxLength(50);

                entity.Property(e => e.MaximumDateBorrow).HasDefaultValueSql("((0))");

                entity.Property(e => e.Pages).HasDefaultValueSql("((0))");

                entity.Property(e => e.PublicationDate).HasColumnType("date");

                entity.Property(e => e.Size).HasMaxLength(20);

                entity.Property(e => e.Tag).HasMaxLength(250);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Books__CategoryI__3F466844");

                entity.HasOne(d => d.Library)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.LibraryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Books__LibraryId__440B1D61");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Books__Publisher__4222D4EF");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Books__SupplierI__4316F928");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Type).HasMaxLength(250);
            });

            modelBuilder.Entity<Libraries>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(15);
            });

            modelBuilder.Entity<PermissionGroupMembers>(entity =>
            {
                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.PermissionGroupMembers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Permissio__Group__2E1BDC42");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PermissionGroupMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Permissio__UserI__2F10007B");
            });

            modelBuilder.Entity<PermissionGroups>(entity =>
            {
                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Publishers>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(15);
            });

            modelBuilder.Entity<Suppliers>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(15);
            });

            modelBuilder.Entity<Titles>(entity =>
            {
                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ShortName).HasMaxLength(50);
            });

            modelBuilder.Entity<UserBookRequests>(entity =>
            {
                entity.HasIndex(e => e.RequestCode)
                    .HasName("UQ__UserBook__CBAB82F6D3B1242E")
                    .IsUnique();

                entity.Property(e => e.RequestCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBookRequests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserBookR__UserI__5441852A");
            });

            modelBuilder.Entity<UserBooks>(entity =>
            {
                entity.Property(e => e.DeadlineDate).HasColumnType("datetime");

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.UserBooks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserBooks__BookI__59FA5E80");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.UserBooks)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserBooks__Reque__59063A47");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBooks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserBooks__UserI__5812160E");
            });

            modelBuilder.Entity<UserNotifications>(entity =>
            {
                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.MessageDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserNotifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserNotif__UserI__5DCAEF64");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__Users__C9F28456308A00A5")
                    .IsUnique();

                entity.Property(e => e.BirthDay).HasColumnType("date");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.FirstName).HasMaxLength(20);

                entity.Property(e => e.JoinDate).HasColumnType("date");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(20);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PersonalEmail).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.SchoolEmail).HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.HasOne(d => d.Title)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.TitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__TitleId__276EDEB3");
            });
        }

        public Task<int> SaveChangesAsync()
        {
            var validationErrors = ChangeTracker
                .Entries<IValidatableObject>()
                .SelectMany(e => e.Entity.Validate(null))
                .Where(r => r != ValidationResult.Success)
                .ToArray();

            if (validationErrors.Any())
            {
                var exceptionMessage = string.Join(Environment.NewLine, validationErrors.Select(error => string.Format("Properties {0} Error: {1}", error.MemberNames, error.ErrorMessage)));
                throw new Exception(exceptionMessage);
            }

            return base.SaveChangesAsync();
        }

        public Task<int> ExecuteSqlCommandAsync(string commandText,
            params object[] parameters) => Database.ExecuteSqlCommandAsync(commandText, parameters: parameters);

        public async Task<List<Dictionary<string, object>>> ExecuteStoredProcedureListAsync(string commandText,
           params object[] parameters)
        {
            using (var cmd = Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    for (var i = 0; i <= parameters.Length - 1; i++)
                    {
                        var p = parameters[i] as DbParameter;
                        if (p == null)
                        {
                            throw new Exception("Not support parameter type");
                        }
                        cmd.Parameters.Add(p);
                    }
                }

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                var list = new List<Dictionary<string, object>>();
                using (var dataReader = await cmd.ExecuteReaderAsync())
                {
                    while (await dataReader.ReadAsync())
                    {
                        var dataRow = new Dictionary<string, object>();
                        for (var iField = 0; iField < dataReader.FieldCount; iField++)
                        {
                            dataRow.Add(dataReader.GetName(iField),
                                dataReader.IsDBNull(iField) ? null : dataReader[iField]);
                        }
                        list.Add(dataRow);
                    }
                }
                return list;
            }
        }
    }

}
