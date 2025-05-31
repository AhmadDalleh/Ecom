using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories.Service
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider fileProvider;
        public ImageManagementService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        //this return list of images src
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)//files the file it self that send in http , src the name of the prouct
        {
            var saveImageSrc = new List<string>(); //the list of image the returns to controller
            var ImageDirectory = Path.Combine("wwwroot", "Images", src.Trim());//where the image directory will be store in the wwwroot directory
            if (Directory.Exists(ImageDirectory) is not true)//just check if the directory exist and created if it not by name of the product
            {
                Directory.CreateDirectory(ImageDirectory);
            }
            foreach (var item in files)//this for the all images that set to product
            {
                if (item.Length > 0)//if there is image in the item
                {
                    //get image name
                    var ImageName = item.FileName;//set image name to file name 

                    var ImageSrc = $"/Images/{src}/{ImageName}";//where image will be store
                    var root = Path.Combine(ImageDirectory, ImageName);//insert the image name to the Image directory
                    using (FileStream stream = new FileStream(root, FileMode.Create)) 
                    {
                        await item.CopyToAsync(stream);
                    }//this copy the image from root in the pc and apply it in image src 
                    saveImageSrc.Add(ImageSrc);
                }
            }
            return saveImageSrc; 
        }
        public void DeleteImageAsync(string src)
        {
            var info = fileProvider.GetFileInfo(src);
            var root = info.PhysicalPath;
            File.Delete(root);

        }
    }
}
