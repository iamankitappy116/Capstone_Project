using Crime.DAO;
using Crime.Models;
using Crime.Exceptions;

namespace Crime
{
    public class MainModule
    {
        private readonly ICrimeService _crimeService;
        private readonly List<string> _results = new List<string>();

        public MainModule(ICrimeService crimeService)
        {
            _crimeService = crimeService;
        }

        public List<string> Run()
        {
            _results.Clear();

            TestIncidentMethods();
            TestCaseMethods();
            TestOfficerMethods();
            TestSuspectMethods();
            TestVictimMethods();
            TestAgencyMethods();
            TestEvidenceMethods();
            TestReportMethods();

            return _results;
        }

        private void Log(string message) => _results.Add(message);

        // ===== Incident Methods =====
        private void TestIncidentMethods()
        {
            Log("========== INCIDENT METHODS ==========");

            try
            {
                var allIncidents = _crimeService.GetAllIncidents();
                Log($"✅ GetAllIncidents() — Found {allIncidents.Count()} incidents");
            }
            catch (Exception ex) { Log($"❌ GetAllIncidents() — {ex.Message}"); }

            try
            {
                var incident = _crimeService.GetIncidentById(1);
                Log($"✅ GetIncidentById(1) — Found: {incident?.IncidentType}");
            }
            catch (IncidentNotFoundException ex) { Log($"❌ GetIncidentById(1) — {ex.Message}"); }

            // Test exception: invalid ID
            try
            {
                _crimeService.GetIncidentById(9999);
                Log("✅ GetIncidentById(9999) — Found");
            }
            catch (IncidentNotFoundException ex) { Log($"⚠️ GetIncidentById(9999) — Exception caught: {ex.Message}"); }

            try
            {
                var results = _crimeService.SearchIncidents("Theft");
                Log($"✅ SearchIncidents(\"Theft\") — Found {results.Count()} results");
            }
            catch (Exception ex) { Log($"❌ SearchIncidents() — {ex.Message}"); }

            try
            {
                var results = _crimeService.GetIncidentsInDateRange(DateTime.MinValue, DateTime.Now);
                Log($"✅ GetIncidentsInDateRange() — Found {results.Count()} results");
            }
            catch (Exception ex) { Log($"❌ GetIncidentsInDateRange() — {ex.Message}"); }

            // Test exception: future date
            try
            {
                var futureIncident = new Incident
                {
                    IncidentType = "Test",
                    IncidentDate = DateTime.Now.AddDays(10),
                    Location = "Test",
                    Status = "Open",
                    VictimId = 1,
                    SuspectId = 1,
                    AgencyId = 1
                };
                _crimeService.CreateIncident(futureIncident);
                Log("✅ CreateIncident(future date) — Created");
            }
            catch (InvalidDateException ex) { Log($"⚠️ CreateIncident(future date) — Exception caught: {ex.Message}"); }

            Log("");
        }

        // ===== Case Methods =====
        private void TestCaseMethods()
        {
            Log("========== CASE METHODS ==========");

            try
            {
                var allCases = _crimeService.GetAllCases();
                Log($"✅ GetAllCases() — Found {allCases.Count()} cases");
            }
            catch (Exception ex) { Log($"❌ GetAllCases() — {ex.Message}"); }

            try
            {
                var caseDetails = _crimeService.GetCaseDetails(1);
                Log($"✅ GetCaseDetails(1) — Found: {caseDetails?.CaseDescription}");
            }
            catch (CaseNotFoundException ex) { Log($"❌ GetCaseDetails(1) — {ex.Message}"); }

            // Test exception: invalid ID
            try
            {
                _crimeService.GetCaseDetails(9999);
                Log("✅ GetCaseDetails(9999) — Found");
            }
            catch (CaseNotFoundException ex) { Log($"⚠️ GetCaseDetails(9999) — Exception caught: {ex.Message}"); }

            Log("");
        }

