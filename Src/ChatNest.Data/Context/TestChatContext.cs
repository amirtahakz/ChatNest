using ChatNest.Data.Entities.Chats;
using ChatNest.Data.Entities.Roles;
using ChatNest.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Data.Context
{
    public class TestChatContext : DbContext
    {
        public TestChatContext(DbContextOptions<TestChatContext> options) : base(options)
        {

        }

        #region Entities

        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var cascades = modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(t => t.GetForeignKeys())
            //    .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            //foreach (var fk in cascades)
            //{
            //    fk.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            modelBuilder.Entity<Chat>()
                .HasOne(b => b.User)
                .WithMany(b => b.Chats)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<UserGroup>()
                .HasOne(b => b.User)
                .WithMany(b => b.UserGroups)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
