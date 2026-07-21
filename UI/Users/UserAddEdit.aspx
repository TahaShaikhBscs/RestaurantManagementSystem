
<%@ Page Title="User Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="UserAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Users.UserAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    User Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="page-title">
                <i class="fas fa-user me-2"></i>
                <asp:Literal ID="ltrPageTitle" runat="server" Text="User Details" />
            </h2>
            <hr />
        </div>
    </div>

    <!-- Status Messages -->
    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show">
        <i class="fas fa-check-circle me-2"></i>
        <asp:Literal ID="ltrSuccessMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show">
        <i class="fas fa-exclamation-circle me-2"></i>
        <asp:Literal ID="ltrErrorMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <!-- User Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-body">
            <div class="row">
                <div class="col-md-6">
                    <!-- Username -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-user me-1"></i>Username <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                            ControlToValidate="txtUsername" CssClass="text-danger small"
                            ErrorMessage="Username is required." Display="Dynamic" />
                    </div>

                    <!-- Full Name -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-user-circle me-1"></i>Full Name <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" MaxLength="200" />
                        <asp:RequiredFieldValidator ID="rfvFullName" runat="server" 
                            ControlToValidate="txtFullName" CssClass="text-danger small"
                            ErrorMessage="Full name is required." Display="Dynamic" />
                    </div>

                    <!-- Email -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-envelope me-1"></i>Email
                        </label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                            ControlToValidate="txtEmail" CssClass="text-danger small"
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                            ErrorMessage="Invalid email format." Display="Dynamic" />
                    </div>

                    <!-- Phone -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-phone me-1"></i>Phone
                        </label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" MaxLength="50" />
                    </div>
                </div>

                <div class="col-md-6">
                    <!-- Branch -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-store me-1"></i>Branch <span class="text-danger">*</span>
                        </label>
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-select" 
                            DataTextField="BranchName" DataValueField="BranchID" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">-- Select Branch --</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvBranch" runat="server" 
                            ControlToValidate="ddlBranch" CssClass="text-danger small"
                            ErrorMessage="Branch is required." Display="Dynamic"
                            InitialValue="0" />
                    </div>

                    <!-- Role -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-user-shield me-1"></i>Role <span class="text-danger">*</span>
                        </label>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select" 
                            DataTextField="RoleName" DataValueField="RoleID" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">-- Select Role --</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvRole" runat="server" 
                            ControlToValidate="ddlRole" CssClass="text-danger small"
                            ErrorMessage="Role is required." Display="Dynamic"
                            InitialValue="0" />
                    </div>

                    <!-- Password (for new users) -->
                    <div class="mb-3" id="divPassword" runat="server">
                        <label class="form-label fw-bold">
                            <i class="fas fa-lock me-1"></i>Password <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100" />
                        <small class="text-muted">Password must be at least 8 characters with uppercase, lowercase, digit, and special character.</small>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                            ControlToValidate="txtPassword" CssClass="text-danger small"
                            ErrorMessage="Password is required for new users." Display="Dynamic" />
                    </div>

                    <!-- Session Timeout -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-clock me-1"></i>Session Timeout (minutes)
                        </label>
                        <asp:TextBox ID="txtSessionTimeout" runat="server" CssClass="form-control" Text="30" />
                        <asp:RangeValidator ID="rvSessionTimeout" runat="server"
                            ControlToValidate="txtSessionTimeout" CssClass="text-danger small"
                            MinimumValue="5" MaximumValue="480" Type="Integer"
                            ErrorMessage="Session timeout must be between 5 and 480 minutes." Display="Dynamic" />
                    </div>

                    <!-- Status -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-toggle-on me-1"></i>Status
                        </label>
                        <div class="form-check form-switch">
                            <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label" for="<%= chkIsActive.ClientID %>">
                                <asp:Literal ID="ltrStatusText" runat="server" Text="Active" />
                            </label>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Buttons -->
            <div class="row mt-3">
                <div class="col-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save User" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#<%= chkIsActive.ClientID %>').change(function() {
                var statusText = $(this).is(':checked') ? 'Active' : 'Inactive';
                $('#<%= ltrStatusText.ClientID %>').text(statusText);
            });

            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>