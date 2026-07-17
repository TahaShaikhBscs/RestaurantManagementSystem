
<%@ Page Title="Forgot Password" Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="RestaurantManagementSystem.UI.ForgotPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Forgot Password - Restaurant Management System</title>
    
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    
    <style>
        body {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .forgot-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            padding: 40px;
            max-width: 420px;
            width: 100%;
        }
        .forgot-header {
            text-align: center;
            margin-bottom: 30px;
        }
        .forgot-header i {
            font-size: 48px;
            color: #667eea;
        }
        .forgot-header h3 {
            font-weight: 700;
            margin-top: 10px;
        }
        .forgot-header p {
            color: #6c757d;
            font-size: 14px;
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
        .btn-secondary {
            padding: 12px;
            font-weight: 600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="forgot-card">
            <div class="forgot-header">
                <i class="fas fa-key"></i>
                <h3>Forgot Password</h3>
                <p>Enter your email address to reset your password</p>
            </div>

            <!-- Status Messages -->
            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert alert-success">
                <i class="fas fa-check-circle me-2"></i>
                <asp:Literal ID="ltrSuccessMessage" runat="server" />
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger">
                <i class="fas fa-exclamation-circle me-2"></i>
                <asp:Literal ID="ltrErrorMessage" runat="server" />
            </asp:Panel>

            <!-- Email Input -->
            <div class="mb-3">
                <label class="form-label fw-bold">
                    <i class="fas fa-envelope me-1"></i>Email Address <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                    placeholder="Enter your registered email address" autocomplete="email" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail" CssClass="text-danger small" 
                    ErrorMessage="Email address is required" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail" CssClass="text-danger small"
                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                    ErrorMessage="Please enter a valid email address" Display="Dynamic" />
            </div>

            <div class="d-grid gap-2">
                <asp:Button ID="btnSendResetLink" runat="server" Text="Send Reset Link" 
                    CssClass="btn btn-primary" OnClick="btnSendResetLink_Click" />
                <asp:Button ID="btnBackToLogin" runat="server" Text="Back to Login" 
                    CssClass="btn btn-secondary" OnClick="btnBackToLogin_Click" />
            </div>

            <hr class="my-3" />
            
            <div class="text-center">
                <small class="text-muted">
                    <i class="fas fa-shield-alt me-1"></i>
                    A password reset link will be sent to your email
                </small>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>