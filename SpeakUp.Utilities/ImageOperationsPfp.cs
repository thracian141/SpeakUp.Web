using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakUp.Utilities
{
    public class ImageOperationsPfp
    {
        IWebHostEnvironment _env;
        public ImageOperationsPfp(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> ImageUpload(IFormFile file)
        {
            if (file != null)
            {
                string fileDirectory = Path.Combine(_env.WebRootPath, "ImagesProfiles");
                string filename = $"{Guid.NewGuid()}-{file.FileName}";
                string filepath = Path.Combine(fileDirectory, filename);

                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                return filename;
            }

            return null;
        }
		public async Task<string> SaveCroppedImage(IFormFile croppedImage, string fileExtension)
		{
			if (croppedImage != null)
			{
				string fileDirectory = Path.Combine(_env.WebRootPath, "ImagesProfiles");
				string filename = $"{Guid.NewGuid()}-cropped{fileExtension}";
				string filepath = Path.Combine(fileDirectory, filename);

				using (FileStream fs = new FileStream(filepath, FileMode.Create))
				{
					await croppedImage.CopyToAsync(fs);
				}

				return filename;
			}

			return null;
		}

		public void DeleteOldProfilePicture(string oldProfilePictureUrl)
		{
			if (!string.IsNullOrEmpty(oldProfilePictureUrl))
			{
				string fileDirectory = Path.Combine(_env.WebRootPath, "ImagesProfiles");
				string oldFilePath = Path.Combine(fileDirectory, oldProfilePictureUrl);

				if (File.Exists(oldFilePath))
				{
					File.Delete(oldFilePath);
				}
			}
		}
	}
}
