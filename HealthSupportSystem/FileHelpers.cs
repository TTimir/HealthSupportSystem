using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HealthSupportSystem
{
    public class FileHelpers
    {
        // Allowed file extensions for photos and documents
        private static readonly List<string> AllowedPhotoExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        private static readonly List<string> AllowedDocumentExtensions = new List<string> { ".pdf", ".png", ".jpg", ".jpeg" };

        // Maximum file size limits (in bytes)
        private const int MaxPhotoSize = 2 * 1024 * 1024; // 2 MB
        private const int MaxDocumentSize = 5 * 1024 * 1024; // 5 MB

        public static bool UploadPhoto(HttpPostedFileBase file, string folder, string name)
        {
            if (file == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(folder))
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
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool UploadDocument(HttpPostedFileBase file, string docfolder, string name)
        {
            if (file == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(docfolder))
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
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

    }
}