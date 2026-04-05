Mục tiêu:
Triển khai Sprint Con người theo hướng bảo mật và đa tenant, cho phép Giám đốc công ty tạo tài khoản nhân viên, phân quyền theo vai trò, và khóa/mở khóa tài khoản mà không làm lộ dữ liệu nhạy cảm.

Task List:

Tạo UserDTO để tách dữ liệu hiển thị khỏi entity gốc, không expose PasswordHash.
Bổ sung 3 hàm cốt lõi async trong UserService: lấy danh sách user theo tenant, tạo user mới, khóa/mở khóa user.
Bổ sung 2 hàm hỗ trợ GUI: lấy danh sách role có thể gán và chi nhánh đang hoạt động theo tenant.
Thêm kiểm soát phân quyền và tenant scope ở tầng BLL trước mọi thao tác quản lý user.
Hoàn thiện ucUserManagement với luồng nạp dữ liệu, tạo user, khóa/mở khóa, validation input.
Tích hợp điều hướng menu quản lý người dùng từ frmMain.
Cập nhật SessionManager để quyền quản lý user chỉ thuộc SuperAdmin và TenantAdmin.
Thu hoạch cho Báo cáo:
Áp dụng được mô hình bảo vệ dữ liệu nhạy cảm qua DTO trong WinForms nhiều tầng.
Chuẩn hóa được cơ chế tenant isolation ở BLL thay vì để GUI tự kiểm soát.
Kết hợp phân quyền theo role và rule nghiệp vụ branch-scoped role hiệu quả.
Xác nhận cách tiếp cận async trong UserControl giúp tránh UI freeze khi tải dữ liệu.
Mô tả:
Luồng quản lý user mới bắt đầu từ menu chính vào ucUserManagement, tại đây hệ thống nạp role và branch theo tenant hiện tại. Khi tạo tài khoản, hệ thống kiểm tra role hợp lệ theo tenant, kiểm tra branch nếu role yêu cầu, hash mật khẩu bằng BCrypt rồi mới lưu. Khi khóa/mở khóa, hệ thống xác minh tenant, chặn tự khóa chính mình, và chặn thao tác không đủ quyền. Toàn bộ truy vấn đọc danh sách dùng AsNoTracking để tối ưu.

Nhận xét:

