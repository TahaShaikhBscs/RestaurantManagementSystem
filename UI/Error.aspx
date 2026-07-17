
<%@ Page Title="Error" Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="RestaurantManagementSystem.UI.Error" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Error - Restaurant Management System</title>
    
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
        .error-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            padding: 50px;
            max-width: 500px;
            width: 100%;
            text-align: center;
        }
        .error-icon {
            font-size: 72px;
            color: #dc3545;
            margin-bottom: 20px;
        }
        .error-title {
            font-weight: 700;
            color: #2d3748;
            margin-bottom: 10px;
        }
        .error-message {
            color: #6c757d;
            margin-bottom: 30px;
        }
        .btn-primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            padding: 12px 30px;
            font-weight: 600;
        }
        .btn-primary:hover {
            opacity: 0.9;
        }
        .error-details {
            background: #f8f9fa;
            border-radius: 10px;
            padding: 15px;
            text-align: left;
            margin-top: 20px;
        }
        .error-details small {
            color: #6c757d;
            font-size: 12px;
        }
        .error-details code {
            display: block;
            padding: 10px;
            background: #e9ecef;
            border-radius: 5px;
            margin-top: 5px;
            font-size: 12px;
            word-break: break-all;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="error-card">
            <div class="error-icon">
                <i class="fas fa-exclamation-triangle"></i>
            </div>
            
            <h2 class="error-title">Oops! Something Went Wrong</h2>
            <p class="error-message">
                <asp:Literal ID="ltrErrorMessage" runat="server" Text="We apologize for the inconvenience. Please try again later." />
            </p>

            <!-- Error Details -->
            <asp:Panel ID="pnlErrorDetails" runat="server" Visible="false" CssClass="error-details">
                <small><strong>Error Details:</strong></small>
                <code><asp:Literal ID="ltrErrorDetails" runat="server" /></code>
            </asp:Panel>

            <div class="d-grid gap-2 mt-3">
                <asp:Button ID="btnGoHome" runat="server" Text="Go to Dashboard" CssClass="btn btn-primary" OnClick="btnGoHome_Click" />
                <asp:Button ID="btnGoBack" runat="server" Text="Go Back" CssClass="btn btn-secondary" OnClick="btnGoBack_Click" />
            </div>

            <hr class="my-3" />
            
            <div class="text-center">
                <small class="text-muted">
                    <i class="fas fa-shield-alt me-1"></i>
                    If you continue to see this error, please contact support.
                </small>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>