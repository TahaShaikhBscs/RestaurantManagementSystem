

<%@ Page Title="Reset Password" Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="RestaurantManagementSystem.UI.ResetPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Reset Password - Restaurant Management System</title>
    
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
        .reset-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            padding: 40px;
            max-width: 420px;
            width: 100%;
        }
        .reset-header {
            text-align: center;
            margin-bottom: 30px;
        }
        .reset-header i {
            font-size: 48px;
            color: #667eea;
        }
        .reset-header h3 {
            font-weight: 700;
            margin-top: 10px;
        }
        .reset-header p {
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
        .password-requirements {
            font-size: 12px;
            color: #6c757d;
            margin-top: 5px;
        }
        .password-requirements .valid {
            color: #28a745;
        }
        .password-requirements .invalid {
            color: #dc3545;
        }
        .btn-secondary {
            padding: 12px;
            font-weight: 600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="reset-card">
            <div class="reset-header">
                <i class="fas fa-unlock-alt"></i>
                <h3>Reset Password</h3>
                <p>Enter your new password</p>
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

            <!-- New Password -->
            <div class="mb-3">
                <label class="form-label fw-bold">
                    <i class="fas fa-lock me-1"></i>New Password <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" 
                    TextMode="Password" placeholder="Enter new password" />
                <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" 
                    ControlToValidate="txtNewPassword" CssClass="text-danger small" 
                    ErrorMessage="Password is required" Display="Dynamic" />
                <div class="password-requirements">
                    <div id="lengthReq" class="invalid">✓ At least 8 characters</div>
                    <div id="upperReq" class="invalid">✓ At least one uppercase letter</div>
                    <div id="lowerReq" class="invalid">✓ At least one lowercase letter</div>
                    <div id="digitReq" class="invalid">✓ At least one digit</div>
                    <div id="specialReq" class="invalid">✓ At least one special character</div>
                </div>
            </div>

            <!-- Confirm Password -->
            <div class="mb-3">
                <label class="form-label fw-bold">
                    <i class="fas fa-check-circle me-1"></i>Confirm Password <span class="text-danger">*</span>
                </label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" 
                    TextMode="Password" placeholder="Confirm new password" />
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword" CssClass="text-danger small" 
                    ErrorMessage="Please confirm your password" Display="Dynamic" />
                <asp:CompareValidator ID="cvPassword" runat="server"
                    ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword"
                    CssClass="text-danger small" Operator="Equal" Type="String"
                    ErrorMessage="Passwords do not match" Display="Dynamic" />
            </div>

            <div class="d-grid gap-2">
                <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" 
                    CssClass="btn btn-primary" OnClick="btnResetPassword_Click" />
                <asp:Button ID="btnBackToLogin" runat="server" Text="Back to Login" 
                    CssClass="btn btn-secondary" OnClick="btnBackToLogin_Click" />
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            // Password strength validation
            $('#<%= txtNewPassword.ClientID %>').on('input', function() {
                var password = $(this).val();
                
                // Length check
                if (password.length >= 8) {
                    $('#lengthReq').removeClass('invalid').addClass('valid');
                } else {
                    $('#lengthReq').removeClass('valid').addClass('invalid');
                }
                
                // Uppercase check
                if (/[A-Z]/.test(password)) {
                    $('#upperReq').removeClass('invalid').addClass('valid');
                } else {
                    $('#upperReq').removeClass('valid').addClass('invalid');
                }
                
                // Lowercase check
                if (/[a-z]/.test(password)) {
                    $('#lowerReq').removeClass('invalid').addClass('valid');
                } else {
                    $('#lowerReq').removeClass('valid').addClass('invalid');
                }
                
                // Digit check
                if (/[0-9]/.test(password)) {
                    $('#digitReq').removeClass('invalid').addClass('valid');
                } else {
                    $('#digitReq').removeClass('valid').addClass('invalid');
                }
                
                // Special character check
                if (/[^a-zA-Z0-9]/.test(password)) {
                    $('#specialReq').removeClass('invalid').addClass('valid');
                } else {
                    $('#specialReq').removeClass('valid').addClass('invalid');
                }
            });
        });
    </script>
</body>
</html>