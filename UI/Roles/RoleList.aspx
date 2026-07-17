

<%@ Page Title="Role Management" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Roles.RoleList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Role Management - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-user-shield text-primary me-2"></i>Role Management
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Roles</li>
                        </ol>
                    </nav>
                </div>
                <div class="mt-2 mt-md-0">
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New Role" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-secondary" OnClick="btnRefresh_Click" />
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Search Section -->
    <div class="card border-0 shadow-sm mb-3">
        <div class="card-body py-3">
            <div class="row g-2 align-items-center">
                <div class="col-md-8">
                    <div class="input-group">
                        <span class="input-group-text bg-white">
                            <i class="fas fa-search text-muted"></i>
                        </span>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search roles by name or description..." />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-warning" OnClick="btnClear_Click" />
                    </div>
                </div>
                <div class="col-md-4 text-end">
                    <span class="text-muted small">
                        <i class="fas fa-info-circle me-1"></i>
                        Manage user roles and permissions
                    </span>
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
                        <div class="col-md-4">
                            <span class="badge bg-primary rounded-pill px-3 py-2">
                                <i class="fas fa-user-shield me-1"></i>
                                Total: <asp:Literal ID="ltrTotalRoles" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-4">
                            <span class="badge bg-success rounded-pill px-3 py-2">
                                <i class="fas fa-check-circle me-1"></i>
                                Active: <asp:Literal ID="ltrActiveRoles" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-4">
                            <span class="badge bg-danger rounded-pill px-3 py-2">
                                <i class="fas fa-times-circle me-1"></i>
                                Inactive: <asp:Literal ID="ltrInactiveRoles" runat="server" Text="0" />
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
                <i class="fas fa-list-ul me-2 text-primary"></i>Roles List
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrTotalCount" runat="server" Text="0" /> Roles
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    OnRowCommand="gvRoles_RowCommand" OnPageIndexChanging="gvRoles_PageIndexChanging"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination"
                    DataKeyNames="RoleID">
                    <Columns>
                        <asp:BoundField DataField="RoleID" HeaderText="ID" Visible="False" />
                        <asp:TemplateField HeaderText="Role" ItemStyle-Width="25%">
                            <ItemTemplate>
                                <div class="d-flex align-items-center">
                                    <div class="role-icon me-2" style="width: 32px; height: 32px; display: flex; align-items: center; justify-content: center;">
                                        <i class="fas fa-user-shield text-primary"></i>
                                    </div>
                                    <div>
                                        <div class="fw-bold"><%# Eval("RoleName") %></div>
                                        <small class="text-muted"><%# Eval("Description") %></small>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# (bool)Eval("Status") ? "success" : "danger" %> px-3 py-2">
                                    <i class="fas fa-<%# (bool)Eval("Status") ? "check-circle" : "times-circle" %> me-1"></i>
                                    <%# (bool)Eval("Status") ? "Active" : "Inactive" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="btn-group btn-group-sm" role="group">
                                    <asp:Button ID="btnPermissions" runat="server" Text="Permissions" CssClass="btn btn-info"
                                        CommandName="Permissions" CommandArgument='<%# Eval("RoleID") %>' />
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary"
                                        CommandName="EditRole" CommandArgument='<%# Eval("RoleID") %>' />
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                                        CommandName="DeleteRole" CommandArgument='<%# Eval("RoleID") %>'
                                        data-bs-toggle="modal" data-bs-target="#deleteModal" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-user-shield fa-4x text-muted mb-3 d-block"></i>
                            <h5 class="text-muted">No roles found</h5>
                            <p class="text-muted small">Start by creating your first role</p>
                            <asp:Button ID="btnAddFirst" runat="server" Text="Add First Role" CssClass="btn btn-success mt-2" OnClick="btnAddNew_Click" />
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
                        Roles define user permissions for the system
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

    <!-- Delete Confirmation Modal -->
    <div class="modal fade" id="deleteModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Confirm Delete</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p>Are you sure you want to delete this role?</p>
                    <p class="text-danger"><strong>Warning:</strong> This action cannot be undone.</p>
                    <p>Role must not be assigned to any users.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnConfirmDelete" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="btnConfirmDelete_Click" />
                    <asp:HiddenField ID="hfDeleteRoleID" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('[data-bs-toggle="modal"]').on('click', function () {
                var roleID = $(this).closest('tr').find('[id$="btnDelete"]').attr('CommandArgument');
                $('#<%= hfDeleteRoleID.ClientID %>').val(roleID);
            });

            setTimeout(function () {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>