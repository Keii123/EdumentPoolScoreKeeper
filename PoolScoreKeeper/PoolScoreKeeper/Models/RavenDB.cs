using Raven.Client;
using Raven.Client.Document;

namespace PoolScoreKeeper.Models
{
    public class RavenDb
    {
        public static IDocumentStore GetStore()
        {
            IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/",
                DefaultDatabase = "PoolScoreDB"
            }.Initialize();

            return store;
        }
    }
}