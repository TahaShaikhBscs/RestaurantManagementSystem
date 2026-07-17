 

<%@ Page Title="Order Management" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Orders.OrderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Order Management - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-receipt text-primary me-2"></i>Order Management
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Orders</li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnAddNew" runat="server" Text="New Order" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-secondary" OnClick="btnRefresh_Click" />
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
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by order # or customer..." />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                                <asp:ListItem Value="">All Status</asp:ListItem>
                                <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                <asp:ListItem Value="Cooking">Cooking</asp:ListItem>
                                <asp:ListItem Value="Ready">Ready</asp:ListItem>
                                <asp:ListItem Value="Completed">Completed</asp:ListItem>
                                <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlTypeFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeFilter_SelectedIndexChanged">
                                <asp:ListItem Value="">All Types</asp:ListItem>
                                <asp:ListItem Value="Dine In">Dine In</asp:ListItem>
                                <asp:ListItem Value="Take Away">Take Away</asp:ListItem>
                                <asp:ListItem Value="Delivery">Delivery</asp:ListItem>
                            </asp:DropDownList>
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
                Manage all restaurant orders
            </span>
        </div>
    </div>

    <!-- Order Statistics -->
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
                            <span class="badge bg-info rounded-pill px-3 py-2">
                                <i class="fas fa-utensils me-1"></i>
                                Cooking: <asp:Literal ID="ltrCookingOrders" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-success rounded-pill px-3 py-2">
                                <i class="fas fa-check-circle me-1"></i>
                                Completed: <asp:Literal ID="ltrCompletedOrders" runat="server" Text="0" />
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
                <i class="fas fa-list-ul me-2 text-primary"></i>Orders List
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrTotalCount" runat="server" Text="0" /> Orders
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    OnRowCommand="gvOrders_RowCommand" OnPageIndexChanging="gvOrders_PageIndexChanging"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination">
                    <Columns>
                        <asp:BoundField DataField="OrderID" HeaderText="ID" Visible="False" />
                        <asp:BoundField DataField="OrderNumber" HeaderText="Order #" ItemStyle-Width="12%" />
                        <asp:BoundField DataField="OrderDate" HeaderText="Date" ItemStyle-Width="12%" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="OrderType" HeaderText="Type" ItemStyle-Width="10%" />
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# GetStatusBadgeClass(Eval("OrderStatus").ToString()) %> px-3 py-2">
                                    <i class="fas fa-<%# GetStatusIconClass(Eval("OrderStatus").ToString()) %> me-1"></i>
                                    <%# Eval("OrderStatus") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total" ItemStyle-Width="10%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                        <asp:TemplateField HeaderText="Payment" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
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
                                        CommandName="ViewOrder" CommandArgument='<%# Eval("OrderID") %>' />
                                    <asp:Button ID="btnUpdateStatus" runat="server" Text="Status" CssClass="btn btn-warning"
                                        CommandName="UpdateStatus" CommandArgument='<%# Eval("OrderID") %>' />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                        CommandName="CancelOrder" CommandArgument='<%# Eval("OrderID") %>'
                                        OnClientClick="return confirm('Are you sure you want to cancel this order?');" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-receipt fa-4x text-muted mb-3 d-block"></i>
                            <h5 class="text-muted">No orders found</h5>
                            <p class="text-muted small">Orders will appear here once customers place them</p>
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
                        Click "View" to see order details and update status
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

    <!-- Update Status Modal -->
    <div class="modal fade" id="statusModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Update Order Status</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfOrderID" runat="server" />
                    <div class="mb-3">
                        <label class="form-label fw-bold">New Status</label>
                        <asp:DropDownList ID="ddlNewStatus" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Cooking">Cooking</asp:ListItem>
                            <asp:ListItem Value="Ready">Ready</asp:ListItem>
                            <asp:ListItem Value="Completed">Completed</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-bold">Notes</label>
                        <asp:TextBox ID="txtStatusNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnUpdateStatus" runat="server" Text="Update Status" CssClass="btn btn-primary" OnClick="btnUpdateStatus_Click" />
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

        function openStatusModal(orderID) {
            $('#<%= hfOrderID.ClientID %>').val(orderID);
            var modal = new bootstrap.Modal(document.getElementById('statusModal'));
            modal.show();
        }
    </script>
</asp:Content>