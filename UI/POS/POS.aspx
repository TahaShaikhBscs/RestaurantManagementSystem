<%@ Page Title="Point of Sale" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="POS.aspx.cs" Inherits="RestaurantManagementSystem.UI.POS.POS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Point of Sale - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-cash-register text-primary me-2"></i>Point of Sale
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item active" aria-current="page">POS</li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <span class="badge bg-primary rounded-pill me-2">
                        <i class="fas fa-clock me-1"></i>
                        <asp:Literal ID="ltrCurrentTime" runat="server" />
                    </span>
                    <asp:Button ID="btnNewOrder" runat="server" Text="New Order" CssClass="btn btn-success" OnClick="btnNewOrder_Click" />
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

    <!-- POS Main Layout -->
    <div class="row g-3">
        <!-- Left Panel - Menu Items -->
        <div class="col-lg-8">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-white py-2">
                    <div class="row g-2 align-items-center">
                        <div class="col-md-5">
                            <div class="input-group input-group-sm">
                                <span class="input-group-text bg-white">
                                    <i class="fas fa-search text-muted"></i>
                                </span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search items..." OnTextChanged="txtSearch_TextChanged" AutoPostBack="true" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                <asp:ListItem Value="">All Categories</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 text-end">
                            <span class="badge bg-primary rounded-pill">
                                <asp:Literal ID="ltrItemCount" runat="server" Text="0" />
                                Items
                            </span>
                        </div>
                    </div>
                </div>
                <div class="card-body p-2" style="max-height: 550px; overflow-y: auto;">
                    <div class="row g-2">
                        <asp:Repeater ID="rptMenuItems" runat="server" OnItemCommand="rptMenuItems_ItemCommand">
                            <ItemTemplate>
                                <div class="col-lg-3 col-md-4 col-sm-6">
                                    <div class="card pos-item h-100" onclick="addToCart(<%# Eval("MenuItemID") %>)">
                                        <div class="card-body text-center p-2">
                                            <div class="menu-item-image bg-light rounded mb-1" style="height: 60px; display: flex; align-items: center; justify-content: center;">
                                                <i class="fas fa-utensils fa-2x text-muted"></i>
                                            </div>
                                            <h6 class="mb-0 small fw-bold"><%# Eval("ItemName") %></h6>
                                            <span class="badge bg-secondary small"><%# Eval("CategoryName") %></span>
                                            <div class="mt-1">
                                                <span class="fw-bold text-success"><%# Eval("Price", "{0:C}") %></span>
                                                <asp:HiddenField ID="hfItemID" runat="server" Value='<%# Eval("MenuItemID") %>' />
                                                <asp:HiddenField ID="hfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                                                <asp:HiddenField ID="hfPrice" runat="server" Value='<%# Eval("Price") %>' />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
        <!-- ============================================ -->
        <!-- UI/POS/POS.aspx - Add Deals Section -->
        <!-- ============================================ -->

        <!-- Add this after the menu items section in the left panel -->

        <!-- Deals Section -->
        <div class="card border-0 shadow-sm mt-3">
            <div class="card-header bg-white py-2">
                <div class="d-flex justify-content-between align-items-center">
                    <h6 class="mb-0">
                        <i class="fas fa-tags me-1 text-primary"></i>Deals & Combos
                    </h6>
                    <span class="badge bg-primary rounded-pill">
                        <asp:Literal ID="ltrDealCount" runat="server" Text="0" />
                    </span>
                </div>
            </div>
            <div class="card-body p-2" style="max-height: 200px; overflow-y: auto;">
                <div class="row g-2">
                    <asp:Repeater ID="rptDeals" runat="server" OnItemCommand="rptDeals_ItemCommand">
                        <ItemTemplate>
                            <div class="col-lg-4 col-md-6 col-sm-6">
                                <div class="card pos-item h-100" onclick="addDealToCart(<%# Eval("DealID") %>)">
                                    <div class="card-body text-center p-2">
                                        <div class="deal-icon bg-light rounded mb-1" style="height: 40px; display: flex; align-items: center; justify-content: center;">
                                            <i class="fas fa-tag fa-2x text-success"></i>
                                        </div>
                                        <h6 class="mb-0 small fw-bold"><%# Eval("DealName") %></h6>
                                        <span class="badge bg-info small"><%# Eval("ItemCount") %> items</span>
                                        <div class="mt-1">
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
        </div>

        <!-- Right Panel - Cart -->
        <div class="col-lg-4">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-white py-2">
                    <div class="d-flex justify-content-between align-items-center">
                        <h6 class="mb-0">
                            <i class="fas fa-shopping-cart me-1 text-primary"></i>Current Order
                        </h6>
                        <div>
                            <asp:Button ID="btnClearCart" runat="server" Text="Clear" CssClass="btn btn-sm btn-danger" OnClick="btnClearCart_Click" />
                        </div>
                    </div>
                </div>
                <div class="card-body p-0" style="max-height: 250px; overflow-y: auto;">
                    <asp:Panel ID="pnlEmptyCart" runat="server" Visible="false">
                        <div class="text-center py-4">
                            <i class="fas fa-shopping-cart fa-3x text-muted mb-2 d-block"></i>
                            <p class="text-muted small">Cart is empty</p>
                        </div>
                    </asp:Panel>
                    <asp:Repeater ID="rptCart" runat="server" OnItemCommand="rptCart_ItemCommand">
                        <ItemTemplate>
                            <div class="cart-item d-flex justify-content-between align-items-center">
                                <div class="flex-grow-1">
                                    <div class="d-flex align-items-center">
                                        <span class="fw-bold me-2"><%# Eval("Quantity") %>x</span>
                                        <span><%# Eval("ItemName") %></span>
                                    </div>
                                    <small class="text-muted"><%# Eval("TotalPrice", "{0:C}") %></small>
                                </div>
                                <div class="btn-group btn-group-sm">
                                    <asp:Button ID="btnRemove" runat="server" Text="-" CssClass="btn btn-outline-danger"
                                        CommandName="RemoveItem" CommandArgument='<%# Eval("MenuItemID") %>' />
                                    <asp:Button ID="btnAdd" runat="server" Text="+" CssClass="btn btn-outline-success"
                                        CommandName="AddItem" CommandArgument='<%# Eval("MenuItemID") %>' />
                                </div>
                            </div>
                        </ItemTemplate>
                        <%-- <emptydatatemplate>
                            <div class="text-center py-4">
                                <i class="fas fa-shopping-cart fa-3x text-muted mb-2 d-block"></i>
                                <p class="text-muted small">Cart is empty</p>
                            </div>
                        </emptydatatemplate>--%>
                    </asp:Repeater>
                </div>
                <div class="card-footer bg-white p-2">
                    <!-- Order Summary -->
                    <div class="row g-1 small">
                        <div class="col-6">Sub Total:</div>
                        <div class="col-6 text-end">
                            <asp:Literal ID="ltrSubTotal" runat="server" Text="0.00" />
                        </div>

                        <div class="col-6">Tax (5%):</div>
                        <div class="col-6 text-end">
                            <asp:Literal ID="ltrTax" runat="server" Text="0.00" />
                        </div>

                        <div class="col-6">Discount:</div>
                        <div class="col-6 text-end">
                            <asp:Literal ID="ltrDiscount" runat="server" Text="0.00" />
                        </div>

                        <div class="col-6 fw-bold">Total:</div>
                        <div class="col-6 text-end fw-bold text-success">
                            <asp:Literal ID="ltrTotal" runat="server" Text="0.00" />
                        </div>
                    </div>

                    <hr class="my-1" />

                    <!-- Order Details -->
                    <div class="row g-1 small">
                        <div class="col-6">Order Type:</div>
                        <div class="col-6 text-end">
                            <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-select form-select-sm">
                                <asp:ListItem Value="Dine In">Dine In</asp:ListItem>
                                <asp:ListItem Value="Take Away">Take Away</asp:ListItem>
                                <asp:ListItem Value="Delivery">Delivery</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-6">Table:</div>
                        <div class="col-6 text-end">
                            <asp:DropDownList ID="ddlTable" runat="server" CssClass="form-select form-select-sm">
                                <asp:ListItem Value="">Select Table</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-6">Customer:</div>
                        <div class="col-6 text-end">
                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control form-control-sm" placeholder="Walk-in" />
                                <asp:Button ID="btnFindCustomer" runat="server" Text="Find" CssClass="btn btn-sm btn-info" OnClick="btnFindCustomer_Click" />
                            </div>
                        </div>
                    </div>

                    <hr class="my-1" />

                    <!-- Payment -->
                    <div class="row g-1 small">
                        <div class="col-6">Payment Method:</div>
                        <div class="col-6 text-end">
                            <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-select form-select-sm">
                                <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                <asp:ListItem Value="Card">Card</asp:ListItem>
                                <asp:ListItem Value="Online">Online</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-6">Amount Paid:</div>
                        <div class="col-6 text-end">
                            <asp:TextBox ID="txtPaidAmount" runat="server" CssClass="form-control form-control-sm" Text="0.00" />
                        </div>

                        <div class="col-6">Change:</div>
                        <div class="col-6 text-end">
                            <asp:Literal ID="ltrChange" runat="server" Text="0.00" />
                        </div>
                    </div>

                    <!-- Action Buttons -->
                    <div class="d-grid gap-1 mt-2">
                        <asp:Button ID="btnProcessOrder" runat="server" Text="Process Order" CssClass="btn btn-success" OnClick="btnProcessOrder_Click" />
                        <asp:Button ID="btnHoldOrder" runat="server" Text="Hold Order" CssClass="btn btn-warning" OnClick="btnHoldOrder_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden Fields -->
    <asp:HiddenField ID="hfOrderID" runat="server" Value="0" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            // Update time
            function updateTime() {
                var now = new Date();
                $('#<%= ltrCurrentTime.ClientID %>').text(now.toLocaleTimeString());
            }
            updateTime();
            setInterval(updateTime, 1000);

            // Cart item click handler
            window.addToCart = function (itemId) {
                __doPostBack('rptMenuItems', 'Add$' + itemId);
            };

            // Calculate change
            $('#<%= txtPaidAmount.ClientID %>').on('input', function () {
                var total = parseFloat($('#<%= ltrTotal.ClientID %>').text().replace(/[^0-9.]/g, '')) || 0;
                var paid = parseFloat($(this).val()) || 0;
                var change = paid - total;
                $('#<%= ltrChange.ClientID %>').text(change >= 0 ? change.toFixed(2) : '0.00');
            });
        });
    </script>
</asp:Content>
