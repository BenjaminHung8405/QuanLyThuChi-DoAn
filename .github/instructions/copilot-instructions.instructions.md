---
applyTo: "**"
---

📜 SYSTEM INSTRUCTIONS FOR AI ASSISTANT

🏗 Architecture Overview
Project Name: CashFlowManagement
Platform: .NET WinForms
Architecture: Logical 3-Tier (Combined in 1 project but divided into 3 folders: DAL, BLL, GUI).
Database: SQL Server (EF Core Code-First).
Multi-tenancy: Every table has a TenantId. All queries MUST filter by the current TenantId.

🛠 Coding Standards

1. DAL (Data Access Layer)
   Entities: Located in CashFlowManagement.DAL.Entities.
   DbContext: Use AppDbContext.
   Constraint: Absolutely no physical deletion (Hard Delete) of transaction data. Use status or Soft Delete if necessary.
   Currency: Always use the decimal type and map Column(TypeName = "decimal(18, 0)") for VND currency.

2. BLL (Business Logic Layer)
   All calculations, permission checks, data filtering by BranchId and TenantId must be located here.
   Session Manager: Use the SessionManager class to get CurrentUserId, CurrentBranchId, and CurrentTenantId information.
   Database Transaction: When paying a Debt that generates Transactions, MUST use IDbContextTransaction to ensure data integrity.

3. GUI (Presentation Layer)
   Design Pattern: Use 1 Main Form and load UserControls into the main Panel.
   Input Validation: Always check input data (not empty, correct number format) before calling the BLL.
   Async/Await: Heavy database query operations or drawing charts must use async/await to prevent UI Freeze.

📊 Business Rules
Cash Flow: A Transaction can be linked to a Debt (DebtId). When a Transaction is successful, the PaidAmount in the Debts table must be updated.
Internal Transfer: Internal transfer business rules create 2 corresponding Transaction records, linked to each other via TransferRefId.
Permissions:

- SuperAdmin: Can see all Tenants and Branches.
- BranchManager: Can only see data belonging to their BranchId.

💬 Prompting Instructions

- "When I ask you to write a new feature, generate code in the following order: 1. Entity (if applicable) -> 2. Service in BLL -> 3. Interface in GUI."
- "Always prioritize using LINQ to query data through AppDbContext."
- "If writing code related to currency amounts, format with thousands separators (E.g.: 1.000.000) and do not include decimals."

# 📝 DOCUMENTATION & PROGRESS TRACKING

Every time you generate or update code for a new feature or a Sprint, you MUST also provide a summary for the README.md file (or a documentation block) with the following sections in Vietnamese:

1. **Mục tiêu**: [Briefly state the goal of this code/feature].
2. **Task List**: [List the specific technical tasks performed in this step].
3. **Thu hoạch cho Báo cáo**: [Key takeaways, new logic applied, or knowledge gained].
4. **Mô tả**: [Technical description of the implementation, including DB changes or logic flow].
5. **Nhận xét**: [Observations on code quality, UI consistency, or business logic accuracy].
6. **Khó khăn & Giải pháp**: [Identify specific bugs or logic hurdles encountered during the generation and how they were solved].

# 🚀 FINAL OUTPUT FORMATTING

- Provide the code first (Entity -> BLL -> GUI).
- Immediately follow with the "Documentation Summary" block above to ensure the user can update their project log.
