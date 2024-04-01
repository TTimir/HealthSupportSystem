using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace HealthSupportSystem
{
    public class FileHelpers
    {
        public static bool UploadPhoto(HttpPostedFileBase file, string folder, string name)
        {
            if (file == null || string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(folder))
            {
                return false;
            }

            try
            {
                string path = string.Empty;

                if (file != null)
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath(folder), name);

                    WebImage img = new WebImage(file.InputStream);
                    if (img.Width > 300)
                        img.Resize(300, 300);
                    img.Save(path);
                }

                return true;
            }
            catch 
            {
                return false;
            }
        }

        public static bool UploadDocument(HttpPostedFileBase file, string docfolder, string name)
        {
            if (file == null || string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(docfolder))
            {
                return false;
            }

            try
            {
                string path = string.Empty;

                if (file != null)
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath(docfolder), name);
                    file.SaveAs(path); // Save the file directly without processing

                }

                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}