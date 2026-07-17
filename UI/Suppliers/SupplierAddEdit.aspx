

<%@ Page Title="Supplier Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="SupplierAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Suppliers.SupplierAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Supplier Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-truck text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Supplier Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="SupplierList.aspx">Suppliers</a></li>
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

    <!-- Supplier Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Supplier Information" />
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <!-- Left Column -->
                <div class="col-md-6">
                    <!-- Supplier Name -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-building me-1 text-primary"></i>Supplier Name <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" MaxLength="200" 
                            placeholder="Enter supplier name" />
                        <asp:RequiredFieldValidator ID="rfvSupplierName" runat="server" 
                            ControlToValidate="txtSupplierName" CssClass="text-danger small mt-1"
                            ErrorMessage="Supplier name is required." Display="Dynamic" />
                    </div>

                    <!-- Contact Person -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-user me-1 text-primary"></i>Contact Person
                        </label>
                        <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" MaxLength="200" 
                            placeholder="Enter contact person name" />
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
                </div>

                <!-- Right Column -->
                <div class="col-md-6">
                    <!-- Address -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-map-marker-alt me-1 text-primary"></i>Address
                        </label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="3" MaxLength="500" placeholder="Enter address" />
                    </div>

                    <!-- Tax Number -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-file-invoice me-1 text-primary"></i>Tax Number
                        </label>
                        <asp:TextBox ID="txtTaxNumber" runat="server" CssClass="form-control" MaxLength="100" 
                            placeholder="Enter tax or VAT number" />
                    </div>

                    <!-- Payment Terms -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-handshake me-1 text-primary"></i>Payment Terms
                        </label>
                        <asp:TextBox ID="txtPaymentTerms" runat="server" CssClass="form-control" MaxLength="200" 
                            placeholder="e.g., Net 30, Cash on Delivery, etc." />
                    </div>

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
                                <span class="ms-2 text-muted small">(Inactive suppliers cannot be used for purchases)</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Info Box -->
            <div class="row mt-2">
                <div class="col-12">
                    <div class="card bg-info bg-opacity-10 border-info">
                        <div class="card-body py-2">
                            <div class="d-flex align-items-center">
                                <i class="fas fa-info-circle text-info fa-lg me-2"></i>
                                <small class="text-muted">
                                    <strong>Note:</strong> Suppliers are used for purchase orders and inventory management.
                                    Keep contact and tax information up to date.
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
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" Text="Save Supplier" />
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
            // Status text toggle
            $('#<%= chkStatus.ClientID %>').change(function() {
                var statusText = $(this).is(':checked') ? 'Active' : 'Inactive';
                $('#<%= ltrStatusText.ClientID %>').text(statusText);
            });

            // Auto-hide alerts after 5 seconds
            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>