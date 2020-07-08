using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;

namespace Common
{
    /// <summary>
    /// 图片上传类
    /// </summary>
    public class ImgUpLoad
    {

        //图片上传
        public static string UpLoad(HttpRequestBase request)
        {
            //判断有没有图片上传
            if (request.Files.Count > 0)
            {
                //将图片流文件保存在 字节序列容器 中
                Stream stream = request.Files[0].InputStream;
                string Extension = System.IO.Path.GetExtension(request.Files[0].FileName);
                //将图片流文件转换为Image图片对象
                Image img = Image.FromStream(stream);

                //将图片保存在服务器上
                //为了防止图片名称重复 我们使用随机数命名
                Random ran = new Random((int)DateTime.Now.Ticks);
                //图片保存的目录  按照日期进行保存
                string subPath = "/imgUploads/" + DateTime.Now.ToString("yyyyMMdd") + "/"; // 20190928
                                                                                           //图片保存的目录的绝对路径
                string path = request.MapPath(subPath);
                //当文件夹路径不存在时 创建文件夹
                if (false == System.IO.Directory.Exists(path))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(path);
                }
                string imgName = ran.Next(99999) + Extension;
                string serverPath = path + imgName;//文件保存位置及命名
                string imgPath = subPath + imgName;
                try
                {
                    img.Save(serverPath);
                    return imgPath;
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 上传指定位置的图片
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static string UpLoadNum(HttpRequestBase request,int Num)
        {
            //判断有没有图片上传
            if (request.Files.Count >= Num)
            {
                //将图片流文件保存在 字节序列容器 中
                Stream stream = request.Files[Num].InputStream;
                string Extension = System.IO.Path.GetExtension(request.Files[Num].FileName);
                //将图片流文件转换为Image图片对象
                Image img = Image.FromStream(stream);

                //将图片保存在服务器上
                //为了防止图片名称重复 我们使用随机数命名
                Random ran = new Random((int)DateTime.Now.Ticks);
                //图片保存的目录  按照日期进行保存
                string subPath = "/imgUploads/" + DateTime.Now.ToString("yyyyMMdd") + "/"; // 20190928
                                                                                           //图片保存的目录的绝对路径
                string path = request.MapPath(subPath);
                //当文件夹路径不存在时 创建文件夹
                if (false == System.IO.Directory.Exists(path))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(path);
                }
                string imgName = ran.Next(99999) + Extension;
                string serverPath = path + imgName;//文件保存位置及命名
                string imgPath = subPath + imgName;
                try
                {
                    img.Save(serverPath);
                    return imgPath;
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

    }
}
