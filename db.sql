USE [ThiTracNghiemKhoaCNTT]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Admin](
	[id_admin] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](20) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[avatar] [varchar](255) NULL,
	[name] [nvarchar](50) NOT NULL,
	[id_permission] [int] NOT NULL,
	[last_login] [datetime] NULL,
	[last_seen] [nvarchar](100) NULL,
	[last_seen_url] [varchar](100) NULL,
 CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED 
(
	[id_admin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Class]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[id_class] [int] IDENTITY(1,1) NOT NULL,
	[class_name] [nvarchar](50) NOT NULL,
	[id_speciality] [int] NOT NULL,
	[class_status] [bit] NOT NULL CONSTRAINT [DF_Class_class_status]  DEFAULT ((1)),
 CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED 
(
	[id_class] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Exam_Room]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Exam_Room](
	[id_room] [int] IDENTITY(1,1) NOT NULL,
	[id_thread] [int] NOT NULL,
	[room_name] [nvarchar](50) NOT NULL,
	[room_pass] [varchar](50) NULL,
 CONSTRAINT [PK_Exam_Room] PRIMARY KEY CLUSTERED 
(
	[id_room] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ExamOfThread]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamOfThread](
	[id_thread] [int] NOT NULL,
	[id_exam] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ExamOfTest] PRIMARY KEY CLUSTERED 
(
	[id_exam] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Question]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Question](
	[id_question] [int] IDENTITY(1,1) NOT NULL,
	[id_thematic] [int] NOT NULL,
	[question_content] [nvarchar](500) NOT NULL,
	[img] [varchar](255) NULL,
	[A] [nvarchar](500) NOT NULL,
	[B] [nvarchar](500) NOT NULL,
	[C] [nvarchar](500) NOT NULL,
	[D] [nvarchar](500) NOT NULL,
	[correct_answer] [nvarchar](500) NOT NULL,
	[is_change] [int] NULL CONSTRAINT [DF_Question_is_change]  DEFAULT ((1)),
	[is_essay] [int] NULL CONSTRAINT [DF_Question_is_essay]  DEFAULT ((0)),
 CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED 
(
	[id_question] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QuestionOfExam]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QuestionOfExam](
	[id_exam] [int] NOT NULL,
	[id_question] [int] NOT NULL,
	[img] [varchar](255) NULL,
	[question_content] [nvarchar](500) NOT NULL,
	[B] [nvarchar](500) NOT NULL,
	[C] [nvarchar](500) NOT NULL,
	[D] [nvarchar](500) NOT NULL,
	[A] [nvarchar](500) NOT NULL,
	[correct_answer] [nvarchar](500) NOT NULL,
	[is_essay] [int] NOT NULL CONSTRAINT [DF_QuestionOfExam_is_essay]  DEFAULT ((0))
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Speciality]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Speciality](
	[id_speciality] [int] IDENTITY(1,1) NOT NULL,
	[speciality_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Speciality] PRIMARY KEY CLUSTERED 
(
	[id_speciality] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Student]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Student](
	[id_student] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](50) NULL,
	[student_name] [nvarchar](100) NOT NULL,
	[student_birtday] [date] NULL,
	[student_gender] [bit] NULL CONSTRAINT [DF_Student_student_gender]  DEFAULT ((1)),
	[student_avatar] [varchar](255) NULL CONSTRAINT [DF_Student_student_avatar]  DEFAULT ('/default.jpg'),
	[id_class] [int] NOT NULL,
	[last_login] [datetime] NULL,
	[last_seen] [nvarchar](100) NULL,
	[last_seen_url] [varchar](100) NULL,
	[ip] [varchar](50) NULL,
	[info_browser] [nvarchar](500) NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[id_student] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Student_Thread_Detail]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Thread_Detail](
	[id_student] [int] NOT NULL,
	[id_exam] [int] NOT NULL,
	[id_question] [int] NOT NULL,
	[student_answer] [nvarchar](100) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Student_Thread_Info]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Student_Thread_Info](
	[id_exam] [int] NULL,
	[id_thread] [int] NOT NULL,
	[id_student] [int] NOT NULL,
	[id_room] [int] NOT NULL,
	[start_time] [datetime] NULL,
	[is_complete] [bit] NULL CONSTRAINT [DF_Student_Thread_Info_count]  DEFAULT ((0)),
	[end_time] [datetime] NULL,
	[score_number] [decimal](4, 2) NULL,
	[time_remaing] [varchar](8) NULL,
	[count_tab_focus] [int] NULL CONSTRAINT [DF_Student_Thread_Info_count_tab_focus]  DEFAULT ((0))
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[id_subject] [int] IDENTITY(1,1) NOT NULL,
	[subject_name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Subject] PRIMARY KEY CLUSTERED 
(
	[id_subject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Thematic]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Thematic](
	[id_thematic] [int] IDENTITY(1,1) NOT NULL,
	[id_subject] [int] NOT NULL,
	[thematic_name] [nvarchar](100) NOT NULL CONSTRAINT [DF_Thematic_thematic_name]  DEFAULT (N'Chuyên đề cơ bản'),
 CONSTRAINT [PK_Thematic] PRIMARY KEY CLUSTERED 
(
	[id_thematic] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Thread]    Script Date: 11/01/2020 11:31:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Thread](
	[id_thread] [int] IDENTITY(1,1) NOT NULL,
	[thread_name] [nvarchar](255) NOT NULL,
	[max_question] [int] NOT NULL CONSTRAINT [DF_Thread_max_question]  DEFAULT ((100)),
	[id_subject] [int] NOT NULL,
	[time_to_do] [int] NOT NULL,
	[thread_status] [bit] NOT NULL CONSTRAINT [DF_Thread_thread_status]  DEFAULT ((0)),
	[essay_score] [int] NOT NULL CONSTRAINT [DF_Thread_essay_score]  DEFAULT ((0)),
 CONSTRAINT [PK_Thread] PRIMARY KEY CLUSTERED 
(
	[id_thread] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
