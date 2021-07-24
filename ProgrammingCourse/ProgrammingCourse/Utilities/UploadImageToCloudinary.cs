using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Utilities
{
    public static class UploadImageToCloudinary
    {

        public static string Upload(IFormFile formFile)
        {
            var myAccount = new Account { ApiKey = "518295469152156", ApiSecret = "j5pSjFUnUM3hZgLfRDBy-5Nqwbo", Cloud = "i-h-c-khoa-h-c-t-nhi-n-hqg-hcm" };
            Cloudinary _cloudinary = new Cloudinary(myAccount);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(formFile.FileName, formFile.OpenReadStream()),
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            return uploadResult.Url.AbsoluteUri;
        }
    }
}
