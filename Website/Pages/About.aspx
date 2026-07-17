
<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Website/MasterPages/WebSiteMaster.master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="RestaurantManagementSystem.Website.Pages.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    About Us - Restaurant Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MetaDescription" runat="server">
    Learn about our story, passion for food, and commitment to excellence. Discover what makes Restaurant Name a premier dining destination.
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <section class="page-header py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="page-title display-4 fw-bold">About Us</h1>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb justify-content-center">
                            <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">About Us</li>
                        </ol>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <!-- About Story -->
    <section class="about-story py-5">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-lg-6">
                    <span class="section-subtitle">Our Story</span>
                    <h2 class="section-title"><asp:Literal ID="ltrStoryTitle" runat="server" Text="A Passion for Culinary Excellence" /></h2>
                    <p class="about-text"><asp:Literal ID="ltrStoryText1" runat="server" Text="Restaurant Name was founded with a simple yet ambitious vision: to create a dining experience that transcends the ordinary. Our journey began with a deep love for food and a desire to share that passion with others." /></p>
                    <p class="about-text"><asp:Literal ID="ltrStoryText2" runat="server" Text="Over the years, we have honed our craft, refined our menu, and assembled a team of talented chefs who share our commitment to excellence. Today, we are proud to be recognized as one of the finest dining establishments in the region." /></p>
                    <p class="about-text"><asp:Literal ID="ltrStoryText3" runat="server" Text="We believe that great food is an art form. Every dish that leaves our kitchen is a masterpiece, crafted with precision, passion, and the finest ingredients available." /></p>
                </div>
                <div class="col-lg-6">
                    <asp:Image ID="imgStory" runat="server" CssClass="img-fluid rounded-4 shadow" 
                        ImageUrl="/Website/Assets/images/about/story.jpg" AlternateText="Our Story" />
                </div>
            </div>
        </div>
    </section>

    <!-- Values Section -->
    <section class="values-section py-5 bg-light">
        <div class="container">
            <div class="text-center mb-5">
                <span class="section-subtitle">Our Values</span>
                <h2 class="section-title">What Drives Us</h2>
            </div>
            <div class="row g-4">
                <div class="col-md-4">
                    <div class="value-card text-center p-4">
                        <div class="value-icon">
                            <i class="fas fa-utensils"></i>
                        </div>
                        <h5>Quality First</h5>
                        <p>We never compromise on quality. Every ingredient is carefully selected for freshness and flavor.</p>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="value-card text-center p-4">
                        <div class="value-icon">
                            <i class="fas fa-handshake"></i>
                        </div>
                        <h5>Warm Hospitality</h5>
                        <p>We treat every guest like family. Our service is attentive, genuine, and always with a smile.</p>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="value-card text-center p-4">
                        <div class="value-icon">
                            <i class="fas fa-leaf"></i>
                        </div>
                        <h5>Sustainability</h5>
                        <p>We are committed to sustainable practices, sourcing local ingredients and reducing our environmental footprint.</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Team Section -->
    <section class="team-section py-5">
        <div class="container">
            <div class="text-center mb-5">
                <span class="section-subtitle">Our Team</span>
                <h2 class="section-title">Meet Our Chefs</h2>
            </div>
            <div class="row g-4">
                <asp:Repeater ID="rptChefs" runat="server">
                    <ItemTemplate>
                        <div class="col-md-4">
                            <div class="chef-card text-center">
                                <div class="chef-image">
                                    <img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("ChefName") %>" class="img-fluid rounded-circle" />
                                </div>
                                <h5 class="mt-3"><%# Eval("ChefName") %></h5>
                                <span class="chef-designation"><%# Eval("Designation") %></span>
                                <p class="chef-bio"><%# Eval("Biography") %></p>
                                <div class="chef-social">
                                    <asp:Literal ID="ltrSocialLinks" runat="server" Text='<%# Eval("SocialLinksHTML") %>' />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
</asp:Content>