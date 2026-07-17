

<%@ Page Title="Profit & Loss Report" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="ProfitLoss.aspx.cs" Inherits="RestaurantManagementSystem.UI.Reports.ProfitLoss" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Profit & Loss Report - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-file-invoice-dollar text-primary me-2"></i>Profit & Loss Report
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="ReportList.aspx">Reports</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Profit & Loss</li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnExportExcel" runat="server" Text="Export Excel" CssClass="btn btn-success" OnClick="btnExportExcel_Click" />
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Report Filters -->
    <div class="card border-0 shadow-sm mb-4">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-filter me-2 text-primary"></i>Report Filters
            </h5>
        </div>
        <div class="card-body">
            <div class="row g-3 align-items-end">
                <div class="col-md-3">
                    <label class="form-label fw-bold small">Date From</label>
                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-3">
                    <label class="form-label fw-bold small">Date To</label>
                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-3">
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-primary w-100" OnClick="btnGenerate_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Report Summary -->
    <div class="card border-0 shadow-sm mb-4" id="divSummary" runat="server" visible="false">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-chart-pie me-2 text-primary"></i>Financial Summary
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="card bg-light border-0">
                        <div class="card-body">
                            <h6 class="text-muted small">Revenue</h6>
                            <h4 class="text-success">$<asp:Literal ID="ltrTotalSales" runat="server" Text="0.00" /></h4>
                            <hr />
                            <div class="d-flex justify-content-between">
                                <span>Total Orders:</span>
                                <strong><asp:Literal ID="ltrTotalOrders" runat="server" Text="0" /></strong>
                            </div>
                            <div class="d-flex justify-content-between">
                                <span>Average Order Value:</span>
                                <strong>$<asp:Literal ID="ltrAvgOrder" runat="server" Text="0.00" /></strong>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="card bg-light border-0">
                        <div class="card-body">
                            <h6 class="text-muted small">Expenses</h6>
                            <h4 class="text-danger">$<asp:Literal ID="ltrTotalExpenses" runat="server" Text="0.00" /></h4>
                            <hr />
                            <div class="d-flex justify-content-between">
                                <span>Cost of Goods Sold:</span>
                                <strong>$<asp:Literal ID="ltrTotalCOGS" runat="server" Text="0.00" /></strong>
                            </div>
                            <div class="d-flex justify-content-between">
                                <span>Total Tax:</span>
                                <strong>$<asp:Literal ID="ltrTotalTax" runat="server" Text="0.00" /></strong>
                            </div>
                            <div class="d-flex justify-content-between">
                                <span>Total Discount:</span>
                                <strong>$<asp:Literal ID="ltrTotalDiscount" runat="server" Text="0.00" /></strong>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Profit/Loss Results -->
    <div class="card border-0 shadow-sm" id="divResults" runat="server" visible="false">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-calculator me-2 text-primary"></i>Profit & Loss Summary
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-4">
                    <div class="card border-0 shadow-sm">
                        <div class="card-body text-center">
                            <h6 class="text-muted small">Gross Profit</h6>
                            <h3 class="fw-bold text-<asp:Literal ID="ltrGrossProfitColor" runat="server" Text="success" />">
                                $<asp:Literal ID="ltrGrossProfit" runat="server" Text="0.00" />
                            </h3>
                            <span class="badge bg-<asp:Literal ID="ltrGrossMarginColor" runat="server" Text="success" />">
                                <asp:Literal ID="ltrGrossMargin" runat="server" Text="0%" /> Margin
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card border-0 shadow-sm">
                        <div class="card-body text-center">
                            <h6 class="text-muted small">Net Profit</h6>
                            <h3 class="fw-bold text-<asp:Literal ID="ltrNetProfitColor" runat="server" Text="success" />">
                                $<asp:Literal ID="ltrNetProfit" runat="server" Text="0.00" />
                            </h3>
                            <span class="badge bg-<asp:Literal ID="ltrNetMarginColor" runat="server" Text="success" />">
                                <asp:Literal ID="ltrNetMargin" runat="server" Text="0%" /> Margin
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card border-0 shadow-sm">
                        <div class="card-body text-center">
                            <h6 class="text-muted small">Net Profit / Sale</h6>
                            <h3 class="fw-bold text-<asp:Literal ID="ltrProfitPerSaleColor" runat="server" Text="success" />">
                                $<asp:Literal ID="ltrProfitPerSale" runat="server" Text="0.00" />
                            </h3>
                            <span class="badge bg-<asp:Literal ID="ltrProfitPerSaleBg" runat="server" Text="success" />">
                                Per Order
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card-footer bg-white py-2">
            <div class="row">
                <div class="col-md-6">
                    <small class="text-muted">
                        <i class="fas fa-info-circle me-1"></i>
                        Profit & Loss statement for the selected period
                    </small>
                </div>
                <div class="col-md-6 text-end">
                    <small class="text-muted">
                        <i class="fas fa-clock me-1"></i>
                        Generated: <%= DateTime.Now.ToString("dd/MM/yyyy HH:mm") %>
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
            var today = new Date();
            var firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
            
            $('#<%= txtDateFrom.ClientID %>').val(formatDate(firstDay));
            $('#<%= txtDateTo.ClientID %>').val(formatDate(today));

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