USE [master]
GO
/****** Object:  Database [HealthSupportSysdb]    Script Date: 5/6/2024 9:23:54 PM ******/
CREATE DATABASE [HealthSupportSysdb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HealthSupportSysdb', FILENAME = N'F:\PCL Project\Database\HealthSupportSysdb.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HealthSupportSysdb_log', FILENAME = N'F:\PCL Project\Database\HealthSupportSysdb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HealthSupportSysdb] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HealthSupportSysdb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HealthSupportSysdb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET ARITHABORT OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [HealthSupportSysdb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HealthSupportSysdb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HealthSupportSysdb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HealthSupportSysdb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HealthSupportSysdb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HealthSupportSysdb] SET  MULTI_USER 
GO
ALTER DATABASE [HealthSupportSysdb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HealthSupportSysdb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HealthSupportSysdb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HealthSupportSysdb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'HealthSupportSysdb', N'ON'
GO
USE [HealthSupportSysdb]
GO
/****** Object:  Table [dbo].[AccountTypeTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountTypeTable](
	[AccountTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_AccountTypeTable] PRIMARY KEY CLUSTERED 
(
	[AccountTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DoctorAppointTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DoctorAppointTable](
	[DoctorAppointID] [int] IDENTITY(1,1) NOT NULL,
	[DoctorID] [int] NOT NULL,
	[PatientID] [int] NOT NULL,
	[DoctorTimeSlotID] [int] NOT NULL,
	[AppointDate] [date] NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[IsFeeSubmit] [bit] NOT NULL,
	[IsChecked] [bit] NOT NULL,
	[TransectionNo] [nvarchar](150) NOT NULL,
	[DoctorComment] [nvarchar](500) NULL,
 CONSTRAINT [PK_DoctorAppointTable] PRIMARY KEY CLUSTERED 
(
	[DoctorAppointID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DoctorTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DoctorTable](
	[DoctorID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[ContactNo] [nvarchar](20) NOT NULL,
	[Fees] [float] NOT NULL,
	[Splztion] [nvarchar](150) NOT NULL,
	[ClinicAddress] [nvarchar](300) NOT NULL,
	[PermanentAddress] [nvarchar](300) NOT NULL,
	[EmailAddress] [nvarchar](150) NOT NULL,
	[ClinicPhoneNo] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[PerDayMaxAppitmnt] [int] NULL,
	[Photo] [nvarchar](300) NULL,
	[AccountTypeID] [int] NOT NULL,
	[AccountNo] [nvarchar](100) NOT NULL,
	[GenderID] [int] NULL,
	[SupportiveDocument] [nvarchar](300) NULL,
 CONSTRAINT [PK_DoctorTable] PRIMARY KEY CLUSTERED 
(
	[DoctorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DoctorTimeSlotTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DoctorTimeSlotTable](
	[DoctorTimeSlotID] [int] IDENTITY(1,1) NOT NULL,
	[DoctorID] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[ToTime] [time](7) NOT NULL,
	[FromTime] [time](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_DoctoTimeSlotTable] PRIMARY KEY CLUSTERED 
(
	[DoctorTimeSlotID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Forum_Answers]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Forum_Answers](
	[AId] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[FK_UserId] [int] NULL,
	[QuestionId] [int] NULL,
	[AnsDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Forum_Questions]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Forum_Questions](
	[QId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[Description] [text] NOT NULL,
	[FK_UserId] [int] NULL,
	[PostDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[QId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GenderTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GenderTable](
	[GenderID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_GenderTable] PRIMARY KEY CLUSTERED 
(
	[GenderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LabAppointTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabAppointTable](
	[LabAppointID] [int] IDENTITY(1,1) NOT NULL,
	[LabTestID] [int] NOT NULL,
	[PatientID] [int] NOT NULL,
	[LabID] [int] NOT NULL,
	[LabTimeSlotID] [int] NOT NULL,
	[AppointDate] [date] NOT NULL,
	[IsFeeSubmit] [bit] NOT NULL,
	[IsComplete] [bit] NOT NULL,
	[Description] [nvarchar](300) NULL,
	[TransectionNo] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_LabAppointTable] PRIMARY KEY CLUSTERED 
(
	[LabAppointID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LabTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabTable](
	[LabID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Name] [nvarchar](300) NOT NULL,
	[Photo] [nvarchar](300) NOT NULL,
	[ContactNo] [nvarchar](20) NOT NULL,
	[PhoneNo] [nvarchar](150) NULL,
	[EmailAddress] [nvarchar](150) NOT NULL,
	[PermanentAddress] [nvarchar](300) NOT NULL,
	[AboutLab] [nvarchar](500) NULL,
	[AccountTypeID] [int] NOT NULL,
	[AccountNo] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_LabTable] PRIMARY KEY CLUSTERED 
(
	[LabID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LabTestDetailsTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabTestDetailsTable](
	[LabTestDetailID] [int] IDENTITY(1,1) NOT NULL,
	[LabTestID] [int] NOT NULL,
	[LabID] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[MinValue] [int] NOT NULL,
	[MaxValue] [int] NOT NULL,
 CONSTRAINT [PK_LabTestDetailsTable] PRIMARY KEY CLUSTERED 
(
	[LabTestDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LabTestTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabTestTable](
	[LabTestID] [int] IDENTITY(1,1) NOT NULL,
	[LabID] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Cost] [float] NOT NULL,
 CONSTRAINT [PK_LabTestTable] PRIMARY KEY CLUSTERED 
(
	[LabTestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LabTimeSlotTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabTimeSlotTable](
	[LabTimeSlotID] [int] IDENTITY(1,1) NOT NULL,
	[LabID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ToTime] [time](7) NOT NULL,
	[FromTime] [time](7) NOT NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_LabTimeSlotTable] PRIMARY KEY CLUSTERED 
(
	[LabTimeSlotID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PatientTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientTable](
	[PatientID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ContactNo] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Description] [nvarchar](500) NULL,
	[Photo] [nvarchar](300) NULL,
	[GenderID] [int] NULL,
 CONSTRAINT [PK_PatientTable] PRIMARY KEY CLUSTERED 
(
	[PatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PatientTestDetailTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientTestDetailTable](
	[LabAppointDetailID] [int] IDENTITY(1,1) NOT NULL,
	[LabAppointID] [int] NOT NULL,
	[LabTestDetailID] [int] NOT NULL,
	[PatientValue] [int] NOT NULL,
 CONSTRAINT [PK_PatientTestDetailTable] PRIMARY KEY CLUSTERED 
(
	[LabAppointDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[quiz_Category]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[quiz_Category](
	[Cat_id] [int] IDENTITY(1,1) NOT NULL,
	[Cat_name] [nvarchar](50) NOT NULL,
	[Cat_fk_DoctorID] [int] NULL,
	[cat_encrypted_string] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Cat_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[quiz_Questions]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[quiz_Questions](
	[q_id] [int] IDENTITY(1,1) NOT NULL,
	[q_text] [nvarchar](max) NOT NULL,
	[QA] [nvarchar](50) NOT NULL,
	[QB] [nvarchar](50) NOT NULL,
	[QC] [nvarchar](50) NOT NULL,
	[QD] [nvarchar](50) NOT NULL,
	[QCorrectAns] [nvarchar](50) NOT NULL,
	[q_fk_Cat_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[q_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[quiz_Result]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[quiz_Result](
	[ExamId] [int] IDENTITY(1,1) NOT NULL,
	[Exam_Date] [datetime] NULL,
	[Exam_fk_stud] [int] NULL,
	[Exam_name] [nvarchar](20) NOT NULL,
	[Patient_Score] [int] NULL,
	[Patient_name] [nvarchar](50) NULL,
	[AttendedQuestions] [nvarchar](max) NULL,
	[TimeTaken] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTable](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserTypeID] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[ContactNo] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsVerified] [bit] NOT NULL,
 CONSTRAINT [PK_UserTable] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserTypeTable]    Script Date: 5/6/2024 9:23:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTypeTable](
	[UserTypeID] [int] IDENTITY(1,1) NOT NULL,
	[UserType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UserTypeTable] PRIMARY KEY CLUSTERED 
(
	[UserTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[DoctorAppointTable] ADD  CONSTRAINT [DF_DoctorAppointTable_IsFeeSubmit]  DEFAULT ((0)) FOR [IsFeeSubmit]
GO
ALTER TABLE [dbo].[DoctorAppointTable] ADD  CONSTRAINT [DF_DoctorAppointTable_IsChecked]  DEFAULT ((0)) FOR [IsChecked]
GO
ALTER TABLE [dbo].[DoctorTimeSlotTable] ADD  CONSTRAINT [DF_DoctoTimeSlotTable_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Forum_Answers] ADD  DEFAULT (getdate()) FOR [AnsDate]
GO
ALTER TABLE [dbo].[Forum_Questions] ADD  DEFAULT (getdate()) FOR [PostDate]
GO
ALTER TABLE [dbo].[LabAppointTable] ADD  CONSTRAINT [DF_LabAppointTable_IsFeeSubmit]  DEFAULT ((0)) FOR [IsFeeSubmit]
GO
ALTER TABLE [dbo].[LabAppointTable] ADD  CONSTRAINT [DF_LabAppointTable_IsComplete]  DEFAULT ((0)) FOR [IsComplete]
GO
ALTER TABLE [dbo].[LabTimeSlotTable] ADD  CONSTRAINT [DF_LabTimeSlotTable_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserTable] ADD  CONSTRAINT [DF_UserTable_IsVerified]  DEFAULT ((1)) FOR [IsVerified]
GO
ALTER TABLE [dbo].[DoctorAppointTable]  WITH CHECK ADD  CONSTRAINT [FK_DoctorAppointTable_DoctorTable] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[DoctorTable] ([DoctorID])
GO
ALTER TABLE [dbo].[DoctorAppointTable] CHECK CONSTRAINT [FK_DoctorAppointTable_DoctorTable]
GO
ALTER TABLE [dbo].[DoctorAppointTable]  WITH CHECK ADD  CONSTRAINT [FK_DoctorAppointTable_DoctorTimeSlotTable] FOREIGN KEY([DoctorTimeSlotID])
REFERENCES [dbo].[DoctorTimeSlotTable] ([DoctorTimeSlotID])
GO
ALTER TABLE [dbo].[DoctorAppointTable] CHECK CONSTRAINT [FK_DoctorAppointTable_DoctorTimeSlotTable]
GO
ALTER TABLE [dbo].[DoctorAppointTable]  WITH CHECK ADD  CONSTRAINT [FK_DoctorAppointTable_PatientTable] FOREIGN KEY([PatientID])
REFERENCES [dbo].[PatientTable] ([PatientID])
GO
ALTER TABLE [dbo].[DoctorAppointTable] CHECK CONSTRAINT [FK_DoctorAppointTable_PatientTable]
GO
ALTER TABLE [dbo].[DoctorTable]  WITH CHECK ADD  CONSTRAINT [FK_DoctorTable_AccountTypeTable] FOREIGN KEY([AccountTypeID])
REFERENCES [dbo].[AccountTypeTable] ([AccountTypeID])
GO
ALTER TABLE [dbo].[DoctorTable] CHECK CONSTRAINT [FK_DoctorTable_AccountTypeTable]
GO
ALTER TABLE [dbo].[DoctorTable]  WITH CHECK ADD  CONSTRAINT [FK_DoctorTable_GenderTable] FOREIGN KEY([GenderID])
REFERENCES [dbo].[GenderTable] ([GenderID])
GO
ALTER TABLE [dbo].[DoctorTable] CHECK CONSTRAINT [FK_DoctorTable_GenderTable]
GO
ALTER TABLE [dbo].[DoctorTable]  WITH CHECK ADD  CONSTRAINT [FK_DoctorTable_UserTable] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserTable] ([UserID])
GO
ALTER TABLE [dbo].[DoctorTable] CHECK CONSTRAINT [FK_DoctorTable_UserTable]
GO
ALTER TABLE [dbo].[DoctorTimeSlotTable]  WITH CHECK ADD  CONSTRAINT [FK_DoctorTimeSlotTable_DoctorTable] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[DoctorTable] ([DoctorID])
GO
ALTER TABLE [dbo].[DoctorTimeSlotTable] CHECK CONSTRAINT [FK_DoctorTimeSlotTable_DoctorTable]
GO
ALTER TABLE [dbo].[Forum_Answers]  WITH CHECK ADD FOREIGN KEY([FK_UserId])
REFERENCES [dbo].[UserTable] ([UserID])
GO
ALTER TABLE [dbo].[Forum_Answers]  WITH CHECK ADD FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Forum_Questions] ([QId])
GO
ALTER TABLE [dbo].[Forum_Questions]  WITH CHECK ADD FOREIGN KEY([FK_UserId])
REFERENCES [dbo].[UserTable] ([UserID])
GO
ALTER TABLE [dbo].[LabAppointTable]  WITH CHECK ADD  CONSTRAINT [FK_LabAppointTable_LabTable] FOREIGN KEY([LabID])
REFERENCES [dbo].[LabTable] ([LabID])
GO
ALTER TABLE [dbo].[LabAppointTable] CHECK CONSTRAINT [FK_LabAppointTable_LabTable]
GO
ALTER TABLE [dbo].[LabAppointTable]  WITH CHECK ADD  CONSTRAINT [FK_LabAppointTable_LabTestTable] FOREIGN KEY([LabTestID])
REFERENCES [dbo].[LabTestTable] ([LabTestID])
GO
ALTER TABLE [dbo].[LabAppointTable] CHECK CONSTRAINT [FK_LabAppointTable_LabTestTable]
GO
ALTER TABLE [dbo].[LabAppointTable]  WITH CHECK ADD  CONSTRAINT [FK_LabAppointTable_LabTimeSlotTable] FOREIGN KEY([LabTimeSlotID])
REFERENCES [dbo].[LabTimeSlotTable] ([LabTimeSlotID])
GO
ALTER TABLE [dbo].[LabAppointTable] CHECK CONSTRAINT [FK_LabAppointTable_LabTimeSlotTable]
GO
ALTER TABLE [dbo].[LabAppointTable]  WITH CHECK ADD  CONSTRAINT [FK_LabAppointTable_PatientTable] FOREIGN KEY([PatientID])
REFERENCES [dbo].[PatientTable] ([PatientID])
GO
ALTER TABLE [dbo].[LabAppointTable] CHECK CONSTRAINT [FK_LabAppointTable_PatientTable]
GO
ALTER TABLE [dbo].[LabTable]  WITH CHECK ADD  CONSTRAINT [FK_LabTable_AccountTypeTable] FOREIGN KEY([AccountTypeID])
REFERENCES [dbo].[AccountTypeTable] ([AccountTypeID])
GO
ALTER TABLE [dbo].[LabTable] CHECK CONSTRAINT [FK_LabTable_AccountTypeTable]
GO
ALTER TABLE [dbo].[LabTable]  WITH CHECK ADD  CONSTRAINT [FK_LabTable_UserTable] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserTable] ([UserID])
GO
ALTER TABLE [dbo].[LabTable] CHECK CONSTRAINT [FK_LabTable_UserTable]
GO
ALTER TABLE [dbo].[LabTestDetailsTable]  WITH CHECK ADD  CONSTRAINT [FK_LabTestDetailsTable_LabTable] FOREIGN KEY([LabID])
REFERENCES [dbo].[LabTable] ([LabID])
GO
ALTER TABLE [dbo].[LabTestDetailsTable] CHECK CONSTRAINT [FK_LabTestDetailsTable_LabTable]
GO
ALTER TABLE [dbo].[LabTestDetailsTable]  WITH CHECK ADD  CONSTRAINT [FK_LabTestDetailsTable_LabTestTable] FOREIGN KEY([LabTestID])
REFERENCES [dbo].[LabTestTable] ([LabTestID])
GO
ALTER TABLE [dbo].[LabTestDetailsTable] CHECK CONSTRAINT [FK_LabTestDetailsTable_LabTestTable]
GO
ALTER TABLE [dbo].[LabTestTable]  WITH CHECK ADD  CONSTRAINT [FK_LabTestTable_LabTable] FOREIGN KEY([LabID])
REFERENCES [dbo].[LabTable] ([LabID])
GO
ALTER TABLE [dbo].[LabTestTable] CHECK CONSTRAINT [FK_LabTestTable_LabTable]
GO
ALTER TABLE [dbo].[LabTimeSlotTable]  WITH CHECK ADD  CONSTRAINT [FK_LabTimeSlotTable_LabTable] FOREIGN KEY([LabID])
REFERENCES [dbo].[LabTable] ([LabID])
GO
ALTER TABLE [dbo].[LabTimeSlotTable] CHECK CONSTRAINT [FK_LabTimeSlotTable_LabTable]
GO
ALTER TABLE [dbo].[PatientTable]  WITH CHECK ADD  CONSTRAINT [FK_PatientTable_GenderTable] FOREIGN KEY([GenderID])
REFERENCES [dbo].[GenderTable] ([GenderID])
GO
ALTER TABLE [dbo].[PatientTable] CHECK CONSTRAINT [FK_PatientTable_GenderTable]
GO
ALTER TABLE [dbo].[PatientTable]  WITH CHECK ADD  CONSTRAINT [FK_PatientTable_UserTable] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserTable] ([UserID])
GO
ALTER TABLE [dbo].[PatientTable] CHECK CONSTRAINT [FK_PatientTable_UserTable]
GO
ALTER TABLE [dbo].[PatientTestDetailTable]  WITH CHECK ADD  CONSTRAINT [FK_PatientTestDetailTable_LabAppointTable] FOREIGN KEY([LabAppointID])
REFERENCES [dbo].[LabAppointTable] ([LabAppointID])
GO
ALTER TABLE [dbo].[PatientTestDetailTable] CHECK CONSTRAINT [FK_PatientTestDetailTable_LabAppointTable]
GO
ALTER TABLE [dbo].[PatientTestDetailTable]  WITH CHECK ADD  CONSTRAINT [FK_PatientTestDetailTable_LabTestDetailsTable] FOREIGN KEY([LabTestDetailID])
REFERENCES [dbo].[LabTestDetailsTable] ([LabTestDetailID])
GO
ALTER TABLE [dbo].[PatientTestDetailTable] CHECK CONSTRAINT [FK_PatientTestDetailTable_LabTestDetailsTable]
GO
ALTER TABLE [dbo].[quiz_Category]  WITH CHECK ADD FOREIGN KEY([Cat_fk_DoctorID])
REFERENCES [dbo].[DoctorTable] ([DoctorID])
GO
ALTER TABLE [dbo].[quiz_Questions]  WITH CHECK ADD FOREIGN KEY([q_fk_Cat_id])
REFERENCES [dbo].[quiz_Category] ([Cat_id])
GO
ALTER TABLE [dbo].[quiz_Result]  WITH CHECK ADD FOREIGN KEY([Exam_fk_stud])
REFERENCES [dbo].[PatientTable] ([PatientID])
GO
ALTER TABLE [dbo].[UserTable]  WITH CHECK ADD  CONSTRAINT [FK_UserTable_UserTypeTable] FOREIGN KEY([UserTypeID])
REFERENCES [dbo].[UserTypeTable] ([UserTypeID])
GO
ALTER TABLE [dbo].[UserTable] CHECK CONSTRAINT [FK_UserTable_UserTypeTable]
GO
USE [master]
GO
ALTER DATABASE [HealthSupportSysdb] SET  READ_WRITE 
GO
