SELECT
    v.Id,
    v2.Id AS Visitor_Id,
    v2.Name AS Visitor_Name,
    v2.Phone as Visitor_Phone,
    o.Id AS Visitor_Organization_Id,
    o.Name AS Visitor_Organization_Name,
    e.Id AS Employee_Id,
    e.Name AS Employee_Name,
    e.Phone AS Employee_Phone,
    e.Email AS Employee_Email,
    v.StartDate,
    v.EndDate,
    v.Reference
FROM 
    Visit v
INNER JOIN
    Visitor v2
ON
    v.VisitorId = v2.Id
INNER JOIN
    Organization o
ON
    v2.OrganizationId = o.Id
INNER JOIN
    Employee e
ON
    v.EmployeeId = e.Id
WHERE
    v2.Name LIKE CONCAT('%',@query,'%') OR 
    v2.Phone LIKE CONCAT('%',@query,'%') OR 
    o.Name LIKE CONCAT('%',@query,'%') OR 
    e.Name LIKE CONCAT('%',@query,'%') OR 
    e.Phone LIKE CONCAT('%',@query,'%') OR 
    e.Email LIKE CONCAT('%',@query,'%') OR 
    v.Reference LIKE CONCAT('%',@query,'%') 
ORDER BY 
    StartDate DESC;