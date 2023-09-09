using Microsoft.AspNetCore.Authorization;

namespace ColegioAPI.Helpers
{
    public class GlobalVerbRoleRequirement: IAuthorizationRequirement
{
    public bool IsAllowed(string role, string verb)
    {
        // allow all verbs if user is "admin"
        if(string.Equals("Rector", role, StringComparison.OrdinalIgnoreCase)) return true;
        if(string.Equals("Coordinador", role, StringComparison.OrdinalIgnoreCase)) return true;
        // allow the "GET" verb if user is "support"
        if(string.Equals("DirectorDeGrupo", role, StringComparison.OrdinalIgnoreCase) && string.Equals("GET",verb, StringComparison.OrdinalIgnoreCase)){
            return true;
        };
        if(string.Equals("Estudiante", role, StringComparison.OrdinalIgnoreCase) && string.Equals("GET",verb, StringComparison.OrdinalIgnoreCase)){
            return true;
        };
        // ... add other rules as you like
        return false;
    }
}
}
