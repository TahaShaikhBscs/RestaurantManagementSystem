<%@ Page Title="Category Management" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="CategoryList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Menu.CategoryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Category Management - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <div class="row mb-4">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h2 class="page-title mb-0">
                        <i class="fas fa-tags text-primary me-2"></i>Category Management
                    </h2>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb small mb-0">
                            <li class="breadcrumb-item"><a href="~/Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="MenuList.aspx">Menu</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Categories</li>
                        </ol>
                    </nav>
                </div>
                <div class="mt-2 mt-md-0">
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New Category" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-secondary" OnClick="btnRefresh_Click" />
                    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-info" OnClick="btnExport_Click" />
                </div>
            </div>
            <hr class="mt-2" />
        </div>
    </div>

    <!-- Search and Filter Section -->
    <div class="card border-0 shadow-sm mb-3">
        <div class="card-body py-3">
            <div class="row g-2 align-items-center">
                <div class="col-md-8">
                    <div class="input-group">
                        <span class="input-group-text bg-white">
                            <i class="fas fa-search text-muted"></i>
                        </span>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search categories by name..." />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-warning" OnClick="btnClear_Click" />
                    </div>
                </div>
                <div class="col-md-4 text-end">
                    <span class="text-muted small">
                        <i class="fas fa-info-circle me-1"></i>
                        Manage menu categories and sub-categories
                    </span>
                </div>
            </div>
        </div>
    </div>

    <!-- Statistics Cards -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="card border-0 shadow-sm">
                <div class="card-body py-2">
                    <div class="row text-center">
                        <div class="col-md-3">
                            <span class="badge bg-primary rounded-pill px-3 py-2">
                                <i class="fas fa-tags me-1"></i>
                                Total:
                                <asp:Literal ID="ltrTotalCount" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-success rounded-pill px-3 py-2">
                                <i class="fas fa-check-circle me-1"></i>
                                Active:
                                <asp:Literal ID="ltrActiveCount" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-info rounded-pill px-3 py-2">
                                <i class="fas fa-sitemap me-1"></i>
                                Parent:
                                <asp:Literal ID="ltrParentCount" runat="server" Text="0" />
                            </span>
                        </div>
                        <div class="col-md-3">
                            <span class="badge bg-warning rounded-pill px-3 py-2">
                                <i class="fas fa-child me-1"></i>
                                Sub:
                                <asp:Literal ID="ltrSubCount" runat="server" Text="0" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Grid View -->
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center flex-wrap">
            <h5 class="mb-0">
                <i class="fas fa-list-ul me-2 text-primary"></i>Categories List
            </h5>
            <div>
                <span class="badge bg-primary rounded-pill">
                    <i class="fas fa-tag me-1"></i>
                    <asp:Literal ID="ltrGridCount" runat="server" Text="0" />
                    Categories
                </span>
            </div>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-hover table-striped mb-0" GridLines="None"
                    OnRowCommand="gvCategories_RowCommand" OnPageIndexChanging="gvCategories_PageIndexChanging"
                    OnRowDataBound="gvCategories_RowDataBound"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination"
                    DataKeyNames="CategoryID">
                    <Columns>
                        <asp:BoundField DataField="CategoryID" HeaderText="ID" Visible="False" />
                        <asp:TemplateField HeaderText="Category" ItemStyle-Width="25%">
                            <ItemTemplate>
                                <div class="d-flex align-items-center">
                                    <div class="category-icon me-2" style="width: 32px; height: 32px; display: flex; align-items: center; justify-content: center;">
                                        <i class="fas fa-<%# string.IsNullOrEmpty(Eval("Icon").ToString()) ? "folder" : Eval("Icon").ToString() %> text-primary"></i>
                                    </div>
                                    <div>
                                        <div class="fw-bold"><%# Eval("CategoryName") %></div>
                                        <small class="text-muted">
                                            <%# Convert.ToBoolean(Eval("IsRootCategory")) ? "Root" : "Sub-Category" %>
                                        </small>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="ParentCategoryName" HeaderText="Parent Category" ItemStyle-Width="15%" />--%>
                        <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="25%" />
                        <asp:BoundField DataField="DisplayOrder" HeaderText="Order" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span class="badge bg-<%# (bool)Eval("Status") ? "success" : "danger" %> px-3 py-2">
                                    <i class="fas fa-<%# (bool)Eval("Status") ? "check-circle" : "times-circle" %> me-1"></i>
                                    <%# (bool)Eval("Status") ? "Active" : "Inactive" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="btn-group btn-group-sm" role="group">
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary"
                                        CommandName="EditCategory" CommandArgument='<%# Eval("CategoryID") %>' />
                                    <%--<asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"
                                        CommandName="DeleteCategory" CommandArgument='<%# Eval("CategoryID") %>'
                                        data-bs-toggle="modal" data-bs-target="#deleteModal" />--%>
                                    <button type="button"
                                        class="btn btn-danger btn-sm"
                                        onclick="showDeleteModal('<%# Eval("CategoryID") %>')">
                                        Delete
                                    </button>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center py-5">
                            <i class="fas fa-tags fa-4x text-muted mb-3 d-block"></i>
                            <h5 class="text-muted">No categories found</h5>
                            <p class="text-muted small">Start by creating your first category</p>
                            <asp:Button ID="btnAddFirst" runat="server" Text="Add First Category" CssClass="btn btn-success mt-2" OnClick="btnAddNew_Click" />
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
                        Categories help organize menu items for easy navigation
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

    <!-- Delete Confirmation Modal -->
    <div class="modal fade" id="deleteModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Confirm Delete</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p>Are you sure you want to delete this category?</p>
                    <p class="text-danger"><strong>Warning:</strong> This action cannot be undone.</p>
                    <p>Category must not have any sub-categories or menu items.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <asp:Button ID="btnConfirmDelete" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="btnConfirmDelete_Click" />
                    <asp:HiddenField ID="hfDeleteCategoryID" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <style>
        .category-icon {
            min-width: 32px;
            min-height: 32px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            // Auto-hide alerts after 5 seconds
            setTimeout(function () {
                $('.alert').fadeOut('slow');
            }, 5000);

            // Set category ID for delete modal
            $('[data-bs-toggle="modal"]').on('click', function () {
                var categoryID = $(this).closest('tr').find('[id$="btnDelete"]').attr('CommandArgument');
                $('#<%= hfDeleteCategoryID.ClientID %>').val(categoryID);
            });
        }); 
        function showDeleteModal(id) {

            document.getElementById('<%= hfDeleteCategoryID.ClientID %>').value = id;

            var modal = new bootstrap.Modal(document.getElementById('deleteModal'));
            modal.show();
        }
    </script>
</asp:Content>
