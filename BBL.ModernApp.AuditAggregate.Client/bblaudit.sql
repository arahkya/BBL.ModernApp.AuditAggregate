﻿drop table dbo.bblaudit
create table dbo.bblaudit
(
	SessionID nvarchar(50) null
    , OperationDateTime datetime not null
    , OperationName nvarchar(32) not null
    , CustomerID nvarchar(32) not null
    , Channel nvarchar(50) not null
    , SubChannel nvarchar(50) null
    , DeviceID int null
    , IPAddress nvarchar(32) null
    , Succeeded bit
    , ErrorCode nvarchar(255) null
    , Info1 nvarchar(100) null
    , Info2 nvarchar(100) null
    , Info3 nvarchar(100) null
    , Info4 nvarchar(100) null
    , Info5 nvarchar(100) null
    , Info6 nvarchar(100) null
    , Info7 nvarchar(255) null
    , Info8 nvarchar(255) null
    , Info9 nvarchar(255) null
    , Info10 nvarchar(255) null
    , Keyword nvarchar(255) null
    , DisplayMessage nvarchar(1000) null
)