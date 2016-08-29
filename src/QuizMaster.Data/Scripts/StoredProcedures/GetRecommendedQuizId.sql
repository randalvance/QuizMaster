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
	SELECT TOP 1 QuizId
	FROM
	(
		SELECT 1 AS TypeIndicator, Q.QuizId, Q.Title, Q.ModifyDate, GETDATE() AS DateCompleted, 0 AS Score FROM Quizes Q
		LEFT JOIN dbo.QuizSessions QS ON QS.QuizId = Q.QuizId
		LEFT JOIN dbo.Sessions S ON S.SessionId = QS.SessionId
		WHERE S.ApplicationUserId IS NULL

		UNION

		SELECT 2 AS TypeIndicator, Q.QuizId, Q.Title, Q.ModifyDate, S.DateCompleted, S.CorrectAnswerCount AS Score FROM Quizes Q
		LEFT JOIN dbo.QuizSessions QS ON QS.QuizId = Q.QuizId
		LEFT JOIN dbo.Sessions S ON S.SessionId = QS.SessionId
		WHERE S.SessionId IS NOT NULL AND S.ApplicationUserId = @UserId
	) X
	ORDER BY X.TypeIndicator, X.Score, X.DateCompleted, NEWID()
END

GO
