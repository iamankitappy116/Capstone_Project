using Xunit;
using Crime.Data;
using Crime.Models;
using Crime.DAO;
using Crime.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Crime.Tests
{
    public class CrimeServiceTests
    {
        private CrimeDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<CrimeDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new CrimeDbContext(options);
        }

        private void SeedRequiredData(CrimeDbContext context)
        {
            context.Agencies.Add(new Agency
            {
                AgencyId = 1,
                AgencyName = "CBI",
                Jurisdiction = "Madhya Pradesh",
                PhoneNumber = "9999999999"
            });

            context.Victims.Add(new Victim
            {
                VictimId = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-25),
                PhoneNumber = "8888888888"
            });

            context.Suspects.Add(new Suspect
            {
                SuspectId = 1,
                FirstName = "Mike",
                LastName = "Smith",
                DateOfBirth = DateTime.Now.AddYears(-30),
                PhoneNumber = "7777777777"
            });

            context.SaveChanges();
        }

        [Fact]
        public void CreateIncident_ShouldAddIncident()
        {
            var context = GetDbContext();
            SeedRequiredData(context);
            var service = new CrimeService(context);

            var incident = new Incident
            {
                IncidentType = "Robbery",
                IncidentDate = DateTime.Now,
                Location = "Bhopal",
                Description = "Bank robbery",
                Status = "Open",
                VictimId = 1,
                SuspectId = 1,
                AgencyId = 1
            };

            service.CreateIncident(incident);

            Assert.Equal(1, context.Incidents.Count());
        }

        [Fact]
        public void UpdateIncidentStatus_ShouldChangeStatus()
        {
            var context = GetDbContext();
            SeedRequiredData(context);
            var service = new CrimeService(context);

            var incident = new Incident
            {
                IncidentType = "Theft",
                IncidentDate = DateTime.Now,
                Location = "Indore",
                Status = "Open",
                VictimId = 1,
                SuspectId = 1,
                AgencyId = 1
            };

            context.Incidents.Add(incident);
            context.SaveChanges();

            service.UpdateIncidentStatus(incident.IncidentId, "Closed");

            var updatedIncident = context.Incidents.Find(incident.IncidentId);
            Assert.Equal("Closed", updatedIncident.Status);
        }

        [Fact]
        public void GetIncidentById_ShouldReturnIncident()
        {
            var context = GetDbContext();
            SeedRequiredData(context);
            var service = new CrimeService(context);

            var incident = new Incident
            {
                IncidentType = "Fraud",
                IncidentDate = DateTime.Now,
                Location = "Delhi",
                Status = "Open",
                VictimId = 1,
                SuspectId = 1,
                AgencyId = 1
            };

            context.Incidents.Add(incident);
            context.SaveChanges();

            var result = service.GetIncidentById(incident.IncidentId);

            Assert.NotNull(result);
            Assert.Equal("Fraud", result.IncidentType);
        }

        [Fact]
        public void UpdateIncidentStatus_ShouldThrowException_WhenNotFound()
        {
            var context = GetDbContext();
            var service = new CrimeService(context);

            Assert.Throws<IncidentNotFoundException>(() =>
                service.UpdateIncidentStatus(999, "Closed"));
        }

        [Fact]
        public void DeleteIncident_ShouldRemoveIncident()
        {
            var context = GetDbContext();
            SeedRequiredData(context);
            var service = new CrimeService(context);

            var incident = new Incident
            {
                IncidentType = "Murder",
                IncidentDate = DateTime.Now,
                Location = "Mumbai",
                Status = "Open",
                VictimId = 1,
                SuspectId = 1,
                AgencyId = 1
            };

            context.Incidents.Add(incident);
            context.SaveChanges();

            service.DeleteIncident(incident.IncidentId);

            Assert.Equal(0, context.Incidents.Count());
        }
    }
}
