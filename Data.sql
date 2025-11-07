USE EVServiceManagement;
GO

-- XÓA DỮ LIỆU CŨ (chỉ dùng cho môi trường DEV)
DELETE FROM ShiftChangeRequests;
DELETE FROM TechnicianSchedules;
DELETE FROM WorkShifts;
DELETE FROM PartRequests;
DELETE FROM PaymentDetails;
DELETE FROM Payments;
DELETE FROM TechnicianAssignments;
DELETE FROM ServiceOrderDetails;
DELETE FROM Appointments;
DELETE FROM ServiceParts;
DELETE FROM Parts;
DELETE FROM Services;
DELETE FROM Vehicles;
DELETE FROM Technicians;
DELETE FROM Managers;
DELETE FROM Staffs;
DELETE FROM Customers;
DELETE FROM Accounts;
GO

---------------------------------------------------
-- 1️⃣ Tài khoản (Accounts)
---------------------------------------------------
INSERT INTO Accounts (Email, Password, FullName, Phone, Address, Status)
VALUES
-- Manager
('manager1@evcenter.com', '123456', N'Nguyễn Văn Quản', '0901111222', N'Hà Nội', 'Active'),
('manager2@evcenter.com', '123456', N'Lê Thị Quản Lý', '0902222333', N'Hồ Chí Minh', 'Active'),

-- Staff
('staff1@evcenter.com', '123456', N'Trần Thị Lễ Tân', '0903333444', N'Hà Nội', 'Active'),
('staff2@evcenter.com', '123456', N'Phạm Văn Tiếp Nhận', '0904444555', N'Đà Nẵng', 'Active'),

-- Technicians
('tech1@evcenter.com', '123456', N'Lê Văn Kỹ', '0905555666', N'Hà Nội', 'Active'),
('tech2@evcenter.com', '123456', N'Phạm Quốc Cường', '0906666777', N'Hà Nội', 'Active'),
('tech3@evcenter.com', '123456', N'Nguyễn Tuấn Dũng', '0907777888', N'Đà Nẵng', 'Active'),
('tech4@evcenter.com', '123456', N'Lê Hữu Nghĩa', '0908888999', N'Hồ Chí Minh', 'Active'),

-- Customers
('customer1@gmail.com', '123456', N'Nguyễn Minh An', '0911111222', N'Hà Nội', 'Active'),
('customer2@gmail.com', '123456', N'Trần Hồng Mai', '0912222333', N'Hồ Chí Minh', 'Active'),
('customer3@gmail.com', '123456', N'Phạm Đức Thịnh', '0913333444', N'Đà Nẵng', 'Active'),
('customer4@gmail.com', '123456', N'Lê Hoài Nam', '0914444555', N'Hà Nội', 'Active'),
('customer5@gmail.com', '123456', N'Nguyễn Anh Thư', '0915555666', N'Hồ Chí Minh', 'Active');
GO

---------------------------------------------------
-- 2️⃣ Liên kết tài khoản
---------------------------------------------------
INSERT INTO Managers (AccountId) VALUES (1), (2);
INSERT INTO Staffs (AccountId) VALUES (3), (4);
INSERT INTO Technicians (AccountId) VALUES (5), (6), (7), (8);
INSERT INTO Customers (AccountId) VALUES (9), (10), (11), (12), (13);
GO

---------------------------------------------------
-- 3️⃣ Xe điện của khách hàng
---------------------------------------------------
INSERT INTO Vehicles (CustomerId, Model, VIN, LicensePlate, CurrentKm)
VALUES
(1, N'VinFast VF5', 'VF50001', '30A-11111', 12300),
(1, N'Yadea G5', 'YD50002', '29B1-22222', 5200),
(2, N'VinFast Klara S', 'KLAR003', '59C1-33333', 8600),
(3, N'Dat Bike Weaver 200', 'DBW004', '43C1-44444', 10400),
(4, N'VinFast Feliz S', 'VF5005', '30E-55555', 6700),
(5, N'Yadea Xmen Neo', 'YD5006', '59H1-66666', 3100);
GO

---------------------------------------------------
-- 4️⃣ Dịch vụ bảo dưỡng
---------------------------------------------------
INSERT INTO Services (Name, Description, Price, Type, Duration, Km)
VALUES
(N'Kiểm tra pin', N'Kiểm tra dung lượng và tình trạng pin xe điện', 200000, 'Check', 30, 5000),
(N'Thay dầu phanh', N'Thay dầu phanh định kỳ', 150000, 'Replace', 20, 10000),
(N'Ve sinh hệ thống điện', N'Làm sạch đầu nối, dây điện và bộ điều khiển', 100000, 'Clean', 25, 8000),
(N'Thay lốp trước', N'Thay lốp trước xe điện', 350000, 'Replace', 40, 15000),
(N'Thay má phanh sau', N'Thay má phanh và cân chỉnh hệ thống phanh', 280000, 'Replace', 30, 12000),
(N'Kiểm tra động cơ điện', N'Đánh giá hiệu suất động cơ, làm sạch cuộn dây', 250000, 'Check', 45, 10000),
(N'Thay bugi sạc', N'Thay đầu nối sạc hoặc dây dẫn sạc', 180000, 'Replace', 25, 8000);
GO

---------------------------------------------------
-- 5️⃣ Phụ tùng
---------------------------------------------------
INSERT INTO Parts (Name, UnitPrice, StockQuantity)
VALUES
(N'Dầu phanh DOT4', 120000, 40),
(N'Lốp xe 14 inch', 400000, 25),
(N'Má phanh sau', 250000, 30),
(N'Bugi sạc', 150000, 20),
(N'Dây cáp điện', 80000, 50),
(N'Cầu chì 30A', 50000, 70);
GO

