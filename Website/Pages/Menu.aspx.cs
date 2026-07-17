// ============================================
// Website/Pages/Menu.aspx.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Web.UI;
using RestaurantManagementSystem.BAL;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Website.Pages
{
    /// <summary>
    /// Menu Page - Public website menu display
    /// </summary>
    public partial class Menu : Page
    {
        private MenuBAL menuBAL;
        private CategoryBAL categoryBAL;
        private CompanyBAL companyBAL;
        private int companyID;
        private int branchID;

        protected void Page_Load(object sender, EventArgs e)
        {
            menuBAL = new MenuBAL();
            categoryBAL = new CategoryBAL();
            companyBAL = new CompanyBAL();

            var companies = companyBAL.GetAllCompanies();
            if (companies.Count > 0)
            {
                companyID = companies[0].CompanyID;
                // Get first branch
                var branchBAL = new BranchBAL();
                var branches = branchBAL.GetBranchesByCompany(companyID);
                if (branches.Count > 0)
                {
                    branchID = branches[0].BranchID;
                }
            }

            if (!IsPostBack)
            {
                LoadMenuData();
            }
        }

        private void LoadMenuData()
        {
            try
            {
                // Load categories
                var categories = categoryBAL.GetCategoriesByBranch(branchID, false);
                rptCategories.DataSource = categories;
                rptCategories.DataBind();

                // Load all menu items
                var items = menuBAL.GetMenuItemsByBranch(branchID, null, false);
                rptMenuItems.DataSource = items;
                rptMenuItems.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading menu: {ex.Message}");
            }
        }
    }
}