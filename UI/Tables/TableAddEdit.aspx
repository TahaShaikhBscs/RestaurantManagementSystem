

<%@ Page Title="Table Details" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="TableAddEdit.aspx.cs" Inherits="RestaurantManagementSystem.UI.Tables.TableAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Table Details - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-chair text-primary me-2"></i>
                        <asp:Literal ID="ltrPageTitle" runat="server" Text="Table Details" />
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="TableList.aspx">Tables</a></li>
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

    <!-- Table Form -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3">
            <h5 class="mb-0">
                <i class="fas fa-edit me-2 text-primary"></i>
                <asp:Literal ID="ltrFormTitle" runat="server" Text="Table Information" />
            </h5>
        </div>
        <div class="card-body">
            <div class="row">
                <!-- Left Column -->
                <div class="col-md-6">
                    <!-- Table Number -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-hashtag me-1 text-primary"></i>Table Number <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtTableNumber" runat="server" CssClass="form-control" MaxLength="50" 
                            placeholder="e.g., T1, Table 1, A1" />
                        <asp:RequiredFieldValidator ID="rfvTableNumber" runat="server" 
                            ControlToValidate="txtTableNumber" CssClass="text-danger small mt-1"
                            ErrorMessage="Table number is required." Display="Dynamic" />
                    </div>

                    <!-- Floor -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-layer-group me-1 text-primary"></i>Floor
                        </label>
                        <asp:TextBox ID="txtFloor" runat="server" CssClass="form-control" MaxLength="50" 
                            placeholder="e.g., Ground Floor, First Floor, Patio" />
                    </div>
                </div>

                <!-- Right Column -->
                <div class="col-md-6">
                    <!-- Capacity -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-users me-1 text-primary"></i>Capacity <span class="text-danger">*</span>
                        </label>
                        <asp:TextBox ID="txtCapacity" runat="server" CssClass="form-control" Text="4" />
                        <asp:RequiredFieldValidator ID="rfvCapacity" runat="server" 
                            ControlToValidate="txtCapacity" CssClass="text-danger small mt-1"
                            ErrorMessage="Capacity is required." Display="Dynamic" />
                        <asp:RangeValidator ID="rvCapacity" runat="server"
                            ControlToValidate="txtCapacity" CssClass="text-danger small mt-1"
                            MinimumValue="1" MaximumValue="50" Type="Integer"
                            ErrorMessage="Capacity must be between 1 and 50." Display="Dynamic" />
                    </div>

                    <!-- Status -->
                    <div class="mb-3">
                        <label class="form-label fw-bold">
                            <i class="fas fa-info-circle me-1 text-primary"></i>Status
                        </label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                            <asp:ListItem Value="Available">Available</asp:ListItem>
                            <asp:ListItem Value="Occupied">Occupied</asp:ListItem>
                            <asp:ListItem Value="Reserved">Reserved</asp:ListItem>
                            <asp:ListItem Value="Cleaning">Cleaning</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <hr />

            <!-- Settings Row -->
            <div class="row">
                <div class="col-md-6">
                    <div class="card bg-light p-3">
                        <div class="form-check form-switch">
                            <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label fw-bold" for="<%= chkIsActive.ClientID %>">
                                <i class="fas fa-power-off me-1 text-success"></i>Active
                            </label>
                        </div>
                        <small class="text-muted">Inactive tables are not available for seating</small>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="card bg-light p-3">
                        <div class="d-flex align-items-center">
                            <i class="fas fa-qrcode text-info fa-2x me-3"></i>
                            <div>
                                <strong>QR Code</strong>
                                <br />
                                <small class="text-muted">QR code will be generated automatically for this table</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Info Box -->
            <div class="row mt-4">
                <div class="col-12">
                    <div class="card bg-info bg-opacity-10 border-info">
                        <div class="card-body py-2">
                            <div class="d-flex align-items-center">
                                <i class="fas fa-info-circle text-info fa-lg me-2"></i>
                                <small class="text-muted">
                                    <strong>Note:</strong> Tables with "Available" status can be assigned to orders. 
                                    Status is automatically updated when orders are placed or completed.
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
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success px-4" OnClick="btnSave_Click" Text="Save Table" />
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
            setTimeout(function() {
                $('.alert').fadeOut('slow');
            }, 5000);
        });
    </script>
</asp:Content>