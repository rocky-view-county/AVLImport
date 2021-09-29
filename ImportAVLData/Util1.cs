using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAVLData
{
    public class Util1
    {
       
        public static void UpdateVehicleListPlot(IEnumerable<ActiveA> entities, IEnumerable<ActiveB> entitiesAgg, svCams.PlotPointRequest vd, string vehicleType)
        {
            try
            {
                
                DateTime startDate = vd.StartGMT.ToLocalTime().Date;
                DateTime endDate = vd.EndGMT.ToLocalTime().Date;

                using (dcAVL dc = new dcAVL())
                {
                    //Often this area has errors. It seems to have not enough commandTimeout. I set up 180 now.
                    dc.Database.CommandTimeout = 7200;

                    switch (vehicleType)
                    {
                        case "Grader":
                            #region
                            var graderAs = entities.Select(s => new GraderA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var graderBs = entitiesAgg.Select(s => new GraderB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });
                            var rmGraderA = dc.GraderAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id);
                            var rmGraderB = dc.GraderBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmGraderA.Count() > 0)
                                dc.GraderAs.RemoveRange(rmGraderA);
                            if (rmGraderB.Count() > 0)
                                dc.GraderBs.RemoveRange(rmGraderB);

                            dc.GraderAs.AddRange(graderAs);
                            dc.GraderBs.AddRange(graderBs);



                            #endregion
                            break;
                        case "Snowplow/Sander":
                            #region
                            var SnowplowSanderAs = entities.Select(s => new SnowplowSanderA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var SnowplowSanderBs = entitiesAgg.Select(s => new SnowplowSanderB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });
                            var rmSnowplowSanderA = dc.SnowplowSanderAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id);
                            var rmSnowplowSanderB = dc.SnowplowSanderBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmSnowplowSanderA.Count() > 0)
                                dc.SnowplowSanderAs.RemoveRange(rmSnowplowSanderA);
                            if (rmSnowplowSanderB.Count() > 0)
                                dc.SnowplowSanderBs.RemoveRange(rmSnowplowSanderB);

                            dc.SnowplowSanderAs.AddRange(SnowplowSanderAs);
                            dc.SnowplowSanderBs.AddRange(SnowplowSanderBs);
                            #endregion
                            break;

                        case "Mower":
                            #region
                            var MowerAs = entities.Select(s => new MowerA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var MowerBs = entitiesAgg.Select(s => new MowerB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });

                            

                            var rmMowerA = dc.MowerAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id);;
                            var rmMowerB = dc.MowerBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmMowerA.Count() > 0)
                                dc.MowerAs.RemoveRange(rmMowerA);
                            if (rmMowerB.Count() > 0)
                                dc.MowerBs.RemoveRange(rmMowerB);

                            dc.MowerAs.AddRange(MowerAs);
                            dc.MowerBs.AddRange(MowerBs);
                            #endregion
                            break;

                        case "Sprayer":
                            #region
                            var SprayerAs = entities.Select(s => new SprayerA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var SprayerBs = entitiesAgg.Select(s => new SprayerB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });
                            var rmSprayerA = dc.SprayerAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id);;
                            var rmSprayerB = dc.SprayerBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmSprayerA.Count() > 0)
                                dc.SprayerAs.RemoveRange(rmSprayerA);
                            if (rmSprayerB.Count() > 0)
                                dc.SprayerBs.RemoveRange(rmSprayerB);

                            dc.SprayerAs.AddRange(SprayerAs);
                            dc.SprayerBs.AddRange(SprayerBs);
                            #endregion
                            break;

                        case "Plow Sander Pickup":
                            #region
                            var PlowSanderPickupAs = entities.Select(s => new PlowSanderPickupA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var PlowSanderPickupBs = entitiesAgg.Select(s => new PlowSanderPickupB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });
                            var rmPlowSanderPickupA = dc.PlowSanderPickupAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id);;
                            var rmPlowSanderPickupB = dc.PlowSanderPickupBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmPlowSanderPickupA.Count() > 0)
                                dc.PlowSanderPickupAs.RemoveRange(rmPlowSanderPickupA);
                            if (rmPlowSanderPickupB.Count() > 0)
                                dc.PlowSanderPickupBs.RemoveRange(rmPlowSanderPickupB);

                            dc.PlowSanderPickupAs.AddRange(PlowSanderPickupAs);
                            dc.PlowSanderPickupBs.AddRange(PlowSanderPickupBs);
                            #endregion
                            break;

                        case "Public Works":
                        case "Public Works Semi":
                            #region
                            var PublicWorksAs = entities.Select(s => new PublicWorksA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var PublicWorksBs = entitiesAgg.Select(s => new PublicWorksB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });
                            var rmPublicWorksA = dc.PublicWorksAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id);;
                            var rmPublicWorksB = dc.PublicWorksBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmPublicWorksA.Count() > 0)
                                dc.PublicWorksAs.RemoveRange(rmPublicWorksA);
                            if (rmPublicWorksB.Count() > 0)
                                dc.PublicWorksBs.RemoveRange(rmPublicWorksB);

                            dc.PublicWorksAs.AddRange(PublicWorksAs);
                            dc.PublicWorksBs.AddRange(PublicWorksBs);
                            #endregion
                            break;

                        case "Supervisor":
                            #region
                            var SupervisorAs = entities.Select(s => new SupervisorA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var SupervisorBs = entitiesAgg.Select(s => new SupervisorB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });
                            var rmSupervisorA = dc.SupervisorAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id);;
                            var rmSupervisorB = dc.SupervisorBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmSupervisorA.Count() > 0)
                                dc.SupervisorAs.RemoveRange(rmSupervisorA);
                            if (rmSupervisorB.Count() > 0)
                                dc.SupervisorBs.RemoveRange(rmSupervisorB);

                            dc.SupervisorAs.AddRange(SupervisorAs);
                            dc.SupervisorBs.AddRange(SupervisorBs);
                            #endregion
                            break;

                        case "Patch Truck":
                            #region
                            var PatchTruckAs = entities.Select(s => new PatchTruckA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var PatchTruckBs = entitiesAgg.Select(s => new PatchTruckB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });
                            var rmPatchTruckA = dc.PatchTruckAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id);;
                            var rmPatchTruckB = dc.PatchTruckBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmPatchTruckA.Count() > 0)
                                dc.PatchTruckAs.RemoveRange(rmPatchTruckA);
                            if (rmPatchTruckB.Count() > 0)
                                dc.PatchTruckBs.RemoveRange(rmPatchTruckB);

                            dc.PatchTruckAs.AddRange(PatchTruckAs);
                            dc.PatchTruckBs.AddRange(PatchTruckBs);
                            #endregion
                            break;

                        case "Water Truck":
                            #region
                            var WaterTruckAs = entities.Select(s => new WaterTruckA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var WaterTruckBs = entitiesAgg.Select(s => new WaterTruckB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });
                            var rmWaterTruckA = dc.WaterTruckAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id); ;
                            var rmWaterTruckB = dc.WaterTruckBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmWaterTruckA.Count() > 0)
                                dc.WaterTruckAs.RemoveRange(rmWaterTruckA);
                            if (rmWaterTruckB.Count() > 0)
                                dc.WaterTruckBs.RemoveRange(rmWaterTruckB);

                            dc.WaterTruckAs.AddRange(WaterTruckAs);
                            dc.WaterTruckBs.AddRange(WaterTruckBs);
                            #endregion
                            break;

                        case "Ag Pickup":
                            #region
                            var AgPickupAs = entities.Select(s => new AgPickupA
                            {
                                SysId = s.SysId,
                                CollectedTime = s.CollectedTime,
                                Lat = s.Lat,
                                Lon = s.Lon,
                                SecDiff = s.SecDiff,
                                SeqOrder = s.SeqOrder,
                                Shape = s.Shape,
                                ShapeGeography = s.ShapeGeography,
                                ShapeLine = s.ShapeLine,
                                Speed = s.Speed,
                                State = s.State,
                                StateDesc = s.StateDesc,
                                SysWithDate = s.SysWithDate,
                                VehicleType = s.VehicleType,
                                X_Coord = s.X_Coord,
                                Y_Coord = s.Y_Coord,
                                StreetView = s.StreetView
                            });
                            var AgPickupBs = entitiesAgg.Select(s => new AgPickupB
                            {
                                Shape = s.Shape,
                                SysID = s.SysID,
                                VehicleType = s.VehicleType,
                                SysWithDate = s.SysWithDate,
                                ActivityTime = s.ActivityTime,
                                ShapeAgg = s.ShapeAgg,
                                ShapeLineAgg = s.ShapeLineAgg,
                                StateDesc = s.StateDesc,
                                SumOfDistance = s.SumOfDistance,
                                SumOfSec = s.SumOfSec,


                            });

                            var rmAgPickupA = dc.AgPickupAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id); ;
                            var rmAgPickupB = dc.AgPickupBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);
                            if (rmAgPickupA.Count() > 0)
                                dc.AgPickupAs.RemoveRange(rmAgPickupA);
                            if (rmAgPickupB.Count() > 0)
                                dc.AgPickupBs.RemoveRange(rmAgPickupB);

                            
                            dc.AgPickupAs.AddRange(AgPickupAs);
                            dc.AgPickupBs.AddRange(AgPickupBs);
                            #endregion
                            break;

                        default:
                            #region
                            var rmEntities = dc.ActiveAs.Where(s => s.CollectedTime > startDate && s.CollectedTime < endDate && s.SysId == vd.EAId.Assetid.Id);;
                            var rmAgg = dc.ActiveBs.Where(s => s.SysID == vd.EAId.Assetid.Id && s.ActivityTime >= startDate && s.ActivityTime <= endDate);

                            if (rmEntities.Count() > 0)
                                dc.ActiveAs.RemoveRange(rmEntities);
                            if (rmAgg.Count() > 0)
                                dc.ActiveBs.RemoveRange(rmAgg);

                            

                            dc.ActiveAs.AddRange(entities);
                            dc.ActiveBs.AddRange(entitiesAgg);
                            #endregion
                            break;

                    } //Switch Case

                    dc.SaveChanges();


                    Console.Write( "SysId: " + vd.EAId.Assetid.Id.ToString() + " process has been done \n\n");

                }//using Entity
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                //Console.ReadKey();
                string fromMail = "cityworks@mdrockyview.ab.ca";
                string toMail = "cmoon@rockyview.ca";
                string mailSubject = "AVL Importing Process failed";
                string mailBody = "\n" + ex.ToString() + "\n";
                Util.SendMail(fromMail, toMail, mailSubject, mailBody);
            }
        }
    
    
    }
}
