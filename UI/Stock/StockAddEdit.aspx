
<%@ Page Title="Stock Item Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="StockAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Stock.StockAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Stock Item Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-box text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Stock Item Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="StockList.aspx">Stock</a></li>
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

    <!-- Stock Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Stock Information" />
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <!-- Left Column -->
                <div class="col-md-6">
                    <!-- Product Name -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-tag me-1 text-primary"></i>Product Name <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" MaxLength="200" 
                            placeholder="Enter product name" />
                        <asp:RequiredFieldValidator ID="rfvProductName" runat="server" 
                            ControlToValidate="txtProductName" CssClass="text-danger small mt-1"
                            ErrorMessage="Product name is required." Display="Dynamic" />
                    </div>

                    <!-- Category -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-list me-1 text-primary"></i>Category
                        </label>
                        <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control" MaxLength="100" 
                            placeholder="e.g., Raw Materials, Packing, Cleaning" />
                    </div>

                    <!-- Barcode -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-barcode me-1 text-primary"></i>Barcode
                        </label>
                        <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control" MaxLength="100" 
                            placeholder="Enter barcode number" />
                    </div>
                </div>

                <!-- Right Column -->
                <div class="col-md-6">
                    <!-- Quantity -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-cubes me-1 text-primary"></i>Quantity <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Text="0" />
                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" 
                            ControlToValidate="txtQuantity" CssClass="text-danger small mt-1"
                            ErrorMessage="Quantity is required." Display="Dynamic" />
                        <asp:RangeValidator ID="rvQuantity" runat="server"
                            ControlToValidate="txtQuantity" CssClass="text-danger small mt-1"
                            MinimumValue="0" MaximumValue="999999" Type="Double"
                            ErrorMessage="Quantity must be between 0 and 999999." Display="Dynamic" />
                    </div>

                    <!-- Unit Price -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-dollar-sign me-1 text-primary"></i>Unit Price <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" Text="0.00" />
                        <asp:RequiredFieldValidator ID="rfvUnitPrice" runat="server" 
                            ControlToValidate="txtUnitPrice" CssClass="text-danger small mt-1"
                            ErrorMessage="Unit price is required." Display="Dynamic" />
                        <asp:RangeValidator ID="rvUnitPrice" runat="server"
                            ControlToValidate="txtUnitPrice" CssClass="text-danger small mt-1"
                            MinimumValue="0" MaximumValue="999999.99" Type="Currency"
                            ErrorMessage="Unit price must be between 0 and 999999.99." Display="Dynamic" />
                    </div>

                    <!-- Total Cost (Read-only) -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-calculator me-1 text-primary"></i>Total Cost
                        </label>
                        <asp:TextBox ID="txtTotalCost" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" Text="0.00" />
                    </div>
                </div>
            </div>

            <hr />

            <!-- Stock Levels -->
            <div class="row">
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-minus-circle me-1 text-primary"></i>Min Stock Level
                        </label>
                        <asp:TextBox ID="txtMinLevel" runat="server" CssClass="form-control" Text="5" />
                        <small class="text-muted">Alert when stock falls below this level</small>
                        <asp:RangeValidator ID="rvMinLevel" runat="server"
                            ControlToValidate="txtMinLevel" CssClass="text-danger small mt-1"
                            MinimumValue="0" MaximumValue="999999" Type="Integer"
                            ErrorMessage="Minimum stock level must be between 0 and 999999." Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-plus-circle me-1 text-primary"></i>Max Stock Level
                        </label>
                        <asp:TextBox ID="txtMaxLevel" runat="server" CssClass="form-control" Text="100" />
                        <small class="text-muted">Warning when stock exceeds this level</small>
                        <asp:RangeValidator ID="rvMaxLevel" runat="server"
                            ControlToValidate="txtMaxLevel" CssClass="text-danger small mt-1"
                            MinimumValue="0" MaximumValue="999999" Type="Integer"
                            ErrorMessage="Maximum stock level must be between 0 and 999999." Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-map-marker-alt me-1 text-primary"></i>Location
                        </label>
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" MaxLength="100" 
                            placeholder="e.g., Shelf A1, Cold Storage, etc." />
                    </div>
                </div>
            </div>

            <!-- Status -->
            <div class="row mt-2">
                <div class="col-md-6">
                    <div class="card bg-light p-3">
                        <div class="form-check form-switch">
                            <asp:CheckBox ID="chkStatus" runat="server" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label fw-bold" for="<%= chkStatus.ClientID %>">
                                <i class="fas fa-toggle-on me-1 text-success"></i>
                                <asp:Literal ID="ltrStatusText" runat="server" Text="Active" />
                            </label>
                        </div>
                        <small class="text-muted">Inactive items are hidden from inventory</small>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="card bg-info bg-opacity-10 border-info">
                        <div class="card-body py-2">
                            <div class="d-flex align-items-center">
                                <i class="fas fa-info-circle text-info fa-lg me-2"></i>
                                <small class="text-muted">
                                    <strong>Note:</strong> Total cost is calculated as Quantity × Unit Price.
                                    Adjust quantities carefully as they affect inventory.
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
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" Text="Save Stock Item" />
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
            // Calculate total cost
            $('#<%= txtQuantity.ClientID %>, #<%= txtUnitPrice.ClientID %>').on('input', function() {
                var quantity = parseFloat($('#<%= txtQuantity.ClientID %>').val()) || 0;
                var price = parseFloat($('#<%= txtUnitPrice.ClientID %>').val()) || 0;
                var total = quantity * price;
                $('#<%= txtTotalCost.ClientID %>').val(total.toFixed(2));
            });

            // Status text toggle
            $('#<%= chkStatus.ClientID %>').change(function() {
                var statusText = $(this).is(':checked') ? 'Active' : 'Inactive';
                $('#<%= ltrStatusText.ClientID %>').text(statusText);
            });

            // Trigger initial calculation
            $('#<%= txtQuantity.ClientID %>').trigger('input');
            $('#<%= txtUnitPrice.ClientID %>').trigger('input');

            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>