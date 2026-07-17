# Project Audit Report

## Scope and method

This review covered all 59 ASPX pages, both master pages, the two public user
controls, code-behind files, session/authentication helpers, DAL entry points,
configuration, JavaScript/CSS references, and the project file. The review was
static because this environment does not include MSBuild, IIS/IIS Express, or a
SQL Server instance configured for the application.

The solution retains its existing ASP.NET Web Forms, ADO.NET, stored-procedure,
master-page, multi-company, multi-branch architecture.

## Page inventory

| Area | Pages reviewed | Primary controls and behavior reviewed |
| --- | --- | --- |
| Administration | Branch add/list, Company add/list, User add/list, Role add/list/permissions, Table add/list | CRUD forms, validators, GridViews, search/filter, delete commands, session scope |
| Operations | Dashboard, POS, Kitchen display, Order add/detail/list | Repeaters, GridViews, cart/order state, status actions, AJAX postbacks |
| Menu and CRM | Category add/list, Menu add/list, Customer add/history/list, Deal add/list | CRUD, repeaters, GridViews, search/filter, server validation |
| Inventory and finance | Purchase add/list, Expense add/list, Stock add/adjust/list, all eight report pages | filters, exports, GridViews, summaries, branch scoping |
| Account and errors | Login, Logout, Forgot Password, Reset Password, Profile, Change Password, Error | authentication, reset state, validation, redirects |
| Public website | Home, About, Blog, BlogDetails, Contact, Gallery, Menu, Offers, Reservation | public master integration, Repeaters, images, client-side gallery/menu behavior |

## Findings

| ID | Page / area | Severity | Root cause | Recommended fix | Impact | Files |
| --- | --- | --- | --- | --- | --- | --- |
| A-01 | All authenticated pages that use `SessionHelper` | Critical | Session values are boxed as `Int32`, so `as int?` always returns `null`. | Read numeric session values with a null-safe conversion helper. | Restores user/company/branch scope, auditing, CRUD ownership, reports, POS, and profile actions. | `Utilities/SessionHelper.cs` |
| A-02 | Administrative master navigation | High | The Reports Dashboard link points to Purchase Orders; Deals appears twice and one dropdown item nests an `li` inside another. | Correct the target and use one valid list item for Deals. | Restores navigation and valid Bootstrap dropdown markup. | `UI/MasterPage.Master` |
| A-03 | Administrative master layout | High | `~/Content/Site.css` and `~/Scripts/Site.js` are used as literal HTML paths; `Scripts/Site.js` does not exist. | Resolve the CSS application path server-side and remove the nonexistent script reference. | Removes CSS/JS 404s and applies shared responsive styling. | `UI/MasterPage.Master` |
| A-04 | Administrative master user menu | Medium | `User.FullName` is rendered through a `Literal` without encoding. | HTML-encode the display value before assignment. | Prevents stored XSS through profile data in global navigation. | `UI/MasterPage.Master.cs` |
| A-05 | Public website Home and Menu | High | Both link to `ProductDetails.aspx`, which is absent from the project. | Replace the unavailable detail action with the existing menu page until a product-details page is intentionally implemented. | Removes user-facing 404s. | `Website/Pages/Home.aspx`, `Website/Pages/Menu.aspx` |
| A-06 | Public website footer | Medium | Privacy Policy and Terms links point to pages that are absent from the project. | Render these as non-navigating links until those pages are implemented. | Removes user-facing 404s without inventing content pages. | `Website/Controls/Footer.ascx` |
| A-07 | Configuration | Critical | A SQL `sa` password and a placeholder encryption key are stored in source control; cookies do not require HTTPS. | Move secrets to deployment configuration, use a real protected key, and enforce secure cookies in production. | Security hardening; deployment-specific change, not safely automated here. | `Web.config` |
| A-08 | Public website data-bound markup | Medium | Several `Eval` expressions write CMS/database values directly into element text and attributes. | Encode text values and validate/allowlist image URLs when editing public content templates. | Reduces stored XSS risk; broad change should be separately regression-tested. | `Website/Pages/*.aspx` |
| A-09 | Build/runtime verification | Medium | No MSBuild/xbuild/dotnet, IIS, or database is installed/configured in this environment. | Build with Visual Studio/MSBuild and exercise all flows against a non-production database. | Runtime CRUD, stored procedure, responsive, and browser-console verification remains required. | Environment |
| A-10 | All administrative pages using `MasterPage.Master` | High | The master page wraps the entire `MainContent` placeholder in one `UpdatePanel`; every child postback becomes asynchronous regardless of page needs. | Remove the master-level panel and allow pages to opt into small, page-owned panels only after a page-specific interaction review. | Prevents partial-render lifecycle, modal, plug-in, and ViewState overhead across the administrative application. | `UI/MasterPage.Master` |
| A-11 | Branch, Company, and Role list delete modals | High | Client code tries to read the server-side `CommandArgument` property as an emitted HTML attribute. It is not rendered to the browser. | Use a `type="button"` client trigger with an explicit `data-delete-id` value and populate the existing hidden field before opening the modal. | Restores reliable delete confirmation without an unintended postback. | `UI/Branchs/BranchList.aspx`, `UI/Companys/CompanyList.aspx`, `UI/Roles/RoleList.aspx` |

