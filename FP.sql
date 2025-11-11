CREATE DATABASE EVServiceManagement;
GO

USE EVServiceManagement;
GO

CREATE TABLE [Accounts] (
	[AccountId] INT IDENTITY(1,1) PRIMARY KEY,
	[Email] VARCHAR(40) NOT NULL,
	[Password] VARCHAR(40) NOT NULL,
	[FullName] NVARCHAR(150) NOT NULL,
	[Phone] VARCHAR(12) NOT NULL,
	[Address] NVARCHAR(250),
	[Status] VARCHAR(10) NOT NULL DEFAULT 'Active'
);

CREATE TABLE [Customers] (
	[CustomerId] INT IDENTITY(1,1) PRIMARY KEY,
	[AccountId] INT NOT NULL UNIQUE,
	FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([AccountId])
);

CREATE TABLE [Staffs] (
	[StaffId] INT IDENTITY(1,1) PRIMARY KEY,
	[AccountId] INT NOT NULL UNIQUE,
	FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([AccountId])
);

CREATE TABLE [Managers] (
	[ManagerId] INT IDENTITY(1,1) PRIMARY KEY,
	[AccountId] INT NOT NULL UNIQUE,
	FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([AccountId])
);

CREATE TABLE [Technicians] (
	[TechnicianId] INT IDENTITY(1,1) PRIMARY KEY,
	[AccountId] INT NOT NULL UNIQUE,
	FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([AccountId])
);

CREATE TABLE [Vehicles] (
	[VehicleId] INT IDENTITY(1,1) PRIMARY KEY,
	[CustomerId] INT NOT NULL,
	[Model] NVARCHAR(250) NOT NULL,
	[VIN] VARCHAR(20),
	[LicensePlate] VARCHAR(20) NOT NULL,
	[CurrentKm] DECIMAL(10,2),
	[Status] VARCHAR(20) NOT NULL DEFAULT 'Active',
	FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([CustomerId])
);

CREATE TABLE [Services] (
	[ServiceId] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(150) NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[Price] INT NOT NULL,
	[Type] VARCHAR(20) NOT NULL CHECK ([Type] IN ('Check', 'Replace', 'Clean')),
	[Duration] INT NOT NULL,
	[Km] DECIMAL(10,2),
	[Status] VARCHAR(20) NOT NULL DEFAULT 'Active'
);

CREATE TABLE [Parts] (
	[PartId] INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(150) NOT NULL,
	[UnitPrice] DECIMAL(10,2) NOT NULL,
	[StockQuantity] INT NOT NULL,
	[Status] VARCHAR(20) NOT NULL DEFAULT 'Active'
);

CREATE TABLE [ServiceParts] (
	[ServicePartId] INT IDENTITY PRIMARY KEY,
	[ServiceId] INT FOREIGN KEY REFERENCES [Services]([ServiceId]),
	[PartId] INT FOREIGN KEY REFERENCES [Parts]([PartId])
);

CREATE TABLE [Appointments] (
    [AppointmentId] INT IDENTITY(1,1) PRIMARY KEY,
    [CustomerId] INT NOT NULL FOREIGN KEY REFERENCES [Customers]([CustomerId]),
    [VehicleId] INT NOT NULL FOREIGN KEY REFERENCES [Vehicles]([VehicleId]),
    [Date] DATETIME NOT NULL,
    [Status] VARCHAR(20) NOT NULL DEFAULT 'Pending', 
    [Notes] NVARCHAR(500),
);

CREATE TABLE [ServiceReviews] (
    [ReviewId] INT IDENTITY(1,1) PRIMARY KEY,
    [AppointmentId] INT NOT NULL FOREIGN KEY REFERENCES [Appointments]([AppointmentId]),
    [CustomerId] INT NOT NULL FOREIGN KEY REFERENCES [Customers]([CustomerId]),
    [Rating] INT NOT NULL,
    [Comment] NVARCHAR(1000),
    [CreatedAt] DATETIME DEFAULT GETDATE()
);

CREATE TABLE [ServiceOrderDetails] (
	[OrderDetailId] INT IDENTITY PRIMARY KEY,
	[AppointmentId] INT FOREIGN KEY REFERENCES [Appointments]([AppointmentId]),
	[ServiceId] INT NULL FOREIGN KEY REFERENCES [Services]([ServiceId]),  
	[Quantity] INT NOT NULL,
	[UnitPrice] DECIMAL(10,2) NOT NULL, 
	[TotalPrice] DECIMAL(10,2) NOT NULL
);

CREATE TABLE [TechnicianAssignments] (
    [AssignmentId] INT IDENTITY(1,1) PRIMARY KEY,
    [AppointmentId] INT NOT NULL FOREIGN KEY REFERENCES [Appointments]([AppointmentId]),
    [TechnicianId] INT NOT NULL FOREIGN KEY REFERENCES [Technicians]([TechnicianId]),
    [AssignedAt] DATETIME DEFAULT GETDATE(),
    [Role] VARCHAR(50) DEFAULT 'Main', -- Main / Assistant
);

