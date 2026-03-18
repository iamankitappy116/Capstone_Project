using Crime.Models;

namespace Crime.DAO
{
    public interface ICrimeService
    {
        bool CreateIncident(Incident incident);

        bool UpdateIncidentStatus(int incidentId, string status);
        bool UpdateIncident(Incident incident);

        IEnumerable<Incident> GetAllIncidents();

        Incident? GetIncidentById(int id);

        bool DeleteIncident(int id);

        IEnumerable<Incident> GetIncidentsInDateRange(DateTime startDate, DateTime endDate);

        IEnumerable<Incident> SearchIncidents(string incidentType);

        Report? GenerateIncidentReport(int incidentId);

        Case CreateCase(string caseDescription, List<int> incidentIds);

        Case? GetCaseDetails(int caseId);

        bool UpdateCaseDetails(Case updatedCase);

        IEnumerable<Case> GetAllCases();
        bool DeleteCase(int caseId);

        // --- Officers By Ayan ---

        IEnumerable<Officer> GetAllOfficers();
        Officer? GetOfficerById(int id);
        bool CreateOfficer(Officer officer);
        bool UpdateOfficer(Officer officer);
        bool DeleteOfficer(int id);

        // --- Suspects By Aakash ---

        IEnumerable<Suspect> GetAllSuspects();       //  (Index page)
        Suspect? GetSuspectById(int id);             // (Details/Edit/Delete page)
        bool CreateSuspect(Suspect suspect);         //  (Create page)
        bool UpdateSuspect(Suspect suspect);         //  (Edit page)
        bool DeleteSuspect(int id);                  //  (Delete page)

        // --- Victims By Diksha ---

        IEnumerable<Victim> GetAllVictims();
        Victim? GetVictimById(int id);
        void CreateVictim(Victim victim);
        void UpdateVictim(Victim victim);
        bool DeleteVictim(int id);


        // --- Agency By Ankit ---
        IEnumerable<Agency> GetAllAgencies();
        Agency? GetAgencyById(int id);
        bool CreateAgency(Agency agency);
        bool UpdateAgency(Agency agency);
        bool DeleteAgency(int id);

        // --- Evidences By Nikhil ---
        IEnumerable<Evidence> GetAllEvidences();
        Evidence? GetEvidenceById(int id);
        void CreateEvidence(Evidence evidence);
        void UpdateEvidence(Evidence evidence);
        bool DeleteEvidence(int id);

        //---Report by Diksha---
        IEnumerable<Report> GetAllReports();
        Report? GetReportById(int id);
        void CreateReport(Report report);
        void UpdateReport(Report report);
        bool DeleteReport(int id);

    }
}