Kiến trúc hiện tại bám sát 3 tầng và giữ được business rule ở BLL.
Tính nhất quán phân quyền đã tốt hơn sau khi đồng bộ lại CanManageUsers.
Mã GUI đã có kiểm tra đầu vào và xử lý async phù hợp yêu cầu sprint.
Mức độ tách biệt dữ liệu nhạy cảm đạt yêu cầu bảo mật cho màn hình quản lý user.
Khó khăn & Giải pháp:
Khó khăn: Trong quá trình cập nhật lớn, file ucUserManagement từng bị lỗi cấu trúc gây lỗi biên dịch hàng loạt.
Giải pháp: Thực hiện tái tạo sạch file theo cấu trúc chuẩn, sau đó build lại và xác nhận hết lỗi compile.
Khó khăn: Mô hình Users hiện chưa có cột CreatedDate trong entity/database nhưng DTO có yêu cầu hiển thị.
Giải pháp: Tạm thời map giá trị CreatedDate trong projection để không chặn tiến độ sprint; bước tiếp theo nên bổ sung cột CreatedDate bằng migration để dữ liệu phản ánh đúng thực tế.
Nếu bạn muốn, mình có thể làm tiếp migration thêm CreatedDate cho Users và cập nhật projection dùng dữ liệu thật từ DB.
Mình cũng có thể bổ sung test checklist cho 4 kịch bản bảo mật quan trọng: vượt tenant, vượt role, tự khóa tài khoản, và trùng username theo tenant.
Mục tiêu: Hoàn thiện giao diện ucUserManagement theo phong cách card-based, tối ưu thao tác quản trị nhân sự với khả năng tìm kiếm và khóa/mở khóa trực quan.
Task List:
Thiết kế Header Panel gồm tiêu đề, tìm kiếm, nút Thêm Nhân Viên, nút Khóa/Mở Khóa.
Thiết kế Main Panel chứa DataGridView theo chuẩn quản trị (read-only, full row select, autosize fill).
Refactor code-behind sang luồng tải danh sách user theo Tenant.
Bổ sung chức năng tìm kiếm theo họ tên hoặc username.
Bổ sung FormatGrid đổi tên cột và format ngày giờ.
Bổ sung CellFormatting hiển thị trạng thái màu sắc trực quan.
Bổ sung thao tác khóa/mở khóa có xác nhận.
Thu hoạch cho Báo cáo:
Tách rõ màn quản lý danh sách và popup thêm mới giúp UX gọn, an toàn hơn.
Cải thiện khả năng quan sát trạng thái tài khoản bằng màu sắc thay vì bool thô.
Giữ nguyên nguyên tắc đa tenant bằng cách nạp dữ liệu theo SessionManager.CurrentTenantId.
Mô tả: UserControl được dựng lại bằng Designer controls để cố định layout Header/Main. DataGridView bind qua BindingSource, sau mỗi lần nạp hoặc lọc đều chạy FormatGrid để chuẩn hóa cột. CellFormatting chuyển IsActive thành nhãn nghiệp vụ và tô màu theo trạng thái. Nút Add mở frmAddUser popup, nút Toggle gọi UserService để đổi trạng thái tài khoản.
Nhận xét:
Giao diện đã đúng định hướng ERP trực quan, thao tác nhanh cho Giám đốc.
Cấu hình lưới đáp ứng an toàn thao tác, tránh sửa trực tiếp dữ liệu.
Mã code-behind gọn hơn, tập trung đúng chức năng của màn quản lý.
Khó khăn & Giải pháp:
Khó khăn: File cũ dựng UI bằng code động, trộn cả luồng tạo user và quản lý danh sách nên khó đồng bộ với thiết kế mới.
Giải pháp: Refactor về Designer-based layout, giữ lại nghiệp vụ cốt lõi (load, search, toggle) và chuyển tạo user sang popup frmAddUser.
Nếu bạn đồng ý, mình làm luôn frmAddUser bản đầy đủ (input, role, branch, validate, tạo tài khoản thật qua UserService).
Mình cũng có thể nối DialogResult từ frmAddUser để chỉ reload danh sách khi tạo thành công.
Mục tiêu: Hoàn thiện code-behind cho popup thêm nhân viên để nạp dữ liệu Chi nhánh/Quyền theo tenant, validate đầu vào, và gọi BLL tạo tài khoản; đồng thời nối popup vào màn quản lý user theo luồng DialogResult chuẩn.
Task List:
Bổ sung hàm async lấy chi nhánh theo tenant trong BranchService.
Viết frmAddUser_Load và LoadComboBoxDataAsync.
Map hiển thị vai trò, xử lý role yêu cầu chi nhánh.
Viết ValidateInput, SetSavingState, btnSave_Click, btnCancel_Click.
Cập nhật ucUserManagement để chỉ reload danh sách khi frmAddUser trả về OK.
Thu hoạch cho Báo cáo:
Chuẩn hóa luồng popup thêm mới theo mô hình Form con trả tín hiệu thành công về Form cha.
Đảm bảo lọc dữ liệu theo tenant ngay tại BLL trước khi bind GUI.
Tăng độ ổn định thao tác bằng cơ chế khóa nút khi đang lưu để tránh submit lặp.
Mô tả:
frmAddUser khởi tạo AppDbContext + UserService + BranchService, nạp danh sách chi nhánh và role trong sự kiện Load.
Khi lưu, form tạo đối tượng User từ dữ liệu nhập, truyền mật khẩu thô xuống UserService.CreateUserAsync để BLL xử lý hash và kiểm tra nghiệp vụ.
ucUserManagement mở frmAddUser bằng ShowDialog và chỉ gọi LoadUsersAsync khi DialogResult.OK.
Nhận xét:
Luồng xử lý phù hợp 3-tier hiện tại, tách rõ GUI và BLL.
Tính nhất quán nghiệp vụ tenant/role tốt hơn nhờ dùng service ở tầng BLL thay vì hard-code tại giao diện.
Trải nghiệm người dùng tốt hơn do có validate rõ ràng và chống click đúp.
Khó khăn & Giải pháp:
Khó khăn: Mẫu bạn đưa dùng tên context/namespace khác so với codebase GD10.
Giải pháp: Điều chỉnh theo thực tế dự án (AppDbContext, namespace hiện hữu, SessionManager dạng nullable) để code chạy ngay không phá vỡ cấu trúc hiện tại.
Nếu bạn muốn, mình có thể làm tiếp phần mở rộng: tự động vô hiệu hóa cbBranch khi role không cần chi nhánh và hiển thị nhãn trạng thái role rõ hơn trong form.
Mình cũng có thể thêm kiểm tra format username và độ mạnh mật khẩu ở GUI trước khi gọi BLL.
Mục tiêu: Chuẩn hóa phân quyền từ mô hình Role cứng sang Role linh hoạt trong CSDL bằng RoleCode và PriorityLevel, vẫn đảm bảo tiến độ MVP và không triển khai ma trận quyền phức tạp.
Task List: Cập nhật Entity Role; thêm seed dữ liệu Role chuẩn; cập nhật SessionManager để hỗ trợ RoleCode; refactor UserService theo PriorityLevel; refactor frmAddUser theo RoleCode; tạo và chỉnh migration chống schema drift; chạy cập nhật CSDL thành công.
Thu hoạch cho Báo cáo: Tách mã quyền kỹ thuật RoleCode khỏi tên hiển thị RoleName giúp đổi tên UI không làm gãy logic; dùng PriorityLevel để kiểm soát tạo tài khoản theo cấp bậc rõ ràng; migration drift-safe giúp triển khai ổn định trên môi trường dữ liệu thực tế.
Mô tả: Bảng Roles được mở rộng với các cột RoleCode, PriorityLevel, IsActive; TenantId khỏi Roles để role dùng chung hệ thống; User vẫn liên kết FK RoleId; BLL lọc role assign theo PriorityLevel lớn hơn người đang đăng nhập; GUI tự bật/tắt chọn chi nhánh dựa trên loại role; migration dùng điều kiện IF EXISTS và cập nhật dữ liệu theo hướng upsert.
Nhận xét: Giải pháp cân bằng tốt giữa tính mở rộng và thời gian triển khai; giữ được kiến trúc 3 tầng; hạn chế rủi ro bug do phụ thuộc tên quyền hoặc ID cứng không kiểm soát.
Khó khăn & Giải pháp: Gặp lỗi drop index không tồn tại và lỗi Invalid column name khi add/update cùng batch trong migration; đã xử lý bằng SQL có điều kiện theo metadata hệ thống và tách batch update sau khi cột đã được tạo.
Mục tiêu: Lưu đầy đủ ngữ cảnh phân quyền vào session gồm mã quyền và cấp quyền để toàn bộ hệ thống dùng thống nhất sau đăng nhập.
Task List: Thêm CurrentRoleCode; thêm CurrentPriorityLevel; cập nhật Logout reset state; mở rộng truy vấn login lấy PriorityLevel; gán session values khi xác thực thành công.
Thu hoạch cho Báo cáo: Tách rõ quyền hiển thị (RoleName) và quyền kỹ thuật (RoleCode, PriorityLevel) giúp kiểm soát nghiệp vụ ổn định và dễ mở rộng.
Mô tả: SessionManager nay có CurrentRoleCode đồng bộ với RoleCode và CurrentPriorityLevel mặc định an toàn; UserService.Authenticate join Roles và đọc thêm PriorityLevel trước khi set session.
Nhận xét: Cách này phù hợp MVP, ít thay đổi giao diện nhưng tăng rõ độ chắc chắn cho kiểm tra phân quyền xuyên suốt BLL/GUI.
Khó khăn & Giải pháp: Do không có LoginService riêng nên dễ sửa nhầm lớp; đã rà đúng luồng frmLogin -> UserService.Authenticate và cập nhật trực tiếp tại điểm xác thực trung tâm.
Mục tiêu: Áp dụng luật phân quyền “quản lý cấp thấp không được tạo/sửa tài khoản quản lý cấp cao” tại UI quản lý nhân viên.
Task List:
Cập nhật LoadComboBoxDataAsync của frmAddUser để lọc role theo CurrentPriorityLevel.
Cập nhật LoadComboBoxDataAsync của frmEditUser với cùng rule lọc.
Kiểm tra lỗi biên dịch cho 2 file sau khi chỉnh sửa.
Thu hoạch cho Báo cáo:
Ràng buộc quyền hiển thị ngay tại ComboBox giúp giảm thao tác sai từ người dùng.
Rule quyền được đồng bộ giữa form thêm và form sửa, tránh lệch hành vi.
Mô tả:
Danh sách role lấy từ service sau đó tiếp tục lọc ở GUI bằng điều kiện PriorityLevel > SessionManager.CurrentPriorityLevel.
Chỉ các role có cấp quyền thấp hơn user hiện tại mới xuất hiện trong ComboBox chức vụ.
Nhận xét:
Cách triển khai ngắn gọn, dễ bảo trì và không ảnh hưởng luồng nhập liệu hiện có.
UX rõ ràng hơn vì người dùng chỉ thấy các lựa chọn hợp lệ.
Khó khăn & Giải pháp:
Khó khăn: cần đảm bảo áp dụng đồng nhất cho cả thêm mới và chỉnh sửa.
Giải pháp: dùng cùng một điều kiện lọc trong LoadComboBoxDataAsync của cả hai form và kiểm tra lỗi ngay sau khi sửa.
Mục tiêu: Khóa hiển thị menu theo role ở tầng giao diện để người dùng thấy rõ giới hạn quyền ngay trên frmMain.
Task List:
Tìm và cập nhật hàm ApplyAuthorization trong frmMain.
Thêm if/else theo SessionManager.CurrentRoleCode.
Áp dụng rule:
STAFF: ẩn mnuReports và mnuManageUsers.
BRANCHMANAGER: ẩn mnuReconciliation.
Kiểm tra lỗi biên dịch sau chỉnh sửa.
Thu hoạch cho Báo cáo: Kết hợp phân quyền tổng quát bằng Can* với rule override theo RoleCode giúp UI rõ ràng hơn và giảm khả năng thao tác sai vai trò.
Mô tả: Sau khi set visibility theo CanManageUsers/CanViewSummaryReports/CanApproveDebt, hệ thống normalize CurrentRoleCode về uppercase rồi áp dụng override giao diện theo từng role cụ thể.
Nhận xét: Cách làm giữ nguyên kiến trúc hiện tại, thay đổi gọn trong một điểm kiểm soát menu, dễ mở rộng thêm role-rule sau này.
Khó khăn & Giải pháp:
Khó khăn: map đúng menu theo yêu cầu nghiệp vụ từ tên hiển thị tiếng Việt.
Giải pháp: đối chiếu designer để xác định chính xác mnuReports, mnuManageUsers, mnuReconciliation trước khi áp dụng logic.
Mục tiêu: Bổ sung đầy đủ API bất đồng bộ cho luồng sửa nhân viên để frmEditUser biên dịch và chạy đúng phân quyền.
Task List:
Thêm GetUserByIdAsync trong UserService.
Thêm UpdateUserAsync trong UserService.
Áp dụng kiểm tra quyền quản lý user, tenant scope, priority level role, branch hợp lệ.
Kiểm tra lại lỗi biên dịch ở file BLL và form sửa user.
Thu hoạch cho Báo cáo:
Luồng Edit User cần có API đọc chi tiết và cập nhật riêng, không thể dùng trực tiếp hàm sync cũ.
Rule phân quyền theo PriorityLevel cần kiểm tra cả role hiện tại của user mục tiêu và role muốn gán.
Mô tả:
GetUserByIdAsync đọc user theo UserId + Tenant scope bằng AsNoTracking.
UpdateUserAsync nạp user hiện tại, kiểm tra tenant, kiểm tra role được phép gán, kiểm tra chi nhánh theo tenant, cập nhật mật khẩu khi có nhập mới, rồi SaveChangesAsync.
Nhận xét:
Thay đổi tập trung ở BLL, không làm lệch cấu trúc GUI hiện có.
Cơ chế kiểm tra quyền/tenant đặt ở service giúp tránh bypass từ UI.
Khó khăn & Giải pháp:
Khó khăn: frmEditUser đã gọi async API nhưng UserService chưa có định nghĩa tương ứng.
Giải pháp: bổ sung đúng chữ ký hàm, đồng bộ với rule quyền hiện tại và kiểm chứng lại compile errors ngay sau khi sửa.
Mục tiêu: Tối ưu luồng thao tác quản lý người dùng, thêm/sửa người dùng để giảm thao tác dư, tránh trạng thái nút sai và tăng độ an toàn khi lưu.
Task List:
Refactor trạng thái nút thao tác trong màn hình quản lý user theo selection và busy state.
Bổ sung tìm kiếm realtime, Esc để clear filter và double click row để mở sửa nhanh.
Giữ selection sau reload/filter để tránh mất ngữ cảnh.
Nâng cấp validate và chuẩn hóa dữ liệu đầu vào cho AddUser/EditUser.
Bật/tắt nút Lưu theo realtime input và trạng thái form đã sẵn sàng.
Chặn đóng form khi đang lưu để tránh ngắt giữa chừng.
Build xác nhận compile sau thay đổi.
Thu hoạch cho Báo cáo: UX ổn định hơn nhờ quản lý trạng thái tập trung, tránh warning popup không cần thiết do người dùng bấm nút khi chưa chọn dòng, đồng thời tăng chất lượng dữ liệu đầu vào trước khi gọi BLL.
Mô tả: Chỉ thay đổi tầng GUI tại 3 file UserManagement/AddUser/EditUser; không đổi schema DB hay nghiệp vụ DAL/BLL. Luồng sửa/thêm vẫn giữ nguyên call service hiện có, chỉ cải thiện trải nghiệm, validation và state management.
Nhận xét: Cách triển khai đồng nhất giữa Add và Edit giúp dễ bảo trì; hành vi nút Lưu và nút thao tác trên lưới nhất quán hơn, giảm lỗi thao tác người dùng.
Khó khăn & Giải pháp: Build ban đầu gặp lỗi CS0165 do biến pattern matching chưa chắc chắn gán; đã chuyển sang kiểu ép as + kiểm tra null rõ ràng để đảm bảo compile và tránh lỗi tương tự trong nullable-enabled code.
Mục tiêu: Sửa lỗi DataGridView báo current cell could not be set khi màn hình user reload.
Task List:
Phân tích luồng restore selection sau filter/reload.
Xác định nguyên nhân set CurrentCell vào cột ẩn.
Sửa logic chọn CurrentCell sang ô hiển thị đầu tiên.
Kiểm tra lỗi compile file đã sửa.
Thu hoạch cho Báo cáo: Khi ẩn cột kỹ thuật, không được set CurrentCell theo index cứng; cần bám cột visible để tránh exception runtime.
Mô tả: Chỉnh tại GUI ucUserManagement, method SelectRowByUserIdOrFallback; thêm vòng lặp tìm visible cell trước khi gán CurrentCell.
Nhận xét: Luồng reload và giữ selection an toàn hơn, không còn văng lỗi do cột ẩn.
Khó khăn & Giải pháp: Build tổng bị khóa file exe vì app đang chạy; giải pháp là stop app trước khi build lại để xác nhận toàn project.
Mục tiêu: Bổ sung dấu hiệu nhận biết trạng thái tài khoản trong lưới người dùng và loại bỏ hành vi popup không mong muốn khi thao tác vào cột trạng thái.

