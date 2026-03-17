CREATE PROCEDURE sp_GetCourseById
    @CourseId INT
AS
BEGIN
    SELECT * FROM Courses
    WHERE CourseId = @CourseId
      AND IsActive = 1
END