using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCKGRND.Models;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BCKGRND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly DBContext _context;

        public LocationController(DBContext context)
        {
            _context = context;
        }

        [HttpPost(Name = "PostNewLocation")]
        public string Post([FromBody]Location Value)
        {
            string responseBody;
            try
            {
                Location location = new Location();
                location.Name = Value.Name;
                location.Description = Value.Description;
                location.Latitude = Value.Latitude;
                location.Longtitude = Value.Longtitude;
                location.Photos = Value.Photos;
                location.Tags = new List<Tag>();
                foreach(Tag newTag in Value.Tags)
                {
                    if (_context.Tags.Count() != 0)
                    {
                        if (_context.Tags.Any(t => t.Name.Equals(newTag.Name)))
                        {
                            location.Tags.Add(_context.Tags.First(t => t.Name.Equals(newTag.Name)));
                        }
                        else
                        {
                            location.Tags.Add(newTag);
                        }
                    }
                    else
                    {
                        location.Tags.Add(newTag);
                    }
                }
                _context.Add(location);
                _context.SaveChanges();
                responseBody = JsonConvert.SerializeObject("New location added");
                Utils.Common.addArcGisFeature(location);
            }
            catch (Exception ex)
            {
                responseBody = JsonConvert.SerializeObject(ex.Message);
            }
            Response.ContentLength = responseBody.Length;
            return responseBody;
        }

        [HttpGet("{searchOptions}")]
        public string Get(string searchOptions)
        {
            string responseBody;
            try
            {
                List<string> optionList = searchOptions.Split(" ").ToList();
                var locations = _context.Locations.Where(location => location.ID.Equals(0));
                if (optionList[0] == "tags")
                {
                    //"tags" tag1 tag2 ... tagN
                    optionList.RemoveAt(0);
                    locations = _context.Locations.Where(location => location.Tags.Any(t => optionList.Contains(t.Name)));
                }
                else if(optionList[0] == "name")
                {
                    //"name" word1 word2 ... wordN
                    optionList.RemoveAt(0);
                    foreach(string value in optionList)
                    {
                        locations = locations.Union(_context.Locations.Where(location => location.Name.Contains(value)));
                    }
                }
                else if (optionList[0] == "proximity")
                {
                    //"proximity" latitude longtitude maxDistance
                    float lat = float.Parse(optionList[1]);
                    float lon = float.Parse(optionList[2]);
                    float dist = float.Parse(optionList[3]);
                    locations = _context.Locations.Where(location => 2 * MathF.Atan2(MathF.Sqrt(MathF.Pow(MathF.Sin(MathF.Abs(lat - location.Latitude) / 2f * 0.01745f), 2) + MathF.Cos(lat * 0.01745f) * MathF.Cos(location.Latitude * 0.01745f) * MathF.Pow(MathF.Sin(MathF.Abs(lon - location.Longtitude) / 2f * 0.01745f), 2)), MathF.Sqrt(1 - (MathF.Pow(MathF.Sin(MathF.Abs(lat - location.Latitude) / 2f * 0.01745f), 2) + MathF.Cos(lat * 0.01745f) * MathF.Cos(location.Latitude * 0.01745f) * MathF.Pow(MathF.Sin(MathF.Abs(lon - location.Longtitude) / 2f * 0.01745f), 2)))) * 6371 < dist);
                }
                else if (optionList[0] == "id")
                {
                    int id = int.Parse(optionList[1]);
                    locations = _context.Locations.Where(location => location.ID.Equals(id));
                }

                List<Location> locations2 = new List<Location>();
                foreach(Location location in locations)
                {
                    Location newLocation = new Location();
                    newLocation.ID = location.ID;
                    newLocation.Name = location.Name;
                    newLocation.Description = location.Description;
                    newLocation.Latitude = location.Latitude;
                    newLocation.Longtitude = location.Longtitude;
                    newLocation.Tags = new List<Tag>();
                    newLocation.Photos = new List<Photo>();

                    locations2.Add(newLocation);
                }
                foreach(Location location in locations2)
                {
                    foreach (Tag tag in _context.Tags.Where(t => t.Location.Any(l => l.ID.Equals(location.ID))))
                    {
                        Tag newTag = new Tag();
                        newTag.ID = tag.ID;
                        newTag.Name = tag.Name;
                        location.Tags.Add(tag);
                    }
                    
                    foreach (Photo photo in _context.Photos.Where(p => p.Location.ID.Equals(location.ID)))
                    {
                        Photo newPhoto = new Photo();
                        newPhoto.ID = photo.ID;
                        newPhoto.Image = photo.Image;
                        newPhoto.Location = null;
                        location.Photos.Add(newPhoto);
                    }
                }

                responseBody = JsonConvert.SerializeObject(locations2);
            }
            catch (Exception ex)
            {
                responseBody = JsonConvert.SerializeObject(ex.Message);
            }
            Response.ContentLength = responseBody.Length;
            return responseBody;
        }
    }
}
