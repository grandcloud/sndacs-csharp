/*******************************************************************************
 *  Copyright 2008-2012 GrandCloud.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 *  API Version: 2006-03-01
 *
 */

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
        internal const string StorageClassHeader = "x-snda-storage-class";


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
        internal static readonly string[] LocationConstraints = {"",
                                                "huadong-1",
                                                "huabei-1"};

        internal const string REGION_HUADONG_1 = "huadong-1";
        internal const string REGION_HUABEI_1 = "huabei-1";


        internal static readonly string[] StorageClasses = {"STANDARD",
                                                "REDUCED_REDUNDANCY"};

        // Error Codes
        internal const string NoSuchBucketPolicy = "NoSuchBucketPolicy";

    }
}