        // ===== Officer Methods =====
        private void TestOfficerMethods()
        {
            Log("========== OFFICER METHODS ==========");

            try
            {
                var allOfficers = _crimeService.GetAllOfficers();
                Log($"✅ GetAllOfficers() — Found {allOfficers.Count()} officers");
            }
            catch (Exception ex) { Log($"❌ GetAllOfficers() — {ex.Message}"); }

            try
            {
                var officer = _crimeService.GetOfficerById(1);
                Log($"✅ GetOfficerById(1) — Found: {officer?.FirstName} {officer?.LastName}");
            }
            catch (OfficerNotFoundException ex) { Log($"❌ GetOfficerById(1) — {ex.Message}"); }

            // Test exception: invalid ID
            try
            {
                _crimeService.GetOfficerById(9999);
                Log("✅ GetOfficerById(9999) — Found");
            }
            catch (OfficerNotFoundException ex) { Log($"⚠️ GetOfficerById(9999) — Exception caught: {ex.Message}"); }

            Log("");
        }

        // ===== Suspect Methods =====
        private void TestSuspectMethods()
        {
            Log("========== SUSPECT METHODS ==========");

            try
            {
                var allSuspects = _crimeService.GetAllSuspects();
                Log($"✅ GetAllSuspects() — Found {allSuspects.Count()} suspects");
            }
            catch (Exception ex) { Log($"❌ GetAllSuspects() — {ex.Message}"); }

            try
            {
                var suspect = _crimeService.GetSuspectById(1);
                Log($"✅ GetSuspectById(1) — Found: {suspect?.FirstName} {suspect?.LastName}");
            }
            catch (SuspectNotFoundException ex) { Log($"❌ GetSuspectById(1) — {ex.Message}"); }

            // Test exception: invalid ID
            try
            {
                _crimeService.GetSuspectById(9999);
                Log("✅ GetSuspectById(9999) — Found");
            }
            catch (SuspectNotFoundException ex) { Log($"⚠️ GetSuspectById(9999) — Exception caught: {ex.Message}"); }

            // Test exception: future date of birth
            try
            {
                var futureSuspect = new Suspect
                {
                    FirstName = "Test",
                    LastName = "Suspect",
                    DateOfBirth = DateTime.Now.AddYears(1),
                    Gender = "Male",
                    PhoneNumber = "1234567890"
                };
                _crimeService.CreateSuspect(futureSuspect);
                Log("✅ CreateSuspect(future DOB) — Created");
            }
            catch (InvalidDateException ex) { Log($"⚠️ CreateSuspect(future DOB) — Exception caught: {ex.Message}"); }

            // Test exception: invalid phone number
            try
            {
                var badPhoneSuspect = new Suspect
                {
                    FirstName = "Test",
                    LastName = "Suspect",
                    DateOfBirth = DateTime.Now.AddYears(-25),
                    Gender = "Male",
                    PhoneNumber = "abc"
                };
                _crimeService.CreateSuspect(badPhoneSuspect);
                Log("✅ CreateSuspect(bad phone) — Created");
            }
            catch (InvalidPhoneNumberException ex) { Log($"⚠️ CreateSuspect(bad phone) — Exception caught: {ex.Message}"); }

            Log("");
        }

        // ===== Victim Methods =====
        private void TestVictimMethods()
        {
            Log("========== VICTIM METHODS ==========");

            try
            {
                var allVictims = _crimeService.GetAllVictims();
                Log($"✅ GetAllVictims() — Found {allVictims.Count()} victims");
            }
            catch (Exception ex) { Log($"❌ GetAllVictims() — {ex.Message}"); }

            try
            {
                var victim = _crimeService.GetVictimById(1);
                Log($"✅ GetVictimById(1) — Found: {victim?.FirstName} {victim?.LastName}");
            }
            catch (VictimNotFoundException ex) { Log($"❌ GetVictimById(1) — {ex.Message}"); }

            // Test exception: invalid ID
            try
            {
                _crimeService.GetVictimById(9999);
                Log("✅ GetVictimById(9999) — Found");
            }
            catch (VictimNotFoundException ex) { Log($"⚠️ GetVictimById(9999) — Exception caught: {ex.Message}"); }

            // Test exception: future date of birth
            try
            {
                var futureVictim = new Victim
                {
                    FirstName = "Test",
                    LastName = "Victim",
                    DateOfBirth = DateTime.Now.AddYears(1),
                    Gender = "Female",
                    PhoneNumber = "9876543210"
                };
                _crimeService.CreateVictim(futureVictim);
                Log("✅ CreateVictim(future DOB) — Created");
            }
            catch (InvalidDateException ex) { Log($"⚠️ CreateVictim(future DOB) — Exception caught: {ex.Message}"); }

            Log("");
        }

