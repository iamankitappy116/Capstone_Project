using Crime.Data;
using Crime.Models;
using Crime.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Crime.DAO
{
    public class CrimeService : ICrimeService
    {
        private readonly CrimeDbContext _context;

        public CrimeService(CrimeDbContext context)
        {
            _context = context;
        }

        // ===== Helper Validation Methods =====

        private void ValidatePhoneNumber(string? phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber) && !Regex.IsMatch(phoneNumber, @"^\d{10}$"))
            {
                throw new InvalidPhoneNumberException("Phone number must be exactly 10 digits.");
            }
        }

        private void ValidateDateNotInFuture(DateTime date, string fieldName)
        {
            if (date > DateTime.Now)
            {
                throw new InvalidDateException($"{fieldName} cannot be in the future.");
            }
        }

        // ===== Incidents =====

        public bool CreateIncident(Incident incident)
        {
            ValidateDateNotInFuture(incident.IncidentDate, "Incident date");
            _context.Incidents.Add(incident);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateIncidentStatus(int incidentId, string status)
        {
            var incident = _context.Incidents.Find(incidentId);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with ID {incidentId} was not found.");

            incident.Status = status;
            return _context.SaveChanges() > 0;
        }

        public bool UpdateIncident(Incident incident)
        {
            ValidateDateNotInFuture(incident.IncidentDate, "Incident date");
            _context.Incidents.Update(incident);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Incident> GetIncidentsInDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Incidents
                .Where(i => i.IncidentDate >= startDate && i.IncidentDate <= endDate)
                .ToList();
        }

        public IEnumerable<Incident> SearchIncidents(string incidentType)
        {
            return _context.Incidents
                .Where(i => i.IncidentType == incidentType)
                .ToList();
        }

        public IEnumerable<Incident> GetAllIncidents()
        {
            return _context.Incidents
                .Include(i => i.Agency)
                .Include(i => i.Suspect)
                .Include(i => i.Victim)
                .ToList();
        }

        public Incident? GetIncidentById(int id)
        {
            var incident = _context.Incidents
                .Include(i => i.Agency)
                .Include(i => i.Suspect)
                .Include(i => i.Victim)
                .FirstOrDefault(i => i.IncidentId == id);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with ID {id} was not found.");

            return incident;
        }

        public bool DeleteIncident(int id)
        {
            var incident = _context.Incidents.Find(id);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with ID {id} was not found.");

            _context.Incidents.Remove(incident);
            return _context.SaveChanges() > 0;
        }

        public Report? GenerateIncidentReport(int incidentId)
        {
            var incident = _context.Incidents
                .Include(i => i.Reports)
                .FirstOrDefault(i => i.IncidentId == incidentId);

            if (incident == null)
                throw new IncidentNotFoundException($"Incident with ID {incidentId} was not found.");

            return incident.Reports?.FirstOrDefault();
        }

        // ===== Cases =====

        public Case CreateCase(string caseDescription, List<int> incidentIds)
        {
            var incidents = _context.Incidents
                .Where(i => incidentIds.Contains(i.IncidentId))
                .ToList();

            var newCase = new Case
            {
                CaseDescription = caseDescription,
                Incidents = incidents
            };

            _context.Cases.Add(newCase);
            _context.SaveChanges();

            return newCase;
        }

        public Case? GetCaseDetails(int caseId)
        {
            var caseObj = _context.Cases
                .Include(c => c.Incidents)
                .FirstOrDefault(c => c.CaseId == caseId);

            if (caseObj == null)
                throw new CaseNotFoundException($"Case with ID {caseId} was not found.");

            return caseObj;
        }

        public bool UpdateCaseDetails(Case updatedCase)
        {
            _context.Cases.Update(updatedCase);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Case> GetAllCases()
        {
            return _context.Cases
                .Include(c => c.Incidents)
                .ToList();
        }

        public bool DeleteCase(int caseId)
        {
            var caseObj = _context.Cases.Find(caseId);

            if (caseObj == null)
                throw new CaseNotFoundException($"Case with ID {caseId} was not found.");

            _context.Cases.Remove(caseObj);
            return _context.SaveChanges() > 0;
        }

        // --- Officers By Ayan ---

        public IEnumerable<Officer> GetAllOfficers()
        {
            return _context.Officers
            .Include(o => o.Agency)
            .ToList();
        }

        public Officer? GetOfficerById(int id)
        {
            var officer = _context.Officers
            .Include(o => o.Agency)
            .FirstOrDefault(o => o.OfficerId == id);

            if (officer == null)
                throw new OfficerNotFoundException($"Officer with ID {id} was not found.");

            return officer;
        }

        public bool CreateOfficer(Officer officer)
        {
            ValidatePhoneNumber(officer.PhoneNumber);
            _context.Officers.Add(officer);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateOfficer(Officer officer)
        {
            ValidatePhoneNumber(officer.PhoneNumber);
            _context.Officers.Update(officer);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteOfficer(int id)
        {
            var officer = _context.Officers.Find(id);
            if (officer == null)
                throw new OfficerNotFoundException($"Officer with ID {id} was not found.");

            _context.Officers.Remove(officer);
            return _context.SaveChanges() > 0;
        }

        // --- Suspects By Aakash ---

        public IEnumerable<Suspect> GetAllSuspects()
        {
            return _context.Suspects.ToList();
        }

        public Suspect? GetSuspectById(int id)
        {
            var suspect = _context.Suspects.FirstOrDefault(s => s.SuspectId == id);

            if (suspect == null)
                throw new SuspectNotFoundException($"Suspect with ID {id} was not found.");

            return suspect;
        }

        public bool CreateSuspect(Suspect suspect)
        {
            ValidateDateNotInFuture(suspect.DateOfBirth, "Date of birth");
            ValidatePhoneNumber(suspect.PhoneNumber);
            _context.Suspects.Add(suspect);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateSuspect(Suspect suspect)
        {
            ValidateDateNotInFuture(suspect.DateOfBirth, "Date of birth");
            ValidatePhoneNumber(suspect.PhoneNumber);
            _context.Suspects.Update(suspect);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteSuspect(int id)
        {
            var suspect = _context.Suspects.Find(id);
            if (suspect == null)
                throw new SuspectNotFoundException($"Suspect with ID {id} was not found.");

            _context.Suspects.Remove(suspect);
            return _context.SaveChanges() > 0;
        }

        // --- Victims By Diksha ---

        public IEnumerable<Victim> GetAllVictims()
        {
            return _context.Victims.ToList();
        }

        public Victim? GetVictimById(int id)
        {
            var victim = _context.Victims.FirstOrDefault(v => v.VictimId == id);

            if (victim == null)
                throw new VictimNotFoundException($"Victim with ID {id} was not found.");

            return victim;
        }

        public void CreateVictim(Victim victim)
        {
            ValidateDateNotInFuture(victim.DateOfBirth, "Date of birth");
            ValidatePhoneNumber(victim.PhoneNumber);
            _context.Victims.Add(victim);
            _context.SaveChanges();
        }

        public void UpdateVictim(Victim victim)
        {
            ValidateDateNotInFuture(victim.DateOfBirth, "Date of birth");
            ValidatePhoneNumber(victim.PhoneNumber);
            _context.Victims.Update(victim);
            _context.SaveChanges();
        }

        public bool DeleteVictim(int id)
        {
            var victim = _context.Victims.Find(id);

            if (victim == null)
                throw new VictimNotFoundException($"Victim with ID {id} was not found.");

            _context.Victims.Remove(victim);
            return _context.SaveChanges() > 0;
        }

        // --- Agency By Ankit ---

        public IEnumerable<Agency> GetAllAgencies()
        {
            return _context.Agencies
                .Include(a => a.Officers)
                .ToList();
        }

        public Agency? GetAgencyById(int id)
        {
            var agency = _context.Agencies
                .Include(a => a.Officers)
                .FirstOrDefault(a => a.AgencyId == id);

            if (agency == null)
                throw new AgencyNotFoundException($"Agency with ID {id} was not found.");

            return agency;
        }

        public bool CreateAgency(Agency agency)
        {
            ValidatePhoneNumber(agency.PhoneNumber);
            _context.Agencies.Add(agency);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateAgency(Agency agency)
        {
            ValidatePhoneNumber(agency.PhoneNumber);
            _context.Agencies.Update(agency);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteAgency(int id)
        {
            var agency = _context.Agencies.Find(id);
            if (agency == null)
                throw new AgencyNotFoundException($"Agency with ID {id} was not found.");

            _context.Agencies.Remove(agency);
            return _context.SaveChanges() > 0;
        }

        // --- Evidences By Nikhil ---

        public IEnumerable<Evidence> GetAllEvidences()
        {
            return _context.Evidences
                .Include(e => e.Incident)
                .ToList();
        }

        public Evidence? GetEvidenceById(int id)
        {
            var evidence = _context.Evidences
                .Include(e => e.Incident)
                .FirstOrDefault(e => e.EvidenceId == id);

            if (evidence == null)
                throw new EvidenceNotFoundException($"Evidence with ID {id} was not found.");

            return evidence;
        }

        public void CreateEvidence(Evidence evidence)
        {
            _context.Evidences.Add(evidence);
            _context.SaveChanges();
        }

        public void UpdateEvidence(Evidence evidence)
        {
            _context.Evidences.Update(evidence);
            _context.SaveChanges();
        }

        public bool DeleteEvidence(int id)
        {
            var evidence = _context.Evidences.Find(id);

            if (evidence == null)
                throw new EvidenceNotFoundException($"Evidence with ID {id} was not found.");

            _context.Evidences.Remove(evidence);
            return _context.SaveChanges() > 0;
        }

        // --- Report by Diksha ---

        public IEnumerable<Report> GetAllReports()
        {
            return _context.Reports
                .Include(r => r.Incident)
                .Include(r => r.ReportingOfficer)
                .ToList();
        }

        public Report? GetReportById(int id)
        {
            var report = _context.Reports
                .Include(r => r.Incident)
                .Include(r => r.ReportingOfficer)
                .FirstOrDefault(r => r.ReportId == id);

            if (report == null)
                throw new ReportNotFoundException($"Report with ID {id} was not found.");

            return report;
        }

        public void CreateReport(Report report)
        {
            ValidateDateNotInFuture(report.ReportDate, "Report date");
            _context.Reports.Add(report);
            _context.SaveChanges();
        }

        public void UpdateReport(Report report)
        {
            ValidateDateNotInFuture(report.ReportDate, "Report date");
            _context.Reports.Update(report);
            _context.SaveChanges();
        }

        public bool DeleteReport(int id)
        {
            var report = _context.Reports.Find(id);

            if (report == null)
                throw new ReportNotFoundException($"Report with ID {id} was not found.");

            _context.Reports.Remove(report);
            return _context.SaveChanges() > 0;
        }
    }
}