using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public class BasePagination
    {
        private int _page;
        private int _limit;

        public BasePagination()
        {
        }

        public BasePagination(int page, int limit) : this()
        {
            Page = page;
            Limit = limit;
        }

        public int Page
        {
            set => _page = value;
            get => _page < 1 ? (_page = 1) : _page;
        }

        /// <summary>
        /// Size 10~100. Formula (10 <= x <= 100)
        /// </summary>
        public int Limit
        {
            set => _limit = value;
            get => _limit < 1 ? (_limit = 1) : _limit > 100 ? (_limit = 100) : _limit;
        }

        public int CalculateOffset()
        {
            return (Page - 1) == 0 || (Page - 1) < 0 ? 0 : (Page - 1) * Limit;
        }
    }
}
