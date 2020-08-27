using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeerShareV2.Models;
using Amazon.Rekognition.Model;
using System.Threading.Tasks;
using System.Linq;
using PeerShareV2.Data;
using Amazon.Rekognition;
using PeerShareV2.Providers;

namespace PeerShareV2.Controllers
{
    public class ImageController : BaseController
    {
        public ImageController(AWSProvider aWSProvider, ApplicationDbContext db) :base(db) { }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(IFormFile file)
        {
            var image = FileToByte(file);
            var response = ViaImage(image).Result;
            var result = FilterDouble(response);
            
            return Json(result);
        }

        [HttpPost]
        public ActionResult DetectText(IFormFile file)
        {
            var image = FileToByte(file);
            var response = ViaImage(image).Result;
            var result = FilterDouble(response);
            
            return Json(result);
        }
        
        [HttpPost]
        public ActionResult ShowImage(ImageResult image)
        {
            return View(image);
        }

        public async Task<List<TextDetection>> ViaImage(ImageResult image)
        {
            Amazon.Rekognition.Model.Image img = new Amazon.Rekognition.Model.Image();
            MemoryStream stream = new MemoryStream(image.FileArray);
            img.Bytes = stream;

            DetectTextRequest detectTextRequest = new DetectTextRequest()
            {
                Image = img
            };

            try
            {
                Task<DetectTextResponse> detectTextResponseTask = _AWSProvider._rekognitionClient.DetectTextAsync(detectTextRequest);
                detectTextResponseTask.Wait();
                return detectTextResponseTask.Result.TextDetections;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        private ImageResult FileToByte(IFormFile file)
        {
            var image = new ImageResult();
            if (file != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.OpenReadStream().CopyTo(ms);
                    image = new ImageResult()
                    { FileArray = ms.ToArray(),
                    ContentType = file.ContentType };
                }
            }
            return image;
        }

        private List<Double> FilterDouble(List<TextDetection> response)
        {
            var words = response.Where(x => x.Type == "word").Select(x => x.DetectedText).ToList();
            var prices = new List<Double>();
            foreach(var i in words)
            {
                double p = 0.0;
                try
                {
                    if(i.Length>1 && (!Char.IsNumber(i, 0) && i[0]!= '-' && Char.IsNumber(i, 1)))
                    {
                        var temp = i.Substring(1);
                        p = Convert.ToDouble(temp);
                    }
                    else
                    {
                        p = Convert.ToDouble(i);
                    }
                    
                    if(p > 0)
                    {
                        prices.Add(p);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(i + " is not castable to double");
                }
            }
            return prices;
        }
    }
}
