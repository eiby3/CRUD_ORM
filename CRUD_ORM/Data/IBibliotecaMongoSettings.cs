namespace CRUD_ORM.Data
{
    public interface IBibliotecaMongoSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string Database { get; set; }
    }
}
