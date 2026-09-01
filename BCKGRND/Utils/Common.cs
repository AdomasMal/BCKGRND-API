using BCKGRND.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace BCKGRND.Utils
{
    public class Common
    {

        private static string arcGisToken;

        private static string arcGisService;

        public static byte[] GetRandomSalt(int length)
        {
            var random = RandomNumberGenerator.Create();
            byte[] salt = new byte[length];
            random.GetNonZeroBytes(salt);
            return salt;
        }

        public static byte[] SaltHashPassword(byte[] password, byte[] salt)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] plainTextWithSaltBytes = new byte[password.Length + salt.Length];
            for (int i = 0; i < password.Length; i++)
            {
                plainTextWithSaltBytes[i] = password[i];
            }
            for(int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[password.Length + i] = salt[i];
            }
            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public static void setupArcGis(string token, string orgUrl, string context, string serviceName, string layerId)
        {
            arcGisToken = token;
            arcGisService = string.Format("https://{0}/{1}/rest/services/{2}/FeatureServer/{3}/addFeatures", orgUrl, context, serviceName, layerId);
        }

        public static async void addArcGisFeature(Location location)
        {
            List<Feature> features = new List<Feature>();
            Feature feature = new Feature();
            Geometry geometry = new Geometry();
            geometry.x = location.Longtitude;
            geometry.y = location.Latitude;
            Attributes attributes = new Attributes();
            attributes.dbID = location.ID;
            attributes.name = location.Name;
            feature.attributes = attributes;
            feature.geometry = geometry;
            features.Add(feature);

            //var stringPayload = "f=json&features=" + JsonConvert.SerializeObject(features) + "&token=AAPK8333550711534ec9aae3d646cd564eca9dSTZKquguegyUN5EMww4QUER1IgyJmlJK5XqFL5Qw3Dl4cYGwtNeZt3GIho66TB";
            var stringPayload = "f=json&features=" + JsonConvert.SerializeObject(features) + "&token=" + arcGisToken;
            Console.WriteLine(stringPayload);
            Console.WriteLine(arcGisService);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/x-www-form-urlencoded");

            var httpClient = new HttpClient();

            //var httpResponse = await httpClient.PostAsync("https://services7.arcgis.com/t9Kgkdbg30MpyY6C/ArcGIS/rest/services/test/FeatureServer/0/addFeatures", httpContent);
            var httpResponse = await httpClient.PostAsync(arcGisService, httpContent);

            Response response = null;

            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                Console.WriteLine(responseContent);
            }
        }

        public class Feature
        {
            public Geometry geometry;
            public Attributes attributes;
        }

        public class Geometry
        {
            public float x;
            public float y;
        }

        public class Attributes
        {
            public int dbID;
            public string name;
        }

        public class Response
        {
            public List<ResponseObj> addResults;
        }

        public class ResponseObj
        {
            public int objectId;
            public int globalId;
            public bool success;
        }
    }
}
