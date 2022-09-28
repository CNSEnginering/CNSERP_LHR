
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_GLTRV]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[V_GLTRV]
AS
SELECT        GLTRH.DocNo, GLTRH.DocDate, GLTRH.NARRATION, GLTRH.Posted, GLBOOKS.BookName,
             isnull(SUM(case when  GLTRD.Amount > 0 then GLTRD.Amount  end),0) as Debit, isnull(SUM(case when  GLTRD.Amount < 0 then GLTRD.Amount end), 0) as Credit  
FROM            GLTRH INNER JOIN
                         GLBOOKS ON GLTRH.BookID = GLBOOKS.BookID INNER JOIN
                         GLTRD ON GLTRH.DetID = GLTRD.DetID
						 group by  GLTRH.DocNo, GLTRH.DocDate, GLTRH.NARRATION, GLTRH.Posted, GLBOOKS.BookName
'