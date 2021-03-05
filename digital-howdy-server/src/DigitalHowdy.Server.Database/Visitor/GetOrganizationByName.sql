SELECT
    o.Id,
    o.Name
FROM
    Organization o
WHERE
    o.Name = @name
LIMIT 1;