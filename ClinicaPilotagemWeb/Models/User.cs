namespace ClinicaPilotagemWeb.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool WaitingConfirmation { get; set; }
        public bool Blocked { get; set; }
        public int Aplication { get; set; }
    }
}