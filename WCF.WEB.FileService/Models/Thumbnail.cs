using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WCF.WEB.FileService.Models
{
    /// <summary>
    /// Thumbnail ��ժҪ˵����
    /// </summary>
    public class Thumbnail
    {
        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="image">Image ����</param>
        /// <param name="savePath">����·��</param>
        /// <param name="ici">ָ����ʽ�ı�������</param>
        private static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            //���� ԭͼƬ ����� EncoderParameters ����
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long)100));
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

        public static byte[] MakeThumbnailImage(string filename, int maxWidth, int maxHeight)
        {
            using (Image original = Image.FromFile(filename))
            {
                Size _newSize = ResizeImage(original.Width, original.Height, maxWidth, maxHeight);
                Image displayImage = new Bitmap(original, _newSize);
                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    displayImage.Save(ms, GetFormat(filename));
                    byte[] buff = ms.ToArray();
                    ms.Dispose();
                    return buff;
                }
                catch (Exception exp)
                {
                    throw (exp);
                }
                finally
                {
                    displayImage.Dispose();
                    original.Dispose();
                }
            }
        }
        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="originalImagePath">Դͼ·��������·���� </param>
        /// <param name="thumbnailPath">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>    
        public static byte[] MakeSquareImage(string filename, int newWidth, int newHeight)
        {
            using (Image image = Image.FromFile(filename))
            {
                int width = image.Width;
                int height = image.Height;

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

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    b.Save(ms, GetFormat(filename));
                    byte[] buff = ms.ToArray();
                    return buff;
                }
                finally
                {
                    image.Dispose();
                    b.Dispose();
                }
            }
        }
    }
}
