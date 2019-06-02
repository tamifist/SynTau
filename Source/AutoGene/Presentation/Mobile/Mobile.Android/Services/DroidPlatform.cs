using System;
using System.IO;
using AutoGene.Mobile.Abstractions;
using AutoGene.Mobile.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(DroidPlatform))]
namespace AutoGene.Mobile.Droid.Services
{
    public class DroidPlatform : IPlatform
    {
        public string GetSyncStore()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "syncstore.db");

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            return path;
        }
    }
}