using AutoGene.Mobile.Abstractions;
using AutoGene.Mobile.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(iOSPlatform))]
namespace AutoGene.Mobile.iOS.Services
{
    public class iOSPlatform : IPlatform
    {
        public string GetSyncStore()
        {
            return "syncstore.db";
        }
    }
}