SELECT
    e.Id,
    e.Date,
    e.Description
FROM
    EventLog e
WHERE
    e.Description LIKE @query
ORDER BY Date DESC;