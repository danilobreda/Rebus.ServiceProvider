namespace Sample.WebAppPipes.Session
{
    public interface ISessionData
    {
        public Guid? Id { get; }

        public void SetNewId();
        public void SetId(Guid? id);
    }
}