CREATE TABLE [Payments] (
    [PaymentId] INT IDENTITY(1,1) PRIMARY KEY,
    [AppointmentId] INT NOT NULL UNIQUE FOREIGN KEY REFERENCES [Appointments]([AppointmentId]),
    [CustomerId] INT NOT NULL FOREIGN KEY REFERENCES [Customers]([CustomerId]),
    [TotalAmount] DECIMAL(12,2) NOT NULL,     
    [PaidAmount] DECIMAL(12,2) DEFAULT 0,     
    [RemainingAmount] AS (TotalAmount - PaidAmount), 
    [Status] VARCHAR(20) NOT NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [UpdatedAt] DATETIME DEFAULT GETDATE()
);

CREATE TABLE [PaymentDetails] (
    [PaymentDetailId] INT IDENTITY(1,1) PRIMARY KEY,
    [PaymentId] INT NOT NULL FOREIGN KEY REFERENCES [Payments]([PaymentId]),
    [Method] VARCHAR(30) NOT NULL,
    [Amount] DECIMAL(12,2) NOT NULL,
    [TransactionCode] NVARCHAR(100) NULL,     
    [TransactionDate] DATETIME DEFAULT GETDATE(),
    [Status] VARCHAR(20) NOT NULL CHECK (Status IN ('Pending', 'Success', 'Failed', 'Refunded')),
    [Note] NVARCHAR(255) NULL
);

CREATE TABLE [PartRequests] (
    [RequestId] INT IDENTITY(1,1) PRIMARY KEY,
    [RequestedBy] INT NOT NULL FOREIGN KEY REFERENCES [Staffs]([StaffId]),
    [ApprovedBy] INT NULL FOREIGN KEY REFERENCES [Managers]([ManagerId]),
	[PartId] INT NULL FOREIGN KEY REFERENCES [Parts]([PartId]), 
	[Quantity] INT NOT NULL,
    [RequestDate] DATETIME DEFAULT GETDATE(),
    [ApprovalDate] DATETIME NULL,
    [Status] VARCHAR(20) NOT NULL DEFAULT 'Pending', 
    [Notes] NVARCHAR(500) NULL
);

-- ====== DỮ LIỆU MẪU CHO EVServiceManagement ======

-- Accounts
INSERT INTO Accounts (Email, Password, FullName, Phone, Address, Status) VALUES
('alice@gmail.com', '123456', N'Alice Nguyễn', '0901000001', N'123 Lê Lợi, Q1, TP.HCM', 'Active'),
('bob@gmail.com', '123456', N'Bob Trần', '0901000002', N'25 Nguyễn Huệ, Q1, TP.HCM', 'Active'),
('charlie@gmail.com', '123456', N'Charlie Phạm', '0901000003', N'89 Trần Hưng Đạo, Q5', 'Active'),
('david@gmail.com', '123456', N'David Lê', '0901000004', N'02 Nguyễn Văn Cừ, Q5', 'Active'),
('eva@gmail.com', '123456', N'Eva Phan', '0901000005', N'56 Hai Bà Trưng, Q3', 'Active'),
('staff1@gmail.com', '123456', N'Nhân viên 1', '0902000001', N'12 Pasteur, Q1', 'Active'),
('staff2@gmail.com', '123456', N'Nhân viên 2', '0902000002', N'15 Võ Văn Kiệt, Q5', 'Active'),
('manager1@gmail.com', '123456', N'Quản lý 1', '0903000001', N'10 Nguyễn Du, Q1', 'Active'),
('technician1@gmail.com', '123456', N'KTV 1', '0904000001', N'15 Điện Biên Phủ, Q10', 'Active'),
('technician2@gmail.com', '123456', N'KTV 2', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician3@gmail.com', '123456', N'KTV 3', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician4@gmail.com', '123456', N'KTV 4', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician5@gmail.com', '123456', N'KTV 5', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician6@gmail.com', '123456', N'KTV 6', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician7@gmail.com', '123456', N'KTV 7', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician8@gmail.com', '123456', N'KTV 8', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician9@gmail.com', '123456', N'KTV 9', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician10@gmail.com', '123456', N'KTV 10', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician11@gmail.com', '123456', N'KTV 11', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active'),
('technician12@gmail.com', '123456', N'KTV 12', '0904000002', N'17 Lý Thường Kiệt, Q10', 'Active');

-- Customers (3 người đầu là khách)
INSERT INTO Customers (AccountId) VALUES (1), (2), (3), (4), (5);

-- Staffs (2 người kế tiếp)
INSERT INTO Staffs (AccountId) VALUES (6), (7);

