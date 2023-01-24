namespace Sample.WebAppPipes.Session
{
    public class SessionData : ISessionData
    {
        public Guid? Id { get; private set; }

        public void SetNewId()
        {
            Id = Guid.NewGuid();
        }

        public void SetId(Guid? id)
        {
            Id = id;
        }

        public SessionData()
        {
            Id = null;
        }

        public override string ToString()
        {
            return $"SessionData Guid: {Id}";
        }

    }
}
