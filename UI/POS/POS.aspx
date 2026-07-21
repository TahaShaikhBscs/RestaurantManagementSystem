<%@ Page Title="Point of Sale" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="POS.aspx.cs" Inherits="RestaurantManagementSystem.UI.POS.POS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Point of Sale - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Toast Container for server-side messages -->
    <div class="toast-container position-fixed bottom-0 end-0 p-3" style="z-index: 9999;">
        <asp:Panel ID="pnlToast" runat="server" Visible="false" CssClass="toast align-items-center text-white border-0 show" role="alert">
            <div class="d-flex">
                <div class="toast-body">
                    <asp:Literal ID="ltrToastMessage" runat="server" />
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </asp:Panel>
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

    <!-- POS Main Container -->
    <div class="pos-container">
        <!-- Left Panel - Menu -->
        <div class="pos-menu-panel">
            <!-- Header -->
            <div class="pos-menu-header">
                <div class="row g-2">
                    <div class="col-md-6">
                        <div class="input-group">
                            <span class="input-group-text bg-white">
                                <i class="fas fa-search text-muted"></i>
                            </span>
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
                                placeholder="Search items..." AutoPostBack="true"
                                OnTextChanged="txtSearch_TextChanged" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            <asp:ListItem Value="">All Categories</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnRefreshMenu" runat="server" Text="Refresh"
                            CssClass="btn btn-primary w-100" OnClick="btnRefreshMenu_Click" />
                    </div>
                </div>
            </div>

            <!-- Menu Items Grid -->
            <div class="pos-items-grid">
                <asp:Repeater ID="rptMenuItems" runat="server" OnItemCommand="rptMenuItems_ItemCommand">
                    <ItemTemplate>
                        <div class="pos-item-card">
                            <div class="item-icon">
                                <i class="fas fa-utensils"></i>
                            </div>
                            <div class="item-name"><%# Eval("ItemName") %></div>
                            <span class="item-category"><%# Eval("CategoryName") %></span>
                            <div class="item-price">$<%# Eval("Price", "{0:F2}") %></div>
                            <asp:HiddenField ID="hfMenuItemID" runat="server" Value='<%# Eval("MenuItemID") %>' />
                            <asp:HiddenField ID="hfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                            <asp:HiddenField ID="hfPrice" runat="server" Value='<%# Eval("Price") %>' />
                            <asp:Button ID="btnAddItem" runat="server" CssClass="btn-add-item"
                                CommandName="AddItem" CommandArgument='<%# Eval("MenuItemID") %>' Text="+" />
                            <%# Eval("IsAvailable") != DBNull.Value && Convert.ToBoolean(Eval("IsAvailable"))
    ? ""
    : "<div class='item-unavailable'>Unavailable</div>" %>
                        </div>
                    </ItemTemplate>
                    <%--<emptydatatemplate>
                        <div class="text-center text-muted p-4">No items found</div>
                    </emptydatatemplate>--%>
                </asp:Repeater>
                <asp:Panel ID="pnlNoItems" runat="server" Visible="false" CssClass="text-center text-muted p-4">
                    No items found
                </asp:Panel>
            </div>

            <!-- Deals Section -->
            <div class="pos-deals-section mt-2">
                <h6 class="fw-bold mb-2"><i class="fas fa-tags text-success me-1"></i>Deals & Combos</h6>
                <div class="pos-items-grid deals-grid">
                    <asp:Repeater ID="rptDeals" runat="server" OnItemCommand="rptDeals_ItemCommand">
                        <ItemTemplate>
                            <div class="pos-item-card deal-card">
                                <div class="item-icon" style="color: #28a745;">
                                    <i class="fas fa-tags"></i>
                                </div>
                                <div class="item-name"><%# Eval("DealName") %></div>
                                <span class="item-category"><%# Eval("ItemCount") %> items</span>
                                <div class="item-price">$<%# Eval("DealPrice", "{0:F2}") %></div>
                                <asp:HiddenField ID="hfDealID" runat="server" Value='<%# Eval("DealID") %>' />
                                <asp:HiddenField ID="hfDealName" runat="server" Value='<%# Eval("DealName") %>' />
                                <asp:HiddenField ID="hfDealPrice" runat="server" Value='<%# Eval("DealPrice") %>' />
                                <asp:Button ID="btnAddDeal" runat="server" CssClass="btn-add-item"
                                    CommandName="AddDeal" CommandArgument='<%# Eval("DealID") %>' Text="+" />
                            </div>
                        </ItemTemplate>
                        <%--<emptydatatemplate>
                            <div class="text-center text-muted p-2">No deals available</div>
                        </emptydatatemplate>--%>
                    </asp:Repeater>
                   <asp:Panel ID="pnlNoDeals" runat="server"
    Visible="false"
    CssClass="text-center text-muted p-2">

    No deals available

