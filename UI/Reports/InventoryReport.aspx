
<%@ Page Title="Inventory Report" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="InventoryReport.aspx.cs" Inherits="RestaurantManagementSystem.UI.Reports.InventoryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Inventory Report - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-boxes text-primary me-2"></i>Inventory Report
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="ReportList.aspx">Reports</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Inventory</li>
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
                <div class="col-md-6">
                    <label class="form-label fw-bold small">Category</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">All Categories</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label fw-bold small">Show</label>
                    <asp:DropDownList ID="ddlStockStatus" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">All Items</asp:ListItem>
                        <asp:ListItem Value="Low">Low Stock</asp:ListItem>
                        <asp:ListItem Value="Normal">Normal</asp:ListItem>
                        <asp:ListItem Value="Over">Over Stocked</asp:ListItem>
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
                    <h6 class="text-muted small">Total Items</h6>
                    <h3 class="fw-bold text-primary"><asp:Literal ID="ltrTotalItems" runat="server" Text="0" /></h3>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card border-0 shadow-sm">
                <div class="card-body text-center">
                    <h6 class="text-muted small">Low Stock</h6>
                    <h3 class="fw-bold text-danger"><asp:Literal ID="ltrLowStock" runat="server" Text="0" /></h3>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card border-0 shadow-sm">
                <div class="card-body text-center">
                    <h6 class="text-muted small">Total Value</h6>
                    <h3 class="fw-bold text-success">$<asp:Literal ID="ltrTotalValue" runat="server" Text="0.00" /></h3>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card border-0 shadow-sm">
                <div class="card-body text-center">
                    <h6 class="text-muted small">Over Stocked</h6>
                    <h3 class="fw-bold text-warning"><asp:Literal ID="ltrOverStocked" runat="server" Text="0" /></h3>
                </div>
            </div>
        </div>
    </div>

    <!-- Report Grid -->
    <div class="card border-0 shadow-sm" id="divReport" runat="server" visible="false">
        <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center">
            <h5 class="mb-0">
                <i class="fas fa-table me-2 text-primary"></i>Inventory Details
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrRecordCount" runat="server" Text="0" /> Items
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="True" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination">
                    <EmptyDataTemplate>
                        <div class="text-center py-4">
                            <i class="fas fa-boxes fa-3x text-muted mb-3 d-block"></i>
                            <p class="text-muted">No inventory data found</p>
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
                        Inventory report shows current stock levels and value
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
            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>