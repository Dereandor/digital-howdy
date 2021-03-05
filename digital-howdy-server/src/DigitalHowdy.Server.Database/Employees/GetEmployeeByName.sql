SELECT 
    Id, 
    Name, 
    Phone, 
    Email 
FROM 
    Employee 
WHERE 
    Name LIKE CONCAT('%',@Name,'%');