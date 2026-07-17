

<%@ Page Title="Stock Adjustment" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="StockAdjustment.aspx.cs" Inherits="RestaurantManagementSystem.UI.Stock.StockAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Stock Adjustment - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-balance-scale text-primary me-2"></i>Stock Adjustment
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="StockList.aspx">Stock</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Adjustment</li>
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

    <!-- Adjustment Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>Adjust Stock Quantity
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6">
                    <!-- Stock Item -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-box me-1 text-primary"></i>Stock Item <span class="text-danger">*</span>
                        </label>
                        <asp:DropDownList ID="ddlStockItem" runat="server" CssClass="form-select"
                            DataTextField="ProductName" DataValueField="StockID" AppendDataBoundItems="True" AutoPostBack="true" OnSelectedIndexChanged="ddlStockItem_SelectedIndexChanged">
                            <asp:ListItem Value="">-- Select Item --</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvStockItem" runat="server" 
                            ControlToValidate="ddlStockItem" CssClass="text-danger small mt-1"
                            ErrorMessage="Stock item is required." Display="Dynamic"
                            InitialValue="" />
                    </div>

                    <!-- Current Quantity -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-cubes me-1 text-primary"></i>Current Quantity
                        </label>
                        <asp:TextBox ID="txtCurrentQuantity" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" Text="0" />
                    </div>

                    <!-- Adjustment Type -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-arrows-alt-h me-1 text-primary"></i>Adjustment Type <span class="text-danger">*</span>
                        </label>
                        <asp:DropDownList ID="ddlAdjustmentType" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Add">Add Stock</asp:ListItem>
                            <asp:ListItem Value="Remove">Remove Stock</asp:ListItem>
                            <asp:ListItem Value="Set">Set Quantity</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-6">
                    <!-- Adjustment Quantity -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-plus-minus me-1 text-primary"></i>Adjustment Quantity <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtAdjustmentQty" runat="server" CssClass="form-control" Text="0" />
                        <asp:RequiredFieldValidator ID="rfvAdjustmentQty" runat="server" 
                            ControlToValidate="txtAdjustmentQty" CssClass="text-danger small mt-1"
                            ErrorMessage="Adjustment quantity is required." Display="Dynamic" />
                        <asp:RangeValidator ID="rvAdjustmentQty" runat="server"
                            ControlToValidate="txtAdjustmentQty" CssClass="text-danger small mt-1"
                            MinimumValue="0" MaximumValue="999999" Type="Double"
                            ErrorMessage="Adjustment quantity must be between 0 and 999999." Display="Dynamic" />
                    </div>

                    <!-- New Quantity (Read-only) -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-calculator me-1 text-primary"></i>New Quantity
                        </label>
                        <asp:TextBox ID="txtNewQuantity" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" Text="0" />
                    </div>

                    <!-- Reason -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-align-left me-1 text-primary"></i>Reason <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="2" MaxLength="500" placeholder="Enter reason for adjustment" />
                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" 
                            ControlToValidate="txtReason" CssClass="text-danger small mt-1"
                            ErrorMessage="Reason is required." Display="Dynamic" />
                    </div>
                </div>
            </div>

            <!-- Current and New Quantity Comparison -->
            <div class="row mt-2" id="divComparison" runat="server" visible="false">
                <div class="col-12">
                    <div class="card bg-light">
                        <div class="card-body">
                            <div class="row text-center">
                                <div class="col-md-4">
                                    <h6 class="text-muted">Current Quantity</h6>
                                    <h3 class="fw-bold text-primary"><asp:Literal ID="ltrCurrentQtyDisplay" runat="server" Text="0" /></h3>
                                </div>
                                <div class="col-md-4">
                                    <h6 class="text-muted">Adjustment</h6>
                                    <h3 class="fw-bold text-warning">
                                        <asp:Literal ID="ltrAdjustmentDisplay" runat="server" Text="0" />
                                    </h3>
                                </div>
                                <div class="col-md-4">
                                    <h6 class="text-muted">New Quantity</h6>
                                    <h3 class="fw-bold text-success"><asp:Literal ID="ltrNewQtyDisplay" runat="server" Text="0" /></h3>
                                </div>
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
                        <asp:Button ID="btnAdjust" runat="server" CssClass="btn btn-primary px-4" OnClick="btnAdjust_Click" Text="Apply Adjustment" />
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
            // Calculate new quantity
            $('#<%= txtAdjustmentQty.ClientID %>, #<%= ddlAdjustmentType.ClientID %>').on('input change', function() {
                calculateNewQuantity();
            });

            function calculateNewQuantity() {
                var current = parseFloat($('#<%= txtCurrentQuantity.ClientID %>').val()) || 0;
                var adjustment = parseFloat($('#<%= txtAdjustmentQty.ClientID %>').val()) || 0;
                var type = $('#<%= ddlAdjustmentType.ClientID %>').val();
                var newQty = current;

                if (type === 'Add') {
                    newQty = current + adjustment;
                } else if (type === 'Remove') {
                    newQty = current - adjustment;
                } else if (type === 'Set') {
                    newQty = adjustment;
                }

                if (newQty < 0) newQty = 0;
                $('#<%= txtNewQuantity.ClientID %>').val(newQty.toFixed(2));
                $('#<%= ltrCurrentQtyDisplay.ClientID %>').text(current.toFixed(2));
                $('#<%= ltrAdjustmentDisplay.ClientID %>').text(adjustment.toFixed(2));
                $('#<%= ltrNewQtyDisplay.ClientID %>').text(newQty.toFixed(2));
                $('#<%= divComparison.ClientID %>').show();
            }

            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>