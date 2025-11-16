using System.Reflection;

namespace WebportSystem.Identity.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}