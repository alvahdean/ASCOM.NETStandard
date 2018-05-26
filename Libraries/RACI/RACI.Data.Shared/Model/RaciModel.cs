using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using Microsoft.Extensions.Configuration;
using ASCOM;
using RACI.Settings;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace RACI.Data
{
    public class RaciModel : DbContext, IRaciModel
    {
        public static AppSettings Settings { get; private set; }

        public DeleteBehavior DefaultDeleteBehavior { get; }
        public bool DefaultRequiredRelationship { get; }

        public DbSet<SimpleValue> SimpleValues { get; set; }
        public DbSet<ProfileValue> ProfileValues { get; set; }
        public DbSet<ProfileNode> ProfileNodes { get; set; }
        public DbSet<AscomDeviceNode> DeviceRoots { get; set; }
        public DbSet<AscomDriverNode> DriverTypes { get; set; }
        public DbSet<RaciUser> Users { get; set; }
        public DbSet<AscomSettingsNode> AscomSettings { get; set; }
        public DbSet<AscomPlatformNode> AscomPlatforms { get; set; }
        public DbSet<RaciSettings> SystemSettings { get; set; }
        public DbSet<RaciSystem> Systems { get; set; }

        static RaciModel()
        {
            Settings = new AppSettings();
        }

        public RaciModel() : this(DeleteBehavior.Cascade, true) { }
 
        public RaciModel(DeleteBehavior defaultDeleteBehavior, bool defaultRequiredRelationship)
        {
            DefaultDeleteBehavior = defaultDeleteBehavior;
            DefaultRequiredRelationship = defaultRequiredRelationship;
            if (LogMessages == null)
            {
                LogMessages = new List<string>();
                this.GetService<ILoggerFactory>().AddProvider(new RaciModelLogProvider());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ProfileNode>()
                .HasMany(e => e.Nodes)
                .WithOne(e => e.Parent)
                .OnDelete(DefaultDeleteBehavior)
                .IsRequired(false);
            modelBuilder
                .Entity<ProfileNode>()
                .HasMany(e => e.Values)
                .WithOne(e => e.Parent)
                .OnDelete(DefaultDeleteBehavior)
                .IsRequired(DefaultRequiredRelationship);
            modelBuilder.Entity<ProfileNode>()
                .HasDiscriminator<string>("ProfileType");
            modelBuilder.Entity<ProfileNode>()
                .HasIndex(p => new { p.ParentProfileNodeId,p.Name })
                .IsUnique();
            modelBuilder.Entity<ProfileValue>()
                .HasIndex(p => new { p.ParentProfileNodeId,p.Key })
                .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ReplaceService<IModelCacheKeyFactory, RaciModelCacheKeyFactory>()
                .EnableSensitiveDataLogging();

            string connName = Settings.Repository.DefaultConnection
                ?? Settings.Repository.ConnectionStrings.Keys.FirstOrDefault();
            if (String.IsNullOrWhiteSpace(connName))
                throw new Exception("No connection found in application settings");
            if (String.IsNullOrWhiteSpace(Settings.Repository.Provider))
                Settings.Repository.Provider="sqlite";
            
            string connString = Settings.Repository.ConnectionStrings[connName];
                //Console.WriteLine($"ASCOM Repository: [{Settings.Repository.Provider}.{Settings.Repository.DefaultConnection}]:{connString}");
                switch (Settings.Repository.Provider.ToUpperInvariant())
                {
                    case "SQLITE":
                        optionsBuilder.UseSqlite(connString);
                        break;
                    default:
                        throw new ASCOM.NotImplementedException($"Unsupported repository type: '{Settings.Repository.Provider}'");
                }
        }

        public override int SaveChanges()
        {
            LogMessages.Clear();
            return base.SaveChanges();
        }

        public class RaciModelCacheKeyFactory : IModelCacheKeyFactory
        {
            public virtual object Create(DbContext context)
            {
                var raciModel = (RaciModel)context;

                var result= (raciModel.DefaultDeleteBehavior, raciModel.DefaultRequiredRelationship);
                return result;
            }
        }

        public static IList<string> LogMessages;

        private class RaciModelLogProvider : ILoggerProvider
        {
            public ILogger CreateLogger(string categoryName) => new SampleLogger();

            public void Dispose() { }

            private class SampleLogger : ILogger
            {
                public bool IsEnabled(LogLevel logLevel) => true;

                public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                    Func<TState, Exception, string> formatter)
                {
                    if (eventId.Id == RelationalEventId.CommandExecuting.Id)
                    {
                        var message = formatter(state, exception);
                        var commandIndex = Math.Max(message.IndexOf("UPDATE"), message.IndexOf("DELETE"));
                        if (commandIndex >= 0)
                        {
                            var truncatedMessage = message.Substring(commandIndex, message.IndexOf(";", commandIndex) - commandIndex).Replace(Environment.NewLine, " ");

                            for (var i = 0; i < 4; i++)
                            {
                                var paramIndex = message.IndexOf($"@p{i}='");
                                if (paramIndex >= 0)
                                {
                                    var paramValue = message.Substring(paramIndex + 5, 1);
                                    if (paramValue == "'")
                                    {
                                        paramValue = "NULL";
                                    }

                                    truncatedMessage = truncatedMessage.Replace($"@p{i}", paramValue);
                                }
                            }

                            LogMessages.Add(truncatedMessage);
                        }
                    }
                }

                public IDisposable BeginScope<TState>(TState state) => null;
            }
        }
    }

}