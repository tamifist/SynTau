using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Contracts.Entities.SystemMonitor;
using Data.Services;
using Infrastructure.API.Synthesizer.Responses.SystemMonitor;
using Infrastructure.API.Synthesizer.Services;
using Microsoft.Azure.WebJobs;
using Z.EntityFramework.Plus;
using SensorState = Data.Contracts.Entities.SystemMonitor.SensorState;
using SensorType = Data.Contracts.Entities.SystemMonitor.SensorType;

namespace Infrastructure.WebJobs
{
    public class Functions
    {
        public static void UpdateSensorListings([TimerTrigger("00:00:02", RunOnStartup = true)] TimerInfo info, TextWriter log)
        {
            log.WriteLine("--- Updating sensor listings");
            
            try
            {
                using (var commonDbContext = new CommonDbContext())
                {
                    var sw = Stopwatch.StartNew();

                    //SensorRestService sensorRestService = new SensorRestService();
                    SensorStubRestService sensorRestService = new SensorStubRestService();
                    SensorListingResponse sensorListingResponse = sensorRestService.GetAllSensors();

                    SensorListing sensorListing = new SensorListing();

                    sensorListing.Time = sensorListingResponse.Time;

                    foreach (SensorResponse sensorResponse in sensorListingResponse.Sensors)
                    {
                        sensorListing.Sensors.Add(new Sensor()
                        {
                            Type = (SensorType) sensorResponse.Type,
                            State = (SensorState) sensorResponse.State,
                            Value = sensorResponse.Value,
                        });
                    }

                    commonDbContext.SensorListings.Add(sensorListing);

                    commonDbContext.SaveChanges();

                    sw.Stop();

                    log.WriteLine($"--- Sensor listings were updated successfully. ElapsedMilliseconds: {sw.ElapsedMilliseconds}.");
                }
            }
            catch (Exception ex)
            {
                log.WriteLine(ex.Message);
            }
        }

        public static async Task DeleteOldSensorListings([TimerTrigger("00:30:00", RunOnStartup = true)] TimerInfo info, TextWriter log)
        {
            log.WriteLine("--- Deleting old sensor listings");

            try
            {
                using (var commonDbContext = new CommonDbContext())
                {
                    var sw = Stopwatch.StartNew();

                    var now = DateTimeOffset.Now;

                    log.WriteLine($"--- Deleting old sensor listings. DateTime.Now: {now}");

                    await commonDbContext.SensorListings.Where(x => DbFunctions.DiffMinutes(x.CreatedAt, now) > 30).DeleteAsync();

                    sw.Stop();
                    log.WriteLine($"--- Old sensor listings were deleted successfully. ElapsedMilliseconds: {sw.ElapsedMilliseconds}.");
                }
            }
            catch (Exception ex)
            {
                log.WriteLine(ex.Message);
            }
        }
    }
}
