
<%@ Page Title="Blog" Language="C#" MasterPageFile="~/Website/MasterPages/WebSiteMaster.master" AutoEventWireup="true" CodeBehind="Blog.aspx.cs" Inherits="RestaurantManagementSystem.Website.Pages.Blog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Blog - Restaurant Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MetaDescription" runat="server">
    Read the latest stories, recipes, and culinary inspiration from Restaurant Name. Stay updated with our blog.
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <section class="page-header py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="page-title display-4 fw-bold">Our Blog</h1>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb justify-content-center">
                            <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Blog</li>
                        </ol>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <!-- Blog Grid -->
    <section class="blog-grid py-5">
        <div class="container">
            <div class="row g-4">
                <asp:Repeater ID="rptBlogPosts" runat="server">
                    <ItemTemplate>
                        <div class="col-md-4">
                            <article class="blog-card">
                                <div class="blog-image">
                                    <img src="<%# Eval("FeaturedImage") %>" alt="<%# Eval("Title") %>" class="img-fluid" />
                                    <span class="blog-date"><%# Convert.ToDateTime(Eval("PublishDate")).ToString("MMM dd, yyyy") %></span>
                                </div>
                                <div class="blog-content">
                                    <span class="blog-category"><%# Eval("Category") %></span>
                                    <h5 class="blog-title"><a href="BlogDetails.aspx?ID=<%# Eval("BlogPostID") %>"><%# Eval("Title") %></a></h5>
                                    <p class="blog-excerpt"><%# Eval("Excerpt") %></p>
                                    <div class="blog-meta">
                                        <span><i class="fas fa-user me-1"></i><%# Eval("Author") %></span>
                                        <span><i class="fas fa-eye me-1"></i><%# Eval("ViewCount") %></span>
                                    </div>
                                    <a href="BlogDetails.aspx?ID=<%# Eval("BlogPostID") %>" class="btn btn-link p-0">
                                        Read More <i class="fas fa-arrow-right ms-1"></i>
                                    </a>
                                </div>
                            </article>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</asp:Content>