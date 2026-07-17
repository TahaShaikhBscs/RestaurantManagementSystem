

<%@ Page Title="Customer Management" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Customers.CustomerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Customer Management - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-users text-primary me-2"></i>Customer Management
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Customers</li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New Customer" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Search Section -->
    <div class="row mb-3">
        <div class="col-md-8">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-3">
                    <div class="row g-2 align-items-center">
                        <div class="col-md-6">
                            <div class="input-group">
                                <span class="input-group-text bg-white">
                                    <i class="fas fa-search text-muted"></i>
                                </span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by name, phone, or email..." />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlCustomerType" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomerType_SelectedIndexChanged">
                                <asp:ListItem Value="">All Types</asp:ListItem>
                                <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                <asp:ListItem Value="VIP">VIP</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
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
                Manage customer information and loyalty
            </span>
        </div>
    </div>

    <!-- Customer Statistics -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="row text-center">
                        <div class="col-md-3">
                            <span class="badge bg-primary rounded-pill px-3 py-2">
                                <i class="fas fa-users me-1"></i>
                                Total: <asp:Literal ID="ltrTotalCustomers" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-success rounded-pill px-3 py-2">
                                <i class="fas fa-user-check me-1"></i>
                                Active: <asp:Literal ID="ltrActiveCustomers" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-warning rounded-pill px-3 py-2">
                                <i class="fas fa-crown me-1"></i>
                                VIP: <asp:Literal ID="ltrVIPCustomers" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-info rounded-pill px-3 py-2">
                                <i class="fas fa-star me-1"></i>
                                Total Points: <asp:Literal ID="ltrTotalPoints" runat="server" Text="0" />
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
                <i class="fas fa-list-ul me-2 text-primary"></i>Customers List
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrTotalCount" runat="server" Text="0" /> Customers
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    OnRowCommand="gvCustomers_RowCommand" OnPageIndexChanging="gvCustomers_PageIndexChanging"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination">
                    <Columns>
                        <asp:BoundField DataField="CustomerID" HeaderText="ID" Visible="False" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="20%" />
                        <asp:BoundField DataField="Phone" HeaderText="Phone" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="20%" />
                        <asp:TemplateField HeaderText="Type" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# ((string)Eval("CustomerType")) == "VIP" ? "warning" : "secondary" %> px-3 py-2">
                                    <i class="fas fa-<%# ((string)Eval("CustomerType")) == "VIP" ? "crown" : "user" %> me-1"></i>
                                    <%# Eval("CustomerType") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LoyaltyPoints" HeaderText="Points" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# (bool)Eval("Status") ? "success" : "danger" %> px-3 py-2">
                                    <i class="fas fa-<%# (bool)Eval("Status") ? "check-circle" : "times-circle" %> me-1"></i>
                                    <%# (bool)Eval("Status") ? "Active" : "Inactive" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="btn-group btn-group-sm" role="group">
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary"
                                        CommandName="EditCustomer" CommandArgument='<%# Eval("CustomerID") %>' />
                                    <asp:Button ID="btnHistory" runat="server" Text="History" CssClass="btn btn-info"
                                        CommandName="ViewHistory" CommandArgument='<%# Eval("CustomerID") %>' />
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                                        CommandName="DeleteCustomer" CommandArgument='<%# Eval("CustomerID") %>'
                                        OnClientClick="return confirm('Are you sure you want to delete this customer?');" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-users fa-4x text-muted mb-3 d-block"></i>
                            <h5 class="text-muted">No customers found</h5>
                            <p class="text-muted small">Start by adding your first customer</p>
                            <asp:Button ID="btnAddFirst" runat="server" Text="Add First Customer" CssClass="btn btn-success mt-2" OnClick="btnAddNew_Click" />
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
                        Loyalty points are earned from purchases and can be redeemed
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