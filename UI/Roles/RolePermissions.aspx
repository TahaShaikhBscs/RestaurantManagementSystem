 
<%@ Page Title="Role Permissions" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="RolePermissions.aspx.cs" Inherits="RestaurantManagementSystem.UI.Roles.RolePermissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Role Permissions - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-lock text-primary me-2"></i>Role Permissions
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="RoleList.aspx">Roles</a></li>
                            <li class="breadcrumb-item active" aria-current="page">
                                <asp:Literal ID="ltrRoleName" runat="server" Text="Permissions" />
                            </li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnBack" runat="server" Text="Back to Roles" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
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

    <!-- Permissions Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <div class="d-flex justify-content-between align-items-center">
                <h5 class="mb-0">
                    <i class="fas fa-user-shield me-2 text-primary"></i>
                    Manage Permissions for: <span class="fw-bold"><asp:Literal ID="ltrRoleDisplay" runat="server" Text="Role" /></span>
                </h5>
                <span class="badge bg-info rounded-pill">
                    <i class="fas fa-info-circle me-1"></i>
                    Select permissions to grant
                </span>
            </div>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <asp:GridView ID="gvPermissions" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped" GridLines="None"
                    OnRowDataBound="gvPermissions_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ModuleName" HeaderText="Module" ItemStyle-Width="20%" />
                        <asp:TemplateField HeaderText="View" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkView" runat="server" CssClass="form-check-input" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Create" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCreate" runat="server" CssClass="form-check-input" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkEdit" runat="server" CssClass="form-check-input" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDelete" runat="server" CssClass="form-check-input" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Print" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkPrint" runat="server" CssClass="form-check-input" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-4">
                            <i class="fas fa-lock fa-3x text-muted mb-3 d-block"></i>
                            <p class="text-muted">No permissions available</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <!-- Action Buttons -->
            <div class="row mt-4">
                <div class="col-12">
                    <hr />
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnSavePermissions" runat="server" Text="Save Permissions" 
                            CssClass="btn btn-success px-4" OnClick="btnSavePermissions_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Legend -->
    <div class="row mt-3">
        <div class="col-12">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="d-flex flex-wrap gap-4 small text-muted">
                        <span><i class="fas fa-check-circle text-success me-1"></i> Granted</span>
                        <span><i class="fas fa-times-circle text-danger me-1"></i> Not Granted</span>
                        <span class="text-muted">|</span>
                        <span><i class="fas fa-info-circle text-info me-1"></i> Select permissions to control user access</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>