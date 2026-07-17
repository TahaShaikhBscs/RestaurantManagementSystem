 

<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="RestaurantManagementSystem.UI.Settings.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-key text-primary me-2"></i>Change Password
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="#">Settings</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Change Password</li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnBack" runat="server" Text="Back to Profile" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
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

    <!-- Change Password Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-lock me-2 text-primary"></i>Change Your Password
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-lock me-1 text-primary"></i>Current Password <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="rfvCurrentPassword" runat="server" 
                            ControlToValidate="txtCurrentPassword" CssClass="text-danger small mt-1"
                            ErrorMessage="Current password is required." Display="Dynamic" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-key me-1 text-primary"></i>New Password <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" 
                            ControlToValidate="txtNewPassword" CssClass="text-danger small mt-1"
                            ErrorMessage="New password is required." Display="Dynamic" />
                        <div class="password-requirements small mt-1">
                            <div id="lengthReq" class="invalid">✓ At least 8 characters</div>
                            <div id="upperReq" class="invalid">✓ At least one uppercase letter</div>
                            <div id="lowerReq" class="invalid">✓ At least one lowercase letter</div>
                            <div id="digitReq" class="invalid">✓ At least one digit</div>
                            <div id="specialReq" class="invalid">✓ At least one special character</div>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-check-circle me-1 text-primary"></i>Confirm New Password <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                            ControlToValidate="txtConfirmPassword" CssClass="text-danger small mt-1"
                            ErrorMessage="Please confirm your password." Display="Dynamic" />
                        <asp:CompareValidator ID="cvPassword" runat="server"
                            ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword"
                            CssClass="text-danger small mt-1" Operator="Equal" Type="String"
                            ErrorMessage="Passwords do not match." Display="Dynamic" />
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="card bg-info bg-opacity-10 border-info">
                        <div class="card-body">
                            <h6 class="fw-bold"><i class="fas fa-info-circle me-1"></i>Password Requirements</h6>
                            <ul class="small text-muted mb-0">
                                <li>Minimum 8 characters long</li>
                                <li>At least one uppercase letter (A-Z)</li>
                                <li>At least one lowercase letter (a-z)</li>
                                <li>At least one digit (0-9)</li>
                                <li>At least one special character (!@#$%^&*)</li>
                            </ul>
                        </div>
                    </div>

                    <div class="card bg-warning bg-opacity-10 border-warning mt-3">
                        <div class="card-body">
                            <h6 class="fw-bold"><i class="fas fa-exclamation-triangle me-1"></i>Security Tips</h6>
                            <ul class="small text-muted mb-0">
                                <li>Don't reuse passwords from other accounts</li>
                                <li>Avoid using personal information</li>
                                <li>Change your password regularly</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="row mt-4">
                <div class="col-12">
                    <hr />
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnChangePassword" runat="server" CssClass="btn btn-primary px-4" OnClick="btnChangePassword_Click" Text="Change Password" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <style>
        .password-requirements .valid { color: #28a745; }
        .password-requirements .invalid { color: #dc3545; }
    </style>
    <script type="text/javascript">
        $(document).ready(function() {
            // Password strength validation
            $('#<%= txtNewPassword.ClientID %>').on('input', function() {
                var password = $(this).val();
                
                $('#lengthReq').toggleClass('valid invalid', password.length >= 8);
                $('#upperReq').toggleClass('valid invalid', /[A-Z]/.test(password));
                $('#lowerReq').toggleClass('valid invalid', /[a-z]/.test(password));
                $('#digitReq').toggleClass('valid invalid', /[0-9]/.test(password));
                $('#specialReq').toggleClass('valid invalid', /[^a-zA-Z0-9]/.test(password));
            });

            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>