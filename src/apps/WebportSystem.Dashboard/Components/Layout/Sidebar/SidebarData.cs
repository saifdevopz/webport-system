namespace WebportSystem.Dashboard.Components.Layout.Sidebar;

internal static class SidebarData
{
    internal sealed class MenuGroup
    {
        public string Title { get; set; } = string.Empty;
        public List<MenuItem> Items { get; set; } = [];
    }

    // Menu Groups
    public static List<MenuGroup> GetAdminMenuGroups() =>
    [
        new MenuGroup
        {
            Title = "Identity",
            Items = GetIdentityItems()
        },
        new MenuGroup
        {
            Title = "Navigation",
            Items = GetStandardMenuItems()
        },
        new MenuGroup
        {
            Title = "Development",
            Items = GetGeneralMenuItems()
        },
    ];

    public static List<MenuGroup> GetTenantMenuGroups() =>
    [
        new MenuGroup
        {
            Title = "General",
            Items = GetInventoryItems()
        },
        new MenuGroup
        {
            Title = "General",
            Items = GetGeneralMenuItems()
        },
    ];

    // Item Lists

    public static List<MenuItem> GetInventoryItems() =>
    [
        new MenuItem(title: "Inventory", icon: "ri-archive-2-fill", suffix: new("Hot", "primary"), childMenuItems:
        [
            new MenuItem(href:"/category/index", title:"Categories"),
            new MenuItem(href:"/item/index", title:"Items"),
        ]),
    ];

    public static List<MenuItem> GetIdentityItems() =>
    [
        new MenuItem(title: "Identity", icon: "ri-archive-2-fill", suffix: new("Hot", "primary"), childMenuItems:
        [
            new MenuItem(href:"/role/index", title:"Roles"),
        ]),
    ];

    public static List<MenuItem> GetStandardMenuItems() =>
    [
        new MenuItem(href:"/home", title: "Home", icon:"ri-add-box-fill"),
        new MenuItem(href:"/counter", title: "Counter", icon:"ri-add-box-fill"),
        new MenuItem(href:"/test/virtualization", title: "Virtualization", icon:"ri-bar-chart-horizontal-line"),
    ];

    public static List<MenuItem> GetGeneralMenuItems() =>
    [
    // ================== DASHBOARD ==================
        new MenuItem(href:"/home", title: "Dashboard", icon:"ri-dashboard-fill"),

        new(title:"Components", icon:"ri-vip-diamond-fill", suffix: new("Hot", "primary"), childMenuItems:
            [
                new MenuItem(href:"#", title:"Grid"),
                new MenuItem(title:"Layout", childMenuItems:
                [
                    new MenuItem(title:"Forms", childMenuItems:
                    [
                        new MenuItem(href:"#", title:"Input"),
                        new MenuItem(href:"#", title:"Select"),
                        new MenuItem(title:"More", childMenuItems:
                        [
                            new MenuItem(href:"#", title:"CheckBox"),
                            new MenuItem(href:"#", title:"Radio"),
                            new MenuItem(title:"Want more", childMenuItems:
                            [
                                new MenuItem(href:"#", title:"You made it"),
                            ]),
                        ]),
                    ]),
                ]),
            ]),

        // ================== INVENTORY ==================
        new MenuItem(title:"Inventory", icon:"ri-archive-2-fill", suffix:new("Hot","primary"), childMenuItems:
        [
            new MenuItem(title:"Stock", childMenuItems:
            [
                new MenuItem(href:"#", title:"Stock Control"),
                new MenuItem(href:"#", title:"Bin Locations"),
                new MenuItem(href:"#", title:"Stock Adjustments"),
                new MenuItem(href:"#", title:"Stock Valuation"),
                new MenuItem(href:"#", title:"Minimum Levels"),
                new MenuItem(href:"#", title:"Stock Forecasting"),
                new MenuItem(href:"#", title:"Reserved Stock"),
                new MenuItem(href:"#", title:"Damaged Stock"),
                new MenuItem(href:"#", title:"Consigned Stock"),
            ]),
            new MenuItem(title:"Items", childMenuItems:
            [
                new MenuItem(href:"#", title:"Item List"),
                new MenuItem(href:"#", title:"Item Categories"),
                new MenuItem(href:"#", title:"Units of Measure"),
                new MenuItem(href:"#", title:"Suppliers"),
                new MenuItem(href:"#", title:"Item Pricing"),
                new MenuItem(href:"#", title:"Item Attributes"),
                new MenuItem(href:"#", title:"Item Kits/Bundles"),
                new MenuItem(href:"#", title:"Discontinued Items"),
            ]),
            new MenuItem(title:"Warehouses", childMenuItems:
            [
                new MenuItem(href:"#", title:"Warehouse List"),
                new MenuItem(href:"#", title:"Transfers"),
                new MenuItem(href:"#", title:"Stock Counts"),
                new MenuItem(href:"#", title:"Warehouse Zones"),
                new MenuItem(href:"#", title:"Cold Storage"),

                new MenuItem(href:"#", title:"Third-Party Warehouses"),
            ]),
        ]),

        // ================== PURCHASING ==================
        new MenuItem(title:"Purchasing", icon:"ri-shopping-cart-2-fill", childMenuItems:
        [
            new MenuItem(title:"Purchase Orders", childMenuItems:
            [
                new MenuItem(href:"#", title:"New PO"),
                new MenuItem(href:"#", title:"PO List"),
                new MenuItem(href:"#", title:"Goods Receipts"),
                new MenuItem(href:"#", title:"Pending Approvals"),
                new MenuItem(href:"#", title:"Cancelled POs"),
                new MenuItem(href:"#", title:"Supplier Backorders"),
            ]),
            new MenuItem(title:"Suppliers", childMenuItems:
            [
                new MenuItem(href:"#", title:"Supplier List"),
                new MenuItem(href:"#", title:"Supplier Groups"),
                new MenuItem(href:"#", title:"Supplier Performance"),
                new MenuItem(href:"#", title:"Preferred Suppliers"),
                new MenuItem(href:"#", title:"Blacklisted Suppliers"),
            ]),
        ]),

        // ================== FINANCE ==================
        new MenuItem(title:"Finance", icon:"ri-bank-card-2-fill", childMenuItems:
        [
            new MenuItem(title:"Accounts Receivable", childMenuItems:
            [
                new MenuItem(href:"#", title:"Invoices"),
                new MenuItem(href:"#", title:"Customer Payments"),
                new MenuItem(href:"#", title:"Credit Notes"),
                new MenuItem(href:"#", title:"Payment Plans"),
                new MenuItem(href:"#", title:"Outstanding Balances"),
                new MenuItem(href:"#", title:"Bad Debts"),
            ]),
            new MenuItem(title:"Accounts Payable", childMenuItems:
            [
                new MenuItem(href:"#", title:"Supplier Invoices"),
                new MenuItem(href:"#", title:"Supplier Payments"),
                new MenuItem(href:"#", title:"Debit Notes"),
                new MenuItem(href:"#", title:"Payment Schedules"),
                new MenuItem(href:"#", title:"Outstanding Bills"),
                new MenuItem(href:"#", title:"Aging Analysis"),
            ]),
            new MenuItem(title:"General Ledger", childMenuItems:
            [
                new MenuItem(href:"#", title:"Chart of Accounts"),
                new MenuItem(href:"#", title:"Journal Entries"),
                new MenuItem(href:"#", title:"Trial Balance"),
                new MenuItem(href:"#", title:"Balance Sheet"),
                new MenuItem(href:"#", title:"Income Statement"),
                new MenuItem(href:"#", title:"Cash Flow Statement"),
            ]),
        ]),

        // ================== HR ==================
        new MenuItem(title:"Human Resources", icon:"ri-group-fill", childMenuItems:
        [
            new MenuItem(title:"Employees", childMenuItems:
            [
                new MenuItem(href:"#", title:"Employee Directory"),
                new MenuItem(href:"#", title:"Departments"),
                new MenuItem(href:"#", title:"Job Roles"),
                new MenuItem(href:"#", title:"Contracts"),
                new MenuItem(href:"#", title:"Probation"),
                new MenuItem(href:"#", title:"Exit Interviews"),
            ]),
            new MenuItem(title:"Payroll", childMenuItems:
            [
                new MenuItem(href:"#", title:"Salary Slips"),
                new MenuItem(href:"#", title:"Deductions"),
                new MenuItem(href:"#", title:"Allowances"),
                new MenuItem(href:"#", title:"Overtime"),
                new MenuItem(href:"#", title:"Bonuses"),
                new MenuItem(href:"#", title:"Tax Reports"),
            ]),
            new MenuItem(title:"Attendance", childMenuItems:
            [
                new MenuItem(href:"#", title:"Timesheets"),
                new MenuItem(href:"#", title:"Leave Requests"),
                new MenuItem(href:"#", title:"Shift Management"),
                new MenuItem(href:"#", title:"Remote Work Logs"),
                new MenuItem(href:"#", title:"Absence Reports"),
            ]),
        ]),

        // ================== REPORTS ==================
        new MenuItem(title:"Reports", icon:"ri-bar-chart-box-fill", childMenuItems:
        [
            new MenuItem(href:"#", title:"Sales Reports"),
            new MenuItem(href:"#", title:"Inventory Reports"),
            new MenuItem(href:"#", title:"Financial Reports"),
            new MenuItem(href:"#", title:"HR Reports"),
            new MenuItem(href:"#", title:"Audit Reports"),
            new MenuItem(href:"#", title:"System Logs"),
            new MenuItem(href:"#", title:"KPI Dashboards"),
        ]),

        // ================== SETTINGS ==================
        new MenuItem(title:"Settings", icon:"ri-settings-3-fill", childMenuItems:
        [
            new MenuItem(href:"#", title:"User Management"),
            new MenuItem(href:"#", title:"Roles & Permissions"),
            new MenuItem(href:"#", title:"System Preferences"),
            new MenuItem(href:"#", title:"Email Templates"),
            new MenuItem(href:"#", title:"Integration Settings"),
            new MenuItem(href:"#", title:"Audit Logs"),
        ]),

        // ================== SALES ==================
        new MenuItem(title:"Sales", icon:"ri-shopping-bag-3-fill", childMenuItems:
        [
            new MenuItem(title:"Orders", childMenuItems:
            [
                new MenuItem(href:"#", title:"New Order"),
                new MenuItem(href:"#", title:"Order List"),
                new MenuItem(href:"#", title:"Invoices"),
                new MenuItem(href:"#", title:"Returns"),
                new MenuItem(href:"#", title:"Dispatch Notes"),
                new MenuItem(href:"#", title:"Cancelled Orders"),
                new MenuItem(href:"#", title:"Archived Orders"),
                new MenuItem(href:"#", title:"Backorders"),
                new MenuItem(href:"#", title:"Pre-Orders"),
            ]),
            new MenuItem(title:"Customers", childMenuItems:
            [
                new MenuItem(href:"#", title:"Customer List"),
                new MenuItem(href:"#", title:"Customer Groups"),
                new MenuItem(href:"#", title:"Loyalty Programs"),
                new MenuItem(href:"#", title:"Customer Feedback"),
                new MenuItem(href:"#", title:"Credit Ratings"),
                new MenuItem(href:"#", title:"Blacklist"),
            ]),
            new MenuItem(title:"Quotations", childMenuItems:
            [
                new MenuItem(href:"#", title:"New Quotation"),
                new MenuItem(href:"#", title:"Quotation List"),
                new MenuItem(href:"#", title:"Approved Quotations"),
                new MenuItem(href:"#", title:"Rejected Quotations"),
                new MenuItem(href:"#", title:"Expired Quotations"),
            ]),
        ]),
    ];
}
