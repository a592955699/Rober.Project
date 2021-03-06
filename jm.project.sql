USE [master]
GO
/****** Object:  Database [JmProject]    Script Date: 2018/12/18 14:18:52 ******/
CREATE DATABASE [JmProject]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'[JmProject]', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\JmProject.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'[JmProject]_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\JmProject.ldf' , SIZE = 7616KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [JmProject] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JmProject].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JmProject] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JmProject] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JmProject] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JmProject] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JmProject] SET ARITHABORT OFF 
GO
ALTER DATABASE [JmProject] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [JmProject] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JmProject] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JmProject] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JmProject] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JmProject] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JmProject] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JmProject] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JmProject] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JmProject] SET  DISABLE_BROKER 
GO
ALTER DATABASE [JmProject] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JmProject] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JmProject] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JmProject] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JmProject] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JmProject] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [JmProject] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JmProject] SET RECOVERY FULL 
GO
ALTER DATABASE [JmProject] SET  MULTI_USER 
GO
ALTER DATABASE [JmProject] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JmProject] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JmProject] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JmProject] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [JmProject] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'JmProject', N'ON'
GO
ALTER DATABASE [JmProject] SET QUERY_STORE = OFF
GO
USE [JmProject]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [JmProject]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_SplitStr]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_SplitStr] (
    @sText      NVARCHAR(Max),
    @sDelim     CHAR(1)=','
)

RETURNS @retArray TABLE (
    value   VARCHAR(100)
)
AS
BEGIN
    DECLARE 
        @posStart        BIGINT,
        @posNext        BIGINT,
        @valLen            BIGINT,
        @sValue            NVARCHAR(100);

    IF @sDelim IS NULL 
    BEGIN
        IF LEN(@sText)>100 SET @sText = SUBSTRING(@sText, 1, 100)
        
        INSERT @retArray (value)
        VALUES (@sText);
    END
    ELSE
    BEGIN
        SET @posStart = 1;

        WHILE @posStart <= LEN(@sText)
        BEGIN
            SET @posNext = CHARINDEX(@sDelim, @sText, @posStart);

            IF @posNext <= 0 
                SET @valLen = LEN(@sText) - @posStart + 1;
            ELSE
                SET @valLen = @posNext - @posStart;

            SET @sValue = SUBSTRING(@sText, @posStart, @valLen);
            SET @posStart = @posStart + @valLen + 1;

            IF LEN(@sValue) > 0
            BEGIN
                IF LEN(@sValue)>100 SET @sValue = SUBSTRING(@sValue, 1, 100)
                
                INSERT @retArray (value)
                VALUES (@sValue);
            END
        END
    END
    RETURN
