using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TransactionTestingSamplingBusinessProvider : ITransactionTestingSamplingBusinessProvider
    {
        private readonly ITransactionTestingSamplingDataProvider _dataProvider;

        public TransactionTestingSamplingBusinessProvider(ITransactionTestingSamplingDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<TransactionTestingSampling> GetById(int id)
        {
            return await _dataProvider.GetById(id);
        }


        public async Task<ResponseViewModel<TransactionTestingSampling>> GetByTestingIdAsync(int TestingId)
        {
            ResponseViewModel<TransactionTestingSampling> result = new ResponseViewModel<TransactionTestingSampling>();
            var  getData  = await _dataProvider.GetByTestingId(TestingId);
            if (getData == null || !getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = getData;
            return result;
        }

        public async Task<List<TransactionTestingSampling>> GetAll()
        {
            return await _dataProvider.GetAll();
        }

        public async Task Create(TransactionTestingSampling entity)
        {
            // Perform any business logic or validation before calling the data provider
            await _dataProvider.Create(entity);
        }

        public async Task Update(TransactionTestingSampling entity)
        {
            // Perform any business logic or validation before calling the data provider
            await _dataProvider.Update(entity);
        }

        public async Task Delete(int id)
        {
            // Perform any business logic or validation before calling the data provider
            await _dataProvider.Delete(id);
        }

    }

}
