
<%@ Page Title="Menu Items" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="MenuList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Menu.MenuList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Menu Items - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-utensils text-primary me-2"></i>Menu Items
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="CategoryList.aspx">Categories</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Menu Items</li>
                        </ol>
                    </nav>
                </div>
                <div class="mt-2 mt-md-0">
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New Item" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-secondary" OnClick="btnRefresh_Click" />
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Search and Filter Section -->
    <div class="card border-0 shadow-sm mb-3">
        <div class="card-body py-3">
            <div class="row g-2 align-items-center">
                <div class="col-md-4">
                    <div class="input-group">
                        <span class="input-group-text bg-white">
                            <i class="fas fa-search text-muted"></i>
                        </span>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
                            placeholder="Search by name, barcode, or SKU..." />
                    </div>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlCategoryFilter" runat="server" CssClass="form-select" 
                        AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged">
                        <asp:ListItem Value="">All Categories</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-select" 
                        AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                        <asp:ListItem Value="">All Status</asp:ListItem>
                        <asp:ListItem Value="Active">Active</asp:ListItem>
                        <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                        <asp:ListItem Value="Available">Available</asp:ListItem>
                        <asp:ListItem Value="Unavailable">Unavailable</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-warning w-100" OnClick="btnClear_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Statistics Cards -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="row text-center">
                        <div class="col-md-3">
                            <span class="badge bg-primary rounded-pill px-3 py-2">
                                <i class="fas fa-utensils me-1"></i>
                                Total: <asp:Literal ID="ltrTotalItems" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-success rounded-pill px-3 py-2">
                                <i class="fas fa-check-circle me-1"></i>
                                Active: <asp:Literal ID="ltrActiveItems" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-info rounded-pill px-3 py-2">
                                <i class="fas fa-store me-1"></i>
                                Available: <asp:Literal ID="ltrAvailableItems" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-warning rounded-pill px-3 py-2">
                                <i class="fas fa-star me-1"></i>
                                Featured: <asp:Literal ID="ltrFeaturedItems" runat="server" Text="0" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Grid View -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center flex-wrap">
            <h5 class="mb-0">
                <i class="fas fa-list-ul me-2 text-primary"></i>Menu Items List
            </h5>
            <div>
                <span class="badge bg-primary rounded-pill me-2">
                    <i class="fas fa-tag me-1"></i>
                    <asp:Literal ID="ltrTotalCount" runat="server" Text="0" /> Items
                </span>
                <span class="badge bg-success rounded-pill">
                    <i class="fas fa-check-circle me-1"></i>
                    <asp:Literal ID="ltrActiveCount" runat="server" Text="0" /> Active
                </span>
            </div>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvMenuItems" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    OnRowCommand="gvMenuItems_RowCommand" OnPageIndexChanging="gvMenuItems_PageIndexChanging"
                    OnRowDataBound="gvMenuItems_RowDataBound"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination"
                    DataKeyNames="MenuItemID">
                    <Columns>
                        <asp:BoundField DataField="MenuItemID" HeaderText="ID" Visible="False" />
                        <asp:TemplateField HeaderText="Item" ItemStyle-Width="25%">
                            <ItemTemplate>
                                <div class="d-flex align-items-center">
                                    <div class="menu-thumbnail bg-light rounded me-2" style="width: 40px; height: 40px; display: flex; align-items: center; justify-content: center; overflow: hidden;">
                                        <i class="fas fa-utensils text-muted"></i>
                                    </div>
                                    <div>
                                        <div class="fw-bold"><%# Eval("ItemName") %></div>
                                        <small class="text-muted"><%# Eval("CategoryName") %></small>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Price" HeaderText="Price" ItemStyle-Width="10%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="SKU" HeaderText="SKU" ItemStyle-Width="10%" />
                        <asp:TemplateField HeaderText="Availability" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <span class="badge bg-<%# (bool)Eval("IsAvailable") ? "success" : "danger" %> px-3 py-2">
                                    <i class="fas fa-<%# (bool)Eval("IsAvailable") ? "check-circle" : "times-circle" %> me-1"></i>
                                    <%# (bool)Eval("IsAvailable") ? "Available" : "Unavailable" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kitchen" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# (bool)Eval("IsKitchen") ? "info" : "secondary" %> px-3 py-2">
                                    <i class="fas fa-<%# (bool)Eval("IsKitchen") ? "check-circle" : "times-circle" %> me-1"></i>
                                    <%# (bool)Eval("IsKitchen") ? "Yes" : "No" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
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
                                        CommandName="EditMenuItem" CommandArgument='<%# Eval("MenuItemID") %>' />
                                    <asp:Button ID="btnToggleAvailability" runat="server" Text="Toggle" CssClass="btn btn-warning"
                                        CommandName="ToggleAvailability" CommandArgument='<%# Eval("MenuItemID") %>' />
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                                        CommandName="DeleteMenuItem" CommandArgument='<%# Eval("MenuItemID") %>'
                                        OnClientClick="return confirm('Are you sure you want to delete this menu item?');" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-utensils fa-4x text-muted mb-3 d-block"></i>
                            <h5 class="text-muted">No menu items found</h5>
                            <p class="text-muted small">Start by adding items to your menu</p>
                            <asp:Button ID="btnAddFirst" runat="server" Text="Add First Menu Item" CssClass="btn btn-success mt-2" OnClick="btnAddNew_Click" />
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
                        Items marked as "Kitchen" will appear in kitchen display
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
    <style>
        .menu-thumbnail {
            min-width: 40px;
            min-height: 40px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function() {
            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>