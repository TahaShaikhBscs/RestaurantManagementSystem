 
<%@ Page Title="Role Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="RoleAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Roles.RoleAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Role Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-user-shield text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Role Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="RoleList.aspx">Roles</a></li>
                            <li class="breadcrumb-item active" aria-current="page">
                                <asp:Literal ID="ltrBreadcrumb" runat="server" Text="Add New" />
                            </li>
                        </ol>
                    </nav>
                </div>
                <div class="mt-2 mt-md-0">
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

    <!-- Role Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Role Information" />
            </h5>
        </div>
        <div class="card-body">
            <!-- Two Column Layout -->
            <div class="row">
                <!-- Left Column -->
                <div class="col-md-6">
                    <!-- Role Name -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-tag me-1 text-primary"></i>Role Name <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtRoleName" runat="server" CssClass="form-control" MaxLength="100" 
                            placeholder="Enter role name (e.g., Manager, Cashier, Chef)" />
                        <asp:RequiredFieldValidator ID="rfvRoleName" runat="server" 
                            ControlToValidate="txtRoleName" CssClass="text-danger small mt-1"
                            ErrorMessage="Role name is required." Display="Dynamic" />
                    </div>

                    <!-- Description -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-align-left me-1 text-primary"></i>Description
                        </label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="4" MaxLength="500" placeholder="Enter role description and responsibilities" />
                    </div>
                </div>

                <!-- Right Column -->
                <div class="col-md-6">
                    <!-- Status -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-toggle-on me-1 text-primary"></i>Status
                        </label>
                        <div class="card bg-light p-3">
                            <div class="form-check form-switch">
                                <asp:CheckBox ID="chkStatus" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label fw-bold" for="<%= chkStatus.ClientID %>">
                                    <asp:Literal ID="ltrStatusText" runat="server" Text="Active" />
                                </label>
                                <span class="ms-2 text-muted small">(Inactive roles cannot be assigned to users)</span>
                            </div>
                        </div>
                    </div>

                    <!-- Info Box -->
                    <div class="card bg-info bg-opacity-10 border-info">
                        <div class="card-body py-2">
                            <div class="d-flex align-items-center">
                                <i class="fas fa-info-circle text-info fa-lg me-2"></i>
                                <small class="text-muted">
                                    <strong>Note:</strong> After creating a role, you can manage its permissions 
                                    from the role list page.
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
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" 
                            Text="Save Role" />
                        <asp:Button ID="btnSaveAndContinue" runat="server" CssClass="btn btn-primary px-4" 
                            OnClick="btnSaveAndContinue_Click" Text="Save & Continue" Visible="false" />
                        <asp:Button ID="btnCancel2" runat="server" Text="Cancel" CssClass="btn btn-secondary" 
                            OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= chkStatus.ClientID %>').change(function () {
                var statusText = $(this).is(':checked') ? 'Active' : 'Inactive';
                $('#<%= ltrStatusText.ClientID %>').text(statusText);
            });

            setTimeout(function () {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>