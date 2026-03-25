
# 🔐 Sprint 3: Phân hệ Đăng nhập & Điều hướng
## (Authentication & Navigation)

**Trạng thái:** ✅ Hoàn thành  
**Ngày hoàn thành:** 25/03/2026  
**Thời gian:** ~2 tuần

---

## 🎯 Mục tiêu Sprint

- ✅ Thiết kế màn hình Đăng nhập chuyên nghiệp với xử lý phản hồi người dùng
- ✅ Triển khai cơ chế xác thực bảo mật với mật khẩu băm (BCrypt)
- ✅ Xây dựng khung giao diện chính (Main Dashboard) hỗ trợ nạp động UserControl
- ✅ Thiết lập hệ thống phân quyền dựa trên Role (Role-based Access Control)
- ✅ Bảo vệ tầng sâu (Deep Security) ở BLL

---

## 🏗 Kiến trúc & Thành phần chính

### 1. Màn hình Đăng nhập (`frmLogin`)

#### **Thiết kế UI**
- Kích thước: 400x550px
- Bộ nhận diện màu: Emerald Green (#2E7D32)
- Font: Segoe UI
- Hiệu ứng: Rounded corners, shadow effects

#### **Xử lý Xác thực**
```
Luồng: Nhập username/password 
       → Kiểm tra rỗng 
       → Gọi UserService.Authenticate() 
       → BCrypt so khớp mật khẩu 
       → Set SessionManager 
       → Mở frmMain
```

**Chi tiết xử lý:**
- Gọi `UserService.Authenticate(username, password)`
- Xác thực mật khẩu với `BCrypt.Net.BCrypt.Verify()`
- Hiển thị lỗi chi tiết khi đăng nhập thất bại
- Tự động focus vào ô mật khẩu nếu tài khoản sai
- Xóa ô mật khẩu sau khi đăng nhập thất bại

#### **Tính năng Remember Me**
```csharp
// Load Form: Nạp dữ liệu cũ
if (Properties.Settings.Default.isRemembered)
{
    txtUsername.Text = Properties.Settings.Default.userSaved;
    chkRememberMe.Checked = true;
}

// Đăng nhập thành công: Lưu dữ liệu mới
if (chkRememberMe.Checked)
{
    Properties.Settings.Default.userSaved = username;
    Properties.Settings.Default.isRemembered = true;
}
Properties.Settings.Default.Save();
```

**Đặc điểm:**
- Sử dụng `Properties.Settings` (User Scope) để lưu cục bộ
- Lần tiếp theo tự động điền username
- Người dùng có thể tắt bất cứ lúc nào

#### **Hỗ trợ Phím tắt**
- `Enter`: Kích hoạt đăng nhập
- `Esc`: Thoát ứng dụng với xác nhận

---

### 2. Khung Điều hướng Chính (`frmMain`)

#### **Kiến trúc Layout**
```
┌─────────────────────────────────────┐
│        MenuStrip (Top)              │
├─────────────────────────────────────┤
│                                     │
│    pnlContent (Main Content Area)   │
│    - UserControl được nạp động      │
│    - Giải phóng cũ, load mới        │
│                                     │
├─────────────────────────────────────┤
│  StatusStrip (Bottom)               │
│  - User info, Role, Branch          │
└─────────────────────────────────────┘
```

#### **Cơ chế Nạp động UserControl**
```csharp
private void ShowUserControl(UserControl uc)
{
    pnlContent.Controls.Clear();      // Giải phóng cũ
    uc.Dock = DockStyle.Fill;
    pnlContent.Controls.Add(uc);      // Load mới
}
```

**Lợi ích:**
- Ứng dụng chạy mượt mà, không "nháy" Form
- Tiết kiệm bộ nhớ khi chuyển đổi màn hình
- Giữ state riêng lẻ cho từng UserControl

#### **Menu Structure (Naming Convention)**

| Menu | Items |
|------|-------|
| **System** (Hệ thống) | Change Password, Manage Users, Branch Config, Logout |
| **Catalog** (Danh mục) | Partners, Cash Funds, Transaction Categories |
| **Transactions** (Nghiệp vụ) | Receipt Voucher, Payment Voucher, Debt Management, Internal Transfer |
| **Reports** (Báo cáo) | Cash Ledger, Report by Period, Debt Summary |

**Variable Naming:**
```csharp
mnuSystem, mnuChangePassword, mnuManageUsers
mnuBranchConfig, mnuLogout
mnuCatalog, mnuPartners, mnuCashFunds
mnuTransactions, mnuReceiptVoucher, mnuDebtSummary
mnuReports
```

#### **StatusStrip Information**
```
"Người dùng: John Doe [SuperAdmin] | Chi nhánh: 1"
```
- Cập nhật tự động từ `SessionManager` sau khi đăng nhập
- Hiển thị role của người dùng
- Hiển thị chi nhánh đang làm việc

---

### 3. Cơ chế Phân quyền & Session

#### **SessionManager (Static Class)**
```csharp
public static class SessionManager
{
    public static int UserId { get; set; }
    public static string Username { get; set; }
    public static string FullName { get; set; }
    public static int TenantId { get; set; }
    public static int? BranchId { get; set; }
    public static int RoleId { get; set; }
    public static string RoleName { get; set; }
    
    public static bool IsLoggedIn => UserId > 0;
    
    public static void Logout()
    {
        UserId = 0;
        Username = string.Empty;
        FullName = string.Empty;
        TenantId = 0;
        BranchId = null;
        RoleId = 0;
        RoleName = string.Empty;
    }
}
```

**Vai trò:**
- Lưu trữ thông tin người dùng xuyên suốt phiên làm việc
- "Chìa khóa" để filter dữ liệu Multi-tenant
- Được kiểm tra ở BLL để kiểm soát quyền hạn

#### **Phân cấp Quyền hạn**

| Role | Quyền hạn |
|------|-----------|
| **SuperAdmin** | - Tất cả chức năng<br>- Quản lý user, chi nhánh<br>- Xem tất cả báo cáo |
| **BranchManager/Manager** | - Danh mục, Nghiệp vụ, Báo cáo<br>- Quản lý nhân viên trong chi nhánh<br>- Cấu hình chi nhánh của mình |
| **Staff** | - Chỉ Nghiệp vụ cơ bản<br>- Không truy cập cấu hình<br>- Không được xóa dữ liệu |

#### **Điều khiển Menu (ApplyAuthorization)**
```csharp
private void ApplyAuthorization()
{
    string role = SessionManager.RoleName;
    
    // Mặc định: ẩn tất cả
    mnuManageUsers.Visible = false;
    mnuBranchConfig.Visible = false;
    
    if (role == "Staff")
    {
        mnuManageUsers.Visible = false;
        mnuCashFunds.Visible = false;
    }
    else if (role == "BranchManager" || role == "Manager")
    {
        mnuManageUsers.Visible = true;
        mnuBranchConfig.Visible = true;
    }
    else if (role == "SuperAdmin")
    {
        mnuManageUsers.Visible = true;
        mnuBranchConfig.Visible = true;
        mnuCashFunds.Visible = true;
    }
}
```

---

### 4. Deep Security (Bảo vệ Tầng Sâu)

#### **Phân quyền Đa tầng**

```
┌──────────────┐
│ GUI Layer    │ ← Ẩn/hiển thị menu
├──────────────┤
│ BLL Layer    │ ← Kiểm tra quyền trước hành động
├──────────────┤
│ DAL Layer    │ ← Enforce foreign key constraints
└──────────────┘
```

#### **Kiểm tra quyền ở Service**

```csharp
// UserService
public void CreateUser(User newUser, string plainPassword)
{
    if (SessionManager.RoleName != "SuperAdmin" && 
        SessionManager.RoleName != "BranchManager")
        throw new UnauthorizedAccessException();
    
    if (SessionManager.RoleName == "BranchManager" && 
        newUser.BranchId != SessionManager.BranchId)
        throw new UnauthorizedAccessException();
    
    // ... create user
}

// PartnerService
public void DeletePartner(int id)
{
    if (SessionManager.RoleName != "SuperAdmin")
        throw new UnauthorizedAccessException();
    
    var partner = _partnerRepo.GetById(id);
    if (partner != null && partner.InitialDebt != 0)
        throw new InvalidOperationException("Không thể xóa đối tác có nợ!");
    
    _partnerRepo.Delete(id);
    _partnerRepo.Save();
}
```

**Lợi ích:**
- Không thể bypass phân quyền chỉnh sửa GUI
- Bảo vệ dữ liệu ở nguồn (BLL)
- Thông báo lỗi rõ ràng cho người dùng

---

### 5. Cơ chế Đăng xuất (Logout)

#### **Luồng Xử lý**
```
User click "Đăng xuất"
    ↓
Hiển thị confirm dialog
    ↓ (Yes)
SessionManager.Logout()
    ↓
this.Hide() → frmMain
    ↓
new frmLogin().Show()
    ↓
this.Dispose() → Giải phóng bộ nhớ
```

#### **Code Implementation**
```csharp
private void menuLogout_Click(object sender, EventArgs e)
{
    var confirm = MessageBox.Show(
        "Bạn có chắc chắn muốn đăng xuất?", 
        "Xác nhận", 
        MessageBoxButtons.YesNo, 
        MessageBoxIcon.Question);
    
    if (confirm != DialogResult.Yes) return;
    
    try
    {
        SessionManager.Logout();        // Xóa session
        this.Hide();                    // Ẩn Main
        
        frmLogin login = new frmLogin();// Form mới
        login.Show();
        
        this.Dispose();                 // Giải phóng bộ nhớ
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
    }
}
```

---

## 📝 Khó khăn & Giải pháp

### 1. RoleName không được set
**Vấn đề:** `SessionManager.RoleName` luôn null  
**Giải pháp:** Include Role entity trong LINQ query
```csharp
var user = _context.Users.Include(u => u.Role)
    .FirstOrDefault(u => u.Username == username);
SessionManager.RoleName = user.Role?.RoleName ?? "Unknown";
```

### 2. Properties.Settings Type Mismatch
**Vấn đề:** `isRemembered` có type `System.String` thay vì `System.Boolean`  
**Giải pháp:** Cập nhật Settings.settings và Settings.Designer.cs

---

## ✅ Checklist Hoàn thành

- [x] frmLogin xác thực thành công
- [x] Remember Me lưu/load username
- [x] frmMain load động UserControl
- [x] Menu phân quyền theo Role
- [x] Deep Security ở BLL
- [x] Logout an toàn, clear session
- [x] Error handling toàn diện
- [x] Code naming convention chuyên nghiệp

---

**Last Updated:** 25/03/2026  
**Status:** ✅ Sprint 3 Hoàn thành