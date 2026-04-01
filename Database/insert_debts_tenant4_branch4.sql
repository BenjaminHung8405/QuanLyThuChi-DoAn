/*
  File: insert_debts_tenant4_branch4.sql
  Mục đích: Chèn dữ liệu công nợ mẫu cho TenantId = 4, BranchId = 4
  Lưu ý: Sao lưu cơ sở dữ liệu trước khi chạy. Chạy trong SSMS hoặc sqlcmd.
*/

SET NOCOUNT ON;

BEGIN TRAN;

-- 1) Kiểm tra công nợ hiện có cho Tenant 4, Branch 4
SELECT DebtId, TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate
FROM Debts
WHERE TenantId = 4 AND BranchId = 4
ORDER BY CreatedDate;

-- 2) Mẫu 1: Nhà cung cấp (PAYABLE)
DECLARE @PartnerId INT;
SELECT @PartnerId = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'NCC Thiết Bị Điện ABC';

IF @PartnerId IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'NCC Thiết Bị Điện ABC', '0909123456', N'789 Đường C, Phường D', 'SUPPLIER', 0);
    SET @PartnerId = SCOPE_IDENTITY();
END

INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES
(4, 4, @PartnerId, 'PAYABLE', 15000000, 0, '2026-04-15', 'PENDING', N'Công nợ thực: Hóa đơn mua linh kiện số HD-202603-001', GETDATE());

-- 3) Mẫu 2: Khách hàng trả nợ cho ta (RECEIVABLE) - một phần đã trả
DECLARE @PartnerId2 INT;
SELECT @PartnerId2 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'KH Thương Mại XYZ';

IF @PartnerId2 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'KH Thương Mại XYZ', '0912345678', N'456 Đường B, Quận 1', 'CUSTOMER', 0);
    SET @PartnerId2 = SCOPE_IDENTITY();
END

INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES
(4, 4, @PartnerId2, 'RECEIVABLE', 25000000, 5000000, '2026-05-01', 'PARTIALLY_PAID', N'Công nợ thực: Bán hàng tháng 03/2026 - hóa đơn BH-202603-045', GETDATE());

-- 4) Mẫu 3: Khách lẻ (RECEIVABLE)
DECLARE @PartnerId3 INT;
SELECT @PartnerId3 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Khách lẻ Tùng';

IF @PartnerId3 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Khách lẻ Tùng', '0987654321', N'12/34 Ngõ E', 'CUSTOMER', 0);
    SET @PartnerId3 = SCOPE_IDENTITY();
END

INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES
(4, 4, @PartnerId3, 'RECEIVABLE', 1200000, 0, '2026-03-31', 'PENDING', N'Công nợ thực: Thanh toán trễ đơn bán #TL-303', GETDATE());

-- 5) Kiểm tra lại dữ liệu mới chèn
SELECT DebtId, TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate
FROM Debts
WHERE TenantId = 4 AND BranchId = 4
ORDER BY CreatedDate DESC;

-- 6) Thêm 10 mẫu công nợ bổ sung (TenantId=4, BranchId=4)
-- Mỗi khối đảm bảo Partner tồn tại trước khi chèn Debt

-- Mẫu bổ sung 1..10
DECLARE @PartnerId4 INT;
SELECT @PartnerId4 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Công ty TNHH Sản Xuất A';
IF @PartnerId4 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Công ty TNHH Sản Xuất A', '0281234001', N'10 Lê Lợi, Quận 1', 'SUPPLIER', 0);
    SET @PartnerId4 = SCOPE_IDENTITY();
END
INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES (4,4,@PartnerId4,'PAYABLE',8000000,0,'2026-04-20','PENDING',N'Công nợ thực: Mua vật tư A - HĐ MA-001',GETDATE());

DECLARE @PartnerId5 INT;
SELECT @PartnerId5 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Công ty TNHH Thương Mại B';
IF @PartnerId5 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Công ty TNHH Thương Mại B', '0281234002', N'22 Nguyễn Huệ, Quận 1', 'SUPPLIER', 0);
    SET @PartnerId5 = SCOPE_IDENTITY();
END
INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES (4,4,@PartnerId5,'PAYABLE',5000000,1000000,'2026-04-25','PARTIALLY_PAID',N'Công nợ thực: Hóa đơn mua B - HĐ MB-010',GETDATE());

