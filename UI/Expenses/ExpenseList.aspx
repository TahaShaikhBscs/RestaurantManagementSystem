

<%@ Page Title="Expense Management" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="ExpenseList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Expenses.ExpenseList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Expense Management - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-money-bill-wave text-primary me-2"></i>Expense Management
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Expenses</li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New Expense" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Search and Filter Section -->
    <div class="row mb-3">
        <div class="col-md-8">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-3">
                    <div class="row g-2 align-items-center">
                        <div class="col-md-4">
                            <div class="input-group">
                                <span class="input-group-text bg-white">
                                    <i class="fas fa-search text-muted"></i>
                                </span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search expenses..." />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlCategoryFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged">
                                <asp:ListItem Value="">All Categories</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" TextMode="Date" placeholder="From Date" />
                        </div>
                        <div class="col-md-2">
                            <div class="d-flex gap-2">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-warning" OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4 text-end">
            <span class="text-muted small">
                <i class="fas fa-info-circle me-1"></i>
                Track and manage business expenses
            </span>
        </div>
    </div>

    <!-- Expense Statistics -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="row text-center">
                        <div class="col-md-3">
                            <span class="badge bg-primary rounded-pill px-3 py-2">
                                <i class="fas fa-list me-1"></i>
                                Total: <asp:Literal ID="ltrTotalExpenses" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-danger rounded-pill px-3 py-2">
                                <i class="fas fa-dollar-sign me-1"></i>
                                Amount: $<asp:Literal ID="ltrTotalAmount" runat="server" Text="0.00" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-warning rounded-pill px-3 py-2">
                                <i class="fas fa-chart-line me-1"></i>
                                Average: $<asp:Literal ID="ltrAverageAmount" runat="server" Text="0.00" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-success rounded-pill px-3 py-2">
                                <i class="fas fa-calendar me-1"></i>
                                This Month: $<asp:Literal ID="ltrMonthAmount" runat="server" Text="0.00" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Grid View -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center">
            <h5 class="mb-0">
                <i class="fas fa-list-ul me-2 text-primary"></i>Expenses List
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrTotalCount" runat="server" Text="0" /> Expenses
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvExpenses" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    OnRowCommand="gvExpenses_RowCommand" OnPageIndexChanging="gvExpenses_PageIndexChanging"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination">
                    <Columns>
                        <asp:BoundField DataField="ExpenseID" HeaderText="ID" Visible="False" />
                        <asp:BoundField DataField="ExpenseDate" HeaderText="Date" ItemStyle-Width="12%" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="ExpenseCategory" HeaderText="Category" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="30%" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="12%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="PaymentMethod" HeaderText="Payment" ItemStyle-Width="12%" />
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# (bool)Eval("Status") ? "success" : "danger" %> px-3 py-2">
                                    <i class="fas fa-<%# (bool)Eval("Status") ? "check-circle" : "times-circle" %> me-1"></i>
                                    <%# (bool)Eval("Status") ? "Active" : "Inactive" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="btn-group btn-group-sm" role="group">
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary"
                                        CommandName="EditExpense" CommandArgument='<%# Eval("ExpenseID") %>' />
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                                        CommandName="DeleteExpense" CommandArgument='<%# Eval("ExpenseID") %>'
                                        OnClientClick="return confirm('Are you sure you want to delete this expense?');" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-money-bill-wave fa-4x text-muted mb-3 d-block"></i>
                            <h5 class="text-muted">No expenses found</h5>
                            <p class="text-muted small">Start by recording your first expense</p>
                            <asp:Button ID="btnAddFirst" runat="server" Text="Add First Expense" CssClass="btn btn-success mt-2" OnClick="btnAddNew_Click" />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <div class="card-footer bg-white py-2">
            <div class="row">
                <div class="col-md-6">
                    <small class="text-muted">
                        <i class="fas fa-info-circle me-1"></i>
                        Track all business expenses for accurate profit/loss reporting
                    </small>
                </div>
                <div class="col-md-6 text-end">
                    <small class="text-muted">
                        <i class="fas fa-clock me-1"></i>
                        Last updated: <%= DateTime.Now.ToString("dd/MM/yyyy HH:mm") %>
                    </small>
                </div>
            </div>
        </div>
    </div>

    <!-- Status Messages -->
    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show mt-3 shadow-sm">
        <div class="d-flex align-items-center">
            <i class="fas fa-check-circle fa-lg me-2"></i>
            <asp:Literal ID="ltrSuccessMessage" runat="server" />
            <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert"></button>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show mt-3 shadow-sm">
        <div class="d-flex align-items-center">
            <i class="fas fa-exclamation-circle fa-lg me-2"></i>
            <asp:Literal ID="ltrErrorMessage" runat="server" />
            <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert"></button>
        </div>
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>