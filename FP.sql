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
    [PreferredDate] DATETIME NOT NULL,
    [Status] VARCHAR(20) NOT NULL DEFAULT 'Pending', -- Pending, Confirmed, InProgress, Completed, Cancelled
    [Notes] NVARCHAR(500),
);

CREATE TABLE [ServiceOrderDetails] (
	[OrderDetailId] INT IDENTITY PRIMARY KEY,
	[AppointmentId] INT FOREIGN KEY REFERENCES [Appointments]([AppointmentId]),
	[ServiceId] INT NULL FOREIGN KEY REFERENCES [Services]([ServiceId]), 
	[PartId] INT NULL FOREIGN KEY REFERENCES [Parts]([PartId]), 
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

CREATE TABLE Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentId INT NOT NULL FOREIGN KEY REFERENCES Appointments(AppointmentId),
    CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customers(CustomerId),
    TotalAmount DECIMAL(12,2) NOT NULL,      -- Tổng cần thanh toán
    PaidAmount DECIMAL(12,2) DEFAULT 0,      -- Tổng đã thanh toán (cộng dồn từ PaymentDetails)
    RemainingAmount AS (TotalAmount - PaidAmount), -- Cột computed tự động
    PaymentStatus VARCHAR(20) NOT NULL CHECK (PaymentStatus IN ('Pending', 'PartiallyPaid', 'Paid', 'Refunded', 'Cancelled')),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE PaymentDetails (
    PaymentDetailId INT IDENTITY(1,1) PRIMARY KEY,
    PaymentId INT NOT NULL FOREIGN KEY REFERENCES Payments(PaymentId),
    Method VARCHAR(30) NOT NULL CHECK (Method IN ('Cash', 'BankTransfer', 'EWallet', 'CreditCard', 'QR', 'Other')),
    Amount DECIMAL(12,2) NOT NULL,
    TransactionCode NVARCHAR(100) NULL,      -- Mã giao dịch online (nếu có)
    TransactionDate DATETIME DEFAULT GETDATE(),
    Status VARCHAR(20) NOT NULL CHECK (Status IN ('Pending', 'Success', 'Failed', 'Refunded')),
    Note NVARCHAR(255) NULL
);

CREATE TABLE PartRequests (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    RequestedBy INT NOT NULL FOREIGN KEY REFERENCES Staffs(StaffId),
    ApprovedBy INT NULL FOREIGN KEY REFERENCES Managers(ManagerId),
	[PartId] INT NULL FOREIGN KEY REFERENCES [Parts]([PartId]), 
	[Quantity] INT NOT NULL,
    RequestDate DATETIME DEFAULT GETDATE(),
    ApprovalDate DATETIME NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending', 
    Notes NVARCHAR(500) NULL
);

CREATE TABLE WorkShifts (
    ShiftId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,       -- Ví dụ: "Ca sáng", "Ca chiều", "Ca tối"
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    IsActive BIT DEFAULT 1
);

CREATE TABLE TechnicianSchedules (
    ScheduleId INT IDENTITY(1,1) PRIMARY KEY,
    TechnicianId INT NOT NULL FOREIGN KEY REFERENCES Technicians(TechnicianId),
    ShiftId INT NOT NULL FOREIGN KEY REFERENCES WorkShifts(ShiftId),
    WorkDate DATE NOT NULL,
    Status VARCHAR(20) DEFAULT 'Scheduled',  -- Scheduled / Completed / Absent / Changed
    Notes NVARCHAR(255)
);

CREATE TABLE ShiftChangeRequests (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    RequesterId INT NOT NULL FOREIGN KEY REFERENCES Technicians(TechnicianId),  -- Người yêu cầu đổi
    ReceiverId INT NOT NULL FOREIGN KEY REFERENCES Technicians(TechnicianId),   -- Người được đề nghị đổi
    ScheduleId INT NOT NULL FOREIGN KEY REFERENCES TechnicianSchedules(ScheduleId), -- Ca làm muốn đổi
    RequestedDate DATETIME DEFAULT GETDATE(),
    Reason NVARCHAR(500) NULL,
    Status VARCHAR(20) DEFAULT 'Pending', -- Pending / Approved / Rejected / Cancelled
    ApprovedDate DATETIME NULL,
    Notes NVARCHAR(255)
);

