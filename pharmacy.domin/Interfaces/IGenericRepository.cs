namespace pharmacy.domin.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Get
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        // Add / Update / Delete
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
