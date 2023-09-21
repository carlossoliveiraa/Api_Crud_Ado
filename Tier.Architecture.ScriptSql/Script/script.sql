USE [Estudos]
GO
/****** Object:  Table [dbo].[Empresas]    Script Date: 21/09/2023 20:42:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
	[Cnpj] [varchar](15) NULL,
	[InscricaoEstadual] [varchar](20) NULL,
	[DataAbertura] [date] NULL,
	[Site] [varchar](1000) NULL,
	[Email] [varchar](50) NULL,
	[Telefone] [varchar](20) NULL,
	[Ativo] [bit] NULL,
 CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enderecos]    Script Date: 21/09/2023 20:42:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enderecos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaId] [int] NULL,
	[Logradouro] [varchar](50) NULL,
	[Numero] [varchar](10) NULL,
	[Bairro] [varchar](50) NULL,
	[Cidade] [varchar](50) NULL,
	[UF] [varchar](2) NULL,
	[Ativo] [bit] NULL,
 CONSTRAINT [PK_Endereco] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Empresas] ON 
GO
INSERT [dbo].[Empresas] ([Id], [Nome], [Cnpj], [InscricaoEstadual], [DataAbertura], [Site], [Email], [Telefone], [Ativo]) VALUES (1, N'Controle Simples', N'72669003000123', N'7177079132699', CAST(N'2018-05-08' AS Date), N'www.controlesimples.com.br', N'atendimento@controlesimples.com.br', N'3532927282', 1)
GO
INSERT [dbo].[Empresas] ([Id], [Nome], [Cnpj], [InscricaoEstadual], [DataAbertura], [Site], [Email], [Telefone], [Ativo]) VALUES (2, N'UseMinas', N'123456789', N'1045', CAST(N'2023-09-21' AS Date), N'www.uol.com.br', N'string', N'string', 1)
GO
SET IDENTITY_INSERT [dbo].[Empresas] OFF
GO
SET IDENTITY_INSERT [dbo].[Enderecos] ON 
GO
INSERT [dbo].[Enderecos] ([Id], [EmpresaId], [Logradouro], [Numero], [Bairro], [Cidade], [UF], [Ativo]) VALUES (4, 1, N'Rua Tapajos', N'20', N'Centro', N'Alfenas', N'MG', 1)
GO
INSERT [dbo].[Enderecos] ([Id], [EmpresaId], [Logradouro], [Numero], [Bairro], [Cidade], [UF], [Ativo]) VALUES (5, 1, N'Rua Guarany', N'100', N'Jardim Eucalipto', N'Rio Janeiro', N'RJ', 1)
GO
INSERT [dbo].[Enderecos] ([Id], [EmpresaId], [Logradouro], [Numero], [Bairro], [Cidade], [UF], [Ativo]) VALUES (6, 2, N'RUa T', N'22', N'VCF', N'CCC', N'AM', 1)
GO
SET IDENTITY_INSERT [dbo].[Enderecos] OFF
GO
