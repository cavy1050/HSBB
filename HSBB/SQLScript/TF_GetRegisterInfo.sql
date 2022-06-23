IF OBJECT_ID('TF_GetRegisterInfo','TF') IS NOT NULL
DROP FUNCTION TF_GetRegisterInfo
GO

CREATE FUNCTION TF_GetRegisterInfo
(
	@ksrq    varchar(10),
	@jsrq    varchar(10)
)
returns @ret table
(
	PageID							INT,
	RowID							INT,
	ID								INT,
	Name							VARCHAR(50),
	Sex								VARCHAR(10),
	Age								INT,
	Address							VARCHAR(256),
	IDNumber						VARCHAR(20),
	cyDate							VARCHAR(20),
	CurrentAddress					VARCHAR(256),
	PhoneNumber						VARCHAR(20),
	CurrentSpecimenCollectionType   VARCHAR(50),
	CurrentPatientCategoryType      VARCHAR(50),
	CurrentDetectionType            VARCHAR(50),
	OccupationName                  VARCHAR(50),
	TravelTrajectory                VARCHAR(100),
	Remarks                         VARCHAR(100),
	SamplingLocation                VARCHAR(50),
	SamplingPerson                  VARCHAR(50)
)
AS

BEGIN

	INSERT INTO @ret(PageID,RowID,ID,Name,Sex,
						Age,Address,IDNumber,cyDate,CurrentAddress,
							PhoneNumber,CurrentSpecimenCollectionType,CurrentPatientCategoryType,CurrentDetectionType,OccupationName,
								TravelTrajectory,Remarks,SamplingLocation,SamplingPerson)
	SELECT c.PageID,c.RowID,c.id,CONVERT(VARCHAR(50),d.name),CONVERT(VARCHAR(10),d.sex),
			d.age,d.addr,CONVERT(VARCHAR(20),d.idCard),CONVERT(VARCHAR(10),d.cyDate,120)+' '+CONVERT(VARCHAR(9),d.cyDate,108),d.addr1,CONVERT(VARCHAR(20),d.phone),
			 CASE WHEN d.blz=1 THEN '鼻咽拭子' WHEN d.ylz=1 THEN '口咽拭子' ELSE '' END,
			  CONVERT(VARCHAR(50),d.hzlb),
			   CASE WHEN d.qtmjz=1 THEN '门急诊' WHEN d.zyhz=1 THEN '住院患者' WHEN d.ph=1 THEN '陪护' WHEN d.ncp=1 THEN 'NCP筛查' WHEN d.frhz=1 THEN '发热患者' WHEN d.tj=1 THEN '体检(自检)' ELSE '' END,
			    CONVERT(VARCHAR(50),d.hzlb2),
				 CONVERT(VARCHAR(100),d.jwfy),
				  CONVERT(VARCHAR(100),d.remark),
				   CONVERT(VARCHAR(50),d.cyAddr),
				    CONVERT(VARCHAR(50),d.cyr)
	FROM
    (
	SELECT CASE b.RowNumber%10 WHEN 0 THEN b.RowNumber/10 ELSE b.RowNumber/10+1 END PageID,
			CASE b.RowNumber%10 WHEN 0 THEN 10 ELSE b.RowNumber%10 END RowID,
			 b.id
	FROM (SELECT ROW_NUMBER() OVER (ORDER BY id) RowNumber,id FROM dbo.hsBBData a (NOLOCK) WHERE  CONVERT(VARCHAR(10),cyDate,120) BETWEEN @ksrq AND @jsrq) AS b
	) AS c INNER JOIN dbo.hsBBData d (NOLOCK) ON c.id=d.id

RETURN

END
GO