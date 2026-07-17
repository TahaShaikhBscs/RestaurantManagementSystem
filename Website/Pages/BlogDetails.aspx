
<%@ Page Title="Blog Details" Language="C#" MasterPageFile="~/Website/MasterPages/WebSiteMaster.master" AutoEventWireup="true" CodeBehind="BlogDetails.aspx.cs" Inherits="RestaurantManagementSystem.Website.Pages.BlogDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Literal ID="ltrPageTitle" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MetaDescription" runat="server">
    <asp:Literal ID="ltrMetaDescription" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <section class="page-header py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="page-title display-4 fw-bold"><asp:Literal ID="ltrBlogTitle" runat="server" /></h1>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb justify-content-center">
                            <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
                            <li class="breadcrumb-item"><a href="Blog.aspx">Blog</a></li>
                            <li class="breadcrumb-item active" aria-current="page"><asp:Literal ID="ltrBreadcrumb" runat="server" /></li>
                        </ol>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <!-- Blog Content -->
    <section class="blog-content py-5">
        <div class="container">
            <div class="row">
                <div class="col-lg-8 mx-auto">
                    <div class="blog-post">
                        <div class="blog-featured-image mb-4">
                            <asp:Image ID="imgFeatured" runat="server" CssClass="img-fluid rounded-4 w-100" />
                        </div>
                        <div class="blog-meta mb-3">
                            <span><i class="fas fa-calendar me-1"></i><asp:Literal ID="ltrPublishDate" runat="server" /></span>
                            <span><i class="fas fa-user me-1"></i><asp:Literal ID="ltrAuthor" runat="server" /></span>
                            <span><i class="fas fa-tag me-1"></i><asp:Literal ID="ltrCategory" runat="server" /></span>
                            <span><i class="fas fa-eye me-1"></i><asp:Literal ID="ltrViews" runat="server" /></span>
                        </div>
                        <div class="blog-body">
                            <asp:Literal ID="ltrContent" runat="server" />
                        </div>
                        <div class="blog-tags mt-4">
                            <strong>Tags:</strong>
                            <asp:Literal ID="ltrTags" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>