

<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Website/MasterPages/WebSiteMaster.master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="RestaurantManagementSystem.Website.Pages.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Contact Us - Restaurant Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MetaDescription" runat="server">
    Get in touch with Restaurant Name. We'd love to hear from you. Contact us for reservations, inquiries, or feedback.
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page Header -->
    <section class="page-header py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="page-title display-4 fw-bold">Contact Us</h1>
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb justify-content-center">
                            <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Contact</li>
                        </ol>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <!-- Contact Section -->
    <section class="contact-section py-5">
        <div class="container">
            <div class="row g-4">
                <!-- Contact Form -->
                <div class="col-lg-6">
                    <div class="contact-form">
                        <h2 class="section-title">Send a Message</h2>
                        <p>We'll get back to you as soon as possible.</p>

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
                                <label class="form-label fw-bold">Name <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Your name" />
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
                                <label class="form-label fw-bold">Phone</label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="+1 234 567 890" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Subject</label>
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="Subject" />
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-bold">Message <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" 
                                    placeholder="Your message..." />
                                <asp:RequiredFieldValidator ID="rfvMessage" runat="server" 
                                    ControlToValidate="txtMessage" CssClass="text-danger small"
                                    ErrorMessage="Message is required" Display="Dynamic" />
                            </div>
                            <div class="col-12">
                                <asp:Button ID="btnSubmit" runat="server" Text="Send Message" CssClass="btn btn-primary w-100" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Contact Info -->
                <div class="col-lg-6">
                    <div class="contact-info p-4 bg-light rounded-4">
                        <h4>Get in Touch</h4>
                        <ul class="list-unstyled">
                            <li class="mb-3">
                                <i class="fas fa-map-marker-alt text-primary fa-lg me-3"></i>
                                <div>
                                    <strong>Address</strong><br />
                                    <asp:Literal ID="ltrAddress" runat="server" Text="123 Restaurant Street, City, Country" />
                                </div>
                            </li>
                            <li class="mb-3">
                                <i class="fas fa-phone text-primary fa-lg me-3"></i>
                                <div>
                                    <strong>Phone</strong><br />
                                    <asp:Literal ID="ltrPhone" runat="server" Text="+1 234 567 890" />
                                </div>
                            </li>
                            <li class="mb-3">
                                <i class="fas fa-envelope text-primary fa-lg me-3"></i>
                                <div>
                                    <strong>Email</strong><br />
                                    <asp:Literal ID="ltrEmail" runat="server" Text="info@restaurantname.com" />
                                </div>
                            </li>
                            <li class="mb-3">
                                <i class="fas fa-clock text-primary fa-lg me-3"></i>
                                <div>
                                    <strong>Opening Hours</strong><br />
                                    <asp:Literal ID="ltrHours" runat="server" Text="Mon-Fri: 11:00 AM - 11:00 PM" /><br />
                                    <asp:Literal ID="ltrSaturday" runat="server" Text="Sat: 10:00 AM - 12:00 AM" /><br />
                                    <asp:Literal ID="ltrSunday" runat="server" Text="Sun: 10:00 AM - 10:00 PM" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>