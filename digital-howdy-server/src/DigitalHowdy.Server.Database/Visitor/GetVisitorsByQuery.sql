SELECT
    v.Id,
    v.Name,
    v.Phone,
    o.Id AS Organization_Id,
    o.Name AS Organization_Name
FROM 
    Visitor v
INNER JOIN 
    Organization o
ON
    v.OrganizationId = o.Id
WHERE
    v.Name LIKE CONCAT('%',@query,'%')
OR
    v.Phone LIKE CONCAT('%',@query,'%')
OR
    o.Name LIKE CONCAT('%',@query,'%')
ORDER BY
    v.Name ASC;