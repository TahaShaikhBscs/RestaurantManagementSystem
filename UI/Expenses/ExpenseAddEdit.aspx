
<%@ Page Title="Expense Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="ExpenseAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Expenses.ExpenseAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Expense Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-money-bill-wave text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Expense Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="ExpenseList.aspx">Expenses</a></li>
                            <li class="breadcrumb-item active" aria-current="page">
                                <asp:Literal ID="ltrBreadcrumb" runat="server" Text="Add New" />
                            </li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnCancel" runat="server" Text="Back to List" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Status Messages -->
    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show shadow-sm">
        <div class="d-flex align-items-center">
            <i class="fas fa-check-circle fa-lg me-2"></i>
            <asp:Literal ID="ltrSuccessMessage" runat="server" />
            <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert"></button>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show shadow-sm">
        <div class="d-flex align-items-center">
            <i class="fas fa-exclamation-circle fa-lg me-2"></i>
            <asp:Literal ID="ltrErrorMessage" runat="server" />
            <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert"></button>
        </div>
    </asp:Panel>

    <!-- Expense Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Expense Information" />
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <!-- Left Column -->
                <div class="col-md-6">
                    <!-- Expense Category -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-tag me-1 text-primary"></i>Category <span class="text-danger">*</span>
                        </label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" AppendDataBoundItems="True">
                            <asp:ListItem Value="">-- Select Category --</asp:ListItem>
                            <asp:ListItem Value="Salary">Salary</asp:ListItem>
                            <asp:ListItem Value="Electricity">Electricity</asp:ListItem>
                            <asp:ListItem Value="Gas">Gas</asp:ListItem>
                            <asp:ListItem Value="Water">Water</asp:ListItem>
                            <asp:ListItem Value="Rent">Rent</asp:ListItem>
                            <asp:ListItem Value="Maintenance">Maintenance</asp:ListItem>
                            <asp:ListItem Value="Supplies">Supplies</asp:ListItem>
                            <asp:ListItem Value="Food">Food</asp:ListItem>
                            <asp:ListItem Value="Beverage">Beverage</asp:ListItem>
                            <asp:ListItem Value="Marketing">Marketing</asp:ListItem>
                            <asp:ListItem Value="Insurance">Insurance</asp:ListItem>
                            <asp:ListItem Value="Tax">Tax</asp:ListItem>
                            <asp:ListItem Value="Miscellaneous">Miscellaneous</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                            ControlToValidate="ddlCategory" CssClass="text-danger small mt-1"
                            ErrorMessage="Category is required." Display="Dynamic"
                            InitialValue="" />
                    </div>

                    <!-- Description -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-align-left me-1 text-primary"></i>Description <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="3" MaxLength="500" placeholder="Enter expense description" />
                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" 
                            ControlToValidate="txtDescription" CssClass="text-danger small mt-1"
                            ErrorMessage="Description is required." Display="Dynamic" />
                    </div>

                    <!-- Amount -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-dollar-sign me-1 text-primary"></i>Amount <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="0.00" />
                        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" 
                            ControlToValidate="txtAmount" CssClass="text-danger small mt-1"
                            ErrorMessage="Amount is required." Display="Dynamic" />
                        <asp:RangeValidator ID="rvAmount" runat="server"
                            ControlToValidate="txtAmount" CssClass="text-danger small mt-1"
                            MinimumValue="0.01" MaximumValue="999999.99" Type="Currency"
                            ErrorMessage="Amount must be between 0.01 and 999999.99." Display="Dynamic" />
                    </div>
                </div>

                <!-- Right Column -->
                <div class="col-md-6">
                    <!-- Expense Date -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-calendar me-1 text-primary"></i>Expense Date <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtExpenseDate" runat="server" CssClass="form-control" TextMode="Date" />
                        <asp:RequiredFieldValidator ID="rfvExpenseDate" runat="server" 
                            ControlToValidate="txtExpenseDate" CssClass="text-danger small mt-1"
                            ErrorMessage="Expense date is required." Display="Dynamic" />
                    </div>

                    <!-- Payment Method -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-credit-card me-1 text-primary"></i>Payment Method
                        </label>
                        <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Cash">Cash</asp:ListItem>
                            <asp:ListItem Value="Bank Transfer">Bank Transfer</asp:ListItem>
                            <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                            <asp:ListItem Value="Credit Card">Credit Card</asp:ListItem>
                            <asp:ListItem Value="Debit Card">Debit Card</asp:ListItem>
                            <asp:ListItem Value="Online">Online</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Receipt Number -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-receipt me-1 text-primary"></i>Receipt Number
                        </label>
                        <asp:TextBox ID="txtReceiptNumber" runat="server" CssClass="form-control" MaxLength="100" 
                            placeholder="Enter receipt or reference number" />
                    </div>

                    <!-- Recurring Expense -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-sync me-1 text-primary"></i>Recurring Expense
                        </label>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-check form-switch">
                                    <asp:CheckBox ID="chkIsRecurring" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="<%= chkIsRecurring.ClientID %>">Recurring</label>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ddlRecurringType" runat="server" CssClass="form-select" Enabled="false">
                                    <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                    <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                    <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                    <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <hr />

            <!-- Status Row -->
            <div class="row">
                <div class="col-md-6">
                    <div class="card bg-light p-3">
                        <div class="form-check form-switch">
                            <asp:CheckBox ID="chkStatus" runat="server" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label fw-bold" for="<%= chkStatus.ClientID %>">
                                <i class="fas fa-toggle-on me-1 text-success"></i>Active
                            </label>
                        </div>
                        <small class="text-muted">Inactive expenses are hidden from reports</small>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="card bg-info bg-opacity-10 border-info">
                        <div class="card-body py-2">
                            <div class="d-flex align-items-center">
                                <i class="fas fa-info-circle text-info fa-lg me-2"></i>
                                <small class="text-muted">
                                    <strong>Note:</strong> Expenses are used for profit/loss calculations.
                                    Keep receipts for audit purposes.
                                </small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="row mt-4">
                <div class="col-12">
                    <hr />
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" Text="Save Expense" />
                        <asp:Button ID="btnCancel2" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            // Toggle recurring type dropdown
            $('#<%= chkIsRecurring.ClientID %>').change(function() {
                var isChecked = $(this).is(':checked');
                $('#<%= ddlRecurringType.ClientID %>').prop('disabled', !isChecked);
                if (!isChecked) {
                    $('#<%= ddlRecurringType.ClientID %>').val('Monthly');
                }
            });

            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>