</asp:Panel>
                </div>
            </div>
        </div>

        <!-- Right Panel - Cart -->
        <div class="pos-cart-panel">
            <!-- Cart Header -->
            <div class="pos-cart-header">
                <h5><i class="fas fa-shopping-cart me-2"></i>Cart</h5>
                <div>
                    <asp:Button ID="btnClearCart" runat="server" Text="Clear"
                        CssClass="btn btn-sm btn-outline-danger" OnClick="btnClearCart_Click"
                        OnClientClick="return confirm('Clear all items from cart?');" />
                </div>
            </div>

            <!-- Cart Items -->
            <div class="pos-cart-items">
                <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-sm table-hover" GridLines="None"
                    OnRowCommand="gvCart_RowCommand" DataKeyNames="MenuItemID">
                    <Columns>
                        <asp:BoundField DataField="ItemName" HeaderText="Item" ItemStyle-Width="40%" />
                        <asp:TemplateField HeaderText="Qty" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="btn-group btn-group-sm">
                                    <asp:Button ID="btnDecrease" runat="server" Text="-" CssClass="btn btn-outline-secondary"
                                        CommandName="DecreaseQty" CommandArgument='<%# Container.DataItemIndex %>' />
                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>' CssClass="px-2 fw-bold" />
                                    <asp:Button ID="btnIncrease" runat="server" Text="+" CssClass="btn btn-outline-secondary"
                                        CommandName="IncreaseQty" CommandArgument='<%# Container.DataItemIndex %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TotalPrice" HeaderText="Total" ItemStyle-Width="20%"
                            DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                        <asp:TemplateField ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnRemove" runat="server" Text="X" CssClass="btn btn-sm btn-danger"
                                    CommandName="RemoveItem" CommandArgument='<%# Container.DataItemIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center text-muted p-4">Cart is empty</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <!-- Order Details -->
            <div class="row g-2">
                <div class="col-6">
                    <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-select form-select-sm"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged">
                        <asp:ListItem Value="Dine In">Dine In</asp:ListItem>
                        <asp:ListItem Value="Take Away">Take Away</asp:ListItem>
                        <asp:ListItem Value="Delivery">Delivery</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-6">
                    <asp:DropDownList ID="ddlTable" runat="server" CssClass="form-select form-select-sm">
                        <asp:ListItem Value="">Select Table</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-12">
                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-select form-select-sm">
                        <asp:ListItem Value="">Walk-in</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-12" id="divDeliveryAddress" runat="server" style="display: none;">
                    <asp:TextBox ID="txtDeliveryAddress" runat="server" CssClass="form-control form-control-sm"
                        placeholder="Delivery address" />
                </div>
                <div class="col-12">
                    <asp:TextBox ID="txtSpecialInstructions" runat="server" CssClass="form-control form-control-sm"
                        placeholder="Special instructions" />
                </div>
            </div>

            <!-- Summary -->
            <div class="pos-cart-summary">
                <div class="summary-row">
                    <span>Sub Total</span>
                    <span>
                        <asp:Literal ID="ltrSubTotal" runat="server" Text="$0.00" /></span>
                </div>
                <div class="summary-row">
                    <span>Tax (5%)</span>
                    <span>
                        <asp:Literal ID="ltrTax" runat="server" Text="$0.00" /></span>
                </div>
                <div class="summary-row">
                    <span>Discount</span>
                    <span>
                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control form-control-sm"
                            Text="0" Width="80px" AutoPostBack="true" OnTextChanged="txtDiscount_TextChanged" />
                    </span>
                </div>
                <div class="summary-row">
                    <span>Service Charge</span>
                    <span>
                        <asp:TextBox ID="txtServiceCharge" runat="server" CssClass="form-control form-control-sm"
                            Text="0" Width="80px" AutoPostBack="true" OnTextChanged="txtServiceCharge_TextChanged" />
                    </span>
                </div>
                <div class="summary-row total">
                    <span>Total</span>
                    <span class="amount">
                        <asp:Literal ID="ltrTotal" runat="server" Text="$0.00" /></span>
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="pos-order-controls">
                <asp:Button ID="btnHold" runat="server" Text="Hold" CssClass="btn btn-outline-secondary"
                    OnClick="btnHold_Click" />
                <asp:Button ID="btnVoid" runat="server" Text="Void" CssClass="btn btn-outline-danger"
                    OnClick="btnVoid_Click" />
                <asp:Button ID="btnSendKitchen" runat="server" Text="Kitchen" CssClass="btn btn-primary"
                    OnClick="btnSendKitchen_Click" />
                <asp:Button ID="btnPayment" runat="server" Text="Pay" CssClass="btn btn-success"
                    OnClick="btnPayment_Click" />
            </div>
        </div>
    </div>

    <!-- Payment Modal -->
    <div class="modal fade pos-payment-modal" id="paymentModal" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"><i class="fas fa-credit-card me-2"></i>Payment</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <!-- Order Summary -->
                    <div class="row">
                        <div class="col-md-6">
                            <h6>Order Summary</h6>
                            <div class="table-responsive">
                                <asp:GridView ID="gvPaymentItems" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-sm" GridLines="None">
                                    <Columns>
                                        <asp:BoundField DataField="ItemName" HeaderText="Item" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Qty" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TotalPrice" HeaderText="Total" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="text-end">
                                <strong>Total:
                                    <asp:Literal ID="ltrPaymentTotal" runat="server" Text="$0.00" /></strong>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <h6>Payment Method</h6>
                            <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">Select Method</asp:ListItem>
                            </asp:DropDownList>

                            <div class="mt-3">
                                <label class="form-label">Amount Paid</label>
                                <div class="input-group">
                                    <span class="input-group-text">$</span>
                                    <asp:TextBox ID="txtPaidAmount" runat="server" CssClass="form-control form-control-lg"
                                        Text="0.00" AutoPostBack="true" OnTextChanged="txtPaidAmount_TextChanged" />
                                </div>
                            </div>
                            <div class="mt-2">
                                <label class="form-label">Change</label>
                                <div class="input-group">
                                    <span class="input-group-text">$</span>
                                    <asp:TextBox ID="txtChange" runat="server" CssClass="form-control form-control-lg"
                                        Text="0.00" ReadOnly="true" BackColor="#f8f9fa" />
                                </div>
                            </div>
                            <div class="mt-2">
                                <label class="form-label">Reference Number</label>
                                <asp:TextBox ID="txtReference" runat="server" CssClass="form-control"
                                    placeholder="Optional reference" />
                            </div>
                        </div>
                    </div>

                    <!-- Actions -->
                    <div class="d-flex gap-2 mt-3">
                        <asp:Button ID="btnConfirmPayment" runat="server" Text="Confirm Payment"
                            CssClass="btn btn-success w-100" OnClick="btnConfirmPayment_Click" />
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden Fields -->
    <asp:HiddenField ID="hfOrderID" runat="server" Value="0" />
    <asp:HiddenField ID="hfShiftID" runat="server" Value="0" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <link href="/Content/POS.css" rel="stylesheet" />

    <script type="text/javascript">
        // Only Bootstrap and minimal JavaScript for modals
        $(document).ready(function () {
            // Auto-hide alerts after 5 seconds
            setTimeout(function () {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>