DECLARE @PartnerId6 INT;
SELECT @PartnerId6 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Khách hàng C';
IF @PartnerId6 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Khách hàng C', '0903000003', N'78 Phố X, P.Y', 'CUSTOMER', 0);
    SET @PartnerId6 = SCOPE_IDENTITY();
END
INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES (4,4,@PartnerId6,'RECEIVABLE',12000000,2000000,'2026-05-10','PARTIALLY_PAID',N'Công nợ thực: Bán hàng C - ĐH C-101',GETDATE());

DECLARE @PartnerId7 INT;
SELECT @PartnerId7 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Khách hàng D';
IF @PartnerId7 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Khách hàng D', '0903000004', N'90 Đường Z', 'CUSTOMER', 0);
    SET @PartnerId7 = SCOPE_IDENTITY();
END
INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES (4,4,@PartnerId7,'RECEIVABLE',3000000,0,'2026-04-05','PENDING',N'Công nợ thực: Đơn bán D - SL 10',GETDATE());

DECLARE @PartnerId8 INT;
SELECT @PartnerId8 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Cửa hàng E';
IF @PartnerId8 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Cửa hàng E', '0911222333', N'12 Hẻm F', 'CUSTOMER', 0);
    SET @PartnerId8 = SCOPE_IDENTITY();
END
INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES (4,4,@PartnerId8,'RECEIVABLE',4500000,4500000,'2026-03-30','PAID',N'Công nợ thực: Thanh toán đầy đủ - HĐ E-200',GETDATE());

DECLARE @PartnerId9 INT;
SELECT @PartnerId9 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Nhà cung cấp F';
IF @PartnerId9 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Nhà cung cấp F', '0281234009', N'55 Đại Lộ G', 'SUPPLIER', 0);
    SET @PartnerId9 = SCOPE_IDENTITY();
END
INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES (4,4,@PartnerId9,'PAYABLE',20000000,5000000,'2026-06-01','PARTIALLY_PAID',N'Công nợ thực: Hợp đồng cung cấp F - HD-F-55',GETDATE());

DECLARE @PartnerId10 INT;
SELECT @PartnerId10 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Khách lẻ G';
IF @PartnerId10 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Khách lẻ G', '0988000010', N'3/21 Ngõ H', 'CUSTOMER', 0);
    SET @PartnerId10 = SCOPE_IDENTITY();
END
INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES (4,4,@PartnerId10,'RECEIVABLE',600000,0,'2026-04-01','PENDING',N'Công nợ thực: Hóa đơn bán lẻ G',GETDATE());

DECLARE @PartnerId11 INT;
SELECT @PartnerId11 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Công ty Hợp Tác H';
IF @PartnerId11 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Công ty Hợp Tác H', '0281234011', N'1 Công Trường I', 'SUPPLIER', 0);
    SET @PartnerId11 = SCOPE_IDENTITY();
END
INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES (4,4,@PartnerId11,'PAYABLE',10000000,2000000,'2026-05-15','PARTIALLY_PAID',N'Công nợ thực: Thanh toán theo hợp đồng H-77',GETDATE());

DECLARE @PartnerId12 INT;
SELECT @PartnerId12 = PartnerId FROM Partners WHERE TenantId = 4 AND PartnerName = N'Khách hàng I';
IF @PartnerId12 IS NULL
BEGIN
    INSERT INTO Partners (TenantId, PartnerName, Phone, Address, Type, InitialDebt)
    VALUES (4, N'Khách hàng I', '0909000012', N'100 Đường J', 'CUSTOMER', 0);
    SET @PartnerId12 = SCOPE_IDENTITY();
END
INSERT INTO Debts (TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate)
VALUES (4,4,@PartnerId12,'RECEIVABLE',7200000,1200000,'2026-05-20','PARTIALLY_PAID',N'Công nợ thực: Đơn hàng I-900',GETDATE());

-- Kiểm tra lại toàn bộ dữ liệu mới chèn
SELECT DebtId, TenantId, BranchId, PartnerId, DebtType, TotalAmount, PaidAmount, DueDate, Status, Notes, CreatedDate
FROM Debts
WHERE TenantId = 4 AND BranchId = 4
ORDER BY CreatedDate DESC;

COMMIT;

-- Nếu muốn rollback khi có lỗi: thay COMMIT bằng ROLLBACK

/*
Hướng dẫn nhanh:
- Mở file này trong SSMS.
- Chạy toàn bộ file để chèn các Partner (nếu chưa có) và 13 mẫu công nợ tổng cộng.
- Hoặc chạy từng khối nếu muốn kiểm tra bước từng bước.
*/