        // ===== Agency Methods =====
        private void TestAgencyMethods()
        {
            Log("========== AGENCY METHODS ==========");

            try
            {
                var allAgencies = _crimeService.GetAllAgencies();
                Log($"✅ GetAllAgencies() — Found {allAgencies.Count()} agencies");
            }
            catch (Exception ex) { Log($"❌ GetAllAgencies() — {ex.Message}"); }

            try
            {
                var agency = _crimeService.GetAgencyById(1);
                Log($"✅ GetAgencyById(1) — Found: {agency?.AgencyName}");
            }
            catch (AgencyNotFoundException ex) { Log($"❌ GetAgencyById(1) — {ex.Message}"); }

            // Test exception: invalid ID
            try
            {
                _crimeService.GetAgencyById(9999);
                Log("✅ GetAgencyById(9999) — Found");
            }
            catch (AgencyNotFoundException ex) { Log($"⚠️ GetAgencyById(9999) — Exception caught: {ex.Message}"); }

            // Test exception: invalid phone
            try
            {
                var badPhoneAgency = new Agency
                {
                    AgencyName = "Test Agency",
                    Jurisdiction = "Test",
                    PhoneNumber = "123"
                };
                _crimeService.CreateAgency(badPhoneAgency);
                Log("✅ CreateAgency(bad phone) — Created");
            }
            catch (InvalidPhoneNumberException ex) { Log($"⚠️ CreateAgency(bad phone) — Exception caught: {ex.Message}"); }

            Log("");
        }

        // ===== Evidence Methods =====
        private void TestEvidenceMethods()
        {
            Log("========== EVIDENCE METHODS ==========");

            try
            {
                var allEvidences = _crimeService.GetAllEvidences();
                Log($"✅ GetAllEvidences() — Found {allEvidences.Count()} evidences");
            }
            catch (Exception ex) { Log($"❌ GetAllEvidences() — {ex.Message}"); }

            try
            {
                var evidence = _crimeService.GetEvidenceById(1);
                Log($"✅ GetEvidenceById(1) — Found: {evidence?.Description}");
            }
            catch (EvidenceNotFoundException ex) { Log($"❌ GetEvidenceById(1) — {ex.Message}"); }

            // Test exception: invalid ID
            try
            {
                _crimeService.GetEvidenceById(9999);
                Log("✅ GetEvidenceById(9999) — Found");
            }
            catch (EvidenceNotFoundException ex) { Log($"⚠️ GetEvidenceById(9999) — Exception caught: {ex.Message}"); }

            Log("");
        }

        // ===== Report Methods =====
        private void TestReportMethods()
        {
            Log("========== REPORT METHODS ==========");

            try
            {
                var allReports = _crimeService.GetAllReports();
                Log($"✅ GetAllReports() — Found {allReports.Count()} reports");
            }
            catch (Exception ex) { Log($"❌ GetAllReports() — {ex.Message}"); }

            try
            {
                var report = _crimeService.GetReportById(1);
                Log($"✅ GetReportById(1) — Found: {report?.ReportDetails}");
            }
            catch (ReportNotFoundException ex) { Log($"❌ GetReportById(1) — {ex.Message}"); }

            // Test exception: invalid ID
            try
            {
                _crimeService.GetReportById(9999);
                Log("✅ GetReportById(9999) — Found");
            }
            catch (ReportNotFoundException ex) { Log($"⚠️ GetReportById(9999) — Exception caught: {ex.Message}"); }

            // Test exception: future report date
            try
            {
                var futureReport = new Report
                {
                    ReportDate = DateTime.Now.AddDays(30),
                    ReportDetails = "Test",
                    Status = "Draft",
                    IncidentId = 1,
                    ReportingOfficerId = 1
                };
                _crimeService.CreateReport(futureReport);
                Log("✅ CreateReport(future date) — Created");
            }
            catch (InvalidDateException ex) { Log($"⚠️ CreateReport(future date) — Exception caught: {ex.Message}"); }

            Log("");
        }
    }
}
