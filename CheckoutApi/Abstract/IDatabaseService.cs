using System.Data.Common;

namespace CheckoutApi.Abstract
{
    public interface IDatabaseService
    {
        DbConnection GetDbConnection();
    }
}