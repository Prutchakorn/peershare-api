using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Rekognition;
using Amazon.Runtime;
using PeerShareV2.Data;

namespace PeerShareV2.Providers
{
    public class AWSProvider : BaseProvider
    {
        public readonly AmazonRekognitionClient _rekognitionClient;
        public AWSProvider(ApplicationDbContext db) : base(db)
        {
            var accessKeyID = "AKIAJDX3ZPA2HOHWY7CA";
            var secretKey = "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";
           
            var awsOptions = new Amazon.Extensions.NETCore.Setup.AWSOptions();
            awsOptions.Credentials = new BasicAWSCredentials(accessKeyID, secretKey);
            awsOptions.Region = RegionEndpoint.USEast1;
            
            var client = new AmazonRekognitionClient(awsOptions.Credentials, awsOptions.Region);
            _rekognitionClient = client;
        }
    }
}