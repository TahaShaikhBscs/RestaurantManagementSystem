
<%@ Page Title="Order Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="OrderAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Orders.OrderAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Order Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-receipt text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Order Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="OrderList.aspx">Orders</a></li>
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

    <!-- Order Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Order Information" />
            </h5>
        </div>
        <div class="card-body">
            <!-- Order Details Row -->
            <div class="row">
                <!-- Left Column - Order Info -->
                <div class="col-md-6">
                    <!-- Order Number (Read-only) -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-hashtag me-1 text-primary"></i>Order Number
                        </label>
                        <asp:TextBox ID="txtOrderNumber" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" />
                    </div>

                    <!-- Order Type -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-utensils me-1 text-primary"></i>Order Type <span class="text-danger">*</span>
                        </label>
                        <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged">
                            <asp:ListItem Value="Dine In">Dine In</asp:ListItem>
                            <asp:ListItem Value="Take Away">Take Away</asp:ListItem>
                            <asp:ListItem Value="Delivery">Delivery</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Table (for Dine In) -->
                    <div class="mb-3" id="divTable" runat="server">
                        <label class="form-label fw-bold">
                            <i class="fas fa-chair me-1 text-primary"></i>Table
                        </label>
                        <asp:DropDownList ID="ddlTable" runat="server" CssClass="form-select"
                            DataTextField="TableNumber" DataValueField="TableID" AppendDataBoundItems="True">
                            <asp:ListItem Value="">-- Select Table --</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Delivery Address (for Delivery) -->
                    <div class="mb-3" id="divDeliveryAddress" runat="server" style="display:none;">
                        <label class="form-label fw-bold">
                            <i class="fas fa-map-marker-alt me-1 text-primary"></i>Delivery Address <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtDeliveryAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" 
                            placeholder="Enter delivery address" />
                    </div>

                    <!-- Order Status -->
                    <div class="mb-3" id="divOrderStatus" runat="server" visible="false">
                        <label class="form-label fw-bold">
                            <i class="fas fa-info-circle me-1 text-primary"></i>Order Status
                        </label>
                        <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Cooking">Cooking</asp:ListItem>
                            <asp:ListItem Value="Ready">Ready</asp:ListItem>
                            <asp:ListItem Value="Completed">Completed</asp:ListItem>
                            <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Right Column - Customer Info -->
                <div class="col-md-6">
                    <!-- Customer -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-user me-1 text-primary"></i>Customer
                        </label>
                        <div class="row g-2">
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-select"
                                    DataTextField="CustomerName" DataValueField="CustomerID" AppendDataBoundItems="True">
                                    <asp:ListItem Value="">-- Walk-in Customer --</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnNewCustomer" runat="server" Text="New" CssClass="btn btn-primary w-100" OnClick="btnNewCustomer_Click" />
                            </div>
                        </div>
                    </div>

                    <!-- Customer Phone (Auto-fill) -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-phone me-1 text-primary"></i>Customer Phone
                        </label>
                        <asp:TextBox ID="txtCustomerPhone" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" />
                    </div>

                    <!-- Special Instructions -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-comment me-1 text-primary"></i>Special Instructions
                        </label>
                        <asp:TextBox ID="txtSpecialInstructions" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" 
                            placeholder="Any special instructions for this order" />
                    </div>
                </div>
            </div>

            <hr />

            <!-- Menu Items Section -->
            <div class="row mt-3">
                <div class="col-12">
                    <h6 class="fw-bold mb-3">
                        <i class="fas fa-utensils me-2 text-primary"></i>Menu Items
                    </h6>
                    
                    <!-- Add Item Row -->
                    <div class="row g-2 mb-3">
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCategoryFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged">
                                <asp:ListItem Value="">All Categories</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-5">
                            <div class="input-group">
                                <span class="input-group-text bg-white">
                                    <i class="fas fa-search text-muted"></i>
                                </span>
                                <asp:TextBox ID="txtSearchItem" runat="server" CssClass="form-control" placeholder="Search menu items..." />
                                <asp:Button ID="btnSearchItem" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearchItem_Click" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnAddDeal" runat="server" Text="Add Deal" CssClass="btn btn-success w-100" OnClick="btnAddDeal_Click" />
                        </div>
                    </div>

                    <!-- Available Items Grid -->
                    <div class="card bg-light border-0 mb-3">
                        <div class="card-body p-2" style="max-height: 200px; overflow-y: auto;">
                            <div class="row g-1">
                                <asp:Repeater ID="rptMenuItems" runat="server" OnItemCommand="rptMenuItems_ItemCommand">
                                    <ItemTemplate>
                                        <div class="col-lg-2 col-md-3 col-sm-4">
                                            <div class="card menu-item-card h-100" onclick="addMenuItem(<%# Eval("MenuItemID") %>)">
                                                <div class="card-body text-center p-1">
                                                    <small class="fw-bold"><%# Eval("ItemName") %></small>
                                                    <div>
                                                        <span class="badge bg-secondary"><%# Eval("CategoryName") %></span>
                                                    </div>
                                                    <div class="mt-1">
                                                        <span class="fw-bold text-success"><%# Eval("Price", "{0:C}") %></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>

                    <!-- Selected Items Grid -->
                    <div class="table-responsive">
                        <asp:GridView ID="gvOrderItems" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-hover table-striped" GridLines="None"
                            OnRowCommand="gvOrderItems_RowCommand" OnRowDataBound="gvOrderItems_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ItemType" HeaderText="Type" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ItemName" HeaderText="Item" ItemStyle-Width="30%" />
                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div class="input-group input-group-sm">
                                            <asp:Button ID="btnDecrease" runat="server" Text="-" CssClass="btn btn-outline-secondary"
                                                CommandName="DecreaseQty" CommandArgument='<%# Container.DataItemIndex %>' />
                                            <asp:TextBox ID="txtQty" runat="server" Text='<%# Eval("Quantity") %>' CssClass="form-control text-center" 
                                                Width="50px" OnTextChanged="txtQty_TextChanged" AutoPostBack="true" />
                                            <asp:Button ID="btnIncrease" runat="server" Text="+" CssClass="btn btn-outline-secondary"
                                                CommandName="IncreaseQty" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" ItemStyle-Width="12%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="TotalPrice" HeaderText="Total" ItemStyle-Width="12%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderText="Notes" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtItemNote" runat="server" Text='<%# Eval("Note") %>' CssClass="form-control form-control-sm" 
                                            placeholder="Special request" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnRemove" runat="server" Text="X" CssClass="btn btn-sm btn-danger"
                                            CommandName="RemoveItem" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center py-3">
                                    <p class="text-muted">No items added to this order</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Totals -->
            <div class="row mt-3">
                <div class="col-md-6 offset-md-6">
                    <div class="table-responsive">
                        <table class="table table-borderless">
                            <tr>
                                <td><strong>Sub Total:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrSubTotal" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr>
                                <td><strong>Tax (5%):</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrTax" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr>
                                <td><strong>Discount:</strong></td>
                                <td class="text-end">
                                    <div class="input-group input-group-sm justify-content-end">
                                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control form-control-sm text-end" 
                                            Width="100px" Text="0.00" OnTextChanged="txtDiscount_TextChanged" AutoPostBack="true" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td><strong>Service Charge:</strong></td>
                                <td class="text-end">
                                    <div class="input-group input-group-sm justify-content-end">
                                        <asp:TextBox ID="txtServiceCharge" runat="server" CssClass="form-control form-control-sm text-end" 
                                            Width="100px" Text="0.00" OnTextChanged="txtServiceCharge_TextChanged" AutoPostBack="true" />
                                    </div>
                                </td>
                            </tr>
                            <tr class="table-primary">
                                <td><strong>Grand Total:</strong></td>
                                <td class="text-end fw-bold"><asp:Literal ID="ltrGrandTotal" runat="server" Text="0.00" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <hr />

            <!-- Payment Section -->
            <div class="row">
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-credit-card me-1 text-primary"></i>Payment Method
                        </label>
                        <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Cash">Cash</asp:ListItem>
                            <asp:ListItem Value="Card">Card</asp:ListItem>
                            <asp:ListItem Value="Online">Online</asp:ListItem>
                            <asp:ListItem Value="Mobile Wallet">Mobile Wallet</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-dollar-sign me-1 text-primary"></i>Amount Paid
                        </label>
                        <asp:TextBox ID="txtPaidAmount" runat="server" CssClass="form-control" Text="0.00" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-exchange-alt me-1 text-primary"></i>Change
                        </label>
                        <asp:TextBox ID="txtChange" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" Text="0.00" />
                    </div>
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="row mt-4">
                <div class="col-12">
                    <hr />
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" Text="Save Order" />
                        <asp:Button ID="btnSaveAndPrint" runat="server" CssClass="btn btn-primary px-4" OnClick="btnSaveAndPrint_Click" Text="Save &amp; Print" />
                        <asp:Button ID="btnCancel2" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Add Deal Modal -->
    <div class="modal fade" id="dealModal" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add Deal / Combo</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <div class="row g-3">
                        <asp:Repeater ID="rptDeals" runat="server" OnItemCommand="rptDeals_ItemCommand">
                            <ItemTemplate>
                                <div class="col-md-4">
                                    <div class="card deal-card" onclick="selectDeal(<%# Eval("DealID") %>)">
                                        <div class="card-body text-center">
                                            <h6 class="fw-bold"><%# Eval("DealName") %></h6>
                                            <span class="badge bg-info"><%# Eval("ItemCount") %> items</span>
                                            <div class="mt-2">
                                                <span class="fw-bold text-success"><%# Eval("DealPrice", "{0:C}") %></span>
                                                <span class="text-muted small"><strike><%# Eval("OriginalPrice", "{0:C}") %></strike></span>
                                            </div>
                                            <asp:HiddenField ID="hfDealID" runat="server" Value='<%# Eval("DealID") %>' />
                                            <asp:HiddenField ID="hfDealName" runat="server" Value='<%# Eval("DealName") %>' />
                                            <asp:HiddenField ID="hfDealPrice" runat="server" Value='<%# Eval("DealPrice") %>' />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden Fields -->
    <asp:HiddenField ID="hfOrderID" runat="server" Value="0" />
    <asp:HiddenField ID="hfSelectedDealID" runat="server" Value="0" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <style>
        .menu-item-card {
            cursor: pointer;
            transition: all 0.2s;
        }
        .menu-item-card:hover {
            transform: scale(1.02);
            border-color: #667eea;
        }
        .deal-card {
            cursor: pointer;
            transition: all 0.2s;
        }
        .deal-card:hover {
            transform: scale(1.02);
            border-color: #28a745;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            // Calculate change
            $('#<%= txtPaidAmount.ClientID %>').on('input', function () {
                var total = parseFloat($('#<%= ltrGrandTotal.ClientID %>').text()) || 0;
                var paid = parseFloat($(this).val()) || 0;
                var change = paid - total;
                $('#<%= txtChange.ClientID %>').val(change >= 0 ? change.toFixed(2) : '0.00');
            });

            // Auto-hide alerts
            setTimeout(function () {
                $('.alert').fadeOut('slow');
            }, 5000);

            // Add menu item
            window.addMenuItem = function (menuItemID) {
                __doPostBack('rptMenuItems', 'Add$' + menuItemID);
            };

            // Select deal
            window.selectDeal = function (dealID) {
                $('#<%= hfSelectedDealID.ClientID %>').val(dealID);
                __doPostBack('rptDeals', 'AddDeal$' + dealID);
            };
        });
    </script>
</asp:Content>