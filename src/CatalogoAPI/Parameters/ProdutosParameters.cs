namespace CatalogoAPI.Parameters
{
    public class ProdutosParameters
    {
        const int MAX_ITEM_PAGE = 30;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > MAX_ITEM_PAGE) ? MAX_ITEM_PAGE : value;
            }
        }
    }
}
