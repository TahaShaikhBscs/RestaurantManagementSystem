

<%@ Page Title="Menu Item Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="MenuAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Menu.MenuAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Menu Item Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-utensils text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Menu Item Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="CategoryList.aspx">Categories</a></li>
                            <li class="breadcrumb-item"><a href="MenuList.aspx">Menu Items</a></li>
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

    <!-- Menu Item Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Menu Item Information" />
            </h5>
        </div>
        <div class="card-body">
            <!-- Two Column Layout -->
            <div class="row">
                <!-- Left Column - Basic Information -->
                <div class="col-md-6">
                    <div class="card bg-light border-0 mb-3">
                        <div class="card-body">
                            <h6 class="fw-bold mb-3">
                                <i class="fas fa-info-circle me-1 text-primary"></i>Basic Information
                            </h6>

                            <!-- Item Name -->
                            <div class="mb-3">
                                <label class="form-label fw-bold">
                                    <i class="fas fa-tag me-1 text-primary"></i>Item Name <span class="text-danger">*</span>
                                </label>
                                <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control" MaxLength="200" 
                                    placeholder="Enter item name" />
                                <asp:RequiredFieldValidator ID="rfvItemName" runat="server" 
                                    ControlToValidate="txtItemName" CssClass="text-danger small mt-1"
                                    ErrorMessage="Item name is required." Display="Dynamic" />
                            </div>

                            <!-- Category -->
                            <div class="mb-3">
                                <label class="form-label fw-bold">
                                    <i class="fas fa-list me-1 text-primary"></i>Category <span class="text-danger">*</span>
                                </label>
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select"
                                    DataTextField="CategoryName" DataValueField="CategoryID" AppendDataBoundItems="True">
                                    <asp:ListItem Value="">-- Select Category --</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                                    ControlToValidate="ddlCategory" CssClass="text-danger small mt-1"
                                    ErrorMessage="Category is required." Display="Dynamic"
                                    InitialValue="" />
                            </div>

                            <!-- Description -->
                            <div class="mb-3">
                                <label class="form-label fw-bold">
                                    <i class="fas fa-align-left me-1 text-primary"></i>Description
                                </label>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                    Rows="3" MaxLength="500" placeholder="Enter item description" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Right Column - Pricing & Settings -->
                <div class="col-md-6">
                    <div class="card bg-light border-0 mb-3">
                        <div class="card-body">
                            <h6 class="fw-bold mb-3">
                                <i class="fas fa-dollar-sign me-1 text-primary"></i>Pricing & Settings
                            </h6>

                            <!-- Price -->
                            <div class="mb-3">
                                <label class="form-label fw-bold">
                                    <i class="fas fa-dollar-sign me-1 text-primary"></i>Price <span class="text-danger">*</span>
                                </label>
                                <div class="input-group">
                                    <span class="input-group-text">$</span>
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="0.00" />
                                </div>
                                <asp:RequiredFieldValidator ID="rfvPrice" runat="server" 
                                    ControlToValidate="txtPrice" CssClass="text-danger small mt-1"
                                    ErrorMessage="Price is required." Display="Dynamic" />
                                <asp:RangeValidator ID="rvPrice" runat="server"
                                    ControlToValidate="txtPrice" CssClass="text-danger small mt-1"
                                    MinimumValue="0.01" MaximumValue="9999.99" Type="Currency"
                                    ErrorMessage="Price must be between 0.01 and 9999.99." Display="Dynamic" />
                            </div>

                            <!-- Cost -->
                            <div class="mb-3">
                                <label class="form-label fw-bold">
                                    <i class="fas fa-coins me-1 text-primary"></i>Cost (COGS)
                                </label>
                                <div class="input-group">
                                    <span class="input-group-text">$</span>
                                    <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" placeholder="0.00" />
                                </div>
                                <small class="text-muted">Cost of ingredients (for profit calculation)</small>
                                <asp:RangeValidator ID="rvCost" runat="server"
                                    ControlToValidate="txtCost" CssClass="text-danger small mt-1"
                                    MinimumValue="0" MaximumValue="9999.99" Type="Currency"
                                    ErrorMessage="Cost must be between 0 and 9999.99." Display="Dynamic" />
                            </div>

                            <!-- Tax & Discount -->
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">
                                            <i class="fas fa-percent me-1 text-primary"></i>Tax (%)
                                        </label>
                                        <asp:TextBox ID="txtTax" runat="server" CssClass="form-control" Text="0" />
                                        <asp:RangeValidator ID="rvTax" runat="server"
                                            ControlToValidate="txtTax" CssClass="text-danger small mt-1"
                                            MinimumValue="0" MaximumValue="100" Type="Double"
                                            ErrorMessage="Tax must be between 0 and 100." Display="Dynamic" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">
                                            <i class="fas fa-tags me-1 text-primary"></i>Discount (%)
                                        </label>
                                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" Text="0" />
                                        <asp:RangeValidator ID="rvDiscount" runat="server"
                                            ControlToValidate="txtDiscount" CssClass="text-danger small mt-1"
                                            MinimumValue="0" MaximumValue="100" Type="Double"
                                            ErrorMessage="Discount must be between 0 and 100." Display="Dynamic" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Third Row - Additional Details -->
            <div class="row mt-2">
                <div class="col-md-12">
                    <div class="card bg-light border-0">
                        <div class="card-body">
                            <h6 class="fw-bold mb-3">
                                <i class="fas fa-cogs me-1 text-primary"></i>Additional Details
                            </h6>
                            <div class="row">
                                <!-- SKU -->
                                <div class="col-md-3">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">
                                            <i class="fas fa-barcode me-1 text-primary"></i>SKU
                                        </label>
                                        <asp:TextBox ID="txtSKU" runat="server" CssClass="form-control" MaxLength="100" 
                                            placeholder="Stock Keeping Unit" />
                                    </div>
                                </div>
                                <!-- Barcode -->
                                <div class="col-md-3">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">
                                            <i class="fas fa-qrcode me-1 text-primary"></i>Barcode
                                        </label>
                                        <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control" MaxLength="100" 
                                            placeholder="Barcode number" />
                                    </div>
                                </div>
                                <!-- Preparation Time -->
                                <div class="col-md-3">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">
                                            <i class="fas fa-clock me-1 text-primary"></i>Prep Time (min)
                                        </label>
                                        <asp:TextBox ID="txtPreparationTime" runat="server" CssClass="form-control" Text="15" />
                                        <asp:RangeValidator ID="rvPreparationTime" runat="server"
                                            ControlToValidate="txtPreparationTime" CssClass="text-danger small mt-1"
                                            MinimumValue="0" MaximumValue="999" Type="Integer"
                                            ErrorMessage="Preparation time must be between 0 and 999." Display="Dynamic" />
                                    </div>
                                </div>
                                <!-- Min Stock Alert -->
                                <div class="col-md-3">
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">
                                            <i class="fas fa-exclamation-triangle me-1 text-primary"></i>Min Stock Alert
                                        </label>
                                        <asp:TextBox ID="txtMinStock" runat="server" CssClass="form-control" Text="5" />
                                        <asp:RangeValidator ID="rvMinStock" runat="server"
                                            ControlToValidate="txtMinStock" CssClass="text-danger small mt-1"
                                            MinimumValue="0" MaximumValue="999" Type="Integer"
                                            ErrorMessage="Min stock must be between 0 and 999." Display="Dynamic" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Fourth Row - Settings Toggles -->
            <div class="row mt-3">
                <div class="col-md-12">
                    <div class="card bg-light border-0">
                        <div class="card-body">
                            <h6 class="fw-bold mb-3">
                                <i class="fas fa-toggle-on me-1 text-primary"></i>Settings
                            </h6>
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="card border-0 shadow-sm h-100">
                                        <div class="card-body text-center py-3">
                                            <div class="form-check form-switch d-flex justify-content-center">
                                                <asp:CheckBox ID="chkIsAvailable" runat="server" CssClass="form-check-input" Checked="true" />
                                                <label class="form-check-label fw-bold ms-2" for="<%= chkIsAvailable.ClientID %>">
                                                    Available
                                                </label>
                                            </div>
                                            <small class="text-muted">Show in menu</small>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="card border-0 shadow-sm h-100">
                                        <div class="card-body text-center py-3">
                                            <div class="form-check form-switch d-flex justify-content-center">
                                                <asp:CheckBox ID="chkIsKitchen" runat="server" CssClass="form-check-input" Checked="true" />
                                                <label class="form-check-label fw-bold ms-2" for="<%= chkIsKitchen.ClientID %>">
                                                    Kitchen Item
                                                </label>
                                            </div>
                                            <small class="text-muted">Display in kitchen</small>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="card border-0 shadow-sm h-100">
                                        <div class="card-body text-center py-3">
                                            <div class="form-check form-switch d-flex justify-content-center">
                                                <asp:CheckBox ID="chkIsFeatured" runat="server" CssClass="form-check-input" />
                                                <label class="form-check-label fw-bold ms-2" for="<%= chkIsFeatured.ClientID %>">
                                                    Featured
                                                </label>
                                            </div>
                                            <small class="text-muted">Highlight on menu</small>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="card border-0 shadow-sm h-100">
                                        <div class="card-body text-center py-3">
                                            <div class="form-check form-switch d-flex justify-content-center">
                                                <asp:CheckBox ID="chkStatus" runat="server" CssClass="form-check-input" Checked="true" />
                                                <label class="form-check-label fw-bold ms-2" for="<%= chkStatus.ClientID %>">
                                                    Active
                                                </label>
                                            </div>
                                            <small class="text-muted">
                                                <asp:Literal ID="ltrStatusText" runat="server" Text="Active" />
                                            </small>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Info Box -->
            <div class="row mt-3">
                <div class="col-12">
                    <div class="card bg-info bg-opacity-10 border-info">
                        <div class="card-body py-2">
                            <div class="d-flex align-items-center">
                                <i class="fas fa-info-circle text-info fa-lg me-2"></i>
                                <small class="text-muted">
                                    <strong>Note:</strong> Items marked as "Kitchen Item" will appear in the kitchen display system. 
                                    Price and cost should be entered with up to 2 decimal places.
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
                            Text="Save Menu Item" />
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