Task List: Cập nhật logic CellDoubleClick để bỏ qua cột IsActive; cải tiến CellFormatting hiển thị trạng thái có marker trực quan; tinh chỉnh cấu hình cột IsActive trong FormatGrid để hiển thị ổn định.

Thu hoạch cho Báo cáo: Việc tách thao tác “xem trạng thái” khỏi thao tác “mở form sửa” giúp giảm nhầm thao tác; marker trạng thái trên cùng cột giúp người dùng nhận diện nhanh hơn khi quét dữ liệu.

Mô tả: Trong ucUserManagement, event double-click được kiểm tra thêm ColumnName IsActive để return sớm; cột IsActive được hiển thị text trạng thái tùy theo bool; đồng thời cột được đặt canh giữa, không sort và thu gọn độ rộng để dễ quan sát.

Nhận xét: Thay đổi nhỏ nhưng cải thiện UX rõ rệt, không ảnh hưởng luồng thêm/sửa/khóa tài khoản hiện có.

Khó khăn & Giải pháp: Khó khăn là popup xuất hiện do event tương tác của lưới dễ bị hiểu nhầm là click cột trạng thái; giải pháp là chặn riêng cột IsActive ở CellDoubleClick và chuyển trọng tâm sang hiển thị trực quan trạng thái.

