using System.Reflection;

namespace WebportSystem.Inventory.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}