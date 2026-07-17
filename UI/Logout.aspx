
<%@ Page Title="Logout" Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="RestaurantManagementSystem.UI.Logout" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Logout - Restaurant Management System</title>
    
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
        .logout-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            padding: 40px;
            max-width: 420px;
            width: 100%;
            text-align: center;
        }
        .logout-icon {
            font-size: 64px;
            color: #667eea;
            margin-bottom: 20px;
        }
        .logout-title {
            font-weight: 700;
            color: #2d3748;
            margin-bottom: 10px;
        }
        .logout-message {
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="logout-card">
            <div class="logout-icon">
                <i class="fas fa-sign-out-alt"></i>
            </div>
            
            <h2 class="logout-title">You Have Been Logged Out</h2>
            <p class="logout-message">
                <asp:Literal ID="ltrMessage" runat="server" Text="Thank you for using the Restaurant Management System." />
            </p>

            <div class="d-grid gap-2">
                <asp:Button ID="btnLogin" runat="server" Text="Login Again" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                <asp:Button ID="btnDashboard" runat="server" Text="Go to Dashboard" CssClass="btn btn-secondary" OnClick="btnDashboard_Click" />
            </div>

            <hr class="my-3" />
            
            <div class="text-center">
                <small class="text-muted">
                    <i class="fas fa-shield-alt me-1"></i>
                    Your session has been securely terminated
                </small>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>