---------------------------------------------------
-- 6️⃣ Liên kết Service – Part
---------------------------------------------------
INSERT INTO ServiceParts (ServiceId, PartId)
VALUES
(2, 1),
(4, 2),
(5, 3),
(7, 4);
GO

---------------------------------------------------
-- 7️⃣ Lịch hẹn (Appointments)
---------------------------------------------------
INSERT INTO Appointments (CustomerId, VehicleId, PreferredDate, Status, Notes)
VALUES
(1, 1, '2025-11-05 09:00', 'Completed', N'Kiểm tra pin + thay dầu phanh'),
(2, 3, '2025-11-06 10:30', 'Completed', N'Ve sinh hệ thống điện + thay má phanh'),
(3, 4, '2025-11-06 14:00', 'InProgress', N'Thay lốp trước'),
(4, 5, '2025-11-07 09:30', 'Confirmed', N'Kiểm tra động cơ điện'),
(5, 6, '2025-11-07 15:00', 'Pending', N'Thay bugi sạc'),
(1, 2, '2025-11-08 08:30', 'Pending', N'Kiểm tra pin'),
(3, 4, '2025-11-08 13:00', 'Pending', N'Thay dầu phanh');
GO

---------------------------------------------------
-- 8️⃣ Chi tiết dịch vụ
---------------------------------------------------
INSERT INTO ServiceOrderDetails (AppointmentId, ServiceId, PartId, Quantity, UnitPrice, TotalPrice)
VALUES
(1, 1, NULL, 1, 200000, 200000),
(1, 2, 1, 1, 120000, 120000),
(2, 3, NULL, 1, 100000, 100000),
(2, 5, 3, 1, 250000, 250000),
(3, 4, 2, 1, 400000, 400000),
(4, 6, NULL, 1, 250000, 250000),
(5, 7, 4, 1, 150000, 150000),
(6, 1, NULL, 1, 200000, 200000),
(7, 2, 1, 1, 120000, 120000);
GO

---------------------------------------------------
-- 9️⃣ Phân công kỹ thuật viên
---------------------------------------------------
INSERT INTO TechnicianAssignments (AppointmentId, TechnicianId, Role)
VALUES
(1, 1, 'Main'),
(1, 2, 'Assistant'),
(2, 2, 'Main'),
(3, 3, 'Main'),
(4, 4, 'Main'),
(5, 1, 'Main'),
(6, 2, 'Main'),
(7, 3, 'Main');
GO

---------------------------------------------------
-- 🔟 Thanh toán
---------------------------------------------------
INSERT INTO Payments (AppointmentId, CustomerId, TotalAmount, PaidAmount, PaymentStatus)
VALUES
(1, 1, 320000, 320000, 'Paid'),
(2, 2, 350000, 200000, 'PartiallyPaid'),
(3, 3, 400000, 0, 'Pending'),
(4, 4, 250000, 0, 'Pending'),
(5, 5, 150000, 0, 'Pending'),
(6, 1, 200000, 0, 'Pending'),
(7, 3, 120000, 0, 'Pending');
GO

---------------------------------------------------
-- 11️⃣ Chi tiết thanh toán
---------------------------------------------------
INSERT INTO PaymentDetails (PaymentId, Method, Amount, TransactionCode, Status)
VALUES
(1, 'Cash', 320000, NULL, 'Success'),
(2, 'EWallet', 200000, 'MOMO555888', 'Success');
GO

---------------------------------------------------
-- 12️⃣ Yêu cầu bổ sung phụ tùng
---------------------------------------------------
INSERT INTO PartRequests (RequestedBy, ApprovedBy, PartId, Quantity, Status, Notes)
VALUES
(1, 1, 2, 10, 'Approved', N'Bổ sung lốp xe sắp hết'),
(2, 2, 1, 20, 'Pending', N'Cần dầu phanh cho tuần tới'),
(1, 1, 3, 5, 'Approved', N'Yêu cầu má phanh bổ sung'),
(2, NULL, 4, 10, 'Pending', N'Bugi sạc sắp hết tồn');
GO

---------------------------------------------------
-- 13️⃣ Ca làm việc
---------------------------------------------------
INSERT INTO WorkShifts (Name, StartTime, EndTime)
VALUES
(N'Ca sáng', '07:30', '11:30'),
(N'Ca chiều', '13:00', '17:00'),
(N'Ca tối', '17:30', '21:00');
GO

---------------------------------------------------
-- 14️⃣ Lịch làm việc kỹ thuật viên
---------------------------------------------------
INSERT INTO TechnicianSchedules (TechnicianId, ShiftId, WorkDate, Status)
VALUES
(1, 1, '2025-11-05', 'Completed'),
(2, 2, '2025-11-05', 'Completed'),
(3, 3, '2025-11-05', 'Scheduled'),
(4, 1, '2025-11-06', 'Scheduled'),
(1, 2, '2025-11-06', 'Scheduled'),
(2, 1, '2025-11-06', 'Scheduled'),
(3, 2, '2025-11-07', 'Scheduled'),
(4, 3, '2025-11-07', 'Scheduled');
GO

---------------------------------------------------
-- 15️⃣ Yêu cầu đổi ca
---------------------------------------------------
INSERT INTO ShiftChangeRequests (RequesterId, ReceiverId, ScheduleId, Reason, Status, Notes)
VALUES
(1, 2, 5, N'Có việc đột xuất gia đình', 'Pending', N'Chờ phản hồi'),
(3, 4, 8, N'Đổi để làm ca sớm hơn', 'Approved', N'Đã duyệt đổi ca 7/11');
GO
