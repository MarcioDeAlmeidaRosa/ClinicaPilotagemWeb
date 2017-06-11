namespace ClinicaPilotagemWeb.Models.Responses.Authentication
{
    public class ResultAutentication
    {
        public string Token { get; set; }
        public StatusLogin StatusLogin { get; set; }
        public User User { get; set; }
    }
}