-- Managers
INSERT INTO Managers (AccountId) VALUES (8);

-- Technicians
INSERT INTO Technicians (AccountId) VALUES (9), (10), (11), (12), (13), (14), (15), (16), (17), (18), (19), (20);

-- Vehicles
INSERT INTO Vehicles (CustomerId, Model, VIN, LicensePlate, CurrentKm, Status) VALUES
(1, N'VinFast Feliz', 'VF001', '59A1-00111', 4500, 'Active'),
(2, N'VinFast Klara S', 'VF002', '59A1-00222', 7200, 'Active'),
(3, N'YADEA G5', 'YD003', '59A1-00333', 11000, 'Active');

-- Services
INSERT INTO Services (Name, Description, Price, Type, Duration, Km, Status) VALUES
(N'Kiểm tra tổng quát', N'Kiểm tra toàn bộ xe và báo cáo tình trạng.', 150000, 'Check', 30, NULL, 'Active'),
(N'Thay pin', N'Thay pin xe điện chính hãng.', 2500000, 'Replace', 120, NULL, 'Active'),
(N'Vệ sinh phanh', N'Làm sạch và bôi trơn hệ thống phanh.', 200000, 'Clean', 60, NULL, 'Active'),
(N'Thay má phanh', N'Thay mới má phanh xe.', 350000, 'Replace', 90, NULL, 'Active'),
(N'Bảo dưỡng động cơ điện', N'Kiểm tra và bảo dưỡng mô-tơ xe điện.', 400000, 'Check', 120, NULL, 'Active');

-- Parts
INSERT INTO Parts (Name, UnitPrice, StockQuantity, Status) VALUES
(N'Pin lithium-ion 60V', 2000000, 10, 'Active'),
(N'Má phanh trước', 150000, 25, 'Active'),
(N'Má phanh sau', 150000, 30, 'Active'),
(N'Dầu bôi trơn phanh', 80000, 50, 'Active'),
(N'Lốp xe điện', 300000, 20, 'Active');

-- ServiceParts (gắn linh kiện với dịch vụ)
INSERT INTO ServiceParts (ServiceId, PartId) VALUES
(2, 1),
(3, 4),
(4, 2),
(4, 3),
(5, 5);

-- Appointments
INSERT INTO Appointments (CustomerId, VehicleId, Date, Status, Notes) VALUES
(1, 1, '2025-11-10 09:00:00', 'Completed', N'Khách bảo dưỡng định kỳ'),
(2, 2, '2025-11-11 10:00:00', 'InProgress', N'Xe có tiếng ồn khi chạy'),
(3, 3, '2025-11-12 14:30:00', 'Pending', N'Yêu cầu kiểm tra pin');

-- ServiceOrderDetails
INSERT INTO ServiceOrderDetails (AppointmentId, ServiceId, Quantity, UnitPrice, TotalPrice) VALUES
(1, 1, 1, 150000, 150000),
(1, 3, 1, 200000, 200000),
(2, 5, 1, 400000, 400000),
(3, 2, 1, 2500000, 2500000);

-- TechnicianAssignments
INSERT INTO TechnicianAssignments (AppointmentId, TechnicianId, AssignedAt, Role) VALUES
(1, 1, '2025-11-09 15:00:00', 'Main'),
(2, 2, '2025-11-10 08:00:00', 'Main'),
(3, 1, '2025-11-11 13:00:00', 'Main');

-- Payments
INSERT INTO Payments (AppointmentId, CustomerId, TotalAmount, PaidAmount, Status) VALUES
(1, 1, 350000, 350000, 'Paid'),
(2, 2, 400000, 0, 'Pending'),
(3, 3, 2500000, 0, 'Pending');

-- PaymentDetails
INSERT INTO PaymentDetails (PaymentId, Method, Amount, TransactionCode, Status, Note) VALUES
(1, 'Cash', 350000, 'TXN001', 'Success', N'Thanh toán tại quầy'),
(2, 'Card', 0, NULL, 'Pending', NULL),
(3, 'Online', 0, NULL, 'Pending', NULL);

-- ServiceReviews
INSERT INTO ServiceReviews (AppointmentId, CustomerId, Rating, Comment) VALUES
(1, 1, 5, N'Dịch vụ nhanh và chuyên nghiệp!'),
(2, 2, 4, N'Nhân viên thân thiện, xe chạy êm hơn.'),
(3, 3, 0, N'Chưa thực hiện dịch vụ.');

-- PartRequests
INSERT INTO PartRequests (RequestedBy, ApprovedBy, PartId, Quantity, RequestDate, ApprovalDate, Status, Notes) VALUES
(1, 1, 2, 10, '2025-11-09', '2025-11-10', 'Approved', N'Bổ sung kho má phanh'),
(2, 1, 1, 5, '2025-11-10', NULL, 'Pending', N'Yêu cầu pin mới cho khách.');

