<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="RestaurantManagementSystem.UI.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Dashboard - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="page-title">
                <i class="fas fa-chart-pie me-2"></i>Dashboard
                <small class="text-muted ms-2">Welcome back, <asp:Literal ID="ltrUserName" runat="server" /></small>
            </h2>
            <hr />
        </div>
    </div>

    <!-- Statistics Cards -->
    <div class="row g-4 mb-4">
        <!-- Today's Sales -->
        <div class="col-xl-3 col-lg-6 col-md-6">
            <div class="card h-100 border-0 shadow-sm">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <p class="text-muted mb-1 small">Today's Sales</p>
                            <h3 class="fw-bold text-success">
                                <asp:Literal ID="ltrTodaySales" runat="server" Text="0" />
                            </h3>
                        </div>
                        <div class="bg-success bg-opacity-10 rounded-circle p-3">
                            <i class="fas fa-wallet text-success fa-2x"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Monthly Sales -->
        <div class="col-xl-3 col-lg-6 col-md-6">
            <div class="card h-100 border-0 shadow-sm">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <p class="text-muted mb-1 small">Monthly Sales</p>
                            <h3 class="fw-bold text-primary">
                                <asp:Literal ID="ltrMonthlySales" runat="server" Text="0" />
                            </h3>
                        </div>
                        <div class="bg-primary bg-opacity-10 rounded-circle p-3">
                            <i class="fas fa-chart-line text-primary fa-2x"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total Orders -->
        <div class="col-xl-3 col-lg-6 col-md-6">
            <div class="card h-100 border-0 shadow-sm">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <p class="text-muted mb-1 small">Total Orders</p>
                            <h3 class="fw-bold text-info">
                                <asp:Literal ID="ltrTotalOrders" runat="server" Text="0" />
                            </h3>
                        </div>
                        <div class="bg-info bg-opacity-10 rounded-circle p-3">
                            <i class="fas fa-shopping-bag text-info fa-2x"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Running Orders -->
        <div class="col-xl-3 col-lg-6 col-md-6">
            <div class="card h-100 border-0 shadow-sm">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <p class="text-muted mb-1 small">Running Orders</p>
                            <h3 class="fw-bold text-warning">
                                <asp:Literal ID="ltrRunningOrders" runat="server" Text="0" />
                            </h3>
                        </div>
                        <div class="bg-warning bg-opacity-10 rounded-circle p-3">
                            <i class="fas fa-clock text-warning fa-2x"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Second Row of Cards -->
    <div class="row g-4 mb-4">
        <!-- Kitchen Orders -->
        <div class="col-xl-3 col-lg-6 col-md-6">
            <div class="card h-100 border-0 shadow-sm">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <p class="text-muted mb-1 small">Kitchen Orders</p>
                            <h3 class="fw-bold text-danger">
                                <asp:Literal ID="ltrKitchenOrders" runat="server" Text="0" />
                            </h3>
                        </div>
                        <div class="bg-danger bg-opacity-10 rounded-circle p-3">
                            <i class="fas fa-utensils text-danger fa-2x"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total Customers -->
        <div class="col-xl-3 col-lg-6 col-md-6">
            <div class="card h-100 border-0 shadow-sm">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <p class="text-muted mb-1 small">Total Customers</p>
                            <h3 class="fw-bold text-info">
                                <asp:Literal ID="ltrTotalCustomers" runat="server" Text="0" />
                            </h3>
                        </div>
                        <div class="bg-info bg-opacity-10 rounded-circle p-3">
                            <i class="fas fa-users text-info fa-2x"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total Products -->
        <div class="col-xl-3 col-lg-6 col-md-6">
            <div class="card h-100 border-0 shadow-sm">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <p class="text-muted mb-1 small">Total Products</p>
                            <h3 class="fw-bold text-primary">
                                <asp:Literal ID="ltrTotalProducts" runat="server" Text="0" />
                            </h3>
                        </div>
                        <div class="bg-primary bg-opacity-10 rounded-circle p-3">
                            <i class="fas fa-hamburger text-primary fa-2x"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total Expenses -->
        <div class="col-xl-3 col-lg-6 col-md-6">
            <div class="card h-100 border-0 shadow-sm">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <p class="text-muted mb-1 small">Total Expenses</p>
                            <h3 class="fw-bold text-danger">
                                <asp:Literal ID="ltrTotalExpenses" runat="server" Text="0" />
                            </h3>
                        </div>
                        <div class="bg-danger bg-opacity-10 rounded-circle p-3">
                            <i class="fas fa-money-bill text-danger fa-2x"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Charts and Recent Data -->
    <div class="row g-4">
        <!-- Chart Section -->
        <div class="col-xl-8">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-transparent border-0">
                    <h5 class="mb-0">
                        <i class="fas fa-chart-area me-2"></i>Sales Overview
                    </h5>
                </div>
                <div class="card-body">
                    <canvas id="salesChart" height="250"></canvas>
                </div>
            </div>
        </div>

        <!-- Top Selling Items -->
        <div class="col-xl-4">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-transparent border-0">
                    <h5 class="mb-0">
                        <i class="fas fa-fire me-2"></i>Top Selling Items
                    </h5>
                </div>
                <div class="card-body p-0">
                    <div class="list-group list-group-flush">
                        <asp:Repeater ID="rptTopSellingItems" runat="server">
                            <ItemTemplate>
                                <div class="list-group-item d-flex justify-content-between align-items-center">
                                    <span><%# Eval("ItemName") %></span>
                                    <span class="badge bg-primary rounded-pill">
                                        <%# Eval("TotalQuantity") %> sold
                                    </span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Recent Orders and Payments -->
    <div class="row g-4 mt-2">
        <!-- Recent Orders -->
        <div class="col-xl-6">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-transparent border-0 d-flex justify-content-between align-items-center">
                    <h5 class="mb-0">
                        <i class="fas fa-receipt me-2"></i>Recent Orders
                    </h5>
                    <a href="Orders/OrderList.aspx" class="text-decoration-none">View All</a>
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <table class="table table-hover mb-0">
                            <thead>
                                <tr>
                                    <th>Order #</th>
                                    <th>Customer</th>
                                    <th>Amount</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptRecentOrders" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("OrderNumber") %></td>
                                            <td><%# Eval("CustomerName") %></td>
                                            <td><%# Eval("TotalAmount", "{0:C}") %></td>
                                            <td>
                                                <span class="badge bg-<%# GetOrderStatusBadgeClass(Eval("OrderStatus").ToString()) %>">
                                                    <%# Eval("OrderStatus") %>
                                                </span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <!-- Recent Payments -->
        <div class="col-xl-6">
            <div class="card border-0 shadow-sm">
                <div class="card-header bg-transparent border-0 d-flex justify-content-between align-items-center">
                    <h5 class="mb-0">
                        <i class="fas fa-credit-card me-2"></i>Recent Payments
                    </h5>
                    <a href="Reports/PaymentReport.aspx" class="text-decoration-none">View All</a>
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <table class="table table-hover mb-0">
                            <thead>
                                <tr>
                                    <th>Order #</th>
                                    <th>Amount</th>
                                    <th>Method</th>
                                    <th>Date</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptRecentPayments" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("OrderNumber") %></td>
                                            <td><%# Eval("Amount", "{0:C}") %></td>
                                            <td><%# Eval("PaymentMethod") %></td>
                                            <td><%# Eval("TransactionDate", "{0:MMM dd, HH:mm}") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    
    <script type="text/javascript">
        $(document).ready(function() {
            // Initialize sales chart
            const ctx = document.getElementById('salesChart').getContext('2d');
            
            // Get chart data from server
            const chartData = {
                labels: <%= GetChartLabels() %>,
                datasets: [{
                    label: 'Daily Sales',
                    data: <%= GetChartData() %>,
                    borderColor: '#667eea',
                    backgroundColor: 'rgba(102, 126, 234, 0.1)',
                    borderWidth: 3,
                    fill: true,
                    tension: 0.4
                }]
            };
            
            new Chart(ctx, {
                type: 'line',
                data: chartData,
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            display: false
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                callback: function(value) {
                                    return '$' + value.toFixed(2);
                                }
                            }
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>