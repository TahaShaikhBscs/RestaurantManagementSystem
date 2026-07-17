<!-- ============================================ -->
<!-- Website/Controls/Footer.ascx -->
<!-- ============================================ -->

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="RestaurantManagementSystem.Website.Controls.Footer" %>

<footer class="footer">
    <div class="container">
        <div class="row g-4">
            <!-- About -->
            <div class="col-lg-4 col-md-6">
                <div class="footer-brand">
                    <asp:Image ID="imgFooterLogo" runat="server" CssClass="footer-logo" ImageUrl="/Website/Assets/images/logo-footer.png" AlternateText="Restaurant Logo" />
                    <p class="footer-description">
                        <asp:Literal ID="ltrFooterDescription" runat="server" Text="Experience fine dining at its best. Where culinary artistry meets warm hospitality." />
                    </p>
                    <div class="footer-social">
                        <asp:Repeater ID="rptSocialLinks" runat="server">
                            <ItemTemplate>
                                <a href="<%# Eval("SocialUrl") %>" target="_blank" aria-label="<%# Eval("SocialName") %>">
                                    <i class="fab fa-<%# Eval("SocialIcon") %>"></i>
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <!-- Quick Links -->
            <div class="col-lg-2 col-md-6">
                <h5 class="footer-title">Quick Links</h5>
                <ul class="footer-links">
                    <li><a href="About.aspx">About Us</a></li>
                    <li><a href="Menu.aspx">Our Menu</a></li>
                    <li><a href="Gallery.aspx">Gallery</a></li>
                    <li><a href="Blog.aspx">Blog</a></li>
                    <li><a href="Contact.aspx">Contact</a></li>
                </ul>
            </div>
            <!-- Opening Hours -->
            <div class="col-lg-3 col-md-6">
                <h5 class="footer-title">Opening Hours</h5>
                <ul class="footer-hours">
                    <li>
                        <span>Monday - Friday</span>
                        <span><asp:Literal ID="ltrWeekdayHours" runat="server" Text="11:00 AM - 11:00 PM" /></span>
                    </li>
                    <li>
                        <span>Saturday</span>
                        <span><asp:Literal ID="ltrSaturdayHours" runat="server" Text="10:00 AM - 12:00 AM" /></span>
                    </li>
                    <li>
                        <span>Sunday</span>
                        <span><asp:Literal ID="ltrSundayHours" runat="server" Text="10:00 AM - 10:00 PM" /></span>
                    </li>
                </ul>
            </div>
            <!-- Contact -->
            <div class="col-lg-3 col-md-6">
                <h5 class="footer-title">Contact</h5>
                <ul class="footer-contact">
                    <li>
                        <i class="fas fa-map-marker-alt"></i>
                        <span><asp:Literal ID="ltrAddress" runat="server" Text="123 Restaurant Street, City, Country" /></span>
                    </li>
                    <li>
                        <i class="fas fa-phone"></i>
                        <span><asp:Literal ID="ltrPhone" runat="server" Text="+1 234 567 890" /></span>
                    </li>
                    <li>
                        <i class="fas fa-envelope"></i>
                        <span><asp:Literal ID="ltrEmail" runat="server" Text="info@restaurantname.com" /></span>
                    </li>
                </ul>
            </div>
        </div>
        <div class="footer-bottom">
            <div class="row align-items-center">
                <div class="col-md-6">
                    <p class="copyright">
                        &copy; <asp:Literal ID="ltrCurrentYear" runat="server" /> <asp:Literal ID="ltrCopyright" runat="server" Text="Restaurant Name" />. All rights reserved.
                    </p>
                </div>
                <div class="col-md-6 text-md-end">
                    <ul class="footer-bottom-links">
                        <li><a href="PrivacyPolicy.aspx">Privacy Policy</a></li>
                        <li><a href="Terms.aspx">Terms &amp; Conditions</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</footer>