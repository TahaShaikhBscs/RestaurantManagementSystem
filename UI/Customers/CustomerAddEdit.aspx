

<%@ Page Title="Customer Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="CustomerAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Customers.CustomerAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Customer Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-user text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Customer Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="CustomerList.aspx">Customers</a></li>
                            <li class="breadcrumb-item active" aria-current="page">
                                <asp:Literal ID="ltrBreadcrumb" runat="server" Text="Add New" />
                            </li>
                        </ol>
                    </nav>
                </div>
                <div>
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

    <!-- Customer Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Customer Information" />
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <!-- Left Column -->
                <div class="col-md-6">
                    <!-- Customer Name -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-user me-1 text-primary"></i>Customer Name <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" MaxLength="200" 
                            placeholder="Enter customer name" />
                        <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" 
                            ControlToValidate="txtCustomerName" CssClass="text-danger small mt-1"
                            ErrorMessage="Customer name is required." Display="Dynamic" />
                    </div>

                    <!-- Phone -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-phone me-1 text-primary"></i>Phone
                        </label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" MaxLength="50" 
                            placeholder="Enter phone number" />
                    </div>

                    <!-- Email -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-envelope me-1 text-primary"></i>Email
                        </label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" 
                            placeholder="Enter email address" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                            ControlToValidate="txtEmail" CssClass="text-danger small mt-1"
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                            ErrorMessage="Invalid email format." Display="Dynamic" />
                    </div>

                    <!-- Address -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-map-marker-alt me-1 text-primary"></i>Address
                        </label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="2" MaxLength="500" placeholder="Enter address" />
                    </div>
                </div>

                <!-- Right Column -->
                <div class="col-md-6">
                    <!-- Customer Type -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-tag me-1 text-primary"></i>Customer Type
                        </label>
                        <asp:DropDownList ID="ddlCustomerType" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Regular">Regular</asp:ListItem>
                            <asp:ListItem Value="VIP">VIP</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Birthday -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-birthday-cake me-1 text-primary"></i>Birthday
                        </label>
                        <asp:TextBox ID="txtBirthday" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>

                    <!-- Gender -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-venus-mars me-1 text-primary"></i>Gender
                        </label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">-- Select --</asp:ListItem>
                            <asp:ListItem Value="Male">Male</asp:ListItem>
                            <asp:ListItem Value="Female">Female</asp:ListItem>
                            <asp:ListItem Value="Other">Other</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Membership Number (Read-only) -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-id-card me-1 text-primary"></i>Membership Number
                        </label>
                        <asp:TextBox ID="txtMembershipNumber" runat="server" CssClass="form-control" 
                            ReadOnly="true" BackColor="#f8f9fa" />
                        <small class="text-muted">Auto-generated for new customers</small>
                    </div>

                    <!-- Loyalty Points (Read-only) -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-star me-1 text-primary"></i>Loyalty Points
                        </label>
                        <asp:TextBox ID="txtLoyaltyPoints" runat="server" CssClass="form-control" 
                            ReadOnly="true" BackColor="#f8f9fa" Text="0" />
                    </div>
                </div>
            </div>

            <hr />

            <!-- Status Row -->
            <div class="row">
                <div class="col-md-6">
                    <div class="card bg-light p-3">
                        <div class="form-check form-switch">
                            <asp:CheckBox ID="chkStatus" runat="server" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label fw-bold" for="<%= chkStatus.ClientID %>">
                                <i class="fas fa-toggle-on me-1 text-success"></i>Active
                            </label>
                        </div>
                        <small class="text-muted">Inactive customers cannot be used for orders</small>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="card bg-info bg-opacity-10 border-info">
                        <div class="card-body py-2">
                            <div class="d-flex align-items-center">
                                <i class="fas fa-info-circle text-info fa-lg me-2"></i>
                                <small class="text-muted">
                                    <strong>Note:</strong> Loyalty points are automatically updated with purchases.
                                    VIP customers get special benefits.
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
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" Text="Save Customer" />
                        <asp:Button ID="btnCancel2" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
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