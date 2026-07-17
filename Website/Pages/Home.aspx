

<%@ Page Title="Home" Language="C#" MasterPageFile="~/Website/MasterPages/WebSiteMaster.master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="RestaurantManagementSystem.Website.Pages.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home - Restaurant Name - Fine Dining Experience
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MetaDescription" runat="server">
    Experience fine dining at its best. Book your table today and enjoy exquisite cuisine in an elegant atmosphere.
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- ============================================ -->
    <!-- HERO SLIDER -->
    <!-- ============================================ -->
    <section class="hero-section">
        <div id="heroSlider" class="carousel slide" data-bs-ride="carousel">
            <div class="carousel-indicators">
                <asp:Repeater ID="rptSliderIndicators" runat="server">
                    <ItemTemplate>
                        <button type="button" data-bs-target="#heroSlider" data-bs-slide-to="<%# Container.ItemIndex %>" 
                            class='<%# Container.ItemIndex == 0 ? "active" : "" %>'>
                        </button>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="carousel-inner">
                <asp:Repeater ID="rptSliders" runat="server">
                    <ItemTemplate>
                        <div class='carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>'>
                            <div class="hero-slide" style="background-image: url('<%# Eval("ImageUrl") %>');">
                                <div class="container h-100 d-flex align-items-center">
                                    <div class="hero-content">
                                        <span class="hero-subtitle"><%# Eval("Subtitle") %></span>
                                        <h1 class="hero-title"><%# Eval("Title") %></h1>
                                        <p class="hero-description"><%# Eval("Description") %></p>
                                        <div class="hero-buttons">
                                            <a href="Menu.aspx" class="btn btn-primary btn-lg">
                                                <i class="fas fa-utensils me-2"></i>View Menu
                                            </a>
                                            <a href="Reservation.aspx" class="btn btn-outline-light btn-lg">
                                                <i class="fas fa-calendar-check me-2"></i>Book Table
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <button class="carousel-control-prev" type="button" data-bs-target="#heroSlider" data-bs-slide="prev">
                <span class="carousel-control-prev-icon"></span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#heroSlider" data-bs-slide="next">
                <span class="carousel-control-next-icon"></span>
            </button>
        </div>
    </section>

    <!-- ============================================ -->
    <!-- ABOUT SECTION -->
    <!-- ============================================ -->
    <section class="about-section py-5">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-lg-6">
                    <div class="about-image">
                        <asp:Image ID="imgAbout" runat="server" CssClass="img-fluid rounded-4 shadow" 
                            ImageUrl="/Website/Assets/images/about/about-restaurant.jpg" AlternateText="About Restaurant" />
                        <div class="about-experience">
                            <span class="number"><asp:Literal ID="ltrExperienceYears" runat="server" Text="20+" /></span>
                            <span class="label">Years of Excellence</span>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="about-content">
                        <span class="section-subtitle">About Us</span>
                        <h2 class="section-title"><asp:Literal ID="ltrAboutTitle" runat="server" Text="A Culinary Journey Like No Other" /></h2>
                        <p class="about-text"><asp:Literal ID="ltrAboutText1" runat="server" Text="Welcome to Restaurant Name, where passion for food meets artistic expression. Our journey began with a simple vision: to create a dining experience that celebrates the finest ingredients and culinary traditions from around the world." /></p>
                        <p class="about-text"><asp:Literal ID="ltrAboutText2" runat="server" Text="Every dish is crafted with meticulous attention to detail, using locally sourced, seasonal ingredients that tell a story of flavor and authenticity." /></p>
                        <div class="about-features">
                            <div class="feature-item">
                                <i class="fas fa-leaf"></i>
                                <span>Fresh Ingredients</span>
                            </div>
                            <div class="feature-item">
                                <i class="fas fa-chef-hat"></i>
                                <span>Expert Chefs</span>
                            </div>
                            <div class="feature-item">
                                <i class="fas fa-wine-glass-alt"></i>
                                <span>Fine Wine Selection</span>
                            </div>
                            <div class="feature-item">
                                <i class="fas fa-heart"></i>
                                <span>Passionate Service</span>
                            </div>
                        </div>
                        <a href="About.aspx" class="btn btn-primary mt-3">
                            <i class="fas fa-arrow-right me-2"></i>Learn More
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- ============================================ -->
    <!-- SPECIAL OFFERS -->
    <!-- ============================================ -->
    <section class="offers-section py-5 bg-light">
        <div class="container">
            <div class="text-center mb-5">
                <span class="section-subtitle">Special Offers</span>
                <h2 class="section-title">Exclusive Dining Experiences</h2>
                <p class="section-description">Discover our curated selection of special menus and promotions</p>
            </div>
            <div class="row g-4">
                <asp:Repeater ID="rptOffers" runat="server">
                    <ItemTemplate>
                        <div class="col-md-4">
                            <div class="offer-card">
                                <div class="offer-image">
                                    <img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("Title") %>" class="img-fluid" />
                                    <span class="offer-badge"><%# Eval("Badge") %></span>
                                </div>
                                <div class="offer-content">
                                    <h4><%# Eval("Title") %></h4>
                                    <p><%# Eval("Description") %></p>
                                    <div class="offer-details">
                                        <span class="price"><%# Eval("Price") %></span>
                                        <span class="per-person"><%# Eval("PerPerson") %></span>
                                    </div>
                                    <a href="Reservation.aspx" class="btn btn-outline-primary w-100">Book Now</a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>

    <!-- ============================================ -->
    <!-- FEATURED MENU ITEMS -->
    <!-- ============================================ -->
    <section class="featured-menu py-5">
        <div class="container">
            <div class="text-center mb-5">
                <span class="section-subtitle">Our Menu</span>
                <h2 class="section-title">Featured Dishes</h2>
                <p class="section-description">A selection of our most celebrated culinary creations</p>
            </div>
            <div class="row g-4">
                <asp:Repeater ID="rptFeaturedItems" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-3 col-md-6">
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
                                        <a href="Menu.aspx" class="btn btn-sm btn-primary">View Menu</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="text-center mt-4">
                <a href="Menu.aspx" class="btn btn-primary">
                    <i class="fas fa-utensils me-2"></i>View Full Menu
                </a>
            </div>
        </div>
    </section>

    <!-- ============================================ -->
    <!-- TESTIMONIALS -->
    <!-- ============================================ -->
    <section class="testimonials-section py-5 bg-light">
        <div class="container">
            <div class="text-center mb-5">
                <span class="section-subtitle">Testimonials</span>
                <h2 class="section-title">What Our Guests Say</h2>
            </div>
            <div class="row g-4">
                <asp:Repeater ID="rptTestimonials" runat="server">
                    <ItemTemplate>
                        <div class="col-md-4">
                            <div class="testimonial-card">
                                <div class="testimonial-rating">
                                    <i class="fas fa-star"></i>
                                    <i class="fas fa-star"></i>
                                    <i class="fas fa-star"></i>
                                    <i class="fas fa-star"></i>
                                    <i class="fas fa-star"></i>
                                </div>
                                <p class="testimonial-text">"<%# Eval("Content") %>"</p>
                                <div class="testimonial-author">
                                    <img src="<%# Eval("CustomerImageUrl") %>" alt="<%# Eval("CustomerName") %>" class="author-image" />
                                    <div class="author-info">
                                        <h6><%# Eval("CustomerName") %></h6>
                                        <span><%# Eval("CustomerDesignation") %></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>

    <!-- ============================================ -->
    <!-- BLOG PREVIEW -->
    <!-- ============================================ -->
    <section class="blog-preview py-5">
        <div class="container">
            <div class="text-center mb-5">
                <span class="section-subtitle">Our Blog</span>
                <h2 class="section-title">Latest Stories &amp; Inspiration</h2>
            </div>
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

    <!-- ============================================ -->
    <!-- CTA SECTION -->
    <!-- ============================================ -->
    <section class="cta-section py-5">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-lg-8">
                    <h2 class="cta-title">Ready for an Unforgettable Experience?</h2>
                    <p class="cta-text">Book your table today and let us create a memorable dining experience for you.</p>
                </div>
                <div class="col-lg-4 text-lg-end">
                    <a href="Reservation.aspx" class="btn btn-light btn-lg">
                        <i class="fas fa-calendar-check me-2"></i>Reserve Now
                    </a>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
