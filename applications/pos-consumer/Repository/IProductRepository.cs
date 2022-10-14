using pos_consumer.Domain;

namespace pos_consumer.Repository
{

    public interface IProductRepository
    {
         void Save(Product product);
    }

}