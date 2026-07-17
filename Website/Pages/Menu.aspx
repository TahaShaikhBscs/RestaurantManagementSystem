

<%@ Page Title="Our Menu" Language="C#" MasterPageFile="~/Website/MasterPages/WebSiteMaster.master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="RestaurantManagementSystem.Website.Pages.Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Our Menu - Restaurant Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MetaDescription" runat="server">
    Explore our curated menu featuring exquisite dishes crafted with the finest ingredients. View our full selection of appetizers, main courses, and desserts.
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <section class="page-header py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="page-title display-4 fw-bold">Our Menu</h1>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb justify-content-center">
                            <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Menu</li>
                        </ol>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <!-- Category Navigation -->
    <section class="menu-navigation py-3">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <ul class="nav nav-pills justify-content-center" id="categoryNav">
                        <li class="nav-item">
                            <a class="nav-link active" data-category="">All</a>
                        </li>
                        <asp:Repeater ID="rptCategories" runat="server">
                            <ItemTemplate>
                                <li class="nav-item">
                                    <a class="nav-link" data-category='<%# Eval("CategoryID") %>'>
                                        <%# Eval("CategoryName") %>
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div>
    </section>

    <!-- Menu Items -->
    <section class="menu-items py-5">
        <div class="container">
            <div class="row g-4">
                <asp:Repeater ID="rptMenuItems" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-4 col-md-6 menu-item" data-category='<%# Eval("CategoryID") %>'>
                            <div class="menu-card">
                                <div class="menu-image">
                                    <img src="<%# Eval("ImagePath") %>" alt="<%# Eval("ItemName") %>" class="img-fluid" />
                                    <span class="menu-category"><%# Eval("CategoryName") %></span>
                                </div>
                                <div class="menu-content">
                                    <h5><%# Eval("ItemName") %></h5>
                                    <p><%# Eval("Description") %></p>
                                    <div class="menu-footer">
                                        <span class="menu-price">$<%# Eval("Price") %></span>
                                        <a href="ProductDetails.aspx?ID=<%# Eval("MenuItemID") %>" class="btn btn-sm btn-primary">View Details</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</asp:Content>