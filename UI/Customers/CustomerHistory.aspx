
<%@ Page Title="Customer History" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="CustomerHistory.aspx.cs" Inherits="RestaurantManagementSystem.UI.Customers.CustomerHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Customer History - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-history text-primary me-2"></i>Customer History
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="CustomerList.aspx">Customers</a></li>
                            <li class="breadcrumb-item active" aria-current="page">
                                <asp:Literal ID="ltrCustomerName" runat="server" Text="History" />
                            </li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnBack" runat="server" Text="Back to Customers" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Customer Info -->
    <div class="card border-0 shadow-sm mb-4">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-user me-2 text-primary"></i>Customer Information
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-3">
                    <strong>Name:</strong>
                    <asp:Literal ID="ltrName" runat="server" Text="-" />
                </div>
                <div class="col-md-3">
                    <strong>Phone:</strong>
                    <asp:Literal ID="ltrPhone" runat="server" Text="-" />
                </div>
                <div class="col-md-3">
                    <strong>Email:</strong>
                    <asp:Literal ID="ltrEmail" runat="server" Text="-" />
                </div>
                <div class="col-md-3">
                    <strong>Loyalty Points:</strong>
                    <asp:Literal ID="ltrPoints" runat="server" Text="0" />
                </div>
            </div>
        </div>
    </div>

    <!-- Order History -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center">
            <h5 class="mb-0">
                <i class="fas fa-receipt me-2 text-primary"></i>Order History
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrOrderCount" runat="server" Text="0" /> Orders
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    OnPageIndexChanging="gvOrders_PageIndexChanging"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination">
                    <Columns>
                        <asp:BoundField DataField="OrderNumber" HeaderText="Order #" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="OrderDate" HeaderText="Date" ItemStyle-Width="15%" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:BoundField DataField="OrderType" HeaderText="Type" ItemStyle-Width="12%" />
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# GetStatusBadgeClass(Eval("OrderStatus").ToString()) %> px-3 py-2">
                                    <%# Eval("OrderStatus") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total" ItemStyle-Width="12%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="PaymentStatus" HeaderText="Payment" ItemStyle-Width="12%" />
                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-sm btn-info"
                                    CommandName="ViewOrder" CommandArgument='<%# Eval("OrderID") %>'
                                    OnClick="btnViewOrder_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-4">
                            <i class="fas fa-receipt fa-3x text-muted mb-3 d-block"></i>
                            <p class="text-muted">No orders found for this customer</p>
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
                        Complete order history for this customer
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
            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>