END
GO
/****** Object:  Table [dbo].[CustomerFile]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Path] [nvarchar](200) NULL,
	[Size] [bigint] NULL,
	[Extension] [varchar](50) NULL,
	[CreatedUserId] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_CustomerFile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GenericAttribute]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GenericAttribute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NOT NULL,
	[KeyGroup] [nvarchar](400) NOT NULL,
	[Key] [nvarchar](400) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[Id] [int] NOT NULL,
	[ParentId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[IsMenu] [bit] NOT NULL,
	[Link] [varchar](200) NOT NULL,
	[Icon] [varchar](200) NOT NULL,
	[Script] [varchar](max) NOT NULL,
	[Order] [int] NOT NULL,
	[Published] [bit] NOT NULL,
	[OpenUrlInNewTab] [bit] NOT NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rule]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[Published] [bit] NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RuleMenuMapping]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleMenuMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RuleId] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
 CONSTRAINT [PK_RuleMenuMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedule]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScheduleCategory] [int] NOT NULL,
	[SubCategoryId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Remark] [nvarchar](max) NULL,
	[Common] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[Top] [bit] NOT NULL,
 CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScheduleMapping]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScheduleId] [int] NOT NULL,
	[EntityId] [int] NOT NULL,
	[EntityName] [nvarchar](400) NOT NULL,
 CONSTRAINT [PK_ScheduleUserMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScheduleSubCategory]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleSubCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScheduleCategory] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[IsCategory] [bit] NOT NULL,
	[Order] [int] NOT NULL,
	[Template] [varchar](200) NOT NULL,
 CONSTRAINT [PK_ScheduleSubCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[NickName] [nvarchar](50) NULL,
	[ErrorCount] [int] NOT NULL,
	[ErrorTime] [datetime] NULL,
	[LoginTime] [datetime] NULL,
	[LoginIp] [varchar](20) NULL,
	[Published] [bit] NOT NULL,
 CONSTRAINT [PK_SysUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserDepartmentMapping]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDepartmentMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
 CONSTRAINT [PK_UserDepartmentMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserMenuMapping]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMenuMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
 CONSTRAINT [PK_UserMenuMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRuleMapping]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRuleMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RuleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRuleMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlow]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlow](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkFlowCatalogId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[JsonConten] [varchar](max) NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlow] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowAction]    Script Date: 2018/12/18 14:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowAction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](10) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[SourceWorkFlowStepId] [int] NOT NULL,
	[TargetWorkFlowStepId] [int] NOT NULL,
	[ActionExecuted] [varchar](200) NULL,
	[ActionExecutedStopOnError] [bit] NULL,
	[ActionExecuting] [varchar](200) NULL,
	[ActionExecutingStopOnError] [bit] NULL,
	[BackExcute] [varchar](200) NULL,
	[BackExcuteStopOnError] [bit] NULL,
	[BackExcuting] [varchar](200) NULL,
	[BackExcutingStopOnError] [bit] NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowAction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowButton]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowButton](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[Icon] [varchar](50) NOT NULL,
	[Class] [varchar](50) NOT NULL,
	[Script] [varchar](max) NOT NULL,
	[Sort] [int] NOT NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowButton] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowCatalog]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowCatalog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Sort] [int] NULL,
	[Status] [varchar](20) NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowCatalog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowForm]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowForm](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkFlowFormCatalogId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[Sort] [int] NOT NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowForm] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowFormCatalog]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowFormCatalog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Sort] [int] NULL,
	[CreateBy] [int] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowFormCatalog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowInstance]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowInstance](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkFlowId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[TargetId] [varchar](50) NOT NULL,
	[WorkFlowTaskId] [int] NOT NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowInstance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowSignature]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowSignature](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[ImageUrl] [varchar](200) NOT NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowSignature] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowStep]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowStep](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkFlowId] [int] NOT NULL,
	[SubWorkFlowId] [int] NOT NULL,
	[WorkFlowFormId] [int] NOT NULL,
	[Name] [nchar](10) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[Catalog] [varchar](50) NOT NULL,
	[CommentCatalog] [int] NOT NULL,
	[AllDoneCheck] [bit] NOT NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowStep] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowStepButtonMapping]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowStepButtonMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkFlowStepId] [int] NOT NULL,
	[WorkFlowButtonId] [int] NOT NULL,
	[Sort] [int] NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowStepButtonMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkFlowTask]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkFlowTask](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[WorkFlowId] [int] NOT NULL,
	[WorkFlowStepId] [int] NOT NULL,
	[PreWorkFlowStepId] [int] NOT NULL,
	[PreWorkFlowTaskId] [int] NOT NULL,
	[WorkFlowSignatureId] [int] NOT NULL,
	[Comment] [nvarchar](500) NOT NULL,
	[Created] [int] NULL,
	[CreatedOnUtc] [datetime] NULL,
	[Updated] [int] NULL,
	[UpdatedOnUtc] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_WorkFlowTask] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Department] ON 

INSERT [dbo].[Department] ([Id], [Name], [Remark]) VALUES (1, N'广州区办', N'')
INSERT [dbo].[Department] ([Id], [Name], [Remark]) VALUES (2, N'佛山区办', N'')
SET IDENTITY_INSERT [dbo].[Department] OFF
SET IDENTITY_INSERT [dbo].[GenericAttribute] ON 

INSERT [dbo].[GenericAttribute] ([Id], [EntityId], [KeyGroup], [Key], [Value]) VALUES (2, 1, N'BaseEntity', N'HideSidebar', N'False')
INSERT [dbo].[GenericAttribute] ([Id], [EntityId], [KeyGroup], [Key], [Value]) VALUES (3, 1, N'User', N'HideSidebar', N'False')
INSERT [dbo].[GenericAttribute] ([Id], [EntityId], [KeyGroup], [Key], [Value]) VALUES (4, 1, N'User', N'rule-advanced-mode', N'False')
SET IDENTITY_INSERT [dbo].[GenericAttribute] OFF
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (1, 0, N'系统管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (2, 1, N'用户管理', N'', 1, N'', N'', N'', 2, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (3, 1, N'角色管理', N'', 1, N'/Admin/Rule/Index', N'', N'', 3, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (4, 1, N'部门管理', N'', 1, N'', N'', N'', 1, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (5, 1, N'菜单管理', N'', 1, N'', N'', N'', 1, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (6, 2, N'用户管理', N'', 1, N'/Admin/User/Index', N'', N'', 3, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (7, 2, N'在线用户', N'', 1, N'/Admin/User/Online', N'', N'', 3, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (9, 0, N'文章管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (10, 9, N'公文管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (11, 9, N'规章制度', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (12, 9, N'公司新闻', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (13, 0, N'合同管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (14, 0, N'申请管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (15, 14, N'用印审批申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (16, 14, N'物品归还申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (17, 14, N'物品维修申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (18, 14, N'物品领用申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (19, 14, N'物品采购申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (20, 14, N'员工出差申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (21, 14, N'经理出差申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (22, 14, N'外及内训申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (23, 14, N'员工省内专项培训申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (24, 14, N'外训申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (25, 14, N'组织培训申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (26, 0, N'报账系统', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (27, 0, N'数据分析', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (28, 27, N'数据管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (29, 27, N'数据报表', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (30, 0, N'人力资源管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (31, 30, N'薪酬类管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (32, 31, N'工资管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (33, 30, N'人事类类管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (34, 0, N'督办管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (35, 34, N'任务计划', N'', 1, N'/Admin/Schedule/index?scheduleCategory=1', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (36, 34, N'日程计划', N'', 1, N'/Admin/Schedule/index?scheduleCategory=0', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (37, 34, N'工作计划', N'', 1, N'/Admin/Schedule/index?scheduleCategory=2', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (38, 0, N'会议管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (39, 38, N'会议室预定', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (40, 38, N'会议议题管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (41, 38, N'会议纪要归档', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (42, 0, N'公会管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (43, 41, N'工会活动', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (44, 41, N'员工祝福', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (45, 0, N'计划管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (46, 45, N'计划管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (47, 45, N'计划报表', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (48, 0, N'招投标/采购/项目管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (49, 0, N'档案管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (50, 49, N'文件管理', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (51, 14, N'请假申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (52, 14, N'调休申请', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (53, 0, N'讨论区', N'', 1, N'', N'', N'', 0, 1, 0)
INSERT [dbo].[Menu] ([Id], [ParentId], [Name], [Remark], [IsMenu], [Link], [Icon], [Script], [Order], [Published], [OpenUrlInNewTab]) VALUES (54, 34, N'我的日历', N'', 1, N'/Admin/Schedule/Myscheduler', N'', N'', 0, 1, 0)
SET IDENTITY_INSERT [dbo].[Rule] ON 

INSERT [dbo].[Rule] ([Id], [Name], [Remark], [Published]) VALUES (2, N'超级管理员', N'asdfasdf', 1)
INSERT [dbo].[Rule] ([Id], [Name], [Remark], [Published]) VALUES (3, N'管理员', N'asdfasdf', 1)
INSERT [dbo].[Rule] ([Id], [Name], [Remark], [Published]) VALUES (4, N'项目经理', N'', 1)
SET IDENTITY_INSERT [dbo].[Rule] OFF
SET IDENTITY_INSERT [dbo].[RuleMenuMapping] ON 

INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (603, 2, 1)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (604, 2, 2)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (605, 2, 3)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (606, 2, 4)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (607, 2, 5)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (608, 2, 6)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (609, 2, 7)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (610, 2, 9)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (611, 2, 10)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (612, 2, 11)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (613, 2, 12)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (614, 2, 13)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (615, 2, 14)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (616, 2, 15)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (617, 2, 16)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (618, 2, 17)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (619, 2, 18)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (620, 2, 19)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (621, 2, 20)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (622, 2, 21)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (623, 2, 22)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (624, 2, 23)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (625, 2, 24)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (626, 2, 25)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (627, 2, 26)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (628, 2, 27)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (629, 2, 28)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (630, 2, 29)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (631, 2, 30)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (632, 2, 31)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (633, 2, 32)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (634, 2, 33)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (635, 2, 34)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (636, 2, 35)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (637, 2, 36)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (638, 2, 37)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (639, 2, 38)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (640, 2, 39)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (641, 2, 40)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (642, 2, 41)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (643, 2, 42)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (644, 2, 43)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (645, 2, 44)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (646, 2, 45)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (647, 2, 46)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (648, 2, 47)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (649, 2, 48)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (650, 2, 49)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (651, 2, 50)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (652, 2, 51)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (653, 2, 52)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (654, 2, 53)
INSERT [dbo].[RuleMenuMapping] ([Id], [RuleId], [MenuId]) VALUES (655, 2, 54)
SET IDENTITY_INSERT [dbo].[RuleMenuMapping] OFF
SET IDENTITY_INSERT [dbo].[Schedule] ON 

INSERT [dbo].[Schedule] ([Id], [ScheduleCategory], [SubCategoryId], [StatusId], [StartTime], [EndTime], [Title], [Content], [Remark], [Common], [CreatedUserId], [CreatedTime], [Top]) VALUES (28, 1, 12, 14, CAST(N'2018-12-13T00:00:00.000' AS DateTime), CAST(N'2018-12-13T00:00:00.000' AS DateTime), N'asdf', N'asdfa', NULL, N'dfasdf', 1, CAST(N'2018-12-13T08:29:51.127' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Schedule] OFF
SET IDENTITY_INSERT [dbo].[ScheduleSubCategory] ON 

INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (1, 0, N'日程提醒', 1, 1, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (2, 0, N'假日安排', 1, 2, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (3, 0, N'一般', 0, 1, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (4, 0, N'重要', 0, 2, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (5, 0, N'紧急', 0, 3, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (6, 2, N'日计划', 1, 1, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (7, 2, N'周计划', 1, 2, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (8, 2, N'月计划', 1, 3, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (9, 2, N'未完成', 0, 1, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (10, 2, N'已完成', 0, 2, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (11, 2, N'已取消', 0, 3, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (12, 1, N'公事', 1, 1, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (13, 1, N'私事', 1, 2, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (14, 1, N'新任务', 0, 1, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (15, 1, N'已接收', 0, 2, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (16, 1, N'进行中', 0, 3, N'')
INSERT [dbo].[ScheduleSubCategory] ([Id], [ScheduleCategory], [Title], [IsCategory], [Order], [Template]) VALUES (17, 1, N'已完成', 0, 4, N'')
SET IDENTITY_INSERT [dbo].[ScheduleSubCategory] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [UserName], [Password], [NickName], [ErrorCount], [ErrorTime], [LoginTime], [LoginIp], [Published]) VALUES (1, N'admin', N'123456', N'管理员', 0, NULL, CAST(N'2018-12-13T08:36:35.817' AS DateTime), N'127.0.0.1', 1)
INSERT [dbo].[User] ([Id], [UserName], [Password], [NickName], [ErrorCount], [ErrorTime], [LoginTime], [LoginIp], [Published]) VALUES (35, N'dd', N'dd', N'dd', 0, NULL, NULL, N'', 1)
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[UserDepartmentMapping] ON 

INSERT [dbo].[UserDepartmentMapping] ([Id], [UserId], [DepartmentId]) VALUES (13, 1, 2)
INSERT [dbo].[UserDepartmentMapping] ([Id], [UserId], [DepartmentId]) VALUES (18, 35, 2)
INSERT [dbo].[UserDepartmentMapping] ([Id], [UserId], [DepartmentId]) VALUES (19, 35, 1)
SET IDENTITY_INSERT [dbo].[UserDepartmentMapping] OFF
SET IDENTITY_INSERT [dbo].[UserRuleMapping] ON 

INSERT [dbo].[UserRuleMapping] ([Id], [UserId], [RuleId]) VALUES (45, 1, 4)
INSERT [dbo].[UserRuleMapping] ([Id], [UserId], [RuleId]) VALUES (63, 35, 4)
INSERT [dbo].[UserRuleMapping] ([Id], [UserId], [RuleId]) VALUES (64, 35, 3)
INSERT [dbo].[UserRuleMapping] ([Id], [UserId], [RuleId]) VALUES (66, 1, 2)
INSERT [dbo].[UserRuleMapping] ([Id], [UserId], [RuleId]) VALUES (67, 35, 2)
SET IDENTITY_INSERT [dbo].[UserRuleMapping] OFF
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_SysUser_ErrorCount]  DEFAULT ((0)) FOR [ErrorCount]
GO
ALTER TABLE [dbo].[WorkFlow] ADD  CONSTRAINT [DF_WorkFlow_JsonConten]  DEFAULT ('{}') FOR [JsonConten]
GO
ALTER TABLE [dbo].[WorkFlow] ADD  CONSTRAINT [DF_WorkFlow_CreateTime]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlow] ADD  CONSTRAINT [DF_WorkFlow_Delete]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowAction] ADD  CONSTRAINT [DF_WorkFlowAction_ActionExecutedStopOnError]  DEFAULT ((1)) FOR [ActionExecutedStopOnError]
GO
ALTER TABLE [dbo].[WorkFlowAction] ADD  CONSTRAINT [DF_WorkFlowAction_ActionExecutingStopOnError]  DEFAULT ((1)) FOR [ActionExecutingStopOnError]
GO
ALTER TABLE [dbo].[WorkFlowAction] ADD  CONSTRAINT [DF_WorkFlowAction_BackExcuteStopOnError]  DEFAULT ((1)) FOR [BackExcuteStopOnError]
GO
ALTER TABLE [dbo].[WorkFlowAction] ADD  CONSTRAINT [DF_WorkFlowAction_BackExcutingStopOnError]  DEFAULT ((1)) FOR [BackExcutingStopOnError]
GO
ALTER TABLE [dbo].[WorkFlowAction] ADD  CONSTRAINT [DF_WorkFlowAction_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowAction] ADD  CONSTRAINT [DF_WorkFlowAction_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowButton] ADD  CONSTRAINT [DF_WorkFlowButton_Order]  DEFAULT ((0)) FOR [Sort]
GO
ALTER TABLE [dbo].[WorkFlowButton] ADD  CONSTRAINT [DF_WorkFlowButton_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowButton] ADD  CONSTRAINT [DF_WorkFlowButton_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowCatalog] ADD  CONSTRAINT [DF_WorkFlowCatalog_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowCatalog] ADD  CONSTRAINT [DF_WorkFlowCatalog_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowForm] ADD  CONSTRAINT [DF_WorkFlowForm_Order]  DEFAULT ((0)) FOR [Sort]
GO
ALTER TABLE [dbo].[WorkFlowForm] ADD  CONSTRAINT [DF_WorkFlowForm_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowForm] ADD  CONSTRAINT [DF_WorkFlowForm_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowFormCatalog] ADD  CONSTRAINT [DF_WorkFlowFormCatalog_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowFormCatalog] ADD  CONSTRAINT [DF_WorkFlowFormCatalog_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowInstance] ADD  CONSTRAINT [DF_WorkFlowInstance_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowInstance] ADD  CONSTRAINT [DF_WorkFlowInstance_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowSignature] ADD  CONSTRAINT [DF_WorkFlowSignature_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowSignature] ADD  CONSTRAINT [DF_WorkFlowSignature_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowStep] ADD  CONSTRAINT [DF_WorkFlowStep_AllDoneCheck]  DEFAULT ((1)) FOR [AllDoneCheck]
GO
ALTER TABLE [dbo].[WorkFlowStep] ADD  CONSTRAINT [DF_WorkFlowStep_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowStep] ADD  CONSTRAINT [DF_WorkFlowStep_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowStepButtonMapping] ADD  CONSTRAINT [DF_WorkFlowStepButtonMapping_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowStepButtonMapping] ADD  CONSTRAINT [DF_WorkFlowStepButtonMapping_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[WorkFlowTask] ADD  CONSTRAINT [DF_WorkFlowTask_CreatedOnUtc]  DEFAULT (getdate()) FOR [CreatedOnUtc]
GO
ALTER TABLE [dbo].[WorkFlowTask] ADD  CONSTRAINT [DF_WorkFlowTask_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[RuleMenuMapping]  WITH CHECK ADD  CONSTRAINT [FK_RuleMenuMapping_Menu] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menu] ([Id])
GO
ALTER TABLE [dbo].[RuleMenuMapping] CHECK CONSTRAINT [FK_RuleMenuMapping_Menu]
GO
ALTER TABLE [dbo].[RuleMenuMapping]  WITH CHECK ADD  CONSTRAINT [FK_RuleMenuMapping_Rule] FOREIGN KEY([RuleId])
REFERENCES [dbo].[Rule] ([Id])
GO
ALTER TABLE [dbo].[RuleMenuMapping] CHECK CONSTRAINT [FK_RuleMenuMapping_Rule]
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK_Schedule_ScheduleSubCategory] FOREIGN KEY([SubCategoryId])
REFERENCES [dbo].[ScheduleSubCategory] ([Id])
GO
ALTER TABLE [dbo].[Schedule] CHECK CONSTRAINT [FK_Schedule_ScheduleSubCategory]
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK_Schedule_ScheduleSubCategory1] FOREIGN KEY([StatusId])
REFERENCES [dbo].[ScheduleSubCategory] ([Id])
GO
ALTER TABLE [dbo].[Schedule] CHECK CONSTRAINT [FK_Schedule_ScheduleSubCategory1]
GO
ALTER TABLE [dbo].[ScheduleMapping]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleMapping_Schedule] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedule] ([Id])
GO
ALTER TABLE [dbo].[ScheduleMapping] CHECK CONSTRAINT [FK_ScheduleMapping_Schedule]
GO
ALTER TABLE [dbo].[UserDepartmentMapping]  WITH CHECK ADD  CONSTRAINT [FK_UserDepartmentMapping_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([Id])
GO
ALTER TABLE [dbo].[UserDepartmentMapping] CHECK CONSTRAINT [FK_UserDepartmentMapping_Department]
GO
ALTER TABLE [dbo].[UserDepartmentMapping]  WITH CHECK ADD  CONSTRAINT [FK_UserDepartmentMapping_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserDepartmentMapping] CHECK CONSTRAINT [FK_UserDepartmentMapping_User]
GO
ALTER TABLE [dbo].[UserMenuMapping]  WITH CHECK ADD  CONSTRAINT [FK_UserMenuMapping_Menu] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menu] ([Id])
GO
ALTER TABLE [dbo].[UserMenuMapping] CHECK CONSTRAINT [FK_UserMenuMapping_Menu]
GO
ALTER TABLE [dbo].[UserMenuMapping]  WITH CHECK ADD  CONSTRAINT [FK_UserMenuMapping_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserMenuMapping] CHECK CONSTRAINT [FK_UserMenuMapping_User]
GO
ALTER TABLE [dbo].[UserRuleMapping]  WITH CHECK ADD  CONSTRAINT [FK_UserRuleMapping_Rule] FOREIGN KEY([RuleId])
REFERENCES [dbo].[Rule] ([Id])
GO
ALTER TABLE [dbo].[UserRuleMapping] CHECK CONSTRAINT [FK_UserRuleMapping_Rule]
GO
ALTER TABLE [dbo].[UserRuleMapping]  WITH CHECK ADD  CONSTRAINT [FK_UserRuleMapping_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserRuleMapping] CHECK CONSTRAINT [FK_UserRuleMapping_User]
GO
ALTER TABLE [dbo].[WorkFlow]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlow_WorkFlowCatalog] FOREIGN KEY([WorkFlowCatalogId])
REFERENCES [dbo].[WorkFlowCatalog] ([Id])
GO
ALTER TABLE [dbo].[WorkFlow] CHECK CONSTRAINT [FK_WorkFlow_WorkFlowCatalog]
GO
ALTER TABLE [dbo].[WorkFlowAction]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowAction_WorkFlowStep] FOREIGN KEY([SourceWorkFlowStepId])
REFERENCES [dbo].[WorkFlowStep] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowAction] CHECK CONSTRAINT [FK_WorkFlowAction_WorkFlowStep]
GO
ALTER TABLE [dbo].[WorkFlowAction]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowAction_WorkFlowStep1] FOREIGN KEY([TargetWorkFlowStepId])
REFERENCES [dbo].[WorkFlowStep] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowAction] CHECK CONSTRAINT [FK_WorkFlowAction_WorkFlowStep1]
GO
ALTER TABLE [dbo].[WorkFlowForm]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowForm_WorkFlowFormCatalog] FOREIGN KEY([WorkFlowFormCatalogId])
REFERENCES [dbo].[WorkFlowFormCatalog] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowForm] CHECK CONSTRAINT [FK_WorkFlowForm_WorkFlowFormCatalog]
GO
ALTER TABLE [dbo].[WorkFlowInstance]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowInstance_WorkFlow] FOREIGN KEY([WorkFlowId])
REFERENCES [dbo].[WorkFlow] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowInstance] CHECK CONSTRAINT [FK_WorkFlowInstance_WorkFlow]
GO
ALTER TABLE [dbo].[WorkFlowInstance]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowInstance_WorkFlowTask] FOREIGN KEY([WorkFlowTaskId])
REFERENCES [dbo].[WorkFlowTask] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowInstance] CHECK CONSTRAINT [FK_WorkFlowInstance_WorkFlowTask]
GO
ALTER TABLE [dbo].[WorkFlowStep]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowStep_WorkFlow] FOREIGN KEY([SubWorkFlowId])
REFERENCES [dbo].[WorkFlow] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowStep] CHECK CONSTRAINT [FK_WorkFlowStep_WorkFlow]
GO
ALTER TABLE [dbo].[WorkFlowStep]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowStep_WorkFlowForm] FOREIGN KEY([WorkFlowFormId])
REFERENCES [dbo].[WorkFlowForm] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowStep] CHECK CONSTRAINT [FK_WorkFlowStep_WorkFlowForm]
GO
ALTER TABLE [dbo].[WorkFlowStepButtonMapping]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowStepButtonMapping_WorkFlowButton] FOREIGN KEY([WorkFlowButtonId])
REFERENCES [dbo].[WorkFlowButton] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowStepButtonMapping] CHECK CONSTRAINT [FK_WorkFlowStepButtonMapping_WorkFlowButton]
GO
ALTER TABLE [dbo].[WorkFlowStepButtonMapping]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowStepButtonMapping_WorkFlowStep] FOREIGN KEY([WorkFlowStepId])
REFERENCES [dbo].[WorkFlowStep] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowStepButtonMapping] CHECK CONSTRAINT [FK_WorkFlowStepButtonMapping_WorkFlowStep]
GO
ALTER TABLE [dbo].[WorkFlowTask]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowTask_WorkFlow] FOREIGN KEY([WorkFlowId])
REFERENCES [dbo].[WorkFlow] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowTask] CHECK CONSTRAINT [FK_WorkFlowTask_WorkFlow]
GO
ALTER TABLE [dbo].[WorkFlowTask]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowTask_WorkFlowStep] FOREIGN KEY([WorkFlowStepId])
REFERENCES [dbo].[WorkFlowStep] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowTask] CHECK CONSTRAINT [FK_WorkFlowTask_WorkFlowStep]
GO
ALTER TABLE [dbo].[WorkFlowTask]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowTask_WorkFlowStep1] FOREIGN KEY([PreWorkFlowStepId])
REFERENCES [dbo].[WorkFlowStep] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowTask] CHECK CONSTRAINT [FK_WorkFlowTask_WorkFlowStep1]
GO
ALTER TABLE [dbo].[WorkFlowTask]  WITH CHECK ADD  CONSTRAINT [FK_WorkFlowTask_WorkFlowTask] FOREIGN KEY([PreWorkFlowTaskId])
REFERENCES [dbo].[WorkFlowTask] ([Id])
GO
ALTER TABLE [dbo].[WorkFlowTask] CHECK CONSTRAINT [FK_WorkFlowTask_WorkFlowTask]
GO
/****** Object:  StoredProcedure [dbo].[up_GetMenuByRule]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
根据用户ID，获取所有权限
*/
create proc [dbo].[up_GetMenuByRule]
(
	@RuleId int
)
as

