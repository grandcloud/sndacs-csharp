﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace GrandCloud.CS.Util
{
    internal static class CSConstants
    {
        internal const int PutObjectDefaultTimeout = 20 * 60 * 1000;

        internal static readonly long MinPartSize = 5 * (long)Math.Pow(2, 20);
        internal const int MaxNumberOfParts = 10000;

        internal const int DefaultBufferSize = 8192;

        internal const string CSDefaultEndpoint = "storage.grandcloud.cn";

        // Bucket Validation constants
        internal const int MinBucketLength = 3;
        internal const int MaxBucketLength = 255;

        // Commonly used headers
        internal const string MetaHeaderPrefix = "x-snda-meta-";
        internal const string RequestIdHeader = "x-snda-request-id";        
        internal const string DateHeader = "x-snda-date";
        internal const string AuthorizationHeader = "Authorization";      


        // Accepted HTTP Verbs
        internal static readonly string[] Verbs = { "GET", "HEAD", "PUT", "DELETE", "POST" };
        internal static readonly string GetVerb = Verbs[0];
        internal static readonly string HeadVerb = Verbs[1];
        internal static readonly string PutVerb = Verbs[2];
        internal static readonly string DeleteVerb = Verbs[3];
        internal static readonly string PostVerb = Verbs[4];

        // Commonly used static strings
        internal const string RequestParam = "request";


        // Location Constraint strings
        // These strings need to be ordered like the CSRegion enumeration
        internal static readonly string[] LocationConstraints = {"huabei-1",
                                                "huadong-1",
                                                "huabei-1"};

        internal const string REGION_HUADONG_1 = "huadong-1";
        internal const string REGION_HUABEI_1 = "huabei-1";


        // Error Codes
        internal const string NoSuchBucketPolicy = "NoSuchBucketPolicy";

    }
}