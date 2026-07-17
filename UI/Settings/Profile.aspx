
<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="RestaurantManagementSystem.UI.Settings.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    My Profile - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-user-circle text-primary me-2"></i>My Profile
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="#">Settings</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Profile</li>
                        </ol>
                    </nav>
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

    <!-- Profile Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>Profile Information
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="mb-3">
                        <label class="form-label fw-bold">Username</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Full Name <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" MaxLength="200" />
                        <asp:RequiredFieldValidator ID="rfvFullName" runat="server" 
                            ControlToValidate="txtFullName" CssClass="text-danger small mt-1"
                            ErrorMessage="Full name is required." Display="Dynamic" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                            ControlToValidate="txtEmail" CssClass="text-danger small mt-1"
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                            ErrorMessage="Invalid email format." Display="Dynamic" />
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="mb-3">
                        <label class="form-label fw-bold">Phone</label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" MaxLength="50" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Role</label>
                        <asp:TextBox ID="txtRole" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Branch</label>
                        <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">Session Timeout (minutes)</label>
                        <asp:TextBox ID="txtSessionTimeout" runat="server" CssClass="form-control" />
                        <asp:RangeValidator ID="rvSessionTimeout" runat="server"
                            ControlToValidate="txtSessionTimeout" CssClass="text-danger small mt-1"
                            MinimumValue="5" MaximumValue="480" Type="Integer"
                            ErrorMessage="Session timeout must be between 5 and 480 minutes." Display="Dynamic" />
                    </div>
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="row mt-4">
                <div class="col-12">
                    <hr />
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" Text="Update Profile" />
                        <asp:Button ID="btnChangePassword" runat="server" CssClass="btn btn-warning" OnClick="btnChangePassword_Click" Text="Change Password" />
                    </div>
                </div>
            </div>
        </div>
    </div>
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