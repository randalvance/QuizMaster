IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRecommendedQuizId]') AND type in (N'P', N'PC')) 
    EXEC sp_executesql @statement = N'CREATE PROCEDURE dbo.GetRecommendedQuizId AS SELECT ''Placeholder'' AS Placeholder';
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetRecommendedQuizId]
	@UserId UNIQUEIDENTIFIER
AS
BEGIN
	DECLARE @TodayDay INT
	DECLARE @TodayMonth INT
	DECLARE @TodayYear INT

	SELECT @TodayDay = DATEPART(DAY, GETDATE()),
		   @TodayMonth = DATEPART(MONTH, GETDATE()),
		   @TodayYear = DATEPART(YEAR, GETDATE())

	SELECT TOP 1 QuizId
	FROM
	(
		SELECT 1 AS TypeIndicator, Q.QuizId, Q.Title, Q.ModifyDate, GETDATE() AS DateCompleted, 0 AS Score FROM Quizes Q
		LEFT JOIN dbo.QuizSessions QS ON QS.QuizId = Q.QuizId
		LEFT JOIN dbo.Sessions S ON S.SessionId = QS.SessionId
		WHERE S.ApplicationuserId IS NULL

		UNION

		SELECT 2 AS TypeIndicator, Q.QuizId, Q.Title, Q.ModifyDate, S.DateCompleted, S.CorrectAnswerCount AS Score FROM Quizes Q
		LEFT JOIN dbo.QuizSessions QS ON QS.QuizId = Q.QuizId
		LEFT JOIN dbo.Sessions S ON S.SessionId = QS.SessionId
		WHERE S.SessionId IS NOT NULL AND S.ApplicationUserId = @UserId
	) X
	-- Do not repeat if taken today
	WHERE X.QuizId NOT IN (
		SELECT QS.QuizId FROM QuizSessions QS
		JOIN Sessions S ON S.SessionId = QS.SessionId
		WHERE S.ApplicationUserId = @UserId AND
			(@TodayMonth = DATEPART(MONTH, S.DateCompleted) AND
			 @TodayDay = DATEPART(DAY, S.DateCompleted) AND
			 @TodayYear = DATEPART(YEAR, S.DateCompleted))
	)
	ORDER BY X.TypeIndicator, X.Score, X.DateCompleted, NEWID()
END

GO
