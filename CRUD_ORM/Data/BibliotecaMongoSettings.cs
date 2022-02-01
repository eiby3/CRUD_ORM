namespace CRUD_ORM.Data
{
    public class BibliotecaMongoSettings : IBibliotecaMongoSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