select * from Menu where
id in(
	select MenuId from RuleMenuMapping where RuleId = @RuleId
)
GO
/****** Object:  StoredProcedure [dbo].[up_GetMenuByRules]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
根据角色ID拼接字符串，获取所有权限
*/
create proc [dbo].[up_GetMenuByRules]
(
	@RuleIds varchar(max)
)
as

select * from Menu where
id in(
	select MenuId from RuleMenuMapping where RuleId  in(
		select * from [fn_SplitStr] (@RuleIds,',')
	)
)
GO
/****** Object:  StoredProcedure [dbo].[up_GetMenuByUser]    Script Date: 2018/12/18 14:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
根据用户ID，获取所有权限
*/
create proc [dbo].[up_GetMenuByUser]
(
	@UserId int
)
as
select b.* from UserMenuMapping a
inner join Menu b
on a.MenuId=b.Id
where a.UserId=@UserId
union 
select * from Menu where
id in(
	select MenuId from RuleMenuMapping where RuleId in(
		select RuleId from UserRuleMapping a where a.UserId=@UserId
	)
)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作流分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlow', @level2type=N'COLUMN',@level2name=N'WorkFlowCatalogId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作流名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlow', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlow', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作流数据json格式化字符串' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlow', @level2type=N'COLUMN',@level2name=N'JsonConten'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlow', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动作名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始步骤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'SourceWorkFlowStepId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目标步骤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'TargetWorkFlowStepId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行后事件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'ActionExecuted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出错后是否中断' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'ActionExecutedStopOnError'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行中事件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'ActionExecuting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出错后是否中断' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'ActionExecutingStopOnError'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退回后事件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'BackExcute'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出错后是否中断' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'BackExcuteStopOnError'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退回中事件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'BackExcuting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出错后是否中断' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'BackExcutingStopOnError'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowAction', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'css类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowButton', @level2type=N'COLUMN',@level2name=N'Class'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowButton', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowCatalog', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowForm', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowFormCatalog', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属工作流' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowInstance', @level2type=N'COLUMN',@level2name=N'WorkFlowId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作流实例名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowInstance', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审批实例状态：处理中、处理完毕、废除的' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowInstance', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'待审批数据的主键ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowInstance', @level2type=N'COLUMN',@level2name=N'TargetId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前任务点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowInstance', @level2type=N'COLUMN',@level2name=N'WorkFlowTaskId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowInstance', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowSignature', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属工作流' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStep', @level2type=N'COLUMN',@level2name=N'WorkFlowId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子流程' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStep', @level2type=N'COLUMN',@level2name=N'SubWorkFlowId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStep', @level2type=N'COLUMN',@level2name=N'WorkFlowFormId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'步骤名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStep', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStep', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'步骤分类：开始步骤，标准步骤，结束步骤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStep', @level2type=N'COLUMN',@level2name=N'Catalog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审批意见类型：无审批意见，文字审批，文字+签章审批' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStep', @level2type=N'COLUMN',@level2name=N'CommentCatalog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否所有分支执行完毕，才能执行下一步' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStep', @level2type=N'COLUMN',@level2name=N'AllDoneCheck'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStep', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowStepButtonMapping', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowTask', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审批状态：审批通过，驳回' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowTask', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属工作流' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowTask', @level2type=N'COLUMN',@level2name=N'WorkFlowId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前步骤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowTask', @level2type=N'COLUMN',@level2name=N'WorkFlowStepId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次步骤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowTask', @level2type=N'COLUMN',@level2name=N'PreWorkFlowStepId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次任务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowTask', @level2type=N'COLUMN',@level2name=N'PreWorkFlowTaskId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'签章' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowTask', @level2type=N'COLUMN',@level2name=N'WorkFlowSignatureId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审批意见' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowTask', @level2type=N'COLUMN',@level2name=N'Comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逻辑删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkFlowTask', @level2type=N'COLUMN',@level2name=N'Deleted'
GO
USE [master]
GO
ALTER DATABASE [JmProject] SET  READ_WRITE 
GO
