IF OBJECT_ID('usp_SynchronizeData','P') IS NOT NULL
DROP PROC usp_SynchronizeData
GO

CREATE PROC usp_SynchronizeData
@Name                        VARCHAR(20),
@SexCode                     VARCHAR(20),
@NationCode                  VARCHAR(20),
@Age                         INT,
@Address                     VARCHAR(20),
@IDNumber                    VARCHAR(20),
@CurrentAddress              VARCHAR(20),
@PhoneNumber                 VARCHAR(20),
@SpecimenCollectionCode      VARCHAR(20),
@CategoryCode                VARCHAR(20),
@DetectionCode               VARCHAR(20),
@OccupationName              VARCHAR(20),
@TravelTrajectory            VARCHAR(20),
@Remarks                     VARCHAR(20),
@SamplingLocation            VARCHAR(20),
@SamplingPerson              VARCHAR(20)
AS

SET NOCOUNT ON

DECLARE @blz       TINYINT,
        @ylz       TINYINT,
		@sexname   VARCHAR(20),
		@qtmjz     TINYINT,
		@zyhz      TINYINT,
		@ph        TINYINT,
		@ncp       TINYINT,
		@frhz      TINYINT,
		@tj        TINYINT,
		@hzlbname  VARCHAR(20)

DECLARE @retVal int

SELECT @sexname=CASE WHEN @SexCode='1' THEN '男' ELSE '女' END,
       @blz=CASE WHEN @SpecimenCollectionCode='1' THEN 1 ELSE 0 END,
	   @ylz=CASE WHEN @SpecimenCollectionCode='2' THEN 1 ELSE 0 END,
	   @hzlbname=CASE @CategoryCode WHEN '1' THEN '密切接触者' WHEN '2' THEN '疑似患者' WHEN '3' THEN '无' ELSE '' END,
	   @qtmjz=CASE WHEN CHARINDEX('1',@DetectionCode)>0 THEN 1 ELSE 0 END,
	   @zyhz=CASE WHEN CHARINDEX('2',@DetectionCode)>0 THEN 1 ELSE 0 END,
	   @ph=CASE WHEN CHARINDEX('3',@DetectionCode)>0 THEN 1 ELSE 0 END,
	   @ncp=CASE WHEN CHARINDEX('5',@DetectionCode)>0 THEN 1 ELSE 0 END,
	   @frhz=CASE WHEN CHARINDEX('6',@DetectionCode)>0 THEN 1 ELSE 0 END,
	   @tj=CASE WHEN CHARINDEX('4',@DetectionCode)>0 THEN 1 ELSE 0 END

INSERT INTO dbo.hsBBData(cyDate,blz,ylz,name,sex,age,idCard,addr,jwfy,qtmjz,zyhz,ph,ncp,frhz,hzlb,remark,hzlb2,phone,cyr,cyAddr,addr1,tj)
VALUES (GETDATE(),@blz,@ylz,@Name,@sexname,@Age,@IDNumber,@Address,@TravelTrajectory,@qtmjz,@zyhz,@ph,@ncp,@frhz,@hzlbname,@Remarks,@OccupationName,@PhoneNumber,@SamplingPerson,@SamplingLocation,@CurrentAddress,@tj)

SELECT @retVal=@@IDENTITY

SELECT @retVal

RETURN

