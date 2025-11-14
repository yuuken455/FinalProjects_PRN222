🚗 EV Service Center Maintenance Management System
Nhóm thực hiện:nhóm 6
Môn học:PRN_222
🔥 1. Giới thiệu ngắn

Hệ thống hỗ trợ trung tâm bảo dưỡng xe điện trong 3 nghiệp vụ chính:

Dashboard báo cáo cho Manager

Quản lý yêu cầu linh kiện (Manager & Staff)

Đặt lịch hẹn bảo dưỡng (Customer → Staff → Manager)

Dự án sử dụng ASP.NET Core Razor Pages, EF Core, SQL Server, SignalR.

📌 2. Các chức năng nhóm đã hoàn thành
✅ 1. Dashboard Manager

Hiển thị thống kê:

Số lịch hẹn hôm nay

Số linh kiện tồn kho < 5

Số khách hàng hoạt động

Doanh thu tháng hiện tại

Biểu đồ cột: doanh thu theo tháng

Biểu đồ tròn: tỷ lệ trạng thái lịch hẹn

Dữ liệu lấy từ Appointments, Payments, Customers, Parts.

✅ 2. Quản lý yêu cầu linh kiện (Part Requests)

Staff:

Tạo yêu cầu linh kiện

Xem danh sách yêu cầu

Xác nhận nhận hàng → kho tự động tăng

Manager:

Xem tất cả yêu cầu

Phê duyệt / Từ chối

SignalR realtime:

Manager nhận thông báo khi Staff nhận hàng

Trang Manager cập nhật tồn kho tự động (không cần refresh)

✅ 3. Đặt lịch hẹn bảo dưỡng

Customer đặt lịch: chọn xe → chọn dịch vụ → chọn ngày → gửi yêu cầu
Staff xử lý: xác nhận lịch, thay đổi trạng thái


🛠️ 3. Công nghệ sử dụng

.NET 8 Razor Pages

Entity Framework Core

SQL Server

SignalR

Bootstrap 5 + Chart.js

▶️ 4. Cách chạy dự án

Import file SQL vào SQL Server

Chỉnh ConnectionStrings trong appsettings.json

Chạy dự án bằng Visual Studio hoặc dotnet run

