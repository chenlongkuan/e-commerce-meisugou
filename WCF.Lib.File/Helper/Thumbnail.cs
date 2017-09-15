using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace WCF.Lib.File.Helper
{
    /// </summary>    
    /// �޸ļ�¼1: SaveImage ����
    /// �޸�����: 2012-3-22
    /// �� �� ��: 
    /// �� �� ��:������
    /// �޸�����:ͼƬ����������100�޸�Ϊ80
    /// </summary>        

    /// <summary>
    /// Thumbnail ��ժҪ˵����
    /// </summary>
    public class Thumbnail
    {
        #region Helper
        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="image">Image ����</param>
        /// <param name="savePath">����·��</param>
        /// <param name="ici">ָ����ʽ�ı�������</param>
        public static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            //���� ԭͼƬ ����� EncoderParameters ����
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long)80));
            image.Save(savePath, ici, parameters);
            parameters.Dispose();
        }

        /// <summary>
        /// ��ȡͼ���������������������Ϣ
        /// </summary>
        /// <param name="mimeType">��������������Ķ���;�����ʼ�����Э�� (MIME) ���͵��ַ���</param>
        /// <returns>����ͼ���������������������Ϣ</returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /// <summary>
        /// �����³ߴ�
        /// </summary>
        /// <param name="width">ԭʼ���</param>
        /// <param name="height">ԭʼ�߶�</param>
        /// <param name="maxWidth">����¿��</param>
        /// <param name="maxHeight">����¸߶�</param>
        /// <returns></returns>
        private static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
        {
            decimal MAX_WIDTH = (decimal)maxWidth;
            decimal MAX_HEIGHT = (decimal)maxHeight;
            decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;

            int newWidth, newHeight;

            decimal originalWidth = (decimal)width;
            decimal originalHeight = (decimal)height;

            if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT)
            {
                decimal factor;
                // determine the largest factor 
                if (originalWidth / originalHeight > ASPECT_RATIO)
                {
                    factor = originalWidth / MAX_WIDTH;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
                else
                {
                    factor = originalHeight / MAX_HEIGHT;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }
            return new Size(newWidth, newHeight);
        }

        /// <summary>
        /// �õ�ͼƬ��ʽ
        /// </summary>
        /// <param name="name">�ļ�����</param>
        /// <returns></returns>
        private static ImageFormat GetFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }
        #endregion
        /// <summary>
        /// ����С������
        /// </summary>
        /// <param name="fileName">ԭͼ���ļ�·��</param>
        /// <param name="newFileName">�µ�ַ</param>
        /// <param name="newSize">���Ȼ���</param>
        public static void MakeSquareImage(string fileName, string newFileName, int newWidth, int newHeight)
        {
            Image image = Image.FromFile(fileName);

            //int i = 0;
            int width = image.Width;
            int height = image.Height;
            //if (width > height)
            //{
            //    i = height;
            //}
            //else
            //{
            //    i = width;
            //}
            Bitmap b = new Bitmap(newWidth, newHeight);

            try
            {
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;

                //���������ͼ�沢��͸������ɫ���
                g.Clear(Color.Transparent);
                if (width < height)
                {
                    g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, (height - width) / 2, width, width), GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), new Rectangle((width - height) / 2, 0, height, height), GraphicsUnit.Pixel);
                }

                SaveImage(b, newFileName, GetCodecInfo("image/" + GetFormat(fileName).ToString().ToLower()));
            }
            finally
            {
                image.Dispose();
                b.Dispose();
            }
        }
        /// <summary>
        /// ����ͼƬ��ָ����С����ͼƬ�������
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="newFileName"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        public static void MakeSquareImage(Image image, string newFileName, int newWidth, int newHeight)
        {
            int i = 0;
            int width = image.Width;
            int height = image.Height;
            if (width > height)
            {
                i = height;
            }
            else
            {
                i = width;
            }
            Bitmap b = new Bitmap(newWidth, newHeight);

            try
            {
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;

                //���������ͼ�沢��͸������ɫ���
                g.Clear(Color.Transparent);
                if (width < height)
                {
                    g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, (height - width) / 2, width, width), GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), new Rectangle((width - height) / 2, 0, height, height), GraphicsUnit.Pixel);
                }

                SaveImage(b, newFileName, GetCodecInfo("image/" + GetFormat(newFileName).ToString().ToLower()));
            }
            finally
            {
                image.Dispose();
                b.Dispose();
            }
        }


        /// <summary>
        /// ָ������ü�
        /// ��ģ��������Χ�Ĳü�ͼƬ��������ģ��ߴ�
        /// </summary>
        /// <param name="initImage">ԭͼ����</param>
        /// <param name="fileSaveUrl">����·��</param>
        /// <param name="maxWidth">����(��λ:px)</param>
        /// <param name="maxHeight">����(��λ:px)</param>
        /// <param name="quality">��������Χ0-100��</param>
        public static void CutForCustom(Image initImage, string fileSaveUrl, int maxWidth, int maxHeight, int quality)
        {

            //ԭͼ��߾�С��ģ�棬��������ֱ�ӱ���
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //ģ��Ŀ�߱���
                double templateRate = (double)maxWidth / maxHeight;
                //ԭͼƬ�Ŀ�߱���
                double initRate = (double)initImage.Width / initImage.Height;

                //ԭͼ��ģ�������ȣ�ֱ������
                if (templateRate == initRate)
                {
                    //��ģ���С��������ͼƬ
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.Transparent);
                    templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //ԭͼ��ģ��������ȣ��ü�������
                else
                {
                    //�ü�����
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //��λ
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//ԭͼ�ü���λ
                    Rectangle toR = new Rectangle(0, 0, 0, 0);//Ŀ�궨λ

                    //��Ϊ��׼���вü�
                    if (templateRate > initRate)
                    {
                        //�ü�����ʵ����
                        pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)Math.Floor(initImage.Width / templateRate));
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        //�ü�Դ��λ
                        fromR.X = 0;
                        fromR.Y = (int)Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int)Math.Floor(initImage.Width / templateRate);

                        //�ü�Ŀ�궨λ
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int)Math.Floor(initImage.Width / templateRate);
                    }
                    //��Ϊ��׼���вü�
                    else
                    {
                        pickedImage = new System.Drawing.Bitmap((int)Math.Floor(initImage.Height * templateRate), initImage.Height);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        fromR.X = (int)Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)Math.Floor(initImage.Height * templateRate);
                        fromR.Height = initImage.Height;

                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)Math.Floor(initImage.Height * templateRate);
                        toR.Height = initImage.Height;
                    }

                    //��������
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //�ü�
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                    //��ģ���С��������ͼƬ
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.Transparent);
                    templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //�ؼ���������
                    //��ȡϵͳ������������,������jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = GetCodecInfo("image/jpeg");
                  
                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                    //��������ͼ
                    templateImage.Save(fileSaveUrl, ici, ep);

                    //�ͷ���Դ
                    templateG.Dispose();
                    templateImage.Dispose();

                    pickedG.Dispose();
                    pickedImage.Dispose();
                }
            }

            //�ͷ���Դ
            initImage.Dispose();
        }


        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="fileName">ԭͼ·��</param>
        /// <param name="newFileName">��ͼ·��</param>
        /// <param name="maxWidth">�����</param>
        /// <param name="maxHeight">���߶�</param>
        public static void MakeThumbnailImage(byte[] buff, string newFileName, int maxWidth, int maxHeight)
        {
            using (MemoryStream memory = new MemoryStream(buff))
            {
                Image original = Image.FromStream(memory);
                Size _newSize = ResizeImage(original.Width, original.Height, maxWidth, maxHeight);
                Image displayImage = new Bitmap(original, _newSize);
                try
                {
                    displayImage.Save(newFileName, GetFormat(newFileName));
                }
                catch (Exception exp)
                {
                    throw (exp);
                }
                finally
                {
                    original.Dispose();
                    displayImage.Dispose();
                }
            }
        }

        public static void SavePhoto(byte[] buff, string newFileName, int maxWidth, int maxHeight)
        {
            using (MemoryStream memory = new MemoryStream(buff))
            {
                Image original = Image.FromStream(memory);
                try
                {
                    SaveImage(original, newFileName, GetCodecInfo("image/" + GetFormat(newFileName).ToString().ToLower()));
                }
                catch (Exception exp)
                {
                    throw (exp);
                }
                finally
                {
                    original.Dispose();
                }
            }
        }

        public static void MakeThumbnailImage(string filename, string newFileName, int maxWidth, int maxHeight)
        {
            using (Image original = Image.FromFile(filename))
            {
                Size _newSize = ResizeImage(original.Width, original.Height, maxWidth, maxHeight);
                Image displayImage = new Bitmap(original, _newSize);
                try
                {
                    displayImage.Save(newFileName, GetFormat(newFileName));
                }
                catch (Exception exp)
                {
                    throw (exp);
                }
                finally
                {
                    original.Dispose();
                    displayImage.Dispose();
                }
            }
        }

    }
}
