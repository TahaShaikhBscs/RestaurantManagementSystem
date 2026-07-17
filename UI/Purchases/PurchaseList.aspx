
<%@ Page Title="Purchase Management" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="PurchaseList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Purchases.PurchaseList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Purchase Management - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-shopping-cart text-primary me-2"></i>Purchase Management
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Purchases</li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnAddNew" runat="server" Text="New Purchase Order" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Search and Filter Section -->
    <div class="row mb-3">
        <div class="col-md-8">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-3">
                    <div class="row g-2 align-items-center">
                        <div class="col-md-4">
                            <div class="input-group">
                                <span class="input-group-text bg-white">
                                    <i class="fas fa-search text-muted"></i>
                                </span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by PO # or supplier..." />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                                <asp:ListItem Value="">All Status</asp:ListItem>
                                <asp:ListItem Value="Draft">Draft</asp:ListItem>
                                <asp:ListItem Value="Sent">Sent</asp:ListItem>
                                <asp:ListItem Value="Received">Received</asp:ListItem>
                                <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" TextMode="Date" placeholder="From Date" />
                        </div>
                        <div class="col-md-2">
                            <div class="d-flex gap-2">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-warning" OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4 text-end">
            <span class="text-muted small">
                <i class="fas fa-info-circle me-1"></i>
                Manage purchase orders from suppliers
            </span>
        </div>
    </div>

    <!-- Statistics -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="row text-center">
                        <div class="col-md-3">
                            <span class="badge bg-primary rounded-pill px-3 py-2">
                                <i class="fas fa-list me-1"></i>
                                Total: <asp:Literal ID="ltrTotalOrders" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-warning rounded-pill px-3 py-2">
                                <i class="fas fa-clock me-1"></i>
                                Pending: <asp:Literal ID="ltrPendingOrders" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-success rounded-pill px-3 py-2">
                                <i class="fas fa-check-circle me-1"></i>
                                Received: <asp:Literal ID="ltrReceivedOrders" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-danger rounded-pill px-3 py-2">
                                <i class="fas fa-times-circle me-1"></i>
                                Cancelled: <asp:Literal ID="ltrCancelledOrders" runat="server" Text="0" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Grid View -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center">
            <h5 class="mb-0">
                <i class="fas fa-list-ul me-2 text-primary"></i>Purchase Orders
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrTotalCount" runat="server" Text="0" /> Orders
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvPurchaseOrders" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    OnRowCommand="gvPurchaseOrders_RowCommand" OnPageIndexChanging="gvPurchaseOrders_PageIndexChanging"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination">
                    <Columns>
                        <asp:BoundField DataField="PurchaseOrderID" HeaderText="ID" Visible="False" />
                        <asp:BoundField DataField="PurchaseOrderNumber" HeaderText="PO #" ItemStyle-Width="12%" />
                        <asp:BoundField DataField="SupplierName" HeaderText="Supplier" ItemStyle-Width="20%" />
                        <asp:BoundField DataField="OrderDate" HeaderText="Date" ItemStyle-Width="12%" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# GetStatusBadgeClass(Eval("OrderStatus").ToString()) %> px-3 py-2">
                                    <i class="fas fa-<%# GetStatusIconClass(Eval("OrderStatus").ToString()) %> me-1"></i>
                                    <%# Eval("OrderStatus") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total" ItemStyle-Width="12%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                        <asp:TemplateField HeaderText="Payment" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# GetPaymentStatusBadgeClass(Eval("PaymentStatus").ToString()) %> px-3 py-2">
                                    <%# Eval("PaymentStatus") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="btn-group btn-group-sm" role="group">
                                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-info"
                                        CommandName="ViewOrder" CommandArgument='<%# Eval("PurchaseOrderID") %>' />
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary"
                                        CommandName="EditOrder" CommandArgument='<%# Eval("PurchaseOrderID") %>' />
                                    <asp:Button ID="btnReceive" runat="server" Text="Receive" CssClass="btn btn-success"
                                        CommandName="ReceiveOrder" CommandArgument='<%# Eval("PurchaseOrderID") %>'
                                        OnClientClick="return confirm('Receive this purchase order and update inventory?');" />
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                                        CommandName="DeleteOrder" CommandArgument='<%# Eval("PurchaseOrderID") %>'
                                        OnClientClick="return confirm('Are you sure you want to delete this purchase order?');" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-shopping-cart fa-4x text-muted mb-3 d-block"></i>
                            <h5 class="text-muted">No purchase orders found</h5>
                            <p class="text-muted small">Start by creating your first purchase order</p>
                            <asp:Button ID="btnAddFirst" runat="server" Text="Create Purchase Order" CssClass="btn btn-success mt-2" OnClick="btnAddNew_Click" />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <div class="card-footer bg-white py-2">
            <div class="row">
                <div class="col-md-6">
                    <small class="text-muted">
                        <i class="fas fa-info-circle me-1"></i>
                        Purchase orders track inventory replenishment from suppliers
                    </small>
                </div>
                <div class="col-md-6 text-end">
                    <small class="text-muted">
                        <i class="fas fa-clock me-1"></i>
                        Last updated: <%= DateTime.Now.ToString("dd/MM/yyyy HH:mm") %>
                    </small>
                </div>
            </div>
        </div>
    </div>

    <!-- Status Messages -->
    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show mt-3 shadow-sm">
        <div class="d-flex align-items-center">
            <i class="fas fa-check-circle fa-lg me-2"></i>
            <asp:Literal ID="ltrSuccessMessage" runat="server" />
            <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert"></button>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show mt-3 shadow-sm">
        <div class="d-flex align-items-center">
            <i class="fas fa-exclamation-circle fa-lg me-2"></i>
            <asp:Literal ID="ltrErrorMessage" runat="server" />
            <button type="button" class="btn-close ms-auto" data-bs-dismiss="alert"></button>
        </div>
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            // Set default dates
            var today = new Date();
            var firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
            
            $('#<%= txtDateFrom.ClientID %>').val(formatDate(firstDay));

            function formatDate(date) {
                var d = date.getDate();
                var m = date.getMonth() + 1;
                var y = date.getFullYear();
                return y + '-' + (m < 10 ? '0' : '') + m + '-' + (d < 10 ? '0' : '') + d;
            }

            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>