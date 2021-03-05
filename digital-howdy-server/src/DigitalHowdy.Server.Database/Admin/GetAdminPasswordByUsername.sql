SELECT
    HashedPassword
FROM
    Admin a
WHERE
    a.Username = @username;