
<%@ Page Title="Table Management" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="TableList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Tables.TableList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Table Management - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-chair text-primary me-2"></i>Table Management
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Tables</li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New Table" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
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
                        <div class="col-md-5">
                            <div class="input-group">
                                <span class="input-group-text bg-white">
                                    <i class="fas fa-search text-muted"></i>
                                </span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by table number or floor..." />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                                <asp:ListItem Value="">All Status</asp:ListItem>
                                <asp:ListItem Value="Available">Available</asp:ListItem>
                                <asp:ListItem Value="Occupied">Occupied</asp:ListItem>
                                <asp:ListItem Value="Reserved">Reserved</asp:ListItem>
                                <asp:ListItem Value="Cleaning">Cleaning</asp:ListItem>
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
                Manage restaurant tables and seating
            </span>
        </div>
    </div>

    <!-- Table Statistics -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="row text-center">
                        <div class="col-md-2">
                            <span class="badge bg-primary rounded-pill px-3 py-2">
                                <i class="fas fa-chair me-1"></i>
                                Total: <asp:Literal ID="ltrTotalTables" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-2">
                            <span class="badge bg-success rounded-pill px-3 py-2">
                                <i class="fas fa-check-circle me-1"></i>
                                Available: <asp:Literal ID="ltrAvailableTables" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-2">
                            <span class="badge bg-danger rounded-pill px-3 py-2">
                                <i class="fas fa-user-circle me-1"></i>
                                Occupied: <asp:Literal ID="ltrOccupiedTables" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-2">
                            <span class="badge bg-warning rounded-pill px-3 py-2">
                                <i class="fas fa-clock me-1"></i>
                                Reserved: <asp:Literal ID="ltrReservedTables" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-2">
                            <span class="badge bg-info rounded-pill px-3 py-2">
                                <i class="fas fa-broom me-1"></i>
                                Cleaning: <asp:Literal ID="ltrCleaningTables" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-2">
                            <span class="badge bg-secondary rounded-pill px-3 py-2">
                                <i class="fas fa-power-off me-1"></i>
                                Inactive: <asp:Literal ID="ltrInactiveTables" runat="server" Text="0" />
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
                <i class="fas fa-list-ul me-2 text-primary"></i>Tables List
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrTotalCount" runat="server" Text="0" /> Tables
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvTables" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    OnRowCommand="gvTables_RowCommand" OnPageIndexChanging="gvTables_PageIndexChanging"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination">
                    <Columns>
                        <asp:BoundField DataField="TableID" HeaderText="ID" Visible="False" />
                        <asp:BoundField DataField="TableNumber" HeaderText="Table #" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="Floor" HeaderText="Floor" ItemStyle-Width="12%" />
                        <asp:BoundField DataField="Capacity" HeaderText="Capacity" ItemStyle-Width="10%" />
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# GetStatusBadgeClass(Eval("Status").ToString()) %> px-3 py-2">
                                    <i class="fas fa-<%# GetStatusIconClass(Eval("Status").ToString()) %> me-1"></i>
                                    <%# Eval("Status") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# (bool)Eval("IsActive") ? "success" : "danger" %> px-3 py-2">
                                    <i class="fas fa-<%# (bool)Eval("IsActive") ? "check-circle" : "times-circle" %> me-1"></i>
                                    <%# (bool)Eval("IsActive") ? "Yes" : "No" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="QR Code" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnQRCode" runat="server" Text="QR" CssClass="btn btn-sm btn-info"
                                    CommandName="QRCode" CommandArgument='<%# Eval("TableID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="btn-group btn-group-sm" role="group">
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary"
                                        CommandName="EditTable" CommandArgument='<%# Eval("TableID") %>' />
                                    <asp:Button ID="btnChangeStatus" runat="server" Text="Status" CssClass="btn btn-warning"
                                        CommandName="ChangeStatus" CommandArgument='<%# Eval("TableID") %>' />
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                                        CommandName="DeleteTable" CommandArgument='<%# Eval("TableID") %>'
                                        OnClientClick="return confirm('Are you sure you want to delete this table?');" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-chair fa-4x text-muted mb-3 d-block"></i>
                            <h5 class="text-muted">No tables found</h5>
                            <p class="text-muted small">Start by adding tables to your restaurant</p>
                            <asp:Button ID="btnAddFirst" runat="server" Text="Add First Table" CssClass="btn btn-success mt-2" OnClick="btnAddNew_Click" />
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
                        Table status can be updated manually or automatically via POS
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