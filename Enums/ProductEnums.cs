
namespace e_commerce.Enums
{
    public class ProductEnums
    {
        public static explicit operator int(ProductEnums v)
        {
            throw new NotImplementedException();
        }

        public enum ProductStatus
        {
            InStock=1,
            OutOfStock=2,
            PreOrder=3,
            UpComming=4
        }
    }
}