## Page-specific AJAX review

The page review found no page-owned `UpdatePanel` before this change. The only
asynchronous behavior came from the master-page wrapper (A-10), so it affected
every administrative content page indiscriminately. The decisions below keep
the current, tested server event flow as normal postbacks. A list page may add a
small, page-owned panel later only when its search/filter/grid refresh is
explicitly tested with that page's modal and scripts; exports, navigation, saves,
and delete confirmation remain full postbacks.

| Pages | Decision | Root cause / page-specific rationale |
| --- | --- | --- |
| `BranchAddEdit`, `CompanyAddEdit`, `CustomerAddEdit`, `ExpenseAddEdit`, `MenuAddEdit`, `CategoryAddEdit`, `PurchaseAddEdit`, `StockAddEdit`, `StockAdjustment`, `SupplierAddEdit`, `TableAddEdit`, `UserAddEdit`, `RoleAddEdit` | Normal postback | Server validation, save/redirect behavior, and dependent values are form-wide; partial rendering has no demonstrated benefit. |
| `DealsAddEdit`, `OrderAddEdit`, `POS` | Normal postback | These pages maintain cart/item state, calculated totals, repeaters, and client handlers. A generic partial update would invalidate client state and is not justified without a targeted interaction design. |
| `BranchList`, `CompanyList`, `CustomerList`, `DealsList`, `ExpenseList`, `CategoryList`, `MenuList`, `OrderList`, `PurchaseList`, `RoleList`, `StockList`, `SupplierList`, `TableList`, `UserList` | Normal postback now; candidate for a page-owned grid/search/statistics/message panel later | Each has filtering and paging, but several also have redirects, exports, or Bootstrap delete modals. The master wrapper was the root cause of generic async behavior. Delete triggers are made client-only where required. |
| `CustomerHistory`, `OrderDetail`, `RolePermissions` | Normal postback | Detail, permission, print, and back actions are navigation/commit operations rather than a targeted refresh. |
| `Dashboard`, `KitchenDisplay` | Normal postback | Dashboard charts and kitchen timers have page-specific client lifecycle/state; no asynchronous refresh contract exists. |
| `SalesReport`, `ExpenseReport`, `InventoryReport`, `CustomerReport`, `KitchenPerformanceReport`, `PaymentReport`, `ProfitLoss`, `ReportList` | Normal postback | Report generation and all exports need full responses; no master async wrapper should intercept them. |
| `Login`, `Logout`, `ForgotPassword`, `ResetPassword`, `Profile`, `ChangePassword`, `Error` | Normal postback | Authentication, redirects, security state, and form validation should retain the standard page lifecycle. |
| Public `Home`, `About`, `Blog`, `BlogDetails`, `Contact`, `Gallery`, `Menu`, `Offers`, `Reservation` | No Web Forms AJAX | These pages use the public master page, not the administrative `ScriptManager`; their public client behavior remains independent. |

### AJAX architecture policy

* `MasterPage.Master` owns only the `ScriptManager`; navigation, footer, and the
  `MainContent` placeholder stay outside every `UpdatePanel`.
* Any future list-page panel must be declared on that page and contain only its
  search/filter, statistics, grid, and messages. Bootstrap modals remain outside
  the panel; downloads and redirects remain full postbacks.
* The master emits a `rms:async-postback` event from the standard
  `PageRequestManager.endRequest` lifecycle. Page-specific Select2, DataTables,
  date picker, toastr, or custom initialization can subscribe to it without
  duplicate global bindings. No such plugin is currently referenced by the
  project, so no speculative plugin initialization is added.

## Implementation plan

1. Fix A-01 through A-06 because they are validated, contained, and do not alter business rules or the database.
2. Run static consistency checks and any available build command.
3. Leave A-07 and A-08 as explicit deployment/security follow-ups because they require production-secret handling and broad content-template regression testing.

## Verification limits

Static analysis confirms all ASPX pages have matching code-behind and designer
files and DAL calls use stored-procedure commands with parameters. Runtime
verification of authentication, CRUD, filters, paging, exports, AJAX, SQL data,
and responsive breakpoints requires the missing Windows/.NET Framework and SQL
Server runtime.
