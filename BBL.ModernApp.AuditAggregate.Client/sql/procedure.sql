USE bbldevdb
GO

CREATE or ALTER PROCEDURE AddAuditLog
      @SessionID nvarchar(50)
    , @OperationDateTime datetime
    , @OperationName nvarchar(32)
    , @CustomerID nvarchar(32)
    , @Channel nvarchar(50)
    , @SubChannel nvarchar(50)
    , @DeviceID int
    , @IPAddress nvarchar(32)
    , @Succeeded bit
    , @ErrorCode nvarchar(255)
    , @Info1 nvarchar(100)
    , @Info2 nvarchar(100)
    , @Info3 nvarchar(100)
    , @Info4 nvarchar(100)
    , @Info5 nvarchar(100)
    , @Info6 nvarchar(100)
    , @Info7 nvarchar(255)
    , @Info8 nvarchar(255)
    , @Info9 nvarchar(255)
    , @Info10 nvarchar(255)
    , @Keyword nvarchar(255)
    , @DisplayMessage nvarchar(1000)
AS
    INSERT INTO dbo.bblaudit(
        SessionID
        , OperationDateTime
        , OperationName
        , CustomerID
        , Channel
        , SubChannel
        , DeviceID
        , IPAddress
        , Succeeded
        , ErrorCode
        , Info1
        , Info2
        , Info3
        , Info4
        , Info5
        , Info6
        , Info7
        , Info8
        , Info9
        , Info10
        , Keyword
        , DisplayMessage)
    VALUES (
        @SessionID
        , @OperationDateTime
        , @OperationName
        , @CustomerID
        , @Channel
        , @SubChannel
        , @DeviceID
        , @IPAddress
        , @Succeeded
        , @ErrorCode
        , @Info1
        , @Info2
        , @Info3
        , @Info4
        , @Info5
        , @Info6
        , @Info7
        , @Info8
        , @Info9
        , @Info10
        , @Keyword
        , @DisplayMessage
    );
GO