CREATE PROCEDURE sp_GetAllCourses
AS
BEGIN
    SELECT * FROM Courses
    WHERE IsActive = 1
END