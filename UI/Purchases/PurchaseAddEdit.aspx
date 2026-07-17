

<%@ Page Title="Purchase Order Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="PurchaseAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Purchases.PurchaseAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Purchase Order Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-shopping-cart text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Purchase Order Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="PurchaseList.aspx">Purchases</a></li>
                            <li class="breadcrumb-item active" aria-current="page">
                                <asp:Literal ID="ltrBreadcrumb" runat="server" Text="Add New" />
                            </li>
                        </ol>
                    </nav>
                </div>
                <div>
                    <asp:Button ID="btnCancel" runat="server" Text="Back to List" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
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

    <!-- Purchase Order Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Purchase Order Information" />
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <!-- Left Column -->
                <div class="col-md-6">
                    <!-- PO Number (Read-only) -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-hashtag me-1 text-primary"></i>PO Number
                        </label>
                        <asp:TextBox ID="txtPONumber" runat="server" CssClass="form-control" ReadOnly="true" BackColor="#f8f9fa" />
                    </div>

                    <!-- Supplier -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-truck me-1 text-primary"></i>Supplier <span class="text-danger">*</span>
                        </label>
                        <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-select"
                            DataTextField="SupplierName" DataValueField="SupplierID" AppendDataBoundItems="True">
                            <asp:ListItem Value="">-- Select Supplier --</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSupplier" runat="server" 
                            ControlToValidate="ddlSupplier" CssClass="text-danger small mt-1"
                            ErrorMessage="Supplier is required." Display="Dynamic"
                            InitialValue="" />
                    </div>

                    <!-- Order Date -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-calendar me-1 text-primary"></i>Order Date
                        </label>
                        <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>

                    <!-- Expected Delivery Date -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-calendar-plus me-1 text-primary"></i>Expected Delivery Date
                        </label>
                        <asp:TextBox ID="txtExpectedDelivery" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>
                </div>

                <!-- Right Column -->
                <div class="col-md-6">
                    <!-- Status -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-info-circle me-1 text-primary"></i>Status
                        </label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Draft">Draft</asp:ListItem>
                            <asp:ListItem Value="Sent">Sent</asp:ListItem>
                            <asp:ListItem Value="Received">Received</asp:ListItem>
                            <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Payment Status -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-credit-card me-1 text-primary"></i>Payment Status
                        </label>
                        <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Partial">Partial</asp:ListItem>
                            <asp:ListItem Value="Paid">Paid</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <!-- Notes -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-align-left me-1 text-primary"></i>Notes
                        </label>
                        <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="3" MaxLength="500" placeholder="Enter any additional notes" />
                    </div>
                </div>
            </div>

            <hr />

            <!-- Order Items Section -->
            <div class="row mt-3">
                <div class="col-12">
                    <h6 class="fw-bold mb-3">
                        <i class="fas fa-list-ul me-2 text-primary"></i>Order Items
                    </h6>
                    
                    <!-- Add Item Row -->
                    <div class="row g-2 mb-3">
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStockItem" runat="server" CssClass="form-select"
                                DataTextField="ProductName" DataValueField="StockID" AppendDataBoundItems="True">
                                <asp:ListItem Value="">-- Select Item --</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Qty" Text="1" />
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" placeholder="Price" Text="0.00" />
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtItemTotal" runat="server" CssClass="form-control" placeholder="Total" ReadOnly="true" BackColor="#f8f9fa" />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnAddItem" runat="server" Text="Add Item" CssClass="btn btn-primary w-100" OnClick="btnAddItem_Click" />
                        </div>
                    </div>

                    <!-- Items Grid -->
                    <div class="table-responsive">
                        <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-hover table-striped" GridLines="None"
                            OnRowCommand="gvItems_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="StockID" HeaderText="ID" Visible="False" />
                                <asp:BoundField DataField="ProductName" HeaderText="Item" ItemStyle-Width="35%" />
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" ItemStyle-Width="15%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="TotalPrice" HeaderText="Total" ItemStyle-Width="20%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-sm btn-danger"
                                            CommandName="RemoveItem" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center py-3">
                                    <p class="text-muted">No items added yet</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Totals -->
            <div class="row mt-3">
                <div class="col-md-6 offset-md-6">
                    <div class="table-responsive">
                        <table class="table table-borderless">
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
                            <tr class="table-primary">
                                <td><strong>Total:</strong></td>
                                <td class="text-end fw-bold"><asp:Literal ID="ltrTotal" runat="server" Text="0.00" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="row mt-4">
                <div class="col-12">
                    <hr />
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" Text="Save Purchase Order" />
                        <asp:Button ID="btnCancel2" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            // Calculate item total
            $('#<%= txtQuantity.ClientID %>, #<%= txtUnitPrice.ClientID %>').on('input', function() {
                var quantity = parseFloat($('#<%= txtQuantity.ClientID %>').val()) || 0;
                var price = parseFloat($('#<%= txtUnitPrice.ClientID %>').val()) || 0;
                var total = quantity * price;
                $('#<%= txtItemTotal.ClientID %>').val(total.toFixed(2));
            });

            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>