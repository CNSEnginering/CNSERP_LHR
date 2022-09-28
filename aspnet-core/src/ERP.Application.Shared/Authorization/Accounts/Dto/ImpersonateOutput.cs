namespace ERP.Authorization.Accounts.Dto
{
    public class ImpersonateOutput
    {
        public string ImpersonationToken { get; set; }

        public string TenancyName { get; set; }
    }
}