Nếu bạn muốn, mình có thể làm thêm kiểu hiển thị badge màu nền (xanh/đỏ) thay vì chỉ text để trạng thái nổi bật hơn nữa.

Nếu bạn muốn thao tác nhanh, mình cũng có thể thêm cơ chế click vào cột Trạng Thái để khóa/mở khóa trực tiếp (có confirm).
Mục tiêu: Ổn định cột Trạng thái trên dgvUsers, tránh hành vi không mong muốn do cột bool auto-generate.
Task List: Thêm hàm chuyển IsActive từ checkbox sang text column; gọi chuẩn hóa cột trong FormatGrid; giữ logic format trạng thái bằng text có dấu hiệu trực quan; giữ chặn popup khi double-click cột trạng thái.
Thu hoạch cho Báo cáo: Với DataGridView auto-generate, cột bool nên được chuẩn hóa sớm nếu cần hiển thị dạng text để tránh lỗi tương tác.
Mô tả: Khi bind dữ liệu, nếu IsActive là DataGridViewCheckBoxColumn thì remove và insert DataGridViewTextBoxColumn cùng DataPropertyName IsActive; sau đó áp dụng style và CellFormatting.
Nhận xét: UX rõ ràng hơn, cột Trạng thái chỉ đóng vai trò hiển thị và không gây nhầm thao tác.
Khó khăn & Giải pháp: Khó khăn là cột bool auto-generate có thể gây hành vi khó kiểm soát khi format text; giải pháp là chuyển sang text column ngay trong UI layer trước khi apply style.