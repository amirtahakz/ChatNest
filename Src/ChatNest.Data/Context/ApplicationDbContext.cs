using ChatNest.Data.Entities.Chats;
using ChatNest.Data.Entities.Roles;
using ChatNest.Data.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNest.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascades = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascades)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Chat>()
                .HasOne(b => b.User)
                .WithMany(b => b.Chats)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroup>()
               .HasOne(ug => ug.ChatGroup)
               .WithMany(cg => cg.UserGroups)
               .HasForeignKey(ug => ug.GroupId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatGroup>()
                .Property(x=>x.ReceiverId).IsRequired(false);


            // ChatGroup → Owner (User)
            modelBuilder.Entity<ChatGroup>()
                .HasOne(cg => cg.Owner)
                .WithMany(u => u.OwnedChatGroups)
                .HasForeignKey(cg => cg.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);


            // ChatGroup → Receiver (User)
            modelBuilder.Entity<ChatGroup>()
                .HasOne(cg => cg.Receiver)
                .WithMany(u => u.ReceivedPrivateGroup)
                .HasForeignKey(cg => cg.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            //// ChatGroup → Chats
            //modelBuilder.Entity<ChatGroup>()
            //    .HasMany(cg => cg.Chats)
            //    .WithOne(c => c.ChatGroup) // فرض بر اینه که کلاس Chat همچین پروپرتی‌ای داره
            //    .HasForeignKey(c => c.GroupId);


            //// User → UserRoles
            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.UserRoles)
            //    .WithOne(ur => ur.User)
            //    .HasForeignKey(ur => ur.UserId);

            //// User → Chats
            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.Chats)
            //    .WithOne(c => c.User) // فرض بر اینه که Chat پروپرتی User داره
            //    .HasForeignKey(c => c.UserId);

            // User ↔ UserGroup (many-to-many)
            //modelBuilder.Entity<UserGroup>()
            //    .HasKey(ug => new { ug.UserId, ug.GroupId });

            //modelBuilder.Entity<UserGroup>()
            //    .HasOne(ug => ug.User)
            //    .WithMany(u => u.UserGroups)
            //    .HasForeignKey(ug => ug.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);



        }

    }

}
