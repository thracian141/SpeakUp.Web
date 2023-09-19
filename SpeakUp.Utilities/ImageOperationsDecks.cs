using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hospitals.Utilities
{
    public class ImageOperationsDecks
    {
        IWebHostEnvironment _env;
        public ImageOperationsDecks(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> ImageUpload(IFormFile file)
        {
            string filename = null;
            if (file != null)
            {
                string fileDirectory = Path.Combine(_env.WebRootPath, "DeckImages");
                filename = Guid.NewGuid() + "-" + file.FileName;
                string filepath = Path.Combine(fileDirectory, filename);
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
            }
            return filename;
        }
    }
}
