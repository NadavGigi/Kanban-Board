namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct SUser
    {
        public readonly string Email;

        internal SUser(string email)
        {
            email = email.ToLower();
            this.Email = email;
        }
    }
}
