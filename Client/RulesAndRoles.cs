namespace YGate.Client
{
    public class RuleAndRoles {
        public RuleAndRoles(int id,string ruleName, string roles)
        {
            RuleName = ruleName;
            Id = id;
            Roles = roles;
        }
        public int Id { get; set; }
        public string RuleName { get; set; }
        public string Roles { get; set; }
    }
    public static class RulesAndRoles
    {
        public static List<RuleAndRoles> Rules { get; set; } 
    }
}
