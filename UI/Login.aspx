<!-- =========================================== -->
<!-- UI/Login.aspx -->
<!-- =========================================== -->

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RestaurantManagementSystem.UI.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login - Restaurant Management System</title>
    
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome 6 -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    
    <style>
        body {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .login-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            padding: 40px;
            max-width: 420px;
            width: 100%;
        }
        .login-header {
            text-align: center;
            margin-bottom: 30px;
        }
        .login-header i {
            font-size: 48px;
            color: #667eea;
        }
        .login-header h3 {
            font-weight: 700;
            margin-top: 10px;
        }
        .login-header p {
            color: #6c757d;
            font-size: 14px;
        }
        .form-control:focus {
            border-color: #667eea;
            box-shadow: 0 0 0 0.2rem rgba(102, 126, 234, 0.25);
        }
        .btn-primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            padding: 12px;
            font-weight: 600;
        }
        .btn-primary:hover {
            opacity: 0.9;
        }
        .alert {
            border-radius: 10px;
        }
        .remember-me {
            display: flex;
            align-items: center;
            gap: 8px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-card">
            <div class="login-header">
                <i class="fas fa-utensils"></i>
                <h3>Restaurant Management</h3>
                <p>Sign in to access your dashboard</p>
            </div>

            <!-- Error Message -->
            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissible fade show">
                <i class="fas fa-exclamation-circle me-2"></i>
                <asp:Literal ID="ltrErrorMessage" runat="server" />
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </asp:Panel>

            <!-- Success Message -->
            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show">
                <i class="fas fa-check-circle me-2"></i>
                <asp:Literal ID="ltrSuccessMessage" runat="server" />
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </asp:Panel>

            <div class="mb-3">
                <label class="form-label fw-bold">
                    <i class="fas fa-user me-1"></i>Username
                </label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" 
                    placeholder="Enter your username" autocomplete="username" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                    ControlToValidate="txtUsername" CssClass="text-danger small" 
                    ErrorMessage="Username is required" Display="Dynamic" />
            </div>

            <div class="mb-3">
                <label class="form-label fw-bold">
                    <i class="fas fa-lock me-1"></i>Password
                </label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" 
                    TextMode="Password" placeholder="Enter your password" autocomplete="current-password" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword" CssClass="text-danger small" 
                    ErrorMessage="Password is required" Display="Dynamic" />
            </div>

            <div class="mb-3 d-flex justify-content-between align-items-center">
                <div class="remember-me">
                    <asp:CheckBox ID="chkRememberMe" runat="server" />
                    <label for="<%= chkRememberMe.ClientID %>" class="mb-0">Remember Me</label>
                </div>
                <a href="ForgotPassword.aspx" class="text-decoration-none small">Forgot Password?</a>
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Sign In" 
                CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
            
            <hr class="my-3" />
            
            <div class="text-center">
                <small class="text-muted">
                    <i class="fas fa-shield-alt me-1"></i>Secure Login
                </small>
            </div>
        </div>
    </form>

    <!-- Bootstrap 5 JavaScript -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>