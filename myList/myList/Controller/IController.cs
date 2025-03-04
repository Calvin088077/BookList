
public interface IController<T> where T : class 
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T item);
    Task UpdateAsync(int id, T item);
    Task DeleteAsync(int id);

}
