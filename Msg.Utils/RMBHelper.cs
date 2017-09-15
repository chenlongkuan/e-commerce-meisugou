

namespace Msg.Utils
{
    /// <summary> 
    /// Rmb ��ժҪ˵���� 
    /// </summary> 
    public class Rmb
    {
        /// <summary>
        /// �ۿ�
        /// </summary>
        /// <param name="marketprice">�г���</param>
        /// <param name="mallprice">�̳Ǽ�</param>
        /// <returns></returns>
        public static string GetDiscount(decimal marketprice, decimal mallprice)
        {
            if ((marketprice <= 0M) || (mallprice <= 0M))
            {
                return "0��";
            }
            decimal num = (mallprice / marketprice) * 10M;
            return (num.ToString("F1") + "��");
        }
        /// <summary>
        /// �ۿ�
        /// </summary>
        /// <param name="marketprice">�г���</param>
        /// <param name="mallprice">�̳Ǽ�</param>
        /// <returns></returns>
        public static float GetDiscountReturnFloat(decimal marketprice, decimal mallprice)
        {
            if ((marketprice <= 0M) || (mallprice <= 0M))
            {
                return 0;
            }
            decimal num = (mallprice / marketprice) * 10M;
            return float.Parse(num.ToString("F1"));
        }

     
    }

}
