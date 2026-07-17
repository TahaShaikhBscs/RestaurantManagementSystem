
<%@ Page Title="User Management" Language="C#" MasterPageFile="~/UI/MasterPage.master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="RestaurantManagementSystem.UI.Users.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    User Management - Restaurant Management System
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4">
        <div class="col-12">
            <h2 class="page-title">
                <i class="fas fa-users me-2"></i>User Management
            </h2>
            <hr />
        </div>
    </div>

    <!-- Search and Actions -->
    <div class="row mb-3">
        <div class="col-md-5">
            <div class="input-group">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search users..." />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" />
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:DropDownList ID="ddlBranchFilter" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlBranchFilter_SelectedIndexChanged">
                    <asp:ListItem Value="0">All Branches</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-3 text-end">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New User" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
        </div>
    </div>

    <!-- Grid View -->
    <div class="card border-0 shadow-sm">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-hover mb-0" GridLines="None"
                    OnRowCommand="gvUsers_RowCommand" OnPageIndexChanging="gvUsers_PageIndexChanging"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="pagination">
                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="ID" Visible="False" />
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" />
                        <asp:BoundField DataField="RoleName" HeaderText="Role" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class="badge bg-<%# (bool)Eval("IsActive") ? "success" : "danger" %>">
                                    <%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary"
                                    CommandName="EditUser" CommandArgument='<%# Eval("UserID") %>' />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-sm btn-danger"
                                    CommandName="DeleteUser" CommandArgument='<%# Eval("UserID") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this user?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center p-4">
                            <i class="fas fa-users fa-3x text-muted mb-3 d-block"></i>
                            <p class="text-muted">No users found.</p>
                            <asp:Button ID="btnAddFirst" runat="server" Text="Add First User" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- Status Messages -->
    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show mt-3">
        <i class="fas fa-check-circle me-2"></i>
        <asp:Literal ID="ltrSuccessMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show mt-3">
        <i class="fas fa-exclamation-circle me-2"></i>
        <asp:Literal ID="ltrErrorMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
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