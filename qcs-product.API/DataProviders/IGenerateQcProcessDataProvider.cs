using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IGenerateQcProcessDataProvider
    {
        Task Generate(QcTransactionGroup data);
    }
}