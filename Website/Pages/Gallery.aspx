
<%@ Page Title="Gallery" Language="C#" MasterPageFile="~/Website/MasterPages/WebSiteMaster.master" AutoEventWireup="true" CodeBehind="Gallery.aspx.cs" Inherits="RestaurantManagementSystem.Website.Pages.Gallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gallery - Restaurant Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MetaDescription" runat="server">
    Explore our gallery of culinary creations, restaurant ambiance, and special events. Get a glimpse of the Restaurant Name experience.
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <section class="page-header py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="page-title display-4 fw-bold">Gallery</h1>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb justify-content-center">
                            <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Gallery</li>
                        </ol>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <!-- Gallery Grid -->
    <section class="gallery-section py-5">
        <div class="container">
            <div class="row g-4">
                <asp:Repeater ID="rptGallery" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-3 col-md-4 col-sm-6">
                            <div class="gallery-item" data-bs-toggle="modal" data-bs-target="#galleryModal" 
                                 data-image="<%# Eval("ImagePath") %>" data-title="<%# Eval("Title") %>" 
                                 data-description="<%# Eval("Description") %>">
                                <img src="<%# Eval("ThumbnailPath") %>" alt="<%# Eval("AltText") %>" class="img-fluid" />
                                <div class="gallery-overlay">
                                    <i class="fas fa-search-plus"></i>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>

    <!-- Lightbox Modal -->
    <div class="modal fade" id="galleryModal" tabindex="-1">
        <div class="modal-dialog modal-lg modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-body p-0">
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    <img id="modalImage" src="" alt="" class="img-fluid w-100" />
                    <div class="modal-caption p-3">
                        <h5 id="modalTitle"></h5>
                        <p id="modalDescription" class="text-muted"></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>