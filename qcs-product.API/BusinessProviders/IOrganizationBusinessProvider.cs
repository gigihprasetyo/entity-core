using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.BusinessProviders
{
    public interface IOrganizationBusinessProvider
    {
        public Task<ResponseViewModel<OrganizationViewModel>> List();
    }
}