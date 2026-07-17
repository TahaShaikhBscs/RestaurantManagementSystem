

<%@ Page Title="Reports Dashboard" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="ReportList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Reports.ReportList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reports Dashboard - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-chart-pie text-primary me-2"></i>Reports Dashboard
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Reports</li>
                        </ol>
                    </nav>
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Report Cards -->
    <div class="row g-4">
        <!-- Sales Report -->
        <div class="col-md-4">
            <div class="card h-100 border-0 shadow-sm hover-shadow">
                <div class="card-body text-center p-4">
                    <div class="bg-primary bg-opacity-10 rounded-circle p-3 d-inline-block mb-3">
                        <i class="fas fa-chart-bar text-primary fa-3x"></i>
                    </div>
                    <h5 class="card-title">Sales Report</h5>
                    <p class="card-text text-muted small">
                        View sales performance by day, month, or year. Track revenue trends and order patterns.
                    </p>
                    <asp:Button ID="btnSalesReport" runat="server" Text="View Sales Report" CssClass="btn btn-primary" OnClick="btnSalesReport_Click" />
                </div>
            </div>
        </div>

        <!-- Profit & Loss -->
        <div class="col-md-4">
            <div class="card h-100 border-0 shadow-sm hover-shadow">
                <div class="card-body text-center p-4">
                    <div class="bg-success bg-opacity-10 rounded-circle p-3 d-inline-block mb-3">
                        <i class="fas fa-file-invoice-dollar text-success fa-3x"></i>
                    </div>
                    <h5 class="card-title">Profit & Loss</h5>
                    <p class="card-text text-muted small">
                        Analyze your business profitability. View gross profit, net profit, and margins.
                    </p>
                    <asp:Button ID="btnProfitLoss" runat="server" Text="View Profit & Loss" CssClass="btn btn-success" OnClick="btnProfitLoss_Click" />
                </div>
            </div>
        </div>

        <!-- Expense Report -->
        <div class="col-md-4">
            <div class="card h-100 border-0 shadow-sm hover-shadow">
                <div class="card-body text-center p-4">
                    <div class="bg-danger bg-opacity-10 rounded-circle p-3 d-inline-block mb-3">
                        <i class="fas fa-money-bill-wave text-danger fa-3x"></i>
                    </div>
                    <h5 class="card-title">Expense Report</h5>
                    <p class="card-text text-muted small">
                        Track all business expenses by category. Identify cost-saving opportunities.
                    </p>
                    <asp:Button ID="btnExpenseReport" runat="server" Text="View Expense Report" CssClass="btn btn-danger" OnClick="btnExpenseReport_Click" />
                </div>
            </div>
        </div>

        <!-- Customer Report -->
        <div class="col-md-4">
            <div class="card h-100 border-0 shadow-sm hover-shadow">
                <div class="card-body text-center p-4">
                    <div class="bg-info bg-opacity-10 rounded-circle p-3 d-inline-block mb-3">
                        <i class="fas fa-users text-info fa-3x"></i>
                    </div>
                    <h5 class="card-title">Customer Report</h5>
                    <p class="card-text text-muted small">
                        View customer analytics, top spenders, and loyalty program performance.
                    </p>
                    <asp:Button ID="btnCustomerReport" runat="server" Text="View Customer Report" CssClass="btn btn-info" OnClick="btnCustomerReport_Click" />
                </div>
            </div>
        </div>

        <!-- Inventory Report -->
        <div class="col-md-4">
            <div class="card h-100 border-0 shadow-sm hover-shadow">
                <div class="card-body text-center p-4">
                    <div class="bg-warning bg-opacity-10 rounded-circle p-3 d-inline-block mb-3">
                        <i class="fas fa-boxes text-warning fa-3x"></i>
                    </div>
                    <h5 class="card-title">Inventory Report</h5>
                    <p class="card-text text-muted small">
                        Monitor stock levels, low inventory alerts, and inventory valuation.
                    </p>
                    <asp:Button ID="btnInventoryReport" runat="server" Text="View Inventory Report" CssClass="btn btn-warning" OnClick="btnInventoryReport_Click" />
                </div>
            </div>
        </div>

        <!-- Payment Report -->
        <div class="col-md-4">
            <div class="card h-100 border-0 shadow-sm hover-shadow">
                <div class="card-body text-center p-4">
                    <div class="bg-secondary bg-opacity-10 rounded-circle p-3 d-inline-block mb-3">
                        <i class="fas fa-credit-card text-secondary fa-3x"></i>
                    </div>
                    <h5 class="card-title">Payment Report</h5>
                    <p class="card-text text-muted small">
                        Analyze payment methods, transaction volumes, and payment trends.
                    </p>
                    <asp:Button ID="btnPaymentReport" runat="server" Text="View Payment Report" CssClass="btn btn-secondary" OnClick="btnPaymentReport_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <style>
        .hover-shadow {
            transition: all 0.3s ease;
        }
        .hover-shadow:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 30px rgba(0,0,0,0.15) !important;
        }
    </style>
</asp:Content>