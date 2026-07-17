

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="RestaurantManagementSystem.Website.Controls.Header" %>

<header class="header">
    <nav class="navbar navbar-expand-lg fixed-top">
        <div class="container">
            <a class="navbar-brand" href="~/Website/Pages/Home.aspx" runat="server">
                <asp:Image ID="imgLogo" runat="server" CssClass="logo-img" ImageUrl="/Website/Assets/images/logo.png" AlternateText="Restaurant Logo" />
                <span class="brand-text"><asp:Literal ID="ltrSiteName" runat="server" Text="Restaurant Name" /></span>
            </a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <a class="nav-link <%# GetActiveClass("Home") %>" href="Home.aspx">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link <%# GetActiveClass("About") %>" href="About.aspx">About</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link <%# GetActiveClass("Menu") %>" href="Menu.aspx">Menu</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link <%# GetActiveClass("Gallery") %>" href="Gallery.aspx">Gallery</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link <%# GetActiveClass("Blog") %>" href="Blog.aspx">Blog</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link <%# GetActiveClass("Contact") %>" href="Contact.aspx">Contact</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link btn-reserve" href="Reservation.aspx">
                            <i class="fas fa-utensils me-1"></i>Reserve
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</header>

<script runat="server">
    /// <summary>
    /// Gets the active class for the current page
    /// </summary>
    protected string GetActiveClass(string pageName)
    {
        string currentPage = System.IO.Path.GetFileNameWithoutExtension(Request.Path);
        return currentPage == pageName ? "active" : "";
    }
</script>