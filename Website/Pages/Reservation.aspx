
<%@ Page Title="Reservation" Language="C#" MasterPageFile="~/Website/MasterPages/WebSiteMaster.master" AutoEventWireup="true" CodeBehind="Reservation.aspx.cs" Inherits="RestaurantManagementSystem.Website.Pages.Reservation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reservation - Restaurant Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MetaDescription" runat="server">
    Book your table online for an unforgettable dining experience at Restaurant Name. Reserve now for lunch, dinner, or special events.
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <section class="page-header py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="page-title display-4 fw-bold">Make a Reservation</h1>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb justify-content-center">
                            <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Reservation</li>
                        </ol>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <!-- Reservation Form -->
    <section class="reservation-section py-5">
        <div class="container">
            <div class="row">
                <div class="col-lg-6">
                    <div class="reservation-form">
                        <h2 class="section-title">Book Your Table</h2>
                        <p>Fill in the details below to make a reservation.</p>

                        <!-- Status Messages -->
                        <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success">
                            <i class="fas fa-check-circle me-2"></i>
                            <asp:Literal ID="ltrSuccessMessage" runat="server" />
                        </asp:Panel>

                        <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
                            <i class="fas fa-exclamation-circle me-2"></i>
                            <asp:Literal ID="ltrErrorMessage" runat="server" />
                        </asp:Panel>

                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Full Name <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Your full name" />
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                                    ControlToValidate="txtName" CssClass="text-danger small"
                                    ErrorMessage="Name is required" Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Email <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="your@email.com" />
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                    ControlToValidate="txtEmail" CssClass="text-danger small"
                                    ErrorMessage="Email is required" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                    ControlToValidate="txtEmail" CssClass="text-danger small"
                                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                                    ErrorMessage="Invalid email format" Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Phone <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="+1 234 567 890" />
                                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" 
                                    ControlToValidate="txtPhone" CssClass="text-danger small"
                                    ErrorMessage="Phone is required" Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Number of Guests <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlGuests" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="1">1 Guest</asp:ListItem>
                                    <asp:ListItem Value="2">2 Guests</asp:ListItem>
                                    <asp:ListItem Value="3">3 Guests</asp:ListItem>
                                    <asp:ListItem Value="4">4 Guests</asp:ListItem>
                                    <asp:ListItem Value="5">5 Guests</asp:ListItem>
                                    <asp:ListItem Value="6">6 Guests</asp:ListItem>
                                    <asp:ListItem Value="7">7 Guests</asp:ListItem>
                                    <asp:ListItem Value="8">8 Guests</asp:ListItem>
                                    <asp:ListItem Value="9">9 Guests</asp:ListItem>
                                    <asp:ListItem Value="10">10 Guests</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Date <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" />
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" 
                                    ControlToValidate="txtDate" CssClass="text-danger small"
                                    ErrorMessage="Date is required" Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Time <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlTime" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="11:00">11:00 AM</asp:ListItem>
                                    <asp:ListItem Value="11:30">11:30 AM</asp:ListItem>
                                    <asp:ListItem Value="12:00">12:00 PM</asp:ListItem>
                                    <asp:ListItem Value="12:30">12:30 PM</asp:ListItem>
                                    <asp:ListItem Value="13:00">1:00 PM</asp:ListItem>
                                    <asp:ListItem Value="13:30">1:30 PM</asp:ListItem>
                                    <asp:ListItem Value="14:00">2:00 PM</asp:ListItem>
                                    <asp:ListItem Value="17:00">5:00 PM</asp:ListItem>
                                    <asp:ListItem Value="17:30">5:30 PM</asp:ListItem>
                                    <asp:ListItem Value="18:00">6:00 PM</asp:ListItem>
                                    <asp:ListItem Value="18:30">6:30 PM</asp:ListItem>
                                    <asp:ListItem Value="19:00">7:00 PM</asp:ListItem>
                                    <asp:ListItem Value="19:30">7:30 PM</asp:ListItem>
                                    <asp:ListItem Value="20:00">8:00 PM</asp:ListItem>
                                    <asp:ListItem Value="20:30">8:30 PM</asp:ListItem>
                                    <asp:ListItem Value="21:00">9:00 PM</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-bold">Special Requests</label>
                                <asp:TextBox ID="txtSpecialRequests" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" 
                                    placeholder="Any special requests or dietary requirements?" />
                            </div>
                            <div class="col-12">
                                <asp:Button ID="btnSubmit" runat="server" Text="Make Reservation" CssClass="btn btn-primary w-100" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="reservation-info p-4 bg-light rounded-4">
                        <h4>Contact Information</h4>
                        <ul class="list-unstyled">
                            <li class="mb-3">
                                <i class="fas fa-phone text-primary me-2"></i>
                                <asp:Literal ID="ltrPhone" runat="server" Text="+1 234 567 890" />
                            </li>
                            <li class="mb-3">
                                <i class="fas fa-envelope text-primary me-2"></i>
                                <asp:Literal ID="ltrEmail" runat="server" Text="info@restaurantname.com" />
                            </li>
                            <li class="mb-3">
                                <i class="fas fa-map-marker-alt text-primary me-2"></i>
                                <asp:Literal ID="ltrAddress" runat="server" Text="123 Restaurant Street, City, Country" />
                            </li>
                        </ul>
                        <hr />
                        <h5>Opening Hours</h5>
                        <ul class="list-unstyled">
                            <li><strong>Monday - Friday:</strong> 11:00 AM - 11:00 PM</li>
                            <li><strong>Saturday:</strong> 10:00 AM - 12:00 AM</li>
                            <li><strong>Sunday:</strong> 10:00 AM - 10:00 PM</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>