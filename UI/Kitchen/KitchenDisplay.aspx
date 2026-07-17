

<%@ Page Title="Kitchen Display" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="KitchenDisplay.aspx.cs" Inherits="RestaurantManagementSystem.UI.Kitchen.KitchenDisplay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Kitchen Display - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-utensil-spoon text-primary me-2"></i>Kitchen Display
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Kitchen</li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <span class="badge bg-success rounded-pill me-2">
                        <i class="fas fa-clock me-1"></i>
                        <asp:Literal ID="ltrCurrentTime" runat="server" />
                    </span>
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-primary" OnClick="btnRefresh_Click" />
                    <asp:Button ID="btnAutoRefresh" runat="server" Text="Auto Refresh" CssClass="btn btn-secondary" OnClick="btnAutoRefresh_Click" />
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

    <!-- Statistics -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="row text-center">
                        <div class="col-md-3">
                            <span class="badge bg-warning rounded-pill px-3 py-2">
                                <i class="fas fa-clock me-1"></i>
                                Pending: <asp:Literal ID="ltrPendingCount" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-info rounded-pill px-3 py-2">
                                <i class="fas fa-fire me-1"></i>
                                Cooking: <asp:Literal ID="ltrCookingCount" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-success rounded-pill px-3 py-2">
                                <i class="fas fa-check-circle me-1"></i>
                                Ready: <asp:Literal ID="ltrReadyCount" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-danger rounded-pill px-3 py-2">
                                <i class="fas fa-exclamation-triangle me-1"></i>
                                Delayed: <asp:Literal ID="ltrDelayedCount" runat="server" Text="0" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Kitchen Orders -->
    <div class="row">
        <!-- Pending Orders -->
        <div class="col-md-4">
            <div class="card border-0 shadow-sm h-100">
                <div class="card-header bg-warning bg-opacity-10 py-2">
                    <h6 class="mb-0">
                        <i class="fas fa-clock text-warning me-1"></i>
                        Pending
                        <span class="badge bg-warning rounded-pill ms-2">
                            <asp:Literal ID="ltrPendingBadge" runat="server" Text="0" />
                        </span>
                    </h6>
                </div>
                <div class="card-body p-2" style="max-height: 500px; overflow-y: auto;">
                    <asp:Repeater ID="rptPendingOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
                        <ItemTemplate>
                            <div class="kitchen-order pending mb-2">
                                <div class="d-flex justify-content-between align-items-start">
                                    <div>
                                        <strong>#<%# Eval("OrderNumber") %></strong>
                                        <span class="badge bg-secondary ms-1"><%# Eval("TableNumber") ?? "TA" %></span>
                                    </div>
                                    <span class="badge bg-<%# GetPriorityBadgeClass(Eval("PriorityStatus")) %>">
                                        <%# Eval("PriorityStatus") %>
                                    </span>
                                </div>
                                <div class="mt-1">
                                    <span class="fw-bold"><%# Eval("Quantity") %>x</span>
                                    <span><%# Eval("ItemName") %></span>
                                </div>
                                <div class="small text-muted">
                                    <i class="fas fa-clock me-1"></i>
                                    <%# Eval("EstimatedTime") %> min
                                    <%# !string.IsNullOrEmpty(Eval("Instructions").ToString()) ? "| " + Eval("Instructions") : "" %>
                                </div>
                                <div class="mt-1">
                                    <asp:Button ID="btnStart" runat="server" Text="Start Cooking" CssClass="btn btn-sm btn-primary"
                                        CommandName="StartCooking" CommandArgument='<%# Eval("KitchenOrderID") %>' />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-sm btn-danger"
                                        CommandName="CancelOrder" CommandArgument='<%# Eval("KitchenOrderID") %>'
                                        OnClientClick="return confirm('Cancel this order item?');" />
                                </div>
                            </div>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="text-center py-4">
                                <i class="fas fa-check-circle fa-2x text-muted mb-2 d-block"></i>
                                <p class="text-muted small">No pending orders</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>

        <!-- Cooking Orders -->
        <div class="col-md-4">
            <div class="card border-0 shadow-sm h-100">
                <div class="card-header bg-info bg-opacity-10 py-2">
                    <h6 class="mb-0">
                        <i class="fas fa-fire text-info me-1"></i>
                        Cooking
                        <span class="badge bg-info rounded-pill ms-2">
                            <asp:Literal ID="ltrCookingBadge" runat="server" Text="0" />
                        </span>
                    </h6>
                </div>
                <div class="card-body p-2" style="max-height: 500px; overflow-y: auto;">
                    <asp:Repeater ID="rptCookingOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
                        <ItemTemplate>
                            <div class="kitchen-order cooking mb-2">
                                <div class="d-flex justify-content-between align-items-start">
                                    <div>
                                        <strong>#<%# Eval("OrderNumber") %></strong>
                                        <span class="badge bg-secondary ms-1"><%# Eval("TableNumber") ?? "TA" %></span>
                                    </div>
                                    <span class="badge bg-<%# GetPriorityBadgeClass(Eval("PriorityStatus")) %>">
                                        <%# Eval("PriorityStatus") %>
                                    </span>
                                </div>
                                <div class="mt-1">
                                    <span class="fw-bold"><%# Eval("Quantity") %>x</span>
                                    <span><%# Eval("ItemName") %></span>
                                </div>
                                <div class="small text-muted">
                                    <i class="fas fa-clock me-1"></i>
                                    Cooking: <%# Eval("EstimatedTime") %> min
                                    <%# !string.IsNullOrEmpty(Eval("Instructions").ToString()) ? "| " + Eval("Instructions") : "" %>
                                </div>
                                <div class="mt-1">
                                    <asp:Button ID="btnComplete" runat="server" Text="Mark Ready" CssClass="btn btn-sm btn-success"
                                        CommandName="MarkReady" CommandArgument='<%# Eval("KitchenOrderID") %>' />
                                    <asp:Button ID="btnCancelCooking" runat="server" Text="Cancel" CssClass="btn btn-sm btn-danger"
                                        CommandName="CancelOrder" CommandArgument='<%# Eval("KitchenOrderID") %>'
                                        OnClientClick="return confirm('Cancel this order item?');" />
                                </div>
                            </div>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="text-center py-4">
                                <i class="fas fa-fire fa-2x text-muted mb-2 d-block"></i>
                                <p class="text-muted small">No orders cooking</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>

        <!-- Ready Orders -->
        <div class="col-md-4">
            <div class="card border-0 shadow-sm h-100">
                <div class="card-header bg-success bg-opacity-10 py-2">
                    <h6 class="mb-0">
                        <i class="fas fa-check-circle text-success me-1"></i>
                        Ready
                        <span class="badge bg-success rounded-pill ms-2">
                            <asp:Literal ID="ltrReadyBadge" runat="server" Text="0" />
                        </span>
                    </h6>
                </div>
                <div class="card-body p-2" style="max-height: 500px; overflow-y: auto;">
                    <asp:Repeater ID="rptReadyOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
                        <ItemTemplate>
                            <div class="kitchen-order ready mb-2">
                                <div class="d-flex justify-content-between align-items-start">
                                    <div>
                                        <strong>#<%# Eval("OrderNumber") %></strong>
                                        <span class="badge bg-secondary ms-1"><%# Eval("TableNumber") ?? "TA" %></span>
                                    </div>
                                    <span class="badge bg-success">Ready</span>
                                </div>
                                <div class="mt-1">
                                    <span class="fw-bold"><%# Eval("Quantity") %>x</span>
                                    <span><%# Eval("ItemName") %></span>
                                </div>
                                <div class="small text-muted">
                                    <i class="fas fa-check me-1"></i>
                                    Ready for serving
                                </div>
                                <div class="mt-1">
                                    <asp:Button ID="btnServe" runat="server" Text="Served" CssClass="btn btn-sm btn-success"
                                        CommandName="MarkServed" CommandArgument='<%# Eval("KitchenOrderID") %>' />
                                </div>
                            </div>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="text-center py-4">
                                <i class="fas fa-utensils fa-2x text-muted mb-2 d-block"></i>
                                <p class="text-muted small">No ready orders</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            // Update time
            function updateTime() {
                var now = new Date();
                $('#<%= ltrCurrentTime.ClientID %>').text(now.toLocaleTimeString());
            }
            updateTime();
            setInterval(updateTime, 1000);

            // Auto-refresh every 30 seconds if enabled
            var autoRefresh = false;
            $('#<%= btnAutoRefresh.ClientID %>').click(function() {
                autoRefresh = !autoRefresh;
                $(this).text(autoRefresh ? 'Auto Refresh On' : 'Auto Refresh');
                $(this).toggleClass('btn-secondary btn-success');
                
                if (autoRefresh) {
                    setInterval(function() {
                        if (autoRefresh) {
                            __doPostBack('btnRefresh', '');
                        }
                    }, 30000);
                }
            });

            // Auto-hide alerts
            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>