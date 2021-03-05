namespace DigitalHowdy.Server.Database
{
    public interface ISqlQueries
    {
        string GetVersionInfo { get; }
        string IsEmptyDatabase { get; }
        string CreateEventLogTable { get; }
        string InsertAdminUser { get; }
        string GetAllVisits { get; }
        string GetAllVisitsByQuery { get; }
        string GetAllCurrentVisitsByQuery { get; }
        string GetVisitById { get; }
        string InsertVisit { get; }
        string UpdateEndDateVisit { get; }
        string GetLastInsertId { get; }
        string DeleteVisit { get; }
        string RemoveVisitReference { get; }
        string CreateTables { get; }
        string CreateTestData { get; }
        string GetAllVisitors { get; }
        string GetVisitorsByQuery { get; }
        string GetVisitorById { get; }
        string GetVisitorByPhone { get; }
        string InsertVisitor { get; }
        string UpdateVisitor { get; }
        string DeleteVisitor { get; }
        string GetOrganizationByName { get; }
        string InsertOrganization { get; }
        string GetAllEmployee { get; }
        string GetAllEmployeeNamesAndId { get; }
        string GetEmployeeById { get; }
        string GetEmployeeByName { get; }
        string InsertEmployee { get; }
        string UpdateEmployeePhone { get; }
        string DeleteEmployee { get; }
        string GetAllAdmins { get; }
        string InsertAdmin { get; }
        string DeleteAdmin { get; }
        string GetAdminPasswordByUsername { get; }
        string InsertVersionInfo { get; }
        string InsertEventLog { get; }
        string GetAllEventLogs { get; }
        string GetAllEventLogsByQuery { get; }
        string GetAllCurrentVisits { get; }
    }
}