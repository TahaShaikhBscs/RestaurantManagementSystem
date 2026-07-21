/* ============================================ */
/* POS.js - Enterprise POS JavaScript */
/* Restaurant Management System */
/* ============================================ */

$(document).ready(function () {
    'use strict';

    // ============================================
    // Global Variables
    // ============================================
    var pos = {
        cart: [],
        selectedCategory: null,
        searchTerm: '',
        orderType: 'Dine In',
        selectedTable: null,
        customer: null,
        discount: 0,
        tip: 0,
        selectedPaymentMethod: null
    };

    // ============================================
    // Initialize POS
    // ============================================
    function initPOS() {
        loadCategories();
        loadMenuItems();
        loadTables();
        loadCustomers();
        loadDeals();
        setupEventListeners();
        setupKeyboardShortcuts();
        updateCartUI();
        updateTotals();
    }

    // ============================================
    // Load Data Functions
    // ============================================
    function loadCategories() {
        // AJAX call to get categories
        $.ajax({
            url: 'POS.aspx/GetCategories',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                renderCategories(response.d);
            },
            error: function (xhr, status, error) {
                console.error('Error loading categories:', error);
            }
        });
    }

    function loadMenuItems() {
        var data = {
            categoryID: pos.selectedCategory,
            searchTerm: pos.searchTerm
        };

        $.ajax({
            url: 'POS.aspx/GetMenuItems',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (response) {
                renderMenuItems(response.d);
            },
            error: function (xhr, status, error) {
                console.error('Error loading menu items:', error);
            }
        });
    }

    function loadTables() {
        $.ajax({
            url: 'POS.aspx/GetTables',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                renderTables(response.d);
            },
            error: function (xhr, status, error) {
                console.error('Error loading tables:', error);
            }
        });
    }

    function loadCustomers() {
        $.ajax({
            url: 'POS.aspx/GetCustomers',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                renderCustomers(response.d);
            },
            error: function (xhr, status, error) {
                console.error('Error loading customers:', error);
            }
        });
    }

    function loadDeals() {
        $.ajax({
            url: 'POS.aspx/GetDeals',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                renderDeals(response.d);
            },
            error: function (xhr, status, error) {
                console.error('Error loading deals:', error);
            }
        });
    }

    // ============================================
    // Render Functions
    // ============================================
    function renderCategories(categories) {
        var html = '<button class="btn active" data-category="">All</button>';
        $.each(categories, function (index, category) {
            html += '<button class="btn" data-category="' + category.CategoryID + '">' +
                category.CategoryName + '</button>';
        });
        $('.pos-categories').html(html);
    }

    function renderMenuItems(items) {
        var html = '';
        if (items.length === 0) {
            html = '<div class="text-center text-muted p-4">No items found</div>';
        } else {
            $.each(items, function (index, item) {
                var availability = item.IsAvailable ? '' : '<div class="item-unavailable">Unavailable</div>';
                html += `
                    <div class="pos-item-card" data-id="${item.MenuItemID}" onclick="addToCart(${item.MenuItemID})">
                        <div class="item-icon"><i class="fas fa-utensils"></i></div>
                        <div class="item-name">${item.ItemName}</div>
                        <span class="item-category">${item.CategoryName}</span>
                        <div class="item-price">$${item.Price.toFixed(2)}</div>
                        ${availability}
                    </div>
                `;
            });
        }
        $('.pos-items-grid').html(html);
    }

    function renderTables(tables) {
        var html = '<option value="">Select Table</option>';
        $.each(tables, function (index, table) {
            html += '<option value="' + table.TableID + '">' + table.TableNumber +
                ' (' + table.Status + ')</option>';
        });
        $('#ddlTable').html(html);
    }

    function renderCustomers(customers) {
        var html = '<option value="">Walk-in</option>';
        $.each(customers, function (index, customer) {
            html += '<option value="' + customer.CustomerID + '">' +
                customer.CustomerName + '</option>';
        });
        $('#ddlCustomer').html(html);
    }

    function renderDeals(deals) {
        var html = '';
        $.each(deals, function (index, deal) {
            html += `
                <div class="pos-item-card" onclick="addDealToCart(${deal.DealID})">
                    <div class="item-icon"><i class="fas fa-tags" style="color:#28a745;"></i></div>
                    <div class="item-name">${deal.DealName}</div>
                    <span class="item-category">${deal.ItemCount} items</span>
                    <div class="item-price">$${deal.DealPrice.toFixed(2)}</div>
                </div>
            `;
        });
        $('#dealsGrid').html(html);
    }

    // ============================================
    // Cart Functions
    // ============================================
    window.addToCart = function (menuItemID) {
        // Get item details from the clicked card
        var card = $('.pos-item-card[data-id="' + menuItemID + '"]');
        var name = card.find('.item-name').text();
        var price = parseFloat(card.find('.item-price').text().replace('$', ''));

        // Check if item already in cart
        var existing = pos.cart.find(function (item) {
            return item.id === menuItemID && item.type === 'item';
        });

        if (existing) {
            existing.quantity++;
        } else {
            pos.cart.push({
                id: menuItemID,
                type: 'item',
                name: name,
                price: price,
                quantity: 1,
                modifiers: []
            });
        }

        updateCartUI();
        updateTotals();
        showToast(name + ' added to cart', 'success');
    };

    window.addDealToCart = function (dealID) {
        // AJAX call to get deal details
        $.ajax({
            url: 'POS.aspx/GetDealDetails',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ dealID: dealID }),
            success: function (response) {
                var deal = response.d;
                pos.cart.push({
                    id: dealID,
                    type: 'deal',
                    name: deal.DealName,
                    price: deal.DealPrice,
                    quantity: 1,
                    items: deal.Items
                });
                updateCartUI();
                updateTotals();
                showToast(deal.DealName + ' added to cart', 'success');
            },
            error: function (xhr, status, error) {
                showToast('Error adding deal', 'danger');
            }
        });
    };

    function updateQuantity(index, change) {
        var item = pos.cart[index];
        var newQty = item.quantity + change;
        if (newQty <= 0) {
            pos.cart.splice(index, 1);
        } else {
            item.quantity = newQty;
        }
        updateCartUI();
        updateTotals();
    }

    function removeItem(index) {
        pos.cart.splice(index, 1);
        updateCartUI();
        updateTotals();
        showToast('Item removed from cart', 'info');
    }

    function updateCartUI() {
        var html = '';
        if (pos.cart.length === 0) {
            html = '<div class="text-center text-muted p-4">Cart is empty</div>';
        } else {
            $.each(pos.cart, function (index, item) {
                var typeIcon = item.type === 'deal' ? 'fa-tags text-success' : 'fa-utensils text-primary';
                var modifiers = '';
                if (item.modifiers && item.modifiers.length > 0) {
                    modifiers = '<div class="item-details">+' + item.modifiers.join(', ') + '</div>';
                }
                html += `
                    <div class="pos-cart-item">
                        <div class="item-info">
                            <div class="item-name"><i class="fas ${typeIcon} me-1"></i>${item.name}</div>
                            ${modifiers}
                        </div>
                        <div class="item-price">$${(item.price * item.quantity).toFixed(2)}</div>
                        <div class="item-actions">
                            <button class="btn btn-outline-secondary" onclick="updateQuantity(${index}, -1)">-</button>
                            <span class="item-qty">${item.quantity}</span>
                            <button class="btn btn-outline-secondary" onclick="updateQuantity(${index}, 1)">+</button>
                            <button class="btn btn-outline-danger" onclick="removeItem(${index})">
                                <i class="fas fa-times"></i>
                            </button>
                        </div>
                    </div>
                `;
            });
        }
        $('.pos-cart-items').html(html);
    }

    // ============================================
    // Totals Calculation
    // ============================================
    function updateTotals() {
        var subTotal = 0;
        $.each(pos.cart, function (index, item) {
            subTotal += item.price * item.quantity;
        });

        var tax = subTotal * 0.05; // 5% tax
        var discount = parseFloat($('#txtDiscount').val()) || 0;
        var serviceCharge = parseFloat($('#txtServiceCharge').val()) || 0;
        var total = subTotal + tax - discount + serviceCharge;

        $('#lblSubTotal').text('$' + subTotal.toFixed(2));
        $('#lblTax').text('$' + tax.toFixed(2));
        $('#lblDiscount').text('$' + discount.toFixed(2));
        $('#lblServiceCharge').text('$' + serviceCharge.toFixed(2));
        $('#lblTotal').text('$' + total.toFixed(2));

        // Update payment amount
        $('#paymentAmount').text('$' + total.toFixed(2));
    }

    // ============================================
    // Payment Processing
    // ============================================
    function processPayment() {
        if (pos.cart.length === 0) {
            showToast('Cart is empty', 'warning');
            return;
        }

        var total = parseFloat($('#lblTotal').text().replace('$', ''));
        var paidAmount = parseFloat($('#txtPaidAmount').val()) || 0;

        if (paidAmount < total) {
            showToast('Insufficient payment amount', 'danger');
            return;
        }

        // Show payment modal
        $('#paymentModal').modal('show');
    }

    function confirmPayment() {
        var paymentMethod = $('#ddlPaymentMethod').val();
        if (!paymentMethod) {
            showToast('Please select a payment method', 'warning');
            return;
        }

        // Process payment via AJAX
        var data = {
            order: pos,
            paymentMethod: paymentMethod,
            amount: parseFloat($('#lblTotal').text().replace('$', ''))
        };

        $.ajax({
            url: 'POS.aspx/ProcessPayment',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.d.success) {
                    showToast('Payment processed successfully!', 'success');
                    $('#paymentModal').modal('hide');
                    resetPOS();
                } else {
                    showToast(response.d.message, 'danger');
                }
            },
            error: function (xhr, status, error) {
                showToast('Error processing payment', 'danger');
            }
        });
    }

    // ============================================
    // Reset POS
    // ============================================
    function resetPOS() {
        pos.cart = [];
        pos.discount = 0;
        pos.tip = 0;
        updateCartUI();
        updateTotals();
        $('#txtDiscount').val('0');
        $('#txtServiceCharge').val('0');
        $('#txtPaidAmount').val('0');
        $('#txtCustomerSearch').val('');
        $('#ddlCustomer').val('');
        $('#ddlTable').val('');
    }

    // ============================================
    // Toast Notifications
    // ============================================
    function showToast(message, type) {
        var colors = {
            success: 'bg-success',
            danger: 'bg-danger',
            warning: 'bg-warning',
            info: 'bg-info'
        };
        var icons = {
            success: 'fa-check-circle',
            danger: 'fa-exclamation-circle',
            warning: 'fa-exclamation-triangle',
            info: 'fa-info-circle'
        };

        var toastHtml = `
            <div class="toast align-items-center text-white ${colors[type] || 'bg-primary'} border-0" role="alert">
                <div class="d-flex">
                    <div class="toast-body">
                        <i class="fas ${icons[type] || 'fa-info-circle'} me-2"></i>
                        ${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `;

        $('.toast-container').append(toastHtml);
        var toast = $('.toast-container .toast:last');
        var bsToast = new bootstrap.Toast(toast);
        bsToast.show();

        setTimeout(function () {
            toast.remove();
        }, 5000);
    }

    // ============================================
    // Keyboard Shortcuts
    // ============================================
    function setupKeyboardShortcuts() {
        $(document).keydown(function (e) {
            // ESC - Clear search
            if (e.key === 'Escape') {
                $('#txtSearch').val('');
                pos.searchTerm = '';
                loadMenuItems();
            }

            // Enter - Process payment
            if (e.key === 'Enter' && $('#txtSearch').is(':focus')) {
                // Search
                pos.searchTerm = $('#txtSearch').val();
                loadMenuItems();
            }

            // F2 - New Order
            if (e.key === 'F2') {
                resetPOS();
                showToast('New order started', 'info');
            }

            // F3 - Process Payment
            if (e.key === 'F3') {
                processPayment();
            }

            // F4 - Hold Order
            if (e.key === 'F4') {
                holdOrder();
            }
        });
    }

    // ============================================
    // Event Listeners
    // ============================================
    function setupEventListeners() {
        // Category filter
        $(document).on('click', '.pos-categories .btn', function () {
            $('.pos-categories .btn').removeClass('active');
            $(this).addClass('active');
            pos.selectedCategory = $(this).data('category');
            loadMenuItems();
        });

        // Search
        $('#txtSearch').on('input', function () {
            pos.searchTerm = $(this).val();
            if (pos.searchTerm.length >= 2 || pos.searchTerm.length === 0) {
                loadMenuItems();
            }
        });

        // Order type change
        $('#ddlOrderType').on('change', function () {
            pos.orderType = $(this).val();
            if (pos.orderType === 'Dine In') {
                $('#divTable').show();
                $('#divDeliveryAddress').hide();
            } else if (pos.orderType === 'Delivery') {
                $('#divTable').hide();
                $('#divDeliveryAddress').show();
            } else {
                $('#divTable').hide();
                $('#divDeliveryAddress').hide();
            }
        });

        // Discount change
        $('#txtDiscount').on('input', function () {
            updateTotals();
        });

        // Service charge change
        $('#txtServiceCharge').on('input', function () {
            updateTotals();
        });

        // Paid amount change
        $('#txtPaidAmount').on('input', function () {
            var total = parseFloat($('#lblTotal').text().replace('$', ''));
            var paid = parseFloat($(this).val()) || 0;
            var change = paid - total;
            $('#lblChange').text('$' + (change >= 0 ? change.toFixed(2) : '0.00'));
        });

        // Payment method selection
        $(document).on('click', '.pos-payment-methods .btn', function () {
            $('.pos-payment-methods .btn').removeClass('active');
            $(this).addClass('active');
            $('#ddlPaymentMethod').val($(this).data('method'));
        });

        // Customer search
        $('#txtCustomerSearch').on('input', function () {
            var term = $(this).val();
            if (term.length >= 2) {
                // AJAX search customers
                $.ajax({
                    url: 'POS.aspx/SearchCustomers',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({ term: term }),
                    success: function (response) {
                        renderCustomers(response.d);
                    }
                });
            }
        });
    }

    // ============================================
    // Initialize on load
    // ============================================
    initPOS();
});