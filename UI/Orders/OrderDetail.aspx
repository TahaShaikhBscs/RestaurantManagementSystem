

<%@ Page Title="Order Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="RestaurantManagementSystem.UI.Orders.OrderDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Order Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-receipt text-primary me-2"></i>Order Details
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="OrderList.aspx">Orders</a></li>
                            <li class="breadcrumb-item active" aria-current="page">
                                <asp:Literal ID="ltrOrderNumber" runat="server" Text="Order #" />
                            </li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnBack" runat="server" Text="Back to List" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_Click" />
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

    <!-- Order Information -->
    <div class="row">
        <div class="col-md-6">
            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-white py-3">
                    <h5 class="mb-0">
                        <i class="fas fa-info-circle me-2 text-primary"></i>Order Information
                    </h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-6">
                            <div class="mb-2">
                                <strong>Order #:</strong>
                                <asp:Literal ID="ltrOrderNum" runat="server" Text="-" />
                            </div>
                            <div class="mb-2">
                                <strong>Date:</strong>
                                <asp:Literal ID="ltrOrderDate" runat="server" Text="-" />
                            </div>
                            <div class="mb-2">
                                <strong>Type:</strong>
                                <asp:Literal ID="ltrOrderType" runat="server" Text="-" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="mb-2">
                                <strong>Status:</strong>
                                <asp:Literal ID="ltrOrderStatus" runat="server" Text="-" />
                            </div>
                            <div class="mb-2">
                                <strong>Payment:</strong>
                                <asp:Literal ID="ltrPaymentStatus" runat="server" Text="-" />
                            </div>
                            <div class="mb-2">
                                <strong>Source:</strong>
                                <asp:Literal ID="ltrOrderSource" runat="server" Text="-" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-white py-3">
                    <h5 class="mb-0">
                        <i class="fas fa-user me-2 text-primary"></i>Customer Information
                    </h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-6">
                            <div class="mb-2">
                                <strong>Name:</strong>
                                <asp:Literal ID="ltrCustomerName" runat="server" Text="Walk-in" />
                            </div>
                            <div class="mb-2">
                                <strong>Phone:</strong>
                                <asp:Literal ID="ltrCustomerPhone" runat="server" Text="-" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="mb-2">
                                <strong>Table:</strong>
                                <asp:Literal ID="ltrTableNumber" runat="server" Text="-" />
                            </div>
                            <div class="mb-2">
                                <strong>Delivery Address:</strong>
                                <asp:Literal ID="ltrDeliveryAddress" runat="server" Text="-" />
                            </div>
                        </div>
                    </div>
                    <div class="mt-2">
                        <strong>Special Instructions:</strong><br />
                        <asp:Literal ID="ltrInstructions" runat="server" Text="None" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Order Items -->
    <div class="card border-0 shadow-sm mb-4">
        <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center">
            <h5 class="mb-0">
                <i class="fas fa-shopping-cart me-2 text-primary"></i>Order Items
            </h5>
            <span class="badge bg-primary rounded-pill">
                <asp:Literal ID="ltrItemCount" runat="server" Text="0" /> Items
            </span>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="ItemName" HeaderText="Item" ItemStyle-Width="35%" />
                        <asp:BoundField DataField="Quantity" HeaderText="Qty" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" ItemStyle-Width="15%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Discount" HeaderText="Discount" ItemStyle-Width="10%" DataFormatString="{0:F2}%" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="TotalPrice" HeaderText="Total" ItemStyle-Width="15%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="KitchenStatus" HeaderText="Kitchen" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-3">
                            <p class="text-muted">No items found for this order.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <div class="card-footer bg-white py-3">
            <div class="row">
                <div class="col-md-6 offset-md-6">
                    <div class="table-responsive">
                        <table class="table table-borderless mb-0">
                            <tr>
                                <td><strong>Sub Total:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrSubTotal" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr>
                                <td><strong>Tax:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrTax" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr>
                                <td><strong>Discount:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrDiscount" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr>
                                <td><strong>Service Charge:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrServiceCharge" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr class="table-primary">
                                <td><strong>Total:</strong></td>
                                <td class="text-end fw-bold"><asp:Literal ID="ltrTotalAmount" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr>
                                <td><strong>Paid:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrPaidAmount" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr>
                                <td><strong>Change:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrChangeAmount" runat="server" Text="0.00" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Payments -->
    <div class="card border-0 shadow-sm mb-4" id="divPayments" runat="server" visible="false">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-credit-card me-2 text-primary"></i>Payments
            </h5>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvPayments" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="PaymentMethod" HeaderText="Method" ItemStyle-Width="25%" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="20%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="ReferenceNumber" HeaderText="Reference" ItemStyle-Width="30%" />
                        <asp:BoundField DataField="TransactionDate" HeaderText="Date" ItemStyle-Width="25%" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-3">
                            <p class="text-muted">No payments recorded for this order.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- Order Timeline -->
    <div class="card border-0 shadow-sm" id="divTimeline" runat="server" visible="false">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-clock me-2 text-primary"></i>Order Timeline
            </h5>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvTimeline" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="20%" />
                        <asp:BoundField DataField="PreviousStatus" HeaderText="Previous Status" ItemStyle-Width="20%" />
                        <asp:BoundField DataField="Notes" HeaderText="Notes" ItemStyle-Width="35%" />
                        <asp:BoundField DataField="TimelineDate" HeaderText="Date" ItemStyle-Width="25%" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-3">
                            <p class="text-muted">No timeline entries found.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <div class="card-footer bg-white py-2">
            <small class="text-muted">
                <i class="fas fa-info-circle me-1"></i>
                Timeline shows all status changes for this order
            </small>
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
    </script>
</asp:Content>