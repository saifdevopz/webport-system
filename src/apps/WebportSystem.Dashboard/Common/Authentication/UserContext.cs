namespace WebportSystem.Dashboard.Common.Authentication;

public class UserContext
{
    // 🔐 Initialization guard
    public bool IsInitialized { get; set; } = false;

    // 👤 Identity (from claims)
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // 🏢 Business scope (VERY IMPORTANT for your system)
    public Guid BusinessProfileId { get; set; }
    public int BranchId { get; set; }

    // ⚙️ User settings
    public string Theme { get; set; } = "Light";

    // 🔧 Helper (optional but useful)
    public bool IsAuthenticated => !string.IsNullOrEmpty(UserId);
}