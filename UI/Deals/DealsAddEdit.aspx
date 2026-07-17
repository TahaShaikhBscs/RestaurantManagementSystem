
<%@ Page Title="Deal Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="DealsAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Deals.DealsAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Deal Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-tag text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Deal Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="DealsList.aspx">Deals</a></li>
                            <li class="breadcrumb-item active" aria-current="page">
                                <asp:Literal ID="ltrBreadcrumb" runat="server" Text="Add New" />
                            </li>
                        </ol>
                    </nav>
                </div>
                <div class="mt-2 mt-md-0">
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

    <!-- Deal Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Deal Information" />
            </h5>
        </div>
        <div class="card-body">
            <!-- Two Column Layout -->
            <div class="row">
                <!-- Left Column -->
                <div class="col-md-6">
                    <!-- Deal Name -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-tag me-1 text-primary"></i>Deal Name <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtDealName" runat="server" CssClass="form-control" MaxLength="200" 
                            placeholder="Enter deal name" />
                        <asp:RequiredFieldValidator ID="rfvDealName" runat="server" 
                            ControlToValidate="txtDealName" CssClass="text-danger small mt-1"
                            ErrorMessage="Deal name is required." Display="Dynamic" />
                    </div>

                    <!-- Description -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-align-left me-1 text-primary"></i>Description
                        </label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="3" MaxLength="500" placeholder="Enter deal description" />
                    </div>

                    <!-- Image -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-image me-1 text-primary"></i>Image URL
                        </label>
                        <asp:TextBox ID="txtImage" runat="server" CssClass="form-control" MaxLength="500" 
                            placeholder="Enter image URL" />
                    </div>
                </div>

                <!-- Right Column -->
                <div class="col-md-6">
                    <!-- Deal Price -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-dollar-sign me-1 text-primary"></i>Deal Price <span class="text-danger">*</span>
                        </label>
                        <div class="input-group">
                            <span class="input-group-text">$</span>
                            <asp:TextBox ID="txtDealPrice" runat="server" CssClass="form-control" placeholder="0.00" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvDealPrice" runat="server" 
                            ControlToValidate="txtDealPrice" CssClass="text-danger small mt-1"
                            ErrorMessage="Deal price is required." Display="Dynamic" />
                        <asp:RangeValidator ID="rvDealPrice" runat="server"
                            ControlToValidate="txtDealPrice" CssClass="text-danger small mt-1"
                            MinimumValue="0.01" MaximumValue="9999.99" Type="Currency"
                            ErrorMessage="Price must be between 0.01 and 9999.99." Display="Dynamic" />
                    </div>

                    <!-- Start Date -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-calendar-plus me-1 text-primary"></i>Start Date
                        </label>
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>

                    <!-- End Date -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-calendar-check me-1 text-primary"></i>End Date
                        </label>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>

                    <!-- Status -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-toggle-on me-1 text-primary"></i>Status
                        </label>
                        <div class="card bg-light p-3">
                            <div class="form-check form-switch">
                                <asp:CheckBox ID="chkStatus" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label fw-bold" for="<%= chkStatus.ClientID %>">
                                    <asp:Literal ID="ltrStatusText" runat="server" Text="Active" />
                                </label>
                                <span class="ms-2 text-muted small">(Inactive deals are hidden)</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <hr />

            <!-- Deal Items Section -->
            <div class="row mt-3">
                <div class="col-12">
                    <h6 class="fw-bold mb-3">
                        <i class="fas fa-list-ul me-2 text-primary"></i>Deal Items
                    </h6>
                    
                    <!-- Add Item Row -->
                    <div class="row g-2 mb-3">
                        <div class="col-md-6">
                            <asp:DropDownList ID="ddlMenuItem" runat="server" CssClass="form-select"
                                DataTextField="ItemName" DataValueField="MenuItemID" AppendDataBoundItems="True">
                                <asp:ListItem Value="">-- Select Menu Item --</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Qty" Text="1" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnAddItem" runat="server" Text="Add Item" CssClass="btn btn-primary w-100" OnClick="btnAddItem_Click" />
                        </div>
                    </div>

                    <!-- Items Grid -->
                    <div class="table-responsive">
                        <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False" 
                            CssClass="table table-hover table-striped" GridLines="None"
                            OnRowCommand="gvItems_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="MenuItemID" HeaderText="ID" Visible="False" />
                                <asp:BoundField DataField="ItemName" HeaderText="Item" ItemStyle-Width="35%" />
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Price" HeaderText="Unit Price" ItemStyle-Width="15%" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
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
                                <td><strong>Original Price:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrOriginalPrice" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr>
                                <td><strong>Deal Price:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrDealPrice" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr class="table-success">
                                <td><strong>You Save:</strong></td>
                                <td class="text-end fw-bold text-success"><asp:Literal ID="ltrDiscount" runat="server" Text="0.00" /></td>
                            </tr>
                            <tr>
                                <td><strong>Discount %:</strong></td>
                                <td class="text-end"><asp:Literal ID="ltrDiscountPercent" runat="server" Text="0%" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <!-- Info Box -->
            <div class="row mt-2">
                <div class="col-12">
                    <div class="card bg-info bg-opacity-10 border-info">
                        <div class="card-body py-2">
                            <div class="d-flex align-items-center">
                                <i class="fas fa-info-circle text-info fa-lg me-2"></i>
                                <small class="text-muted">
                                    <strong>Note:</strong> Deals combine multiple menu items at a discounted price. 
                                    The deal price must be less than the total original price.
                                </small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Action Buttons -->
            <div class="row mt-4">
                <div class="col-12">
                    <hr />
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" 
                            Text="Save Deal" />
                        <asp:Button ID="btnCancel2" runat="server" Text="Cancel" CssClass="btn btn-secondary" 
                            OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            // Status text toggle
            $('#<%= chkStatus.ClientID %>').change(function() {
                var statusText = $(this).is(':checked') ? 'Active' : 'Inactive';
                $('#<%= ltrStatusText.ClientID %>').text(statusText);
            });

            // Auto-hide alerts after 5 seconds
            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>