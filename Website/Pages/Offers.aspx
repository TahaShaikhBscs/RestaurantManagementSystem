
<%@ Page Title="Special Offers" Language="C#" MasterPageFile="~/Website/MasterPages/WebSiteMaster.master" AutoEventWireup="true" CodeBehind="Offers.aspx.cs" Inherits="RestaurantManagementSystem.Website.Pages.Offers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Special Offers - Restaurant Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MetaDescription" runat="server">
    Discover our exclusive special offers and promotions at Restaurant Name. Enjoy exceptional dining experiences at great value.
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <section class="page-header py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="page-title display-4 fw-bold">Special Offers</h1>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb justify-content-center">
                            <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Offers</li>
                        </ol>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <!-- Offers Grid -->
    <section class="offers-grid py-5">
        <div class="container">
            <div class="row g-4">
                <asp:Repeater ID="rptOffers" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6 col-lg-4">
                            <div class="offer-card">
                                <div class="offer-image">
                                    <img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("Title") %>" class="img-fluid" />
                                    <span class="offer-badge"><%# Eval("DiscountText") %></span>
                                </div>
                                <div class="offer-content">
                                    <h4><%# Eval("Title") %></h4>
                                    <p><%# Eval("Description") %></p>
                                    <div class="offer-details">
                                        <span class="price"><%# Eval("DiscountText") %></span>
                                        <span class="per-person"><%# Eval("MinOrderAmount") > 0 ? "Min. Order $" + Eval("MinOrderAmount") : "" %></span>
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
</asp:Content>