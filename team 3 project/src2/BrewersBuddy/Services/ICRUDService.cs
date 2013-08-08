
namespace BrewersBuddy.Services
{
    public interface ICRUDService<T>
    {
        void Create(T @object);
        void Delete(T @object);
        T Get(int id);
        void Update(T @object);
    }
}
