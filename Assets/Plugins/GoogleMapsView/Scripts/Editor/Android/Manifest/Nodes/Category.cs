﻿using System.Collections.Generic;

namespace GetSocialSdk.Editor.Android.Manifest.GoogleMaps
{
    public class Category : AndroidManifestNode
    {
        public Category(string name) : base("category", IntentFilterTag, new Dictionary<string, string> { { NameAttribute, name } })
        {
        }
    }
}