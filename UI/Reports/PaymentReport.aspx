<!-- ============================================ -->
<!-- UI/Reports/PaymentReport.aspx -->
<!-- ============================================ -->

<%@ Page Title="Payment Report" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="PaymentReport.aspx.cs" Inherits="RestaurantManagementSystem.UI.Reports.PaymentReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Payment Report - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-credit-card text-primary me-2"></i>Payment Report
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="ReportList.aspx">Reports</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Payments</li>
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
                    <label class="form-label fw-bold small">Payment Method</label>
                    <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">All Methods</asp:ListItem>
                        <asp:ListItem Value="Cash">Cash</asp:ListItem>
                        <asp:ListItem Value="Card">Card</asp:ListItem>
                        <asp:ListItem Value="Online">Online</asp:ListItem>
                        <asp:ListItem Value="Mobile Wallet">Mobile Wallet</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-primary w-100" OnClick="btnGenerate_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Summary Cards -->
    <div class="row mb-4" id="divSummary" runat="server" visible="false">
        <div class="col-md-3">
            <div class="card border-0 shadow-sm">
                <div class="card-body text-center">
                    <h6 class="text-muted small">Total Payments</h6>
                    <h3 class="fw-bold text-primary">$<asp:Literal ID="ltrTotalAmount" runat="server" Text="0.00" /></h3>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card border-0 shadow-sm">
                <div class="card-body text-center">
                    <h6 class="text-muted small">Total Transactions</h6>
                    <h3 class="fw-bold text-success"><asp:Literal ID="ltrTotalTransactions" runat="server" Text="0" /></h3>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card border-0 shadow-sm">
                <div class="card-body text-center">
                    <h6 class="text-muted small">Average Payment</h6>
                    <h3 class="fw-bold text-warning">$<asp:Literal ID="ltrAverageAmount" runat="server" Text="0.00" /></h3>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card border-0 shadow-sm">
                <div class="card-body text-center">
                    <h6 class="text-muted small">Top Method</h6>
                    <h3 class="fw-bold text-info"><asp:Literal ID="ltrTopMethod" runat="server" Text="-" /></h3>
                </div>
            </div>
        </div>
    </div>

    <!-- Report Grid -->
    <div class="card border-0 shadow-sm" id="divReport" runat="server" visible="false">
        <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center">
            <h5 class="mb-0">
                <i class="fas fa-table me-2 text-primary"></i>Payment Details
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrRecordCount" runat="server" Text="0" /> Records
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="True" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination">
                    <EmptyDataTemplate>
                        <div class="text-center py-4">
                            <i class="fas fa-credit-card fa-3x text-muted mb-3 d-block"></i>
                            <p class="text-muted">No payment data found for the selected period</p>
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
                        Payment report shows payment